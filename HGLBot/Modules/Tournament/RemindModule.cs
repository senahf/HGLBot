using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;

namespace HGLBot.Modules.Tournament
{

    internal class RemindModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        private readonly System.Timers.Timer t = new System.Timers.Timer { Interval = 2000 };
        public string file_get_contents(string fileName)
        {

            string sContents = string.Empty;
            if (fileName.ToLower().IndexOf("http:") > -1)
            {
                // URL 
                WebClient wc = new WebClient();
                wc.UseDefaultCredentials = false;
                byte[] response = wc.DownloadData(fileName);
                sContents = System.Text.Encoding.ASCII.GetString(response);

            }
            else {
                // Regular Filename 
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                sContents = sr.ReadToEnd();
                sr.Close();
            }
            return sContents;
        }

        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            _client.UserJoined += UserJoined;

            var client = manager.Client;
            //TrelloAuthorization.Default.UserToken = "[your user token]";

            //            Channel HGL = _client.GetServer(121658903728488448).GetChannel(121658903728488448);
            Channel HGL = null;

            List<string> last5ActionIDs = null;
            t.Elapsed += async (s, e) =>
            {
                try
                {
                    HGL = _client.GetServer(121658903728488448).GetChannel(174365963091705857);

                    if (HGL == null) // If HGL doesn't exist...
                        return;
                    if (File.ReadAllText(@"tournie.txt") == "1")
                    {
                        return;
                    }
                    string link = file_get_contents("http://aegyo.pro/HGL/api.php?verify=y&cmd=today");
                    if (link == "paid")
                    { }
                    else
                    {
                        if (DateTime.Now.TimeOfDay.Hours == 15)
                        {
                            await Task.Run(async () =>
                            {
                                var msgs = (await HGL.DownloadMessages(100).ConfigureAwait(false)).Where(m => m.User.Id == _client.CurrentUser.Id);
                                foreach (var m in msgs)
                                {
                                    try
                                    {
                                        await m.Delete().ConfigureAwait(false);
                                    }
                                    catch { }
                                    await Task.Delay(100).ConfigureAwait(false);
                                }
                                }).ConfigureAwait(false);

                            await HGL.SendMessage($"@everyone{Environment.NewLine}Tonight's HGL Daily Tournament starts in an hour! Sign up and check-in here{Environment.NewLine + link}");
                            File.WriteAllText("tournie.txt", "1");

                            Thread.Sleep(3600000);
                            File.WriteAllText("tournie.txt", "0");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Timer failed " + ex.ToString());
                }
            };

                manager.CreateCommands("", group =>
            {
                group.CreateCommand("start")
                .Do(async e => {
                    t.Start();
                    await e.Channel.SendMessage("Started!");
                });
            });
        }
        private async void UserJoined(object sender, UserEventArgs e)
        {
            /*if (e.Server.Id == 170293626927185921)
            {
                await _client.GetServer(170293626927185921).GetChannel(170293626927185921).SendMessage($"Welcome to #KShows, {e.User.Mention}!");
            }*/
        }
    }
}
