using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeraldicBox
{
    class HeraldicInfo
    {
        public static List<HeraldicInfo> allInfo = new List<HeraldicInfo>();

        public string actorName = "Unknown";
        public string kingdom = "None";
        public string city = "Nowhere";

        public ActorData actorData;
        public HeraldicInfo father, mother;

        [NonSerialized] public List<HeraldicInfo> children = new List<HeraldicInfo>();  // <-- Automatically added by the save system.
        [NonSerialized] public Family family;                                           // <-- Also automatically added by the save system.
        [NonSerialized] public Actor actor;
        [NonSerialized] public Sprite savedSprite = Resources.Load<Sprite>("ui/icons/dead");

        public void TryUpdateActorInfo()
        {
            if(actor == null)
            {
                return;
            }

            GameObject loader_object = new GameObject("loader");
            UnitAvatarLoader loader = loader_object.AddComponent<UnitAvatarLoader>() as UnitAvatarLoader;
            loader.load(actor);
            Image avatar = loader.transform.GetChild(0).gameObject.GetComponent<Image>();
            savedSprite = avatar.sprite;
            UnityEngine.Object.Destroy(loader_object);

            actorName = actor.getName();

            if (actor.kingdom != null)
            {
                kingdom = actor.kingdom.name;
            }

            if (actor.city != null)
            {
                city = actor.city.getCityName();
            }
        }

        public HeraldicInfo(ActorData pData, Actor pActor = null, Family pFamily = null, HeraldicInfo pFather = null)
        {
            actorData = pData;
            actor = pActor;
            father = pFather;

            if (pFamily != null)
            {
                Family.addToFamily(this, pFamily);
            }

            if (father != null)
            {
                if(father.family != null)
                {
                    Family.addToFamily(this, father.family);
                }
            }

            allInfo.Add(this);
        }
    }
}
