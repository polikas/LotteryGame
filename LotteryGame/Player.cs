using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryGame
{
    public class Player
    {
        public decimal Balance { get; set; }
        public int PlayerId { get; set; }
      
        public Player(decimal balance, int id)
        {
            Balance = balance;
            PlayerId = id;
        }

        public int BuyTickets(int tickets, int price)
        {
            
            int ticketsToBuy = Math.Min(tickets, (int)Balance / price);
            Balance -= (ticketsToBuy * price);
  
            return ticketsToBuy;
        }
    }
}
