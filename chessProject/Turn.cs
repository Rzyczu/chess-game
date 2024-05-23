namespace chess
{
    public class Turn
    {
        public  int Number { get; set; }
        public Player Player { get; set; }
        public Move? Move { get; set; }
        public Turn(int number, Player player)
        {
            Player = player;
            Number = number;
        }
    }
}
