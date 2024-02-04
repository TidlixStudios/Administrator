using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Commands.Verify_System
{
    public class Verify
    {
        public int Code { get; set; }
        public async Task SendCode (DiscordMember member, DiscordChannel verificationChannel)
        {
            var DMChannel = await member.CreateDmChannelAsync();

            var random = new Random();
            this.Code = random.Next(1000,9999);

            var message = new DiscordEmbedBuilder()
            {
                Title = "Verifizierungs Prozess!",
                Description = $"Dein Code für die Verifizierung: {this.Code}" +
                $"\n\nGehe zurück in den Verifizierungskanal und gebe /Verify {this.Code} ein, um den Verifizierungsprozess abzuschließen und somit alle Kanäle freizuschalten!" +
                $"\n Du hast 5 minuten dafür!" +
                $"\nZum Verifizierungskanal: {verificationChannel.Mention}",
                Color = DiscordColor.White
            };
            await DMChannel.SendMessageAsync(message);
        }

    }
}
