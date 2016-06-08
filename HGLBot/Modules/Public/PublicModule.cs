using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.Net.Http;
using System.Net;
using System.Web;

namespace HGLBot.Modules.Public
{
    internal class PublicModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

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
            else
            {
                // Regular Filename 
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                sContents = sr.ReadToEnd();
                sr.Close();
            }
            return sContents;
        }

        public static string GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are 
                result[1] = strSource;
            return result[0];
        }
        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;
            

            manager.CreateCommands("", group =>
            {
                group.MinPermissions((int)PermissionLevel.User);

                group.CreateCommand("leave")
                    .Description("Instructs the bot to leave this server.")
                    .MinPermissions((int)PermissionLevel.ServerModerator)
                    .Do(async e =>
                    {
                        await _client.Reply(e, $"Leaving~");
                        await e.Server.Leave();
                    });
                group.CreateCommand("sid")
                .Description("Gets the Server Id")
                .Do(async e =>
                {
                    await e.Channel.SendMessage(e.Server.Id.ToString());
                });
                group.CreateCommand("cid")
                .Description("Gets the Channel Id")
                .Do(async e =>
                {
                    await e.Channel.SendMessage(e.Channel.Id.ToString());
                });
                group.CreateCommand("top3")
                .Description("Gets the rankings")
                .Do(async e =>
                {
                    String rawr = file_get_contents("http://aegyo.pro/HGL/api.php?verify=y&cmd=top3");
                    if (rawr.Contains("Invalid Battletag"))
                    {
                        await e.Channel.SendMessage($"```xl\r{rawr}```");
                        return;
                    }
                   await e.Channel.SendMessage($"```xl\r{rawr}```");
                });
                group.CreateCommand("rank")
                .Parameter("battletag", ParameterType.Required)
                .Description("Gets the rankings")
                .Do(async e =>
                {
                    try
                    {
                        String name = e.GetArg(0).ToLower();
                        if (!e.Message.ToString().Contains("#"))
                        { await e.Channel.SendMessage("Remember that you must include your Battletag #!"); return; }
                        String rawr = file_get_contents("http://aegyo.pro/HGL/api.php?verify=y&cmd=rank");
                        string output = GetStringInBetween($"[{name}]", $"[/{name}]", rawr, false, false);
                        String output2 = file_get_contents("http://aegyo.pro/HGL/api.php?verify=y&cmd=points&name="+HttpUtility.UrlEncode(e.GetArg(0)));

                        await e.Channel.SendMessage($"{e.GetArg(0)} is currently in {output} place with {output2} points!");
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
                group.CreateCommand("?")
                    .Alias("!")
                    .Description("Ping/Pong to see if bot is live")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage("Rawr!");
                    });
            });
        }
    }
}
