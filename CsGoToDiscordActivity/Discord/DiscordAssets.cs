using System.Net;
using System.Web.Script.Serialization;

namespace CsGoToDiscordActivity
{
    internal static class DiscordAssets
    {
        private const string placeholder = "random";

        private static dynamic assetList;
        public static void Init(string clientId)
        {
            using (WebClient client = new WebClient())
            {
                string assetsJson = client.DownloadString("https://discordapp.com/api/oauth2/applications/" + clientId + "/assets");
                assetList = new JavaScriptSerializer().Deserialize<dynamic>(assetsJson);
            }
        }

        public static string AssetForGameString(string name)
        {
            return AssetExists(name) ? name : placeholder;
        }

        public static bool AssetExists(string name)
        {
            foreach (dynamic asset in assetList)
            {
                if (asset["name"] == name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
