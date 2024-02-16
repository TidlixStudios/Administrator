using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands.GameCommands
{
    public class CountingCommands : ApplicationCommandModule
    {
        [SlashCommand("Counting_Regeln", "Erhalte die Regeln von Counting")]
        public async Task Counting_Rules (InteractionContext ctx)
        {
            await ctx.DeferAsync();
            var Rules = new DiscordEmbedBuilder()
            {
                Title = "Counting Regeln!",
                Description = "Die Regeln von Counting sind simpel:" +
                "\n- Zählt als Team so weit wie möglich" +
                "\n- Zählt in der richtigen Reihenfolge" +
                "\n- Du darfst nicht 2 mal Zählen",
                Color = DiscordColor.Azure
            };
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(Rules));
        }
    }
}
