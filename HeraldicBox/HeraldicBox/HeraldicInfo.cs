﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HeraldicBox
{
    class HeraldicInfo
    {
        //private static List<HeraldicInfo> allInfo = new List<HeraldicInfo>();

        public bool IsDead = false;
        public Actor actor;
        public ActorData actorData;
        public List<HeraldicInfo> childs = new List<HeraldicInfo>();
        public HeraldicInfo father, mother;
        public Family family;



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
            
        }
    }
}