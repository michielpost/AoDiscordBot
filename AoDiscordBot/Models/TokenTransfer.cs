using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoDiscordBot.Enums;

namespace AoDiscordBot.Models
{
    public class TokenTransfer : AoTransaction
    {
        public string? TokenId { get; set; }
        public long Quantity { get; set; }

        public TokenTransferType TokenTransferType { get; set; }
    }
}
