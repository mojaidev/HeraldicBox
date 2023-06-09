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
        public List<HeraldicInfo> children = new List<HeraldicInfo>();
        public Family family;
        public HeraldicInfo father, mother;

        public string publicID; // <-- This is for the save system.

        [NonSerialized] public Actor actor;
        [NonSerialized] public Sprite savedSprite = Resources.Load<Sprite>("ui/icons/dead");

        public void TryUpdateActorInfo()
        {
            if(actor != null)
            {
                GameObject loader_object = new GameObject("loader");
                UnitAvatarLoader loader = loader_object.AddComponent<UnitAvatarLoader>() as UnitAvatarLoader;
                loader.load(actor);
                Image avatar = loader.transform.GetChild(0).gameObject.GetComponent<Image>();
                savedSprite = avatar.sprite;
                loader_object.SetActive(false);

                actorName = actor.getName();

                if(actor.kingdom != null)
                {
                    kingdom = actor.kingdom.name;
                }

                if(actor.city != null)
                {
                    city = actor.city.getCityName();
                }
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
            publicID = Guid.NewGuid().ToString();
        }
    }
}
