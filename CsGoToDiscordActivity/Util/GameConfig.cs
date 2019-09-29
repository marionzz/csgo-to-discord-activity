using System.IO;

namespace CsGoToDiscordActivity
{
    internal static class GameConfig
    {
        private const string cfgPath = @"730\local\cfg";
        private const string cfgFileName = "gamestate_integration_discord.cfg";

        private static readonly string[] steamAppsPaths = { @"C:\Program Files (x86)\Steam\userdata", @"C:\Program Files\Steam\userdata" };

        public static void AddCfgFile()
        {
            foreach (string steamAppsPath in steamAppsPaths)
            {
                if (!Directory.Exists(steamAppsPath))
                {
                    continue;
                }

                foreach (string profileDir in Directory.GetDirectories(steamAppsPath))
                {
                    string profileAppDir = profileDir + Path.DirectorySeparatorChar + cfgPath;
                    
                    if (!Directory.Exists(profileAppDir))
                    {
                        continue;
                    }

                    string destCfgFile = profileAppDir + Path.DirectorySeparatorChar + cfgFileName;

                    if (File.Exists(destCfgFile))
                    {
                        continue;
                    }

                    File.WriteAllBytes(destCfgFile, Properties.Resources.cfg1);
                }
            }
        }
    }
}
