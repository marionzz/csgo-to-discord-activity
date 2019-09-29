using Discord.RPC;
using System;
using System.Web.Script.Serialization;

namespace CsGoToDiscordActivity
{
    internal static class PresenceManager
    {
        private const string discordAppId = "623138508722143232";
        private static bool rpcInitialized = false;
        private static int startAt = 0;
        private static bool matchStarted = false;
        private static int matchStartAt = 0;

        public static void ProcessRunningStatusUpdated(bool isGameRunning)
        {
            if (!isGameRunning)
            {
                DiscordRPC.Shutdown();
                rpcInitialized = false;
            }

        }

        public static void GameStateMessage(string csgoDataJson)
        {
            Console.WriteLine(csgoDataJson);

            var csgoData = new JavaScriptSerializer().Deserialize<dynamic>(csgoDataJson);
            string steamId = csgoData["provider"]["steamid"];

            if (!rpcInitialized)
            {
                rpcInitialized = true;
                var eventHandler = new DiscordEventHandler();
                DiscordRPC.Init(discordAppId, ref eventHandler, true, steamId);
                startAt = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                DiscordAssets.Init(discordAppId);
            }

            DiscordPresence presence = FormatPresence(csgoData);

            DiscordRPC.UpdatePresence(ref presence);
        }


        private static DiscordPresence FormatPresence(dynamic csgoData)
        {
            DiscordPresence presence = new DiscordPresence
            {
                startTimestamp = startAt
            };
            string map = "";
            string tScore = "0";
            string ctScore = "0";
            if (!csgoData.ContainsKey("player") || !csgoData["player"].ContainsKey("activity"))
            {
                presence.state = "?";
                presence.largeImageKey = "menu";
            }
            else
            {
                string activity = csgoData["player"]["activity"];
                presence.state = activity;
                presence.largeImageKey = "menu";
                if (csgoData.ContainsKey("map"))
                {
                    if (csgoData["map"].ContainsKey("mode"))
                    {
                        presence.smallImageKey = DiscordAssets.AssetForGameString(csgoData["map"]["mode"]);
                        string mode = csgoData["map"]["mode"];
                        presence.smallImageText = "Mode: " + csgoData["map"]["mode"];

                        presence.state = mode + " - " + activity;
                    }

                    if (csgoData["map"].ContainsKey("name"))
                    {
                        presence.largeImageKey = DiscordAssets.AssetForGameString(csgoData["map"]["name"]);
                        map = csgoData["map"]["name"];
                        presence.largeImageText = "Map: " + map;
                    }

                    if (csgoData["map"].ContainsKey("phase"))
                    {
                        if ("warmup" == csgoData["map"]["phase"])
                        {
                            presence.details = "Warmup";
                        }
                        else
                        {
                            if (csgoData.ContainsKey("player") && csgoData["player"].ContainsKey("team"))
                            {
                                if (csgoData["map"].ContainsKey("team_t") && csgoData["map"]["team_t"].ContainsKey("score"))
                                {
                                    tScore = csgoData["map"]["team_t"]["score"].ToString();
                                }
                                if (csgoData["map"].ContainsKey("team_ct") && csgoData["map"]["team_ct"].ContainsKey("score"))
                                {
                                    ctScore = csgoData["map"]["team_ct"]["score"].ToString();
                                }

                                if (tScore == "0" && ctScore == "0")
                                {
                                    if (!matchStarted)
                                    {
                                        matchStarted = true;
                                        matchStartAt = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                                    }
                                }
                                else
                                {
                                    matchStarted = false;
                                }

                                string teamName = csgoData["player"]["team"];
                                string displayScore;
                                if ("T" == csgoData["player"]["team"])
                                {
                                    displayScore = "[ " + tScore + " : " + ctScore + " ]";
                                }
                                else if ("CT" == csgoData["player"]["team"])
                                {
                                    displayScore = "[ " + ctScore + " : " + tScore + " ]";
                                }
                                else
                                {
                                    displayScore = "[ T:" + tScore + " , CT:" + ctScore + " ]";
                                }

                                presence.details = displayScore + " " + map + " (" + teamName + ")";
                                presence.startTimestamp = matchStartAt != 0 ? matchStartAt : startAt;
                            }
                            else
                            {
                                presence.details = map;
                            }
                        }
                    }
                }
                else if ("menu" == csgoData["player"]["activity"])
                {
                    presence.state = "In Main Menu";
                    presence.largeImageKey = "menu";
                    presence.largeImageText = "Main Menu";
                    if (csgoData.ContainsKey("player") && csgoData["player"].ContainsKey("name"))
                    {
                        //presence.details = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(csgoData["player"]["name"]));
                    }
                }
            }

            return presence;
        }
    }
}
