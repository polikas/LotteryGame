using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryGame
{
    public class Ticket
    {
        public int Price { get; set; }
        public int TicketId { get; set; }

        public Ticket() 
        {
            Price = 1;
        }
    }
}
