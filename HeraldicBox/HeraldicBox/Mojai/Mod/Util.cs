namespace Mojai.Mod
{
    class Config
    {
        public static bool Debug = true;
    }

    class Util
    {
        public enum PrintType
        {
            Modification,
            Addition,
            Substraction,
        }


        // I could use switch() here but...
        public static void UserPrint(string text, PrintType printType = PrintType.Addition)
        {
            if (Config.Debug == true)
            {
                if (printType == PrintType.Modification)
                {
                    WorldTip.showNowTop(" [*] " + text);
                }
                if (printType == PrintType.Substraction)
                {
                    WorldTip.showNowTop(" [-] " + text);
                }
                if (printType == PrintType.Addition)
                {
                    WorldTip.showNowTop(" [+] " + text);
                }
            }
        }

        public static void Print(string text, PrintType printType = PrintType.Addition)
        {
            if(Config.Debug == true)
            {
                if(printType == PrintType.Modification)
                {
                    WorldBoxConsole.Console.print(" [*] " + text);
                }
                if (printType == PrintType.Substraction)
                {
                    WorldBoxConsole.Console.print(" [-] " + text);
                }
                if (printType == PrintType.Addition)
                {
                    WorldBoxConsole.Console.print(" [+] " + text);
                }
            }
        }
    }
}
