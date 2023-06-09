﻿using HarmonyLib;

namespace HeraldicBox
{
    class Patches
    {
        public static void Setup()
        {

            // ====================================================
            // CHILDTRACKER PATCHES
            // ====================================================
            Mojai.Managers.PatchManager.NewPatch("ChildTracker Patch", AccessTools.Method(typeof(ai.behaviours.CityBehProduceUnit), "tryToProduceUnit"), AccessTools.Method(typeof(ChildTracker), "tryToProduceUnit_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix);
            //Mojai.Managers.PatchManager.NewPatch("ChildTracker Patch", AccessTools.Method(typeof(ai.behaviours.CityBehProduceUnit), "produceNewCitizen"), AccessTools.Method(typeof(ChildTracker), "produceNewCitizen_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix); // Prevent conflicts with CollectionMod
            Mojai.Managers.PatchManager.NewPatch("ChildTracker Patch", AccessTools.Method(typeof(ActorManager), "finalizeActor"), AccessTools.Method(typeof(ChildTracker), "finalizeActor_Postfix"), Mojai.Managers.PatchManager.Methods.Postfix);
            Mojai.Managers.PatchManager.NewPatch("ChildTracker Patch", AccessTools.Method(typeof(ActorData), "inheritTraits"), AccessTools.Method(typeof(ChildTracker), "inheritTraits_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix);

            // ====================================================
            // FAMILY PATCHES
            // ====================================================
            Mojai.Managers.PatchManager.NewPatch("Family Patch", AccessTools.Method(typeof(ActorBase), "getName"), AccessTools.Method(typeof(Family), "getName_Prefix"), Mojai.Managers.PatchManager.Methods.Prefix);
            Mojai.Managers.PatchManager.NewPatch("Family Patch", AccessTools.Method(typeof(Actor), "killHimself"), AccessTools.Method(typeof(Family), "killHimself_Postfix"), Mojai.Managers.PatchManager.Methods.Postfix);
            Mojai.Managers.PatchManager.NewPatch("Family Patch", AccessTools.Method(typeof(MapBox), "clearTiles"), AccessTools.Method(typeof(Family), "clearTiles_Postfix"), Mojai.Managers.PatchManager.Methods.Postfix);
            Mojai.Managers.PatchManager.NewPatch("Family Patch", AccessTools.Method(typeof(MapBox), "generateNewMap"), AccessTools.Method(typeof(Family), "clearTiles_Postfix"), Mojai.Managers.PatchManager.Methods.Postfix);

            // ====================================================
            // HERALDICBOXUI PATCHES
            // ====================================================
            Mojai.Managers.PatchManager.NewPatch("HeraldicBoxUi Patch", AccessTools.Method(typeof(ScrollWindow), "clickHide"), AccessTools.Method(typeof(HeraldicBoxUI.inspect_family_window), "clickHide_Postfix"), Mojai.Managers.PatchManager.Methods.Postfix);
        }
    }
}
