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

namespace HGLBot.Modules.Public
{

    internal class GreetModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            _client.UserJoined += UserJoined;

            manager.CreateCommands("", group =>
            {
                
            });
        }
        private async void UserJoined(object sender, UserEventArgs e)
        {
            if (e.Server.Id == 170293626927185921)
            {
                await _client.GetServer(170293626927185921).GetChannel(170293626927185921).SendMessage($"Welcome to #KShows, {e.User.Mention}!");
            }
        }
    }
}
