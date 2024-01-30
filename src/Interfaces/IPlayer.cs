namespace RogueMod
{
    public interface IPlayer : ICharacter
    {
        public double MaxHP { get; }
        public int Food { get; set; }
        
        public string LevelName { get; }
        public void IncreaseLevel();
    }
}