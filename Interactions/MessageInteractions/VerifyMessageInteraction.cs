using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Interactions.MessageInteractions
{
    public class VerifyMessageInteraction
    {
        public async Task DeleteNonVerifyMessages(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {
            if (args.Message == null) return;
            if (args.Message.Id != 1203682646446841908)
            {
                await Task.Delay(10000);
                if (args.Message == null) return;
                await args.Message.DeleteAsync();
            }
        }
    }
}
