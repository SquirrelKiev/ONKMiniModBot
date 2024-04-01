using BotBase.Modules.ConfigCommand;
using MemBotReal.Modules.ConfigCommand.Pages;

namespace MemBotReal.Modules.ConfigCommand;

public class ConfigCommandService : ConfigCommandServiceBase<ConfigPage.Page>
{
    public ConfigCommandService(IServiceProvider services) : base(services)
    {
    }
}