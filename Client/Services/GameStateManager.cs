using PlanningPoker.Shared;

namespace PlanningPoker.Client.Services
{
    public class GameStateManager
    {
        public Player Player { get; set; } = new Player();
        public Game Game { get; set; } = new Game();
        public bool IsStateReady
        {
            get
            {
                return !(string.IsNullOrWhiteSpace(Player.Name) || string.IsNullOrWhiteSpace(Game.Id));
            }
        }
    }
}