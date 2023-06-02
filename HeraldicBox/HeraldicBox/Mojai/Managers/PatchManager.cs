using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Mojai.Managers
{
    public class PatchManager
    {
        public enum Methods
        {
            Prefix,
            Postfix
        }

        public static string patchName;
        public static List<Harmony> allPatches = new List<Harmony>();
        public static bool debug = true;

        public static void NewPatch(string name, System.Reflection.MethodInfo originalMethod, System.Reflection.MethodInfo patchMethod, Methods method)
        {
            name = patchName + "_" + name + "_" + allPatches.Count.ToString();
            Mojai.Mod.Util.Print("LOADING PATCH: " + name, Mod.Util.PrintType.Modification);
            Harmony patch = new Harmony(name);
            if(method == Methods.Prefix)
            {
                patch.Patch(originalMethod, new HarmonyMethod(patchMethod));
            }

            if (method == Methods.Postfix)
            {
                patch.Patch(originalMethod, null, new HarmonyMethod(patchMethod));
            }

            allPatches.Add(patch);
            Mojai.Mod.Util.Print("PATCH LOADED: " + name);
        }
    }
}

