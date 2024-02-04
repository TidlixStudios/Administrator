using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands
{
    public class ModerationCommands : ApplicationCommandModule
    {
        // ----------  BAN SYSTEM  ----------
        [SlashCommand("Ban", "Sperrt einen Nutzer dauerhaft!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.BanMembers)]
        public async Task Ban(InteractionContext ctx,
            [Option("User", "Wähle einen Nutzer, welchen du bannen willst!")] DiscordUser TargetUser,
            [Option("Reason", "Aus welchem Grund möchtest du diesen Nutzer bannen?")] string reason = "kein genannter Grund!")
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();
            DiscordMember TargetMember = (DiscordMember)TargetUser;

            // Send Message to Target
            var DMChannel = await TargetMember.CreateDmChannelAsync();
            var BanMessage = new DiscordEmbedBuilder()
            {
                Title = "Du wurdest gebannt!",
                Description = $"Du wurdest dauerhaft von dem Server {ctx.Guild.Name} gesperrt!" +
                $"\nGrund: {reason}",
                Color = DiscordColor.IndianRed
            };
            await DMChannel.SendMessageAsync(BanMessage);

            // Ban Target
            await ctx.Guild.BanMemberAsync(TargetMember, 7, reason);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Ban!",
                Description = $"{TargetUser.Mention} wurde gebannt! \n" +
                $"Zeit: {DateTime.Now}\n" +
                $"Grund: {reason}\n" +
                $"Gebannt von: {ctx.User.Mention}\n",
                Color = DiscordColor.Red
            };
            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }

        [SlashCommand("Banlist", "Erhalte eine Lister, aller gebannten Nutzer")]
        [SlashCommandPermissions(DSharpPlus.Permissions.BanMembers)]
        public async Task Banlist(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            var banList = await ctx.Guild.GetBansAsync();
            string[] responseArray = new string[banList.Count];

            int i = 0;
            foreach (DiscordBan ban in banList)
            {
                string message = $"**__Ban nr. {i+1}:__** {ban.User.Username} ist gebannt wegen: '{ban.Reason}'! ID: {ban.User.Id} \n";
                responseArray[i] = message;
                i++;
            }

            string description = "";
            foreach (string m in responseArray)
            {
                description = description + m;
            }

            var response = new DiscordEmbedBuilder()
            {
                Title = "gebannte Nutzer:",
                Description = description,
                Color = DiscordColor.IndianRed
            };
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));
            
        }

        [SlashCommand("Unban", "Entsperrt einen gebannten Nutzer!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.BanMembers)]
        public async Task Unban(InteractionContext ctx,
            [Option("UserID", "Gebe die User ID von der gebannten Person ein! (/banlist)")] string TargetIDString)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            // Get User
            var banlist = await ctx.Guild.GetBansAsync();
            DiscordUser user = null;
            ulong.TryParse(TargetIDString, out ulong TargetID);
            foreach (DiscordBan ban in banlist)
            {
                if (ban.User.Id == TargetID) { user = ban.User; }
            }
            if (user == null)
            {
                var errorResponse = new DiscordEmbedBuilder()
                {
                    Title = "Error!",
                    Description = $"Der Nutzer mit der ID {TargetID} wurde nicht gefunden!" +
                    $"\n Bitte überprüfe, ob die ID in der Ban-Liste existiert! (/Banlist)",
                    Color = DiscordColor.Red
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(errorResponse));
                return;
            } // User not found Error

            // Unban User   
            await ctx.Guild.UnbanMemberAsync(user);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Aufgehobener Ban!",
                Description = $"{user.Mention} wurde entbannt! \n" +
                $"Zeit: {DateTime.Now}\n" +
                $"Entbannt von: {ctx.User.Mention}\n",
                Color = DiscordColor.Azure
            };
            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }
        // ----------  BAN SYSTEM  ----------


        // ----------  KICK SYSTEM ----------
        [SlashCommand("Kick", "Kickt einen Nutzer vom Server")]
        [SlashCommandPermissions(DSharpPlus.Permissions.KickMembers)]
        public async Task Kick(InteractionContext ctx,
            [Option("User", "Wähle einen Nutzer, wechen du kicken willst!")] DiscordUser TargetUser,
            [Option("Reason", "Aus welchem Grund möchtest du diesen Nutzer kicken?")] string reason = "kein genannter Grund!")
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();
            DiscordMember TargetMember = (DiscordMember)TargetUser;

            // Send Message to Target
            var DMChannel = await TargetMember.CreateDmChannelAsync();
            var BanMessage = new DiscordEmbedBuilder()
            {
                Title = "Du wurdest gekickt!",
                Description = $"Du wurdest vorübergehend von dem Server {ctx.Guild.Name} geworfen!" +
                $"\nGrund: {reason}" +
                $"\n\nDu kannst zwar wieder auf den Server kommen, beachte aber, dass weitere Auffälligkeiten mit einem permanenten Ban bestraft werden können",
                Color = DiscordColor.IndianRed
            };
            await DMChannel.SendMessageAsync(BanMessage);

            await TargetMember.RemoveAsync(reason);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Kick!",
                Description = $"{TargetUser.Mention} wurde gekickt! \n" +
                $"Zeit: {DateTime.Now}\n" +
                $"Grund: {reason}\n" +
                $"Gekickt von: {ctx.User.Mention}\n",
                Color = DiscordColor.Red
            };
            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }
        // ----------  KICK SYSTEM ----------


        // ----------  MUTE SYSTEM ----------
        [SlashCommand("Mute", "Mute einen Nutzer für eine gewisse Zeit!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.MuteMembers)]
        public async Task Mute(InteractionContext ctx,
            [Option("User", "Wähle einen Nutzer, welchen du muten willst!")] DiscordUser TargetUser,
            [Option("Time", "Wie lange soll der Nutzer gemutet sein? (Stunden)")] long TimeSpan,
            [Option("Reason", "Aus welchem Grund möchtest du diesen Nutzer muten?")] string reason = "kein genannter Grund!")
        {
            DiscordMember TargetMember = (DiscordMember)TargetUser;

            DateTimeOffset Offset = DateTimeOffset.UtcNow.AddHours(TimeSpan);
            
            await TargetMember.TimeoutAsync(Offset, reason);

            var DMChannel = await TargetMember.CreateDmChannelAsync();
            var MuteMessage = new DiscordEmbedBuilder()
            {
                Title = "Du wurdest gemutet!",
                Description = $"Du wurdest vorübergehend auf dem Server {ctx.Guild.Name} gemutet!" +
                $"\nGrund: {reason}" +
                $"\nBis: {Offset}",
                Color = DiscordColor.IndianRed
            };
            await DMChannel.SendMessageAsync(MuteMessage);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Mute!",
                Description = $"{TargetUser.Mention} wurde gemutet! \n" +
                $"Zeit: {DateTime.Now}\n" +
                $"Bis: {Offset}\n" +
                $"Grund: {reason}\n" +
                $"Gemutet von: {ctx.User.Mention}\n",
                Color = DiscordColor.Red
            };
            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }

        [SlashCommand("Mutelist", "Erhalte eine Liste aller gemuteten Nutzer!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.MuteMembers)]
        public async Task Mutelist(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var members = await ctx.Guild.GetAllMembersAsync();
            string mutedList = "";

            var response = new DiscordEmbedBuilder()
            {
                Title = "Gemutete User:",
                Description = mutedList,
                Color = DiscordColor.IndianRed
            };

            int i = 1;
            foreach (DiscordMember member in members)
            {
                if (member.CommunicationDisabledUntil == null) break;
                response.Description = response.Description +
                    $"\n\n__Nr: {i}:__" +
                    $"\nName: {member.Mention}" +
                    $"\nTimeout bis: {member.CommunicationDisabledUntil.Value.AddHours(1)}";
                i++;
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(response));
        }

        [SlashCommand("Unmute", "Entmute einen bestimmen Nutzer")]
        [SlashCommandPermissions(DSharpPlus.Permissions.MuteMembers)]
        public async Task Unmute(InteractionContext ctx,
            [Option ("User", "Wähle einen Nutzer, welchen du enmuten willst!")] DiscordUser TargetUser)
        {
            await ctx.DeferAsync();
            DiscordMember TargetMember = (DiscordMember)TargetUser;

            await ctx.DeleteResponseAsync();
            if (TargetMember.CommunicationDisabledUntil == null) return;

            DateTimeOffset OffSet = DateTimeOffset.UtcNow;
            Console.WriteLine(OffSet);

            await TargetMember.TimeoutAsync(OffSet);
            var DMChannel = await TargetMember.CreateDmChannelAsync();
            var MuteMessage = new DiscordEmbedBuilder()
            {
                Title = "Du wurdest entmutet!",
                Description = $"Dein mute wurde auf dem Server {ctx.Guild.Name} aufgehoben!",
                Color = DiscordColor.Green
            };
            await DMChannel.SendMessageAsync(MuteMessage);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Aufgehobener Mute!",
                Description = $"{TargetUser.Mention} wurde entmutet! \n" +
                $"Zeit: {DateTime.Now}\n" +
                $"Entmutet von: {ctx.User.Mention}\n",
                Color = DiscordColor.Azure
            };

            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }
        // ----------  MUTE SYSTEM ----------


        // ---------- CHATCLEAR SYSTEM ----------
        [SlashCommand("Clear", "Lösche eine bestimmte anzahl von Nachrichten!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.ManageMessages)]
        public async Task ClearChat(InteractionContext ctx,
            [Option("Anzahl", "Wähle eine Anzahl von Nachrichten, welche gelöscht werden sollen!")] long messages)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            var channel = ctx.Channel;
            var lastMessages = await channel.GetMessagesAsync((int)messages);
            await channel.DeleteMessagesAsync(lastMessages);

            var message = new DiscordEmbedBuilder()
            {
                Title = "Dieser Chat wurde gelöscht!",
                Description = $"Ein Teammitglied hat die letzen {messages} Nachrichten gelöscht!",
                Color = DiscordColor.Red
            };
            await channel.SendMessageAsync(message);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Chat Clear!",
                Description = $"{messages} Nachrichten wurden in {ctx.Channel.Mention} gelöscht!" +
                $"\nAusgefüht von: {ctx.User.Mention}" +
                $"\nZeit: {DateTime.Now}",
                Color = DiscordColor.Azure
            };

            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }

        [SlashCommand("ClearAll", "Löscht ALLE Nachrichten in diesem Channel!")]
        [SlashCommandPermissions(DSharpPlus.Permissions.ManageChannels)]
        public async Task ClearAll(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            await ctx.DeleteResponseAsync();

            var channel = ctx.Channel;
            while (channel.GetMessagesAsync(1)  != null)
            {
                var lastMessages = await channel.GetMessagesAsync(50);
                if (lastMessages == null) { lastMessages = await channel.GetMessagesAsync(25); }
                if (lastMessages == null) { lastMessages = await channel.GetMessagesAsync(12); }
                if (lastMessages == null) { lastMessages = await channel.GetMessagesAsync(6); }
                if (lastMessages == null) { lastMessages = await channel.GetMessagesAsync(3); }
                if (lastMessages == null) { lastMessages = await channel.GetMessagesAsync(1); }

                await channel.DeleteMessagesAsync(lastMessages);
            }

            var message = new DiscordEmbedBuilder()
            {
                Title = "Dieser Chat wurde gelöscht!",
                Description = $"Ein Teammitglied hat alle Nachrichten in diesem Chat gelöscht!",
                Color = DiscordColor.Red
            };
            await channel.SendMessageAsync(message);

            // Logs
            var LogMessage = new DiscordEmbedBuilder()
            {
                Title = "Chat Clear!",
                Description = $"Alle Nachrichten wurden in {ctx.Channel.Mention} gelöscht!" +
                $"\nAusgefüht von: {ctx.User.Mention}" +
                $"\nZeit: {DateTime.Now}",
                Color = DiscordColor.Azure
            };

            DiscordChannel LogChannel = ctx.Guild.GetChannel(1188947815569817683);
            await LogChannel.SendMessageAsync(LogMessage);
        }
        // ---------- CHATCLEAR SYSTEM ----------
    }
}
