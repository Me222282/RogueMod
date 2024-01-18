using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public interface IRoom : IEnumerable<Door>
    {
        public bool Dark { get; set; }
        public RectangleI Bounds { get; }
        public RectangleI InnerBounds { get; }
        public int DoorCount { get; }
        
        public Door this[int index] { get; }
        
        public void Render(IOutput scr);
    }
}