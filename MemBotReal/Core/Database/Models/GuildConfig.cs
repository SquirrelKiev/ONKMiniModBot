using System.ComponentModel.DataAnnotations;

namespace MemBotReal.Database.Models;

public class GuildConfig
{
    [Key]
    public required ulong GuildId { get; set; }

    public ulong MuteRole { get; set; } = 0ul;
    public ulong ModRole { get; set; } = 0ul;
    public ulong LoggingChannel { get; set; } = 0ul;
    public List<ulong> CasesAllowedChannels { get; set; } = [];
}