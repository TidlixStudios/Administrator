using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands.Verify_System
{
    public class VerifyCommands : ApplicationCommandModule
    {
        [SlashCommand("CreateVerify", "Erstelle eine Nachricht, mit der sich nutzer Verifizieren können!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task createVerifyMessage(InteractionContext ctx) 
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            var embed = new DiscordEmbedBuilder()
            {
                Title = "Verifizierung:",
                Description = "**Um alle Kanäle freizuschalten, musst du zuerst die Verifizierung abschließen!**" +
                "\nLieß dazu bitte als erstes die Regeln durch!" +
                "\n\nUm die Verifizierung abzuschließen musst du folgende Schritte befolgen:" +
                "\n1. Drücke auf den grünen Knopf, unter dieser Nachricht!" +
                "\n2. Schau dir den Code an, welcher dir von dem Administrator Bot gesendet wurde!" +
                "\n3. Komm in diesen Channel hier zurück, und nutze den Befehl /Verify **<Dein Code>** um die Verifizierung abzuschließen!" +
                "\nBeachte dass du logischer weise das **<Dein Code>** gegen den Code, welchen du vom Bot erhalten hast, ersetzen musst!" +
                $"\n\nDas wars auch schon! Viel spaß auf {ctx.Guild.Name}",
                Color = DiscordColor.DarkGreen
            };
            var button = new DiscordButtonComponent(
                ButtonStyle.Success,
                "Verify_BTN",
                "Verifiziere dich!",
                false);

            var message = new DiscordMessageBuilder()
                .WithContent("")
                .AddEmbed(embed)
                .AddComponents(button);

            var sendedMessage = await ctx.Channel.SendMessageAsync(message);
            var Verify = new Verify();
        }



        [SlashCommand("Verify", "Verifiziere dich!")]

        public async Task Verify (InteractionContext ctx, [Option("Code", "Gebe den Code ein, welchen du von dem Bot erhalten hast!")] long Code)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            var Channel = ctx.Guild.GetChannel(1201115683895914536);

            var embed = new DiscordEmbedBuilder()
            {
                Title = ctx.User.Id.ToString(),
                Description = Code.ToString(),
            };
            await Channel.SendMessageAsync(embed);
        }
    }
}
