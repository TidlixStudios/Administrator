using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands.Server_Commands
{
    public class MessageCommands : ApplicationCommandModule
    {
        [SlashCommand("CreateRuleMessage", "Erstelle eine Nachricht mit allen Regeln!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task CreateRuleMessage(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            string RuleDescription = "**Die folgenden Regeln müssen immer eingehalten werden!**" +
                "\n\n> **1. Diskriminierung**" +
                "\n> Jede Form von Diskriminierung wird nicht gedultet! Auf diesem Server wird jeder gleich behandelt!" +
                "\n\n> **2. Beleidigungen**" +
                "\n> Beleidigungen sind hier strengstens Verboten! Darunter zählen natürlich Worte, welche eine oder mehrere Personen verletzen könnten, aber auch Emojis, Sticker, GIFs, etc. welche das gleiche bewirken könnten" +
                "\n\n> **3. Spam**" +
                "\n> Spam ist einfach nur nervig und wird deshalb ungern gesehen. Darunter zählen viele Nachrichten in einer bestimmten Zeit oder einfach viele Zeichen in einer Nachricht!" +
                "\n\n> **4. Verbreitung von NSFW Kontent**" +
                "\n> NSFW ist hier Tabu! Darunter zählen Pornografische Inhalte und Inhalte, welche von Personen als verstörend angesehen werden könnten!" +
                "\n\n> **5. Werbung**" +
                "\n> Werbung in jeder Art ist Verboten! Darunter zählt sowohl Eigenwerbung, aber auch Werbung für andere." +
                "\n\n> **6. Persönliche Daten**" +
                "\n> Persönliche Daten dürfen hier nicht geteilt werden. Darunter zählen Telefon Nummern, Adressen, Private Chats, Kontodaten, usw." +
                "\n\n> **7. Verbreitung gefährlicher Dateien**" +
                "\n> Das Verbreiten gefährlicher Daten wie Mailware, IP-Logger, etc. ist strengstens Verboten!" +
                "\n\n> **8. Banumgehung**" +
                "\n> Einen Ban/Mute in jeglicher Form zu umgehen (z.B.: Zweitaccount) ist Verboten!" +
                "\n\n- Ein Verstoß dieser Regeln wird sofort mit einem Timeout/Ban bestraft!" +
                "\n__Hinweis:__ Alle dieser Regeln gelten auch, wenn ihr Nutzern von diesem Server per Privatnachricht schreibt!";


            var Rules = new DiscordEmbedBuilder()
            {
                Title = "Regeln!",
                Description = RuleDescription,
                Color = DiscordColor.Magenta,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"Letze Aktualisierung: {DateTime.Now}"
                }
            };
            await ctx.Channel.SendMessageAsync(Rules);
        }

        [SlashCommand("News", "Erstelle eine Nachricht mit Neulichkeiten!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task CreateNewsMessage(InteractionContext ctx, [Option("Text", "Was soll die Nachricht sein?")] string text)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            var Embed = new DiscordEmbedBuilder()
            {
                Title = "News!",
                Description = text,
                Color = DiscordColor.Yellow
            };
            await ctx.Channel.SendMessageAsync(Embed);
        }
    }
}
