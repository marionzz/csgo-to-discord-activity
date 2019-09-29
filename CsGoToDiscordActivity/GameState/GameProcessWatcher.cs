using System;
using System.Diagnostics;
using System.Threading;

namespace CsGoToDiscordActivity
{
    internal class GameProcessWatcher
    {
        public static Thread thread;
        private static bool threadStop = false;

        public static volatile bool gameRunning = false;

        public static void MainLoop()
        {
            while (!threadStop)
            {
                bool isGameRunning = Process.GetProcessesByName("csgo").Length > 0;

                if (gameRunning != isGameRunning)
                {
                    PresenceManager.ProcessRunningStatusUpdated(isGameRunning);
                    gameRunning = isGameRunning;
                }

                Thread.Sleep(1000);
            }
        }


        public static void Spawn()
        {
            if (thread != null && thread.IsAlive)
            {
                return;
            }

            threadStop = false;
            thread = new Thread(MainLoop);
            thread.Start();
        }

        public static void End()
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = null;
                return;
            }

            threadStop = true;
        }
    }
}
