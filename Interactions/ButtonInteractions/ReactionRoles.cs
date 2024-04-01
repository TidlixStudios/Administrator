using Administrator.Config;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Interactions.ButtonInteractions
{
    public class ReactionRoles
    {
        public ConfigReader Config = Program.reader;


        public async Task ChangeGameRole (DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            DiscordGuild guild = args.Guild;
            DiscordRole role = guild.GetRole(Config.gameRoleID);
            DiscordMember member = (DiscordMember)args.User;

            if (member.Roles.Contains(role))
            {
                await member.RevokeRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle entfernt!",
                    Description = $"Bei dir wurde die Rolle {role.Mention} entfernt!",
                    Color = DiscordColor.Red
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // remove role
            else
            {
                await member.GrantRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle gegeben!",
                    Description = $"Dir wurde die Rolle {role.Mention} gegeben!",
                    Color = DiscordColor.Green
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // grand role
        }

        public async Task ChangeDevRole(DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            DiscordGuild guild = args.Guild;
            DiscordRole role = guild.GetRole(Config.developerRoleID);
            DiscordMember member = (DiscordMember)args.User;

            if (member.Roles.Contains(role))
            {
                await member.RevokeRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle entfernt!",
                    Description = $"Bei dir wurde die Rolle {role.Mention} entfernt!",
                    Color = DiscordColor.Red
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // remove role
            else
            {
                await member.GrantRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle gegeben!",
                    Description = $"Dir wurde die Rolle {role.Mention} gegeben!",
                    Color = DiscordColor.Green
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // grand role
        }

        public async Task ChangeNtfYtRole(DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            DiscordGuild guild = args.Guild;
            DiscordRole role = guild.GetRole(Config.notifierRoleYoutubeID);
            DiscordMember member = (DiscordMember)args.User;

            if (member.Roles.Contains(role))
            {
                await member.RevokeRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle entfernt!",
                    Description = $"Bei dir wurde die Rolle {role.Mention} entfernt!",
                    Color = DiscordColor.Red
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // remove role
            else
            {
                await member.GrantRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle gegeben!",
                    Description = $"Dir wurde die Rolle {role.Mention} gegeben!",
                    Color = DiscordColor.Green
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // grand role
        }

        public async Task ChangeNtfStRole(DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            DiscordGuild guild = args.Guild;
            DiscordRole role = guild.GetRole(Config.notifierRoleStudiosID);
            DiscordMember member = (DiscordMember)args.User;

            if (member.Roles.Contains(role))
            {
                await member.RevokeRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle entfernt!",
                    Description = $"Bei dir wurde die Rolle {role.Mention} entfernt!",
                    Color = DiscordColor.Red
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // remove role
            else
            {
                await member.GrantRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle gegeben!",
                    Description = $"Dir wurde die Rolle {role.Mention} gegeben!",
                    Color = DiscordColor.Green
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // grand role
        }

        public async Task ChangeNtfTwRole(DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            DiscordGuild guild = args.Guild;
            DiscordRole role = guild.GetRole(Config.notifierRoleTwitchID);
            DiscordMember member = (DiscordMember)args.User;

            if (member.Roles.Contains(role))
            {
                await member.RevokeRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle entfernt!",
                    Description = $"Bei dir wurde die Rolle {role.Mention} entfernt!",
                    Color = DiscordColor.Red
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // remove role
            else
            {
                await member.GrantRoleAsync(role);
                var responseEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Rolle gegeben!",
                    Description = $"Dir wurde die Rolle {role.Mention} gegeben!",
                    Color = DiscordColor.Green
                };

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed).AsEphemeral());

            } // grand role
        }
    }
}
