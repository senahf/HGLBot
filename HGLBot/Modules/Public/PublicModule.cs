using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using CloudFlareUtilities;
using System.Net.Http;

namespace HGLBot.Modules.Public
{
    internal class PublicModule : IModule
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
