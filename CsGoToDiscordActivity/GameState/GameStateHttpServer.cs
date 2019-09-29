using Discord.RPC;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace CsGoToDiscordActivity
{
    internal static class GameStateHttpServer
    {
        public static Thread thread;
        private static bool threadStop = false;

        public static string LastMessage;

        public static void MainLoop()
        {
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://*:21812/");

                listener.Start();
                while (!threadStop)
                {
                    var context = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                    context.AsyncWaitHandle.WaitOne(1000, true);
                }
                listener.Stop();
            }
        }

        public static void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;

                HttpListenerContext context = listener.EndGetContext(result);

                HttpListenerRequest request = context.Request;

                string streamContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        streamContents = readStream.ReadToEnd();
                    }
                }

                LastMessage = streamContents;
                PresenceManager.GameStateMessage(streamContents);

                HttpListenerResponse response = context.Response;
                string responseString = "OK";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            catch (HttpListenerException)
            {
            }
            catch (ObjectDisposedException)
            {
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
