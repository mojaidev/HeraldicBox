using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraldicBox
{
    class HeraldicBox
    {
        public static void Start()
        {
            Mojai.Mod.Util.Print("GAME TARGET VERSION: " + Config.versionCodeText);
            HeraldicBoxSettings.Setup();
            Test.Init();
            Patches.Setup();
            HeraldicBoxActions.Setup();
            HeraldicBoxUI.SetupAll();
        }
    }
}
