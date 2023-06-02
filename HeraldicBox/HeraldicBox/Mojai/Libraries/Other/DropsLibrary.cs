using System;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using ReflectionUtility;
using NCMS;

namespace Mojai.Libraries.Other
{
    static class DropsLibrary
    {

        public static bool SpawnDrops(WorldTile tTile, GodPower pPower)
        {
            AssetManager.powers.CallMethod("spawnDrops", tTile, pPower);
            return true;
        }

        public class Drop
        {
            public DropAsset dropAsset;
            public GodPower dropPower;

            public Drop(string dropId, DropsAction dropAction, string cloneOf = "blessing")
            {
                dropAsset = AssetManager.drops.clone(dropId, cloneOf);
                dropAsset.action_landed = dropAction;

                dropPower = AssetManager.powers.clone(dropId, "_drops");
                dropPower.name = dropId;
                dropPower.dropID = dropId;
                dropPower.fallingChance = 0.01f;
                dropPower.click_power_brush_action = new PowerAction(SpawnDrops);
            }
        }
    }
}
