using DSharpPlus.Entities;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Interactions.ButtonInteractions
{
    public class SupportTicket
    {
        public async Task CreateTicket (DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            var modal = new DiscordInteractionResponseBuilder()
                   .WithTitle("Ticket erstellen!")
                   .WithCustomId("Ticket_MDL")
                   .AddComponents(new TextInputComponent(
                       label: "Dein Problem:", 
                       customId: "sup_ticket_topic", 
                       placeholder: "Nenne dein Problem kurz und Knapp!", 
                       required: true, 
                       style: TextInputStyle.Short))
                   .AddComponents(new TextInputComponent(
                       label: "Problembeschreibung:", 
                       customId: "sup_ticket_description", 
                       placeholder: "Beschreibe dein Problem so genau wie möglich!", 
                       required: true, 
                       style: TextInputStyle.Paragraph));
            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }
    }
}
