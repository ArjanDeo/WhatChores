using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Models.RaiderIO.Character;
using Pathoschild.Http.Client;

namespace DiscordBot.Commands.Character
{
    public class CharacterCommands : ApplicationCommandModule
    {
        public FluentClient client;
        public CharacterCommands()
        {
            client = new();
        }
        [SlashCommand("characterlookup", "Lookup a specific character")]
        public async Task CharacterLookupCommand(InteractionContext ctx, 
        [Option("name", "Name of the character to lookup")] string name, 
        [Choice("us", "us")]
        [Choice("kr", "kr")]
        [Choice("eu", "eu")]
        [Choice("tw", "tw")]
        [Option("region", "The region the character's on")] string region,
        [Option("realm", "The realm the character's on")] string realm)
        {
            IResponse Response = await client
                .GetAsync("https://raider.io/api/v1/characters/profile")
                .WithArgument("region", region)
                .WithArgument("name", name)
                .WithArgument("realm", realm.Replace(" ", "-"))
                .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs,guild,mythic_plus_scores_by_season:current,gear");

            RaiderIOCharacterDataModel response = await Response.As<RaiderIOCharacterDataModel>();

            DiscordEmbedBuilder embed = new() { Footer = new(), Thumbnail = new()};

            embed.Footer.Text = "Credit: Raider.IO API";
            embed.Footer.IconUrl = "https://cdnassets.raider.io/images/brand/Icon_FullColor_Square.png";
            embed.Color = DiscordColor.Magenta;
            embed.Url = response.profile_url;
            embed.AddField("M+ Score", response.mythic_plus_scores_by_season[0].scores.all.ToString());
            embed.AddField("Vault of the Incarnates", response.raid_progression.vaultoftheincarnates.summary, true);
            embed.AddField("Aberrus, the Shadowed Crucible", response.raid_progression.aberrustheshadowedcrucible.summary, true);
            embed.AddField("Amirdrassil, the Dream's Hope", response.raid_progression.amirdrassilthedreamshope.summary, true);
            embed.Thumbnail.Url = response.thumbnail_url;
            embed.ImageUrl = $"https://cdnassets.raider.io/images/profile/masthead_backdrops/v2/{response.profile_banner}.jpg";
            embed.Description = $"[WoW Armory](https://worldofwarcraft.blizzard.com/en-us/character/{response.region}/{response.realm}/{response.name}) | [Warcraft Logs](https://www.warcraftlogs.com/character/{response.region}/{response.realm}/{response.name}) | [Raider.io](https://raider.io/characters/{response.region}/{response.realm}/{response.name})\nClass/Specialization: {response.active_spec_name} {response.char_class}\nRealm: {response.realm}";
            embed.Title = $"{response.name} <{response.guild.name}>";
            await ctx.CreateResponseAsync(embed.Build());
        }
    }
}
