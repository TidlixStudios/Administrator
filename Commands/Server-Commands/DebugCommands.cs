using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands
{
    public class DebugCommands : ApplicationCommandModule
    {
        [SlashCommand("Reload", "Reload all Commands")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task Reload (InteractionContext ctx)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            await Program.Client.UseSlashCommands().RefreshCommands();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[DEBUG!] Commands reloaded!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        [SlashCommand("Shutdown", "Stop the Bot!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task Shutdown(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"BOT SHUTDOWN! (started by {ctx.User.Username})");

            var response = new DiscordEmbedBuilder()
            {
                Title = "Shutdown!",
                Description = "Der Bot wird nun ausgeschalten!" +
                $"\n(aktiviert durch: {ctx.Member.Mention})",
                Color = DiscordColor.DarkRed
            };
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));

            await Program.Client.DisconnectAsync();
            Environment.Exit(0);
        }
    }
}
