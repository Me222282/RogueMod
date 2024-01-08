using Zene.Structs;

namespace RogueMod
{
    public class Gold : IEntity
    {
        public Gold(int v)
        {
            Value = v;
        }
        
        public int Value { get; }
        
        public Vector2I Position { get; set; }
        public bool Seen { get; set; }
        public char Graphic => (char)Draw.Gold;
        public char UnderChar { get; set; }
    }
}