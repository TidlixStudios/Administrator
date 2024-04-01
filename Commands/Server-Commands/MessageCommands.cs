using Administrator.Config;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
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
        [SlashCommandPermissions(Permissions.Administrator)]
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
        [SlashCommandPermissions(Permissions.Administrator)]
        public async Task CreateNewsMessage(InteractionContext ctx)
        {
            string title = "Platzhalter Text";
            string description = "Platzhalter Text";

            var Modal = new DiscordInteractionResponseBuilder()
                .WithTitle("News")
                .WithCustomId("News_MDL")
                .AddComponents(new TextInputComponent(
                    label: "Überschrift",
                    customId: "news_title", 
                    placeholder: "z.B: Neuer Channel!", 
                    required: true, 
                    style: TextInputStyle.Short))
                .AddComponents(new TextInputComponent(
                    label: "Beschreibung",
                    customId: "news_description",
                    placeholder: "z.B: Ab sofort gibt es einen neuen Kanal auf diesem Server! ",
                    required: true,
                    style: TextInputStyle.Paragraph));

            await ctx.CreateResponseAsync(InteractionResponseType.Modal, Modal);

            var Interactivity = ctx.Client.GetInteractivity();
            var ModalResponse = Interactivity.WaitForModalAsync(modal_id: "News_MDL").Result;

            title = ModalResponse.Result.Values["news_title"];
            description = ModalResponse.Result.Values["news_description"];


            var Embed = new DiscordEmbedBuilder()
            {
                Title = title,
                Description = description,
                Color = DiscordColor.Yellow
            };
            await ctx.Channel.SendMessageAsync(Embed);
        }

        [SlashCommand("CreateReactionRoles", "Erstelle eine Nachricht, mit der sich Nutzer, verschiedene Rollen auswählen können")]
        [SlashCommandPermissions(Permissions.Administrator)]
        public async Task CreateReactionRoles (InteractionContext ctx)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            var Config = Program.reader;
            var Guild = await Program.Client.GetGuildAsync(ctx.Guild.Id);

            DiscordRole game = Guild.GetRole(Config.gameRoleID);
            DiscordRole dev = Guild.GetRole(Config.developerRoleID);
            DiscordRole ntf_yt = Guild.GetRole(Config.notifierRoleYoutubeID);
            DiscordRole ntf_st = Guild.GetRole(Config.notifierRoleStudiosID);
            DiscordRole ntf_tw = Guild.GetRole(Config.notifierRoleTwitchID);

            var Embed = new DiscordEmbedBuilder()
            {
                Title = "Rollenauswahl!",
                Description = "**Bitte wähle welche Rollen du haben möchtest!**" +
                "\nDrücke dazu einfach auf den dafür vorgesehenen Knopf!" +
                "\n\nFolgende Rollen stehen dir zur Auswahl:" +
                $"\n> {game.Mention} ==> Du magst Minigames? Dann wähle diese Rolle um Zugriff auf die Minigames dieses Servers zu erhalten!" +
                $"\n\n> {dev.Mention} ==> Du brauchst hilfe beim Programmieren, willst zeigen was du erschaffen hast, oder einfach über das Programmieren schreiben? dann wähle diese Rolle aus!" +
                $"\n\n> {ntf_yt.Mention} ==> Du möchtest Benachrichtigt werden, wenn ein neues Video auf dem Kanal Tidlix hochgeladen wurde? Dann wähle diese Rolle, um kein Video mehr zu verpassen!" + 
                $"\n\n> {ntf_st.Mention} ==> Du möchtest eine Benachrichtigung erhalten, wenn es neues Video auf dem Kanal TidlixStudios gibt? Mit dieser Rolle verpasst du keinen Stream mehr!" +
                $"\n\n> {ntf_tw.Mention} ==> Du möchtest sofort wissen, wenn Tidlix auf Twitch Live ist? Dann wähle diese Rolle, um bei jedem Stream dabei zu sein!" +
                $"\n\nUm eine Rolle wieder zu entfernen, drücke einfach ein zweites mal auf den Knopf, um die Rolle los zu werden!",
                Color = DiscordColor.SpringGreen
            };

            DiscordButtonComponent gameButton = new DiscordButtonComponent(
                label: $"{game.Name}",
                customId: "RR_Game_BTN",
                style: ButtonStyle.Secondary);
            DiscordButtonComponent devButton = new DiscordButtonComponent(
                label: $"{dev.Name}",
                customId: "RR_Dev_BTN",
                style: ButtonStyle.Secondary);
            DiscordButtonComponent ntfYtButton = new DiscordButtonComponent(
                label: $"{ntf_yt.Name}",
                customId: "RR_NtfYt_BTN",
                style: ButtonStyle.Secondary);
            DiscordButtonComponent ntfStButton = new DiscordButtonComponent(
                label: $"{ntf_st.Name}",
                customId: "RR_NtfSt_BTN",
                style: ButtonStyle.Secondary);
            DiscordButtonComponent ntfTwButton = new DiscordButtonComponent(
                label: $"{ntf_tw.Name}",
                customId: "RR_NtfTw_BTN",
                style: ButtonStyle.Secondary);



            DiscordMessageBuilder Message = new DiscordMessageBuilder()
                .AddEmbed(Embed)
                .AddComponents(
                    gameButton, 
                    devButton, 
                    ntfYtButton, 
                    ntfStButton, 
                    ntfTwButton);

            await ctx.Channel.SendMessageAsync(Message);
        }
    }
}
