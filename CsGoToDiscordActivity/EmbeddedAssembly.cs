using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CsGoToDiscordActivity
{
    internal class EmbeddedAssembly
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        public static void Load()
        {
            string tempFile = Path.GetTempPath() + "discord-rpc.dll";

            File.WriteAllBytes(tempFile, Properties.Resources.discord_rpc);

            LoadLibrary(tempFile);
        }
    }
}
