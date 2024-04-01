using BotBase.Database;

namespace MemBotReal.Database.Models;

public class CustomCommand : DbModel
{
    public required ulong GuildId { get; set; }
    public required ulong OwnerId { get; set; }
    public required string Name { get; set; }
    public required string Contents { get; set; }
}