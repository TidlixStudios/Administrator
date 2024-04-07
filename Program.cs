using Administrator.Commands;
using Administrator.Commands.Verify_System;
using Administrator.Config;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Administrator.Commands.Server_Commands.Support_System;
using System.Reflection.Emit;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;
using Administrator.Commands.Server_Commands;
using System.Runtime.CompilerServices;
using Administrator.Interactions.MessageInteractions;
using Administrator.Commands.GameCommands;
using Administrator.Interactions.ButtonInteractions;
using Administrator.Notifiers.Youtube;
using System.Timers;
using Timer = System.Timers.Timer;
using Administrator.Notifiers.Studios;

namespace Administrator
{
    internal class Program
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static DiscordClient Client { get; set; }
        public static ConfigReader reader { get; set; }
        public static ulong BotID { get; set; }

        private static YoutubeVideo _videoYT = new YoutubeVideo();
        private static YoutubeVideo tempYT = new YoutubeVideo();
        private static YoutubeEngine _YoutubeEngine = new YoutubeEngine();

        private static StudiosVideo _videoSt = new StudiosVideo();
        private static StudiosVideo tempSt = new StudiosVideo();
        private static StudiosEngine _StudiosEnige = new StudiosEngine();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Programm Started");
            Console.ForegroundColor = ConsoleColor.White;


            // Create Client
            reader = new ConfigReader();
            await reader.ReadJSON();
            DiscordConfiguration ClientConig = new DiscordConfiguration()
            {
                Token = reader.token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true
            };
            Client = new DiscordClient(ClientConig);
            Client.UseInteractivity();

            // Methods
            Client.Ready += Client_Ready;
            Client.GuildMemberAdded += MemberJoined;
            Client.GuildMemberRemoved += MeberLeft;
            Client.ComponentInteractionCreated += ComponentInteraction;
            Client.ModalSubmitted += ModalSubbmitted;
            Client.MessageCreated += MessageSended;
            Client.MessageDeleted += MessageDeleted;

            // Register Commands
            var SlashCommands = Client.UseSlashCommands();
            SlashCommands.RegisterCommands<DebugCommands>();
            SlashCommands.RegisterCommands<ModerationCommands>();
            SlashCommands.RegisterCommands<VerifyCommands>();
            SlashCommands.RegisterCommands<TicketCommands>();
            SlashCommands.RegisterCommands<MessageCommands>();
            SlashCommands.RegisterCommands<CountingCommands>();

            await Task.Delay(1000);

            // Connect Client
            await Client.ConnectAsync();
            await SlashCommands.RefreshCommands();

            await Task.Delay(1000);

            // Start Loops
            await StartYoutubeNotifier();
            await StartStudiosNotifier();


