using BotBase;
using BotBase.Modules;
using BotBase.Modules.ConfigCommand;
using Discord;
using Discord.Interactions;
using MemBotReal.Modules.ConfigCommand.Pages;

namespace MemBotReal.Modules.ConfigCommand;

public class ConfigCommandModule : ConfigCommandModuleBase<ConfigPage.Page>
{
    public ConfigCommandModule(ConfigCommandServiceBase<ConfigPage.Page> configService) : base(configService)
    {
    }

    [SlashCommand("basically-just-prefix", "I'm lazy so you set the prefix here")]
    [RequireUserPermission(GuildPermission.ManageGuild, Group = BaseModulePrefixes.PERMISSION_GROUP)]
    [RequireContext(ContextType.DM | ContextType.Group, Group = BaseModulePrefixes.PERMISSION_GROUP)]
    [HasOverride(Group = BaseModulePrefixes.PERMISSION_GROUP)]
    public override Task ConfigSlash()
    {
        return base.ConfigSlash();
    }
}