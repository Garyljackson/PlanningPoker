using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Server.Hubs
{
    public class GameHub : Hub
    {
        private static List<GamePlay> _gamePlays = new List<GamePlay>();

        public async Task RegisterPlayerAsync(RegisterPlayerRequest registerPlayerRequest)
        {
            var alreadyExists = _gamePlays.Exists(ga => ga.HubConnectionId == Context.ConnectionId);

            if (alreadyExists)
                return;

            var gamePlay = new GamePlay
            {
                HubConnectionId = Context.ConnectionId,
                Game = registerPlayerRequest.Game,
                Player = registerPlayerRequest.Player,
                CardPlayed = new Card()
            };

            _gamePlays.Add(gamePlay);

            await Groups.AddToGroupAsync(Context.ConnectionId, registerPlayerRequest.Game.Id);

            await UpdateGame(gamePlay.Game.Id);
        }

        public async Task PlayCardAsync(PlayCardRequest playCardRequest) 
        {
            var gamePlay = _gamePlays.FirstOrDefault(ga => ga.HubConnectionId == Context.ConnectionId);

            if (gamePlay == null)
                return;

            gamePlay.CardPlayed = playCardRequest.CardPlayed;

            await UpdateGame(gamePlay.Game.Id);
        }

        public async Task ResetGame(ResetGameRequest resetGameRequest) 
        {
            var gamesPlaysForThisGame = _gamePlays.Where(gp => gp.Game.Id == resetGameRequest.Game.Id).ToList();
            gamesPlaysForThisGame.ForEach(gp => gp.CardPlayed = new Card());

            await UpdateGame(resetGameRequest.Game.Id);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var gamePlay = _gamePlays.FirstOrDefault(ga => ga.HubConnectionId == Context.ConnectionId);

            if (gamePlay != null)
            {
                _gamePlays.Remove(gamePlay);
                await UpdateGame(gamePlay.Game.Id);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private async Task UpdateGame(string gameId) 
        {
            var gamesPlaysForThisGame = _gamePlays.Where(gp => gp.Game.Id == gameId).ToList();

            await Clients.Group(gameId).SendAsync("UpdateGame", gamesPlaysForThisGame);
        }

    }
}
