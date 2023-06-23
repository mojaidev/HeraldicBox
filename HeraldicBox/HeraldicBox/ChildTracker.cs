using System.Collections.Generic;
using ai;
using ReflectionUtility;


namespace HeraldicBox
{
	// ====================================================
	// CHILDTRACKER
	// this is the most messed up code of all the mod so
	// enjoy it!
	// ====================================================

	class ChildTracker
	{
		private static List<HeraldicInfo> RegisterA = new List<HeraldicInfo>();

		// ====================================================
		// IMPORTANT PATCHES SECTION
		// ====================================================

		// tryToProduceUnit_Prefix has been made to prevent conflicts with CollectionMod
		public static bool tryToProduceUnit_Prefix(City pCity, ai.behaviours.CityBehProduceUnit __instance)
		{
			if (getFoodItem(null, pCity) == null)
			{
				return false;
			}
			Building buildingType = pCity.CallMethod("getBuildingType", SB.type_house, true, false) as Building;
			if (buildingType == null)
			{
				return false;
			}
			if (produceNewCitizen_Prefix(buildingType, pCity, __instance))
			{
				Reflection.SetField(__instance, "unitProduced", true);
			}

			return false;
		}

		public static bool produceNewCitizen_Prefix(Building pBuilding, City pCity, ai.behaviours.CityBehProduceUnit __instance = null)
		{
			List<Actor> _possibleParents = Reflection.GetField(__instance.GetType(), __instance, "_possibleParents") as List<Actor>;

			Actor actor = _possibleParents.Pop<Actor>();

			ActorData actor_data = Reflection.GetField(typeof(Actor), actor, "data") as ActorData;

			HeraldicComponent heraldicComponent1 = actor.gameObject.GetComponent<HeraldicComponent>();
			HeraldicComponent heraldicComponent2 = null;

			if (actor == null)
			{
				return false;
			}
			BaseStats actor_stats = Reflection.GetField(typeof(Actor), actor, "stats") as BaseStats;
			if (!Toolbox.randomChance(actor_stats[S.fertility]))
			{
				return false;
			}
			Actor actor2 = null;
			ActorData actor_data2 = null;

			// This nesting is horrible...
			if (_possibleParents.Count > 0)
			{
				if (heraldicComponent1 != null)
				{
					for (int i = 0; i < _possibleParents.Count; i++)
					{
						Actor pActor = _possibleParents[i];
						ActorData pData = Reflection.GetField(typeof(Actor), pActor, "data") as ActorData;
						if ((bool)HeraldicBoxSettings.GetSetting("LGBT_Reproduction") == false && pData.gender == actor_data.gender)
                        {
                        }
                        else
                        {
							if (pActor.gameObject.GetComponent<HeraldicComponent>() != null)
							{
								if (pActor.gameObject.GetComponent<HeraldicComponent>().Heraldic.family != heraldicComponent1.Heraldic.family)
								{
									actor2 = pActor;
									actor_data2 = pData;
									heraldicComponent2 = actor2.gameObject.GetComponent<HeraldicComponent>();
									_possibleParents.Remove(pActor);
									break;
								}
							}
						}
					}
				}
			}

			ActorData actorData = new ActorData();

			HeraldicInfo newInfo = null;
			if (heraldicComponent1 != null)
			{
				if (heraldicComponent2 == null && (bool)HeraldicBoxSettings.GetSetting("Asexual_Reproduction") == false)
				{
					return false;
				}

				newInfo = new HeraldicInfo(actorData, null, null, heraldicComponent1.Heraldic);
				RegisterA.Add(newInfo);
				newInfo.father.children.Add(newInfo);
			}
			if (heraldicComponent2 != null)
			{
				newInfo.mother = heraldicComponent2.Heraldic;
				newInfo.mother.children.Add(newInfo);
			}

			ResourceAsset foodItem = Reflection.CallMethod(pCity, "getFoodItem", (string)null) as ResourceAsset;
			pCity.CallMethod("eatFoodItem", foodItem.id);
			pCity.status.housingFree--;
			pCity.data.born++;
			Kingdom pCity_kingdom = Reflection.GetField(typeof(City), pCity, "kingdom") as Kingdom;
			if (pCity_kingdom != null)
			{
				pCity_kingdom.data.born++;
			}
			Race actor_race = Reflection.GetField(typeof(Actor), actor, "race") as Race;
			MapBox world = MapBox.instance;
			ActorAsset asset = actor.asset;

			actorData.created_time = world.getCreationTime();
			actorData.cityID = pCity.data.id;
			actorData.id = world.mapStats.getNextId("unit");
			actorData.asset_id = asset.id;
			ActorBase.generateCivUnit(actor.asset, actorData, actor_race);
			actorData.generateTraits(asset, actor_race);
			actorData.CallMethod("inheritTraits", actor_data.traits);
			actorData.hunger = asset.maxHunger / 2;
			actor_data.makeChild(world.getCurWorldTime());

			if (actor2 != null)
			{
				actorData.CallMethod("inheritTraits", actor_data2.traits);
				actor_data2.makeChild(world.getCurWorldTime());
			}
			Clan clan = checkGreatClan(actor, actor2);
			actorData.skin = ActorTool.getBabyColor(actor, actor2);
			actorData.skin_set = actor_data.skin_set;
			Culture babyCulture = getBabyCulture(actor, actor2);
			if (babyCulture != null)
			{
				actorData.culture = babyCulture.data.id;
				actorData.level = babyCulture.getBornLevel();
			}
			if (clan != null)
			{
				Actor pActor = pCity.spawnPopPoint(actorData, actor.currentTile);
				clan.addUnit(pActor);
			}
			else
			{
				pCity.addPopPoint(actorData);
			}
			return true;
		}

