using Discord.RPC;
using System;
using System.Windows.Forms;

namespace CsGoToDiscordActivity
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            EmbeddedAssembly.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GameConfig.AddCfgFile();

            GameProcessWatcher.Spawn();
            GameStateHttpServer.Spawn();

            Application.Run(new GameStateUi());

            GameStateHttpServer.End();
            GameProcessWatcher.End();
            DiscordRPC.Shutdown();
        }
    }
}
