﻿using BotBase;
using Discord;
using Discord.WebSocket;
using LinqToDB.EntityFrameworkCore;
using MemBotReal.Database;
using MemBotReal.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MemBotReal.Modules.Mute;

[Inject(ServiceLifetime.Singleton)]
public class DelayedExecuteService(DiscordSocketClient client, MuteService muteService, DbService dbService)
{
    public Task? timerTask;

    public void StartBackgroundTask()
    {
        timerTask ??= Task.Run(TimerTask);
    }

    /// <remarks>Should only be one of these running!</remarks>
    private async Task TimerTask()
    {
        Log.Debug("away we go");
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        while (await timer.WaitForNextTickAsync())
        {
            try
            {
                await OnFinishWaitingForTick();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Unhandled exception in TimerTask! {message}", ex.Message);
            }
        }
    }

    private async Task OnFinishWaitingForTick()
    {
        var now = DateTimeOffset.UtcNow;

        await using var context = dbService.GetDbContext();
        var mutes = await GetUnmutesBeforeTime(context, now);

        if (mutes.Count == 0)
        {
            return;
        }

        foreach (var mute in mutes)
        {
            var guild = client.GetGuild(mute.GuildId);

            var offender = guild.GetUser(mute.OffenderId);

            if (offender == null)
            {
                continue;
            }

            await muteService.UnmuteUser(context, await ((IGuild)offender.Guild).GetCurrentUserAsync(), offender, "Auto");

            context.Remove(mute);
        }

        Log.Debug("{count} actions", mutes.Count);


        await context.SaveChangesAsync();
    }

    private async Task<List<DelayedUnmute>> GetUnmutesBeforeTime(BotDbContext context, DateTimeOffset time)
    {


        // harold
        return await context.DelayedUnmutes.ToLinqToDBTable()
            .Where(x => x.UnmuteWhen < time).ToListAsyncLinqToDB();
    }
}