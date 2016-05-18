using System;
using System.Threading.Tasks;
using Discord.Modules;
using Discord.Commands;
using Discord.Audio;
using Discord;
using HGLBot.Services;
using HGLBot.Modules.Public;
using HGLBot.Modules.Drama;
using HGLBot.Modules.Admin;
using Discord.Commands.Permissions.Levels;
using System.Threading;
using System.IO;

namespace HGLBot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().Start(args);

        private const string AppName = "HGLBot";
        private const string AppUrl = "https://github.com/senahf/HGLBot";
        private const string AppVer = "0.1";

        public static DiscordClient _client;

        private void Start(string[] args)
        {
            Console.WriteLine($"Welcome to {AppName} ({AppVer})!");
            /* Song Joong Ki*/
            GlobalSettings.Load();
            _client = new DiscordClient(x =>
            {
                x.AppName = AppName;
                x.AppUrl = AppUrl;
                x.MessageCacheSize = 0;
                x.UsePermissionsCache = true;
                x.EnablePreUpdateEvents = true;
            })
            .UsingCommands(x =>
             {
                 x.AllowMentionPrefix = false;
                 x.PrefixChar = Convert.ToChar("!");
                 x.HelpMode = HelpMode.Public;
                 // x.ExecuteHandler
                 // x.ErrorHandler [ChatterBotAPI? (aka CleverBot for Invalid commands)]
             })
            .UsingModules()
            .UsingPermissionLevels(PermissionResolver)
            .UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
                x.EnableMultiserver = true;
                x.EnableEncryption = true;
                x.Bitrate = AudioServiceConfig.MaxBitrate;
                x.BufferLength = 10000;
            });
            _client.AddModule<ServerModule>("Server", ModuleFilter.None);
            _client.AddModule<PublicModule>("Public", ModuleFilter.None);
            _client.AddModule<GreetModule>("Greet", ModuleFilter.None);

            _client.ExecuteAndWait(async () =>
             {
                 while (true)
                 {
                     try
                     {
                         await _client.Connect(GlobalSettings.Discord.Token); // await _client.Connect(GlobalSettings.Discord.User, GlobalSettings.Discord.Pass);
                         _client.SetGame("HearthGamingLeague");
                         Console.WriteLine("HGL Bot has been initialized!");
                         break;
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine($"Login Failed {ex}");
                         await Task.Delay(_client.Config.FailedReconnectDelay);
                     }
                 }
             });
        }
        private static async void Client_MessageReceived(object sender, MessageEventArgs e)
        {
        }

        private int PermissionResolver(User user, Channel channel)
        {
            if (user.Id == GlobalSettings.users.owner || user.Id == GlobalSettings.users.owner)
                return (int)PermissionLevel.BotOwner;
            if (user.Server != null)
            {
                if (user == channel.Server.Owner)
                    return (int)PermissionLevel.ServerOwner;

                var serverPerms = user.ServerPermissions;
                if (serverPerms.ManageRoles)
                    return (int)PermissionLevel.ServerAdmin;
                if (serverPerms.ManageMessages && serverPerms.KickMembers && serverPerms.BanMembers)
                    return (int)PermissionLevel.ServerModerator;

                var channelPerms = user.GetPermissions(channel);
                if (channelPerms.ManagePermissions)
                    return (int)PermissionLevel.ChannelAdmin;
                if (channelPerms.ManageMessages)
                    return (int)PermissionLevel.ChannelModerator;
            }
            return (int)PermissionLevel.User;
        }
    }
}
