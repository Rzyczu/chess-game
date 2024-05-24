using chess.Helpers.Enums;

namespace chess.Components
{
    public class Player
    {
        public ColorType Color;
        public int Score { get; set; }

        public Player(ColorType color)
        {
            Color = color;
            Score = 0;
        }
    }
}