		public static void finalizeActor_Postfix(string pStats, Actor pActor, WorldTile pTile, float pZHeight = 0f)
		{
			ActorData actor_data = Reflection.GetField(typeof(Actor), pActor, "data") as ActorData;

			foreach (HeraldicInfo heraldicInfo in RegisterA)
			{
				if (heraldicInfo.actorData == actor_data)
				{
					HeraldicComponent newComponent = pActor.gameObject.AddComponent<HeraldicComponent>();
					newComponent.Heraldic = heraldicInfo;
					heraldicInfo.actor = pActor;
					heraldicInfo.TryUpdateActorInfo();
				}
			}
		}

		public static bool inheritTraits_Prefix(List<string> pTraits, ActorData __instance)
        {
			if( (bool)HeraldicBoxSettings.GetSetting("inheritance") == false)
            {
				return true;
            }

			for (int i = 0; i < pTraits.Count; i++)
			{
				string pID = pTraits[i];
				ActorTrait actorTrait = AssetManager.traits.get(pID);
				if (actorTrait != null && actorTrait.inherit != 0f)
				{
					float num = Toolbox.randomFloat( float.Parse((string)HeraldicBoxSettings.GetSetting("Traits_Probability")) , 100f);
					if (num == 100f && !__instance.traits.Contains(actorTrait.id) && !(bool)__instance.CallMethod("haveOppositeTrait", actorTrait))
					{
						__instance.addTrait(actorTrait.id);
					}
				}
			}
			return false;
		}

		// ====================================================
		// SHITS SECTION
		// ====================================================

		static private ResourceAsset getFoodItem(string pFavoriteFood = null, City __instance = null)
		{
			if (!string.IsNullOrEmpty(pFavoriteFood) && __instance.data.storage.get(pFavoriteFood) > 0)
			{
				return AssetManager.resources.get(pFavoriteFood);
			}
			return __instance.data.storage.getRandomFood();
		}
		private static Culture getBabyCulture(Actor pActor1, Actor pActor2)
		{
			MapBox world = MapBox.instance;
			ActorData actor_data = Reflection.GetField(typeof(Actor), pActor1, "data") as ActorData;
			ActorData actor_data2 = null;

			if (pActor2 != null)
			{
				actor_data2 = Reflection.GetField(typeof(Actor), pActor2, "data") as ActorData;
			}

			string text = actor_data.culture;
			string text2 = text;
			if (pActor2 != null)
			{
				text2 = actor_data2.culture;
			}
			if (string.IsNullOrEmpty(text))
			{
				City city = pActor1.city;
				text = ((city != null) ? city.data.culture : null);
			}
			if (string.IsNullOrEmpty(text2) && pActor2 != null)
			{
				City city2 = pActor2.city;
				text2 = ((city2 != null) ? city2.data.culture : null);
			}
			Culture culture = pActor1.currentTile.zone.culture;
			Race actor_race = Reflection.GetField(typeof(Actor), pActor1, "race") as Race;
			if (culture != null && culture.data.race == actor_race.id && Toolbox.randomChance(culture.stats.culture_spread_convert_chance.value))
			{
				text = culture.data.id;
			}
			if (Toolbox.randomBool())
			{
				return world.cultures.get(text);
			}
			return world.cultures.get(text2);
		}
		private static Clan checkGreatClan(Actor pParent1, Actor pParent2)
		{
			string text = string.Empty;
			MapBox world = MapBox.instance;
			ActorData actor_data = Reflection.GetField(typeof(Actor), pParent1, "data") as ActorData;
			ActorData actor_data2 = null;

			if (pParent2 != null)
			{
				actor_data2 = Reflection.GetField(typeof(Actor), pParent2, "data") as ActorData;
			}


			if (string.IsNullOrEmpty(text))
			{
				if (pParent1.isKing())
				{
					text = actor_data.clan;
				}
				else if (pParent2 != null && pParent2.isKing())
				{
					text = actor_data2.clan;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				if (pParent1.isCityLeader() && pParent2 != null && pParent2.isCityLeader())
				{
					if (Toolbox.randomBool())
					{
						text = actor_data.clan;
					}
					else
					{
						text = actor_data2.clan;
					}
				}
				else if (pParent1 != null && pParent1.isCityLeader())
				{
					text = actor_data.clan;
				}
				else if (pParent2 != null && pParent2.isCityLeader())
				{
					text = actor_data2.clan;
				}
			}
			Clan result = null;
			if (!string.IsNullOrEmpty(text))
			{
				result = world.clans.get(text);
			}
			return result;
		}
	}
}