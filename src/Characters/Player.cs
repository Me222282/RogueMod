namespace RogueMod
{
    public sealed class Player : Character
    {
        public Player()
            : base((char)Draw.Player, 23, 12)
        {
            
        }
        
        public int Gold
        {
            get => Backpack.Gold;
            set => Backpack.Gold = value;
        }
    }
}