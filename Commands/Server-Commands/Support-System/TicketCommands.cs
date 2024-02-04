using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands.Server_Commands.Support_System
{
    public class TicketCommands : ApplicationCommandModule
    {
        [SlashCommand("CreateTicketMessage", "Erstelle eine neue Nachricht, mit der man ein Ticket erstellen kann!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task CreateTicketMessage(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            // Embed
            var embed = new DiscordEmbedBuilder()
            {
                Title = "Tickets!",
                Description = "Um ein Ticket zu erstellen, drücke auf den grünen Knopf **\"Ticket erstellen...\"**!" +
                "\nWähle dann aus, um was für ein Ticket es sich handelt, und beschreibe dein Anliegen so genau wie möglich!" +
                "\n\n**__Ticket möglichkeiten:__**" +
                "\n**:question: Support** => Dieses Ticket ist für den allgemeinen Support, wenn du Fragen, Probleme, etc. hast!" +
                "\n**:exclamation: Report** => Dieses Ticket ist dafür da, um andere Nutzer auf grund ihres Verhaltens innerhalb und außerhalb des Servers zu melden!" +
                "\n**:mailbox: Bewerbung** => Wenn du dich für etwas Bewerben möchtest, nutze dieses Ticket!",
                Color = DiscordColor.MidnightBlue
            };

            // Button 
            var button = new DiscordButtonComponent(
                DSharpPlus.ButtonStyle.Success,
                "Sup_Ticket_create_BTN",
                "Ticket erstellen..."
            );

            // Message
            var message = new DiscordMessageBuilder()
                .AddEmbed(embed)
                .AddComponents(button);

            // Send Message
            await ctx.Channel.SendMessageAsync(message);
        }


        [SlashCommandGroup("Support", "Befehle für Support Tickets")]
        [SlashCommandPermissions(DSharpPlus.Permissions.ModerateMembers)]
        public class SupportTickets()
        {
            [SlashCommand("Close", "Schließe das Aktuelle Ticket")]
            public async Task CloseTicket(InteractionContext ctx)
            {
                await ctx.DeferAsync();

                if (ctx.Channel.Topic != "Support-Ticket")
                {
                    var error = new DiscordEmbedBuilder()
                    {
                        Title = "Error",
                        Description = "Dieser Befehl geht nur für Support-Tickets!",
                        Color = DiscordColor.Red
                    };
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(error));
                    return;
                }

                var response = new DiscordEmbedBuilder()
                {
                    Title = "Ticket System!",
                    Description = "Dieses Ticket wird in wenigen Sekunden geschlossen!",
                    Color = DiscordColor.IndianRed
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));
                await Task.Delay(30000);

                var logMessage = new DiscordEmbedBuilder()
                {
                    Title = "Ticket geschlossen!",
                    Description = $"{ctx.User.Mention} hat das Ticket {ctx.Channel.Name} geschlossen!" +
                    $"\n Zeit: {DateTime.Now}"
                };
                await ctx.Channel.DeleteAsync();

                var LogChannel = ctx.Guild.GetChannel(1188947815569817683);
                await LogChannel.SendMessageAsync(logMessage);
            }
        }
    }
}
