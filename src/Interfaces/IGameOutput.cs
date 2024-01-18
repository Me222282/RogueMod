using System;
using Zene.Structs;

namespace RogueMod
{
    public interface IGameOutput
    {
        public Vector2I Size { get; }
        
        public IOutput DirectOut { get; set; }
        public int Layers { get; }
        
        public IOutput this[int layer] { get; }
        /// <summury>
        /// Visability map.
        /// </summury>
        public bool this[int x, int y] { get; set; }
        
        public void ClearAll();
        public void FillCorridorMap(ICorridorManager corridors, int layer);
        
        public void PrintLine(int line);
        public void PrintAll();
    }
}