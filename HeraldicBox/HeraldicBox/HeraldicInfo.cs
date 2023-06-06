using System;
using System.Collections.Generic;
using UnityEngine;

namespace HeraldicBox
{
    class HeraldicInfo
    {
        public static List<HeraldicInfo> allInfo = new List<HeraldicInfo>();

        public string actorName = "Unknown";
        [NonSerialized] public Actor actor;
        public ActorData actorData;
        public List<HeraldicInfo> childs = new List<HeraldicInfo>();
        [NonSerialized] public HeraldicInfo father, mother;
        public Family family;

        public string publicID; // <-- This is for the save system.


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
