using Administrator.GameData;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Interactions.MessageInteractions
{
    internal class CountingMessageInteraction
    {
        public async Task CheckNumber(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            var GameReader = new GameReader();
            await GameReader.ReadCounting();

            var currentNumber = GameReader.Counting_CurrentNumber;
            DiscordMember lastSender = await args.Guild.GetMemberAsync(GameReader.Counting_LastUserID);

            // Check if is Number
            bool isNumber = int.TryParse(args.Message.Content.ToString(), out int nr);
            if (!isNumber) return;

            // Check if is Last Sender
            if (args.Message.Author == lastSender)
            {
                var lastSenderError = new DiscordEmbedBuilder()
                {
                    Title = "Fehler!",
                    Description = $"{args.Message.Author.Mention} Du darfst nicht 2 Nummern hintereinander schreiben!" +
                    $"\n\nDie nächste Zahl ist 1!",
                    Color = DiscordColor.Red,
                };
                await args.Message.Channel.SendMessageAsync(lastSenderError);
                await GameReader.SetCounting();

                return;
            }

            // Check Number 
            if (nr == currentNumber+1)
            {
                await GameReader.SetCounting(nr, args.Author.Id);

                var check = DiscordEmoji.FromName(Program.Client, ":white_check_mark:");
                await args.Message.CreateReactionAsync(check);
                
            }
            else
            {
                var wrongNumberError = new DiscordEmbedBuilder()
                {
                    Title = "Fehler!",
                    Description = $"{args.Message.Author.Mention} Du die Falsche Nummer genannt!" +
                    $"\n\nDie nächste Zahl ist 1!",
                    Color = DiscordColor.Red,
                };
                await args.Message.Channel.SendMessageAsync(wrongNumberError);
                await GameReader.SetCounting();

                return;
            }
        }

        public async Task DeletedMessage(DSharpPlus.EventArgs.MessageDeleteEventArgs args)
        {
            var Reader = new GameReader();
            await Reader.ReadCounting();

            DiscordMember member = (DiscordMember)args.Message.Author;
            DiscordMember LastAuthor = args.Guild.GetMemberAsync(Reader.Counting_LastUserID).Result; 
            var CurrentNumber = Reader.Counting_CurrentNumber;
            var NextNumber = Reader.Counting_CurrentNumber + 1;

            int.TryParse(args.Message.Content, out int MessageNumber);

            if (member != LastAuthor) return;
            if (MessageNumber != CurrentNumber) return;

            var embed = new DiscordEmbedBuilder()
            {
                Title = "Counting System!",
                Description = $"Die Nachricht von {member.Mention} wurde gelöscht!" +
                $"\n die nächste Zahl ist: **{NextNumber}**",
                Color = DiscordColor.Goldenrod
            };

            await args.Channel.SendMessageAsync(embed);
        }
    }
}