            // Always oline
            await Task.Delay(-1);
        }

        private static async Task MemberJoined(DiscordClient sender, DSharpPlus.EventArgs.GuildMemberAddEventArgs args)
        {
            DiscordChannel channel = args.Guild.GetChannel(reader.welcomeChannelID);
            DiscordChannel vChannel = args.Guild.GetChannel(reader.verifyChannelID);
            DiscordMember member = args.Member;

            var Random = new Random();
            var MessageNr = Random.Next(1, 5);
            string[] WelcomeTitle = new string[6];
            string[] WelcomeDescription = new string[6];


            WelcomeTitle[1] = "Ein neues Mitglied ist erschienen!";
            WelcomeTitle[2] = "Klopf Klopf! Wer ist da? Ein neues Mitglied!";
            WelcomeTitle[3] = "Psst! Ich habe ein neues Mitglied gesichtet!";
            WelcomeTitle[4] = "EIN EINDRINGLING!";
            WelcomeTitle[5] = "Wir haben Zuwachs!";

            WelcomeDescription[1] = $"Begrüßt mit mir unser neustes Mitglied {member.Mention}!";
            WelcomeDescription[2] = $"{member.Mention} hat gerade diesen Server betreten! Willkommen!";
            WelcomeDescription[3] = $"Ich habe ein seltenes {member.Mention} gesichtet! Hallo!";
            WelcomeDescription[4] = $"Ach ne! Es ist doch nur {member.Mention}! Willkommen!";
            WelcomeDescription[5] = $"{member.Mention} ist soeben ganz frisch auf diesem Server gelandet! Wie geht's?";

            var welcomeEmbed = new DiscordEmbedBuilder()
            {
                Title = WelcomeTitle[MessageNr],
                Description = WelcomeDescription[MessageNr],
                Color = DiscordColor.SpringGreen,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = member.AvatarUrl
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Um den Zugriff auf alle Kanäle zu erhalten, bitte gehe zu #{vChannel.Name} und schließe die Verifizierung ab!"
                },
            };

            await channel.SendMessageAsync(welcomeEmbed);
        }

        private static async Task MeberLeft(DiscordClient sender, DSharpPlus.EventArgs.GuildMemberRemoveEventArgs args)
        {
            DiscordChannel channel = args.Guild.GetChannel(reader.welcomeChannelID);
            DiscordMember member = args.Member;

            var LeaveEmbed = new DiscordEmbedBuilder()
            {
                Title = "Ein Mitglied hat uns verlassen!",
                Description = $"{member.Mention} hat sich leider von diesem Server verabschiedet! :(",
                Color = DiscordColor.DarkRed
            };

            await channel.SendMessageAsync(LeaveEmbed);
        }

        private static async Task MessageSended(DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            var VerifyChannel = args.Guild.GetChannel(reader.verifyChannelID);
            var CountingChannel = args.Guild.GetChannel(reader.countingChannelID);

            var Verify = new VerifyMessageInteraction();
            var Counting = new CountingMessageInteraction();

            if (args.Message.Channel == VerifyChannel) await Verify.DeleteNonVerifyMessages(args);
            else if (args.Message.Channel == CountingChannel) await Counting.CheckNumber(args);
            else return;
        }

        private static async Task MessageDeleted(DiscordClient sender, DSharpPlus.EventArgs.MessageDeleteEventArgs args)
        {
            var CountingChannel = args.Guild.GetChannel(reader.countingChannelID);

            var Counting = new CountingMessageInteraction();

            if (args.Message.Channel == CountingChannel) await Counting.DeletedMessage(args);
            else return;
        }

        private static async Task ModalSubbmitted(DiscordClient sender, DSharpPlus.EventArgs.ModalSubmitEventArgs args)
        {
            if (args.Interaction.Type == InteractionType.ModalSubmit)
            {
                await args.Interaction.DeferAsync();
                await args.Interaction.DeleteOriginalResponseAsync();
                var values = args.Values;
                values.TryGetValue("sup_ticket_topic", out string Sup_Ticket_Topic);
                values.TryGetValue("sup_ticket_description", out string Sup_Ticket_Description);

                if (Sup_Ticket_Topic != null && Sup_Ticket_Description != null)
                {
                    DiscordChannel SupportCategorie = args.Interaction.Guild.GetChannel(reader.supportCategorieID);
                    DiscordMember member = (DiscordMember)args.Interaction.User;
                    DiscordRole teamRole = args.Interaction.Guild.GetRole(reader.teamRoleID);

                    var TicketChannel = await args.Interaction.Guild.CreateChannelAsync(
                        name: $"{member.Username}'s Ticket!",
                        topic: "Support-Ticket",
                        type: ChannelType.Text,
                        parent: SupportCategorie);

                    await TicketChannel.AddOverwriteAsync(member, allow: Permissions.AccessChannels);

                    Task.Delay(10).Wait();
                    var embed = new DiscordEmbedBuilder()
                    {
                        Title = $"{member.Username} hat ein Ticket erstellt!",
                        Description = $"**{Sup_Ticket_Topic}**" +
                        $"\n{Sup_Ticket_Description}" +
                        $"\n\n> Ein Mitglied des {teamRole.Mention} wird sich so schnell wie möglich um dein Problem kümmern!" +
                        $"\n> Falls du etwas zu dem Ticket hinzu fügen möchtest, kannst du das jederzeit machen, indem du Nachrichten in dieses Ticket sendest!" +
                        $"\n> *Falls sich dein Problem gelöst hat, schreibe dies bitte ebenfalls in diesen Textkanal, damit ein Teammitglied dieses Ticket schließen kann!*"
                    };
                    await TicketChannel.SendMessageAsync(embed);

                    var MentionMessage = new DiscordMessageBuilder()
                        .WithContent($"{teamRole.Mention}, {member.Mention}")
                        .WithAllowedMentions(new IMention[]
                        {
                            new UserMention((DiscordUser)member),
                            new RoleMention(teamRole)
                        })
                        .AddMention(new UserMention((DiscordUser)member))
                        .AddMention(new RoleMention(teamRole));

                    var sentMentionMessage = await TicketChannel.SendMessageAsync(MentionMessage);

                    await Task.Delay(500);
                    await TicketChannel.DeleteMessageAsync(sentMentionMessage);

                    return;
                }
            }
        }

        private static async Task ComponentInteraction(DiscordClient sender, DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            var Support = new SupportTicket();
            var Verify = new VerifyButton();
            var RR = new ReactionRoles();

            if (args.Id == "Verify_BTN") await Verify.SendCode(args);
            else if (args.Id == "Sup_Ticket_create_BTN") await Support.CreateTicket(args);
            else if (args.Id == "RR_Game_BTN") await RR.ChangeGameRole(args);
            else if (args.Id == "RR_Dev_BTN") await RR.ChangeDevRole(args);
            else if (args.Id == "RR_NtfYt_BTN") await RR.ChangeNtfYtRole(args);
            else if (args.Id == "RR_NtfSt_BTN") await RR.ChangeNtfStRole(args);
            else if (args.Id == "RR_NtfTw_BTN") await RR.ChangeNtfTwRole(args);
        }


        private static async Task StartYoutubeNotifier ()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Youtube Notifier Started");
            Console.ForegroundColor = ConsoleColor.White;

            var Config = reader;

            DiscordGuild guild = await Client.GetGuildAsync(Config.guildID);

            DiscordRole notifierRole = guild.GetRole(Config.notifierRoleYoutubeID);
            RoleMention mention = new RoleMention(notifierRole);

            DiscordChannel channel = guild.GetChannel(Config.youtubeNotifierChannelID);

            var timer = new Timer(60000);
            timer.Elapsed += async (sender, e) =>
            {
                _videoYT = _YoutubeEngine.GetLatestVideo();
                var lastCheckedAt = DateTime.Now;

                if (_videoYT != null)
                {
                    if (_videoYT.publishedAt < lastCheckedAt)
                    {
                        if (_videoYT.videoUrl == tempYT.videoUrl)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[Youtube Notifier] Same Video detectet!");
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        }
                        DiscordEmbedBuilder messageEmbed = new DiscordEmbedBuilder()
                        {
                            Title = "Ein neues Video wurde hochgeladen!",
                            Description = "Tidlix hat ein neues Video hochgeladen! Es würde mich freuen, wenn du es dir anschauen würdest!" +
                            $"\n\nTitel: {_videoYT.videoTitle}" +
                            $"\nURL: {_videoYT.videoUrl}",
                            Color = DiscordColor.Blurple,
                            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail 
                            {
                                Url = _videoYT.thumbnail
                            },
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Hochgeladen am: {_videoYT.publishedAt}"
                            }
                        };

                        DiscordMessageBuilder message = new DiscordMessageBuilder()
                            .WithContent(notifierRole.Mention)
                            .WithAllowedMention(mention)
                            .AddEmbed(messageEmbed);

                        await channel.SendMessageAsync(message);
                        tempYT = _videoYT;
                    }
                }
            };

            timer.Start();
        }

        private static async Task StartStudiosNotifier()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Studios Notifier Started");
            Console.ForegroundColor = ConsoleColor.White;

            var Config = reader;

            DiscordGuild guild = await Client.GetGuildAsync(Config.guildID);

            DiscordRole notifierRole = guild.GetRole(Config.notifierRoleStudiosID);
            RoleMention mention = new RoleMention(notifierRole);

            DiscordChannel channel = guild.GetChannel(Config.studiosNotifierChannelID);

            var timer = new Timer(1800000);
            timer.Elapsed += async (sender, e) =>
            {
                _videoSt = _StudiosEnige.GetLatestVideo();
                var lastCheckedAt = DateTime.Now;

                if (_videoSt != null)
                {
                    if (_videoSt.publishedAt < lastCheckedAt)
                    {
                        if (_videoSt.videoTitle == tempSt.videoTitle)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[Studios Notifier] Same Video detectet!");
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        }
                        DiscordEmbedBuilder messageEmbed = new DiscordEmbedBuilder()
                        {
                            Title = "Ein neues Video wurde hochgeladen!",
                            Description = "Tidlix hat ein neues Video hochgeladen! Es würde mich freuen, wenn du es dir anschauen würdest!" +
                            $"\n\nTitel: {_videoSt.videoTitle}" +
                            $"\nURL: {_videoSt.videoUrl}",
                            Color = DiscordColor.Blurple,
                            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                            {
                                Url = _videoSt.thumbnail
                            },
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                Text = $"Hochgeladen am: {_videoSt.publishedAt}"
                            }
                        };

                        DiscordMessageBuilder message = new DiscordMessageBuilder()
                            .WithContent(notifierRole.Mention)
                            .WithAllowedMention(mention)
                            .AddEmbed(messageEmbed);

                        await channel.SendMessageAsync(message);
                        tempSt = _videoSt;
                    }
                }
            };

            timer.Start();
        }


        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The Bot is now online! \n\n");
            Console.ForegroundColor = ConsoleColor.White;

            return Task.CompletedTask;
        }
    }
}
