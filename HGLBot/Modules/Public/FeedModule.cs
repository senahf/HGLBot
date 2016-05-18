using Discord;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using CloudFlareUtilities;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System;

namespace HGLBot.Modules.Drama
{
    internal class FeedModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", group =>
            {
                group.MinPermissions((int)PermissionLevel.User);

                group.CreateCommand("test")
                .Do(async e =>
                {
                    try
                    {
                        var handler = new ClearanceHandler();
                        var client = new HttpClient(handler);
                        client.Timeout = TimeSpan.FromSeconds(8);
                        File.AppendAllText("kissasian.html", await client.GetStringAsync("http://kissasian.com/"));
                        /*HtmlDocument doc = new HtmlDocument();
                        doc.Load("kissasian.html");
                        var root = doc.DocumentNode;
                        var a = root.Descendants()
                            .Where(n => n.GetAttributeValue("class", "").Equals("barContent"))
                            .Single()
                            .Descendants("a")
                            .Single();
                        var content = a.InnerText;*/
                        HtmlDocument doc = new HtmlDocument();
                        doc.Load("kissasian.html");
                        var root = doc.DocumentNode;
                        var a_nodes = root.Descendants("a").ToList();
                        
                        foreach (var a_node in a_nodes)
                        {
                            Console.WriteLine("TEXT: {0}", a_node.InnerHtml);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            });
        }
    }
}
