﻿using DiscordBot.Commands.Character;
using DiscordBot.Commands.MythicPlus;
using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace DiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = "MTE5ODU2MjE0NjA5OTkyNTA2Mw.G-92px.nNkDR3k0Vyd0zSgGMm_DC1aOdpryGcwRHXNTIk",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });
           
            var slash = discord.UseSlashCommands();

            slash.RegisterCommands<MythicPlusCommands>(856305487615885332);
            slash.RegisterCommands<CharacterCommands>(856305487615885332);
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
