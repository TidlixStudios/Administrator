using Administrator.Commands.Verify_System;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Interactions.ButtonInteractions
{
    public class VerifyButton
    {
        public async Task SendCode (DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            var reader = Program.reader;

            await args.Interaction.DeferAsync();
            await args.Interaction.DeleteOriginalResponseAsync();
            DiscordMember member = (DiscordMember)args.User;

            var Verification = new Verify();
            await Verification.SendCode(member, args.Channel);

            var userRole = args.Guild.GetRole(reader.memberRoleID);
            var Channel = args.Guild.GetChannel(reader.verifyBotChannelID);

            bool gotCode = false;
            DateTime time = DateTime.Now.AddMinutes(5);
            while (!gotCode)
            {
                if (DateTime.Now >= time) return; //time up
                var waiter = Channel.GetNextMessageAsync();
                var message = waiter.Result;
                var embed = message.Result.Embeds.First();

                ulong.TryParse(embed.Title, out ulong id);
                int.TryParse(embed.Description, out int code);

                if (id != args.User.Id) break; //wrong User
                if (code != Verification.Code)
                {
                    var Error = new DiscordEmbedBuilder()
                    {
                        Title = "Verifizierungs Prozess!",
                        Description = "Du hast den falschen Code eingegeben! Bitte versuche es erneut!",
                        Color = DiscordColor.Red
                    };
                    await member.CreateDmChannelAsync().Result.SendMessageAsync(Error);
                    break;
                } //wrong Code

                await member.GrantRoleAsync(userRole);
                gotCode = true;
            }
        }
    }
}
