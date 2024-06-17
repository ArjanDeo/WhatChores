using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Models.RaiderIO.MythicPlus;
using Pathoschild.Http.Client;

namespace DiscordBot.Commands.MythicPlus
{
    public class MythicPlusCommands : ApplicationCommandModule
    {
        private readonly FluentClient client;
        public MythicPlusCommands() 
        {
            client = new();
        }
        static DateTime GetLastTuesday(DateTime currentDate)
        {
            int daysToSubtract = (int)currentDate.DayOfWeek - (int)DayOfWeek.Tuesday;
            return currentDate.AddDays(-daysToSubtract).Date;
        }

        [SlashCommand("affixes", "Get the current M+ affixes for the week.")]
        public async Task AffixesCommand(InteractionContext ctx) 
        {            
            IResponse Response = await client
            .GetAsync("https://raider.io/api/v1/mythic-plus/affixes")
            .WithArgument("region", "us");

            RaiderIOMythicPlusAffixesModel ResponseData = await Response.As<RaiderIOMythicPlusAffixesModel>();

            DiscordEmbedBuilder embed = new() { Footer = new() };

            embed.Footer.Text = "Credit: Raider.IO API";
            embed.Footer.IconUrl = "https://cdnassets.raider.io/images/brand/Icon_FullColor_Square.png";
            embed.Color = DiscordColor.Magenta;
            embed.Title = $"Affixes for the week starting {GetLastTuesday(DateTime.Now):MM/dd/yyyy}";
            foreach (var affix in ResponseData.affix_details)
            {
                embed.AddField(affix.name, affix.description, true);
            }
            await ctx.CreateResponseAsync(embed.Build());
        }
    }
}
