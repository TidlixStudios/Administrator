﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Config
{
    public class ConfigReader
    {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string token { get; set; } = "";
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        public ulong verifyChannelID { get; set; }
        public ulong verifyBotChannelID { get; set; }
        public ulong logChannelID { get; set; } 

        public ulong supportCategorieID { get; set; }


        public ulong teamRoleID { get; set; }
        public ulong memberRoleID { get; set; } 



        public async Task ReadJSON ()
        {
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}config.json"))
            {
                string json = await sr.ReadToEndAsync();

                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                this.token = data.token;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                this.verifyChannelID = data.verifyChannelID;
                this.verifyBotChannelID = data.verifyBotChannelID;
                this.logChannelID = data.logChannelID;

                this.supportCategorieID = data.supportCategorieID;

                this.teamRoleID = data.teamRoleID;
                this.memberRoleID = data.memberRoleID;

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Read Config...");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        internal sealed class JSONStructure 
        {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public string token { get; set; } = "";
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


            public ulong verifyChannelID { get; set; }
            public ulong verifyBotChannelID { get; set; }
            public ulong logChannelID {  get; set; }

            public ulong supportCategorieID { get; set; }


            public ulong teamRoleID {  get; set; }
            public ulong memberRoleID {  get; set; }    
        }
    }
}
