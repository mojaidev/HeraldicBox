using System.Collections.Generic;
using HarmonyLib;
using ReflectionUtility;

namespace HeraldicBox
{
    class Patches
    {
        public static void Setup()
        {

            // ====================================================
            // CHILDTRACKER PATCHES
            // ====================================================
            Mojai.Managers.PatchManager.NewPatch("ChildTracker Patch", AccessTools.Method(typeof(ai.behaviours.CityBehProduceUnit), "produceNewCitizen"), AccessTools.Method(typeof(ChildTracker), "produceNewCitizen_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix);
            Mojai.Managers.PatchManager.NewPatch("ChildTracker Patch", AccessTools.Method(typeof(ActorManager), "finalizeActor"), AccessTools.Method(typeof(ChildTracker), "finalizeActor_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix);
            Mojai.Managers.PatchManager.NewPatch("Family Patch", AccessTools.Method(typeof(ActorBase), "getName"), AccessTools.Method(typeof(Family), "getName_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix);
        }
    }
}
