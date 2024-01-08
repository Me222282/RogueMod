using Zene.Structs;

namespace RogueMod
{
    public interface IEntity : IDisplayable
    {
        public Vector2I Position { get; set; }
        public bool Seen { get; set; }
    }
}