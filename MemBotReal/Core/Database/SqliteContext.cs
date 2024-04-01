﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MemBotReal.Database;

public class SqliteContext : BotDbContext
{
    public SqliteContext(string connStr = "Data Source=data/botDb.db") : base(connStr)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new SqliteConnectionStringBuilder(connectionString);
        builder.DataSource = Path.Combine(AppContext.BaseDirectory, builder.DataSource);
        optionsBuilder.UseSqlite(builder.ToString());
    }
}