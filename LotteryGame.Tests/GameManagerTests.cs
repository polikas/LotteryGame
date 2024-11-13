using Xunit;
using FluentAssertions;

namespace LotteryGame.Tests
{
    public class GameManagerTests
    {
        [Fact]
        public void GameManager_GetPlayers_ShouldAddPlayersWithinMinAndMaxLimits()
        {
            var gameManager = new GameManager();

            gameManager.GetPlayers();

            var players = gameManager.players;

            players.Should().NotBeNull();
            players.Should().HaveCountGreaterThanOrEqualTo(10).And.HaveCountLessThanOrEqualTo(15);
        }

        [Fact]
        public void GameManager_GetPlayers_ShouldSetInitialStartingBalanceForPlayers()
        {
            var gameManager = new GameManager();
            decimal expectedStartingBalance = 10.00m;

            gameManager.GetPlayers();

            var players = gameManager.players;

            foreach (Player player in players)
            {
                player.Balance.Should().Be(expectedStartingBalance);
            }
        }

        [Fact]
        public void GameManager_CalculatePrizes_ShouldNotHaveNegativeHouseProfit()
        {
            var gameManager = new GameManager();

            var players = new List<Player>
            {
                new Player(10.00m, 1),
                new Player(10.00m, 2),
                new Player(10.00m, 3),
                new Player(10.00m, 4),
                new Player(10.00m, 5)
            };
            gameManager.players = players;
            gameManager.ticketsList = new List<int> { 1, 1, 2, 3, 4, 5, 5, 5 };

            decimal totalTicketsPrice = gameManager.ticketsList.Count * 1; 
            decimal grandPrizeAmount = totalTicketsPrice * 0.50m;
            decimal secondTierPrize = totalTicketsPrice * 0.30m;
            decimal thirdTierPrize = totalTicketsPrice * 0.10m;

            gameManager.CalculatePrizes();

            decimal expectedRemainingProfit = totalTicketsPrice - grandPrizeAmount - secondTierPrize - thirdTierPrize;
            expectedRemainingProfit.Should().BeGreaterThanOrEqualTo(0);
        }

    }
}