using ReflectionUtility;

namespace Mojai.Libraries.Other
{
    static class mDropsLibrary
    {

        public static bool SpawnDrops(WorldTile tTile, GodPower pPower)
        {
            AssetManager.powers.CallMethod("spawnDrops", tTile, pPower);
            return true;
        }

        public static bool loopWithCurrentBrushPower(WorldTile pCenterTile, GodPower pPower)
        {
            AssetManager.powers.CallMethod("loopWithBrushPower", pCenterTile, pPower, Config.currentBrushData);
            return true;
        }

        public class spawnDropPower
        {
            public GodPower dropPower;

            public spawnDropPower(string dropId, DropsAction powerAction)
            {
                DropAsset warriorDrop = AssetManager.drops.clone(dropId, "blessing");
                warriorDrop.path_texture = "drops/drop_pixel"; // <-- whatever this is
                warriorDrop.action_landed = new DropsAction(powerAction);

                dropPower = AssetManager.powers.add(new GodPower
                {
                    id = dropId,
                    holdAction = true,
                    fallingChance = 0.05f,
                    showToolSizes = true,
                    unselectWhenWindow = true
                });
                dropPower.name = dropId;
                dropPower.dropID = dropId;
                dropPower.click_power_action = new PowerAction(SpawnDrops);
                dropPower.click_power_brush_action = new PowerAction(loopWithCurrentBrushPower);
            }
        }

        public class customDropPower
        {
            public GodPower dropPower;

            public customDropPower(string dropId, PowerAction powerAction)
            {
                dropPower = AssetManager.powers.add(new GodPower
                {
                    id = dropId, 
                    type = PowerActionType.SpawnActor,
                    rank = PowerRank.Rank0_free
                });
                dropPower.name = dropId;
                dropPower.dropID = "_drops"; // <-- Avoid the "Description" problem, change this and you will see what i mean
                dropPower.click_power_action = powerAction;
            }
        }

        public class Drop
        {
            public DropAsset dropAsset;
            public GodPower dropPower;

            public Drop(string dropId, DropsAction dropAction)
            {
                dropAsset = AssetManager.drops.clone(dropId, "blessing");
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
