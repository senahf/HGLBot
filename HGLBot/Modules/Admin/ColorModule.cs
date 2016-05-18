using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;

namespace HGLBot.Modules.Admin
{

    internal class ColorModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", group =>
            {
                group.MinPermissions((int)PermissionLevel.ServerAdmin);

                group.CreateCommand("color")
                    .Parameter("role")
                    .Parameter("color")
                    .MinPermissions((int)PermissionLevel.ServerAdmin)
                    .Description("Sets a role's color to another (HTML Code)")
                    .Do(e =>
                    {
                        return SetColor(e, e.Args[0], e.Args[1]);
                    });
            });
        }
        private async Task SetColor(CommandEventArgs e, String rolename, String colorName)
        {
            if (!e.Server.CurrentUser.ServerPermissions.ManageRoles)
            {
                await _client.ReplyError(e, "This command requires the bot have Manage Roles permission.");
                return;
            }
            Role role = e.Server.Roles.Where(x => x.Name.Equals(rolename, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (role == null)
            {
                await _client.Reply(e, $"The role `{rolename}` does not exist!");
            }
            Color rawr = new Color(Convert.ToUInt32(colorName, 16));
            await role.Edit(color: rawr);
            await _client.Reply(e, $"Set `{role.Name}`'s color to `#{colorName}`");
        }
    }
}
