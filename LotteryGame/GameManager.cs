using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryGame
{
    public class GameManager
    {

        public List<Player>? players;
        private readonly int minPlayers = 10;
        private readonly int maxPlayers = 15;
        private readonly int maxTicketsBuy = 10;
        private decimal startBalance = 10.00m;
        public List<int> ticketsList;

        public GameManager()
        {
            players = new List<Player>();
            ticketsList = new List<int>();
        }

        public void StartGame()
        {
            GetPlayers();
            GenerateTicketsForPlayers();
            CalculatePrizes();
        }

        public void WelcomePlayer()
        {

            Ticket ticket = new();
            Console.WriteLine("Welcome to the Bede Lottery, Player 1!");
            Console.WriteLine();
            Console.WriteLine($"* Your digital balance is: ${startBalance}");
            Console.WriteLine($"* Ticket Price: ${ticket.Price}");
            Console.WriteLine();
        }

        public void GetPlayers()
        {
            Random rand = new();
            int totalPlayers = rand.Next(minPlayers, maxPlayers);

            for (int i = 1; i <= totalPlayers; i++) 
            {
                players?.Add(new Player(startBalance, i));
            }

            WelcomePlayer();
        }

        public void GenerateTicketsForPlayers()
        {
            Random rand = new();
            Ticket ticket = new();

            foreach(var player in players)
            {
                int generateTickets;

                if(player.PlayerId == 1)
                {
                    generateTickets = GetPlayerInputForTickets();
                }
                else
                {
                    generateTickets = rand.Next(1, maxTicketsBuy);
                }

                int ticketsBought = player.BuyTickets(generateTickets, ticket.Price);

                for(int i = 0; i < ticketsBought; i++)
                {
                    ticketsList.Add(player.PlayerId);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"{players.Count} other CPU players also have purchased tickets.");
        }

        public int GetPlayerInputForTickets()
        {
            int tickets = 0;
            bool isValid = false;
            
            while(!isValid)
            {
                Console.WriteLine("How many tickets do you want to buy, Player 1?");
                string input = Console.ReadLine();

                if (int.TryParse(input, out tickets))
                {
                    if (tickets >= 1 && tickets <= maxTicketsBuy)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number between 1 and 10.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number");
                }
            }

            return tickets;
        }

        public void CalculatePrizes()
        {
            Ticket ticket = new();
            int totalTicketsPrice = ticketsList.Count * ticket.Price;

            int grandTierPrize = (int)(totalTicketsPrice * 0.50m);
            int secondTierPrize = (int)(totalTicketsPrice * 0.30m);
            int thirdTierPrize = (int)(totalTicketsPrice * 0.10m);

            int secondTierWinners = (int)Math.Round(ticketsList.Count * 0.10);
            int thirdTierWinners = (int)Math.Round(ticketsList.Count * 0.20);

            List<int> grandPrizeWinners = new List<int>();
            List<int> secondTierWinnersList = new List<int>();
            List<int> thirdTierWinnersList = new List<int>();

            Random rand = new Random();
            int grandPrizeWinner = rand.Next(1, players.Count + 1);
            grandPrizeWinners.Add(grandPrizeWinner);

            List<int> availablePlayersForSecondTier = players.Select(p => p.PlayerId).Except(grandPrizeWinners).ToList();
            for (int i = 0; i < secondTierWinners; i++)
            {
                int winner = availablePlayersForSecondTier[rand.Next(availablePlayersForSecondTier.Count)];
                secondTierWinnersList.Add(winner);
                availablePlayersForSecondTier.Remove(winner);
            }

            List<int> availablePlayersForThirdTier = players.Select(p => p.PlayerId).Except(grandPrizeWinners.Concat(secondTierWinnersList)).ToList();
            for (int i = 0; i < thirdTierWinners; i++)
            {
                if(availablePlayersForThirdTier.Count > 0)
                {
                    int winner = availablePlayersForThirdTier[rand.Next(availablePlayersForThirdTier.Count)];
                    thirdTierWinnersList.Add(winner);
                    availablePlayersForThirdTier.Remove(winner);
                }
            }

            decimal grandPrizeAmount = grandTierPrize / grandPrizeWinners.Count;
            decimal secondTierAmount = secondTierPrize / secondTierWinnersList.Count;
            decimal thirdTierAmount = thirdTierPrize / thirdTierWinnersList.Count;

            Console.WriteLine();
            Console.WriteLine("Ticket Draw Results:");
            Console.WriteLine();

            foreach (var winner in grandPrizeWinners)
            {
                Console.WriteLine($"* Grand Prize: Player {winner} wins ${grandPrizeAmount:F2}!");
            }

            if (secondTierWinnersList.Any())
            {
                string secondTierWinnersStr = string.Join(", ", secondTierWinnersList);
                Console.WriteLine($"* Second Tier: Players {secondTierWinnersStr} win ${secondTierAmount:F2} each!");
            }

            if (thirdTierWinnersList.Any())
            {
                string thirdTierWinnersStr = string.Join(", ", thirdTierWinnersList);
                Console.WriteLine($"* Third Tier: Players {thirdTierWinnersStr} win ${thirdTierAmount:F2} each!");
            }

            decimal remainingProfit = totalTicketsPrice - grandTierPrize - secondTierPrize - thirdTierPrize;
            Console.WriteLine();
            Console.WriteLine($"Congratulations to the winners!");
            Console.WriteLine();
            Console.WriteLine($"House Revenue: ${remainingProfit:F2}");
        }

    }
}
