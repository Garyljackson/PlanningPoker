namespace PlanningPoker.Shared
{
    public class GamePlay
    {
        public string HubConnectionId { get; set; }
        public Game Game { get; set; }
        public Player Player { get; set; }
        public Card CardPlayed { get; set; }
        public bool HasPlayed => string.IsNullOrWhiteSpace(CardPlayed.Value) == false;
    }
}
