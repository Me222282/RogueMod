using System.Collections.Generic;
using CursesSharp;

namespace RogueMod
{
    public static class Extensions
    {
        public static int Roll(this string die) => ((Dice)die).NewRoll();
        public static int Roll(this string die, bool negate)
        {
            int n = Program.RNG.Next(1);
            
            if (!negate) { n = 0; }
            
            if (n == 0)
            {
                return Roll(die);
            }
            
            return -Roll(die);
        }
        
        public static char RndChar(this string str) => str[Program.RNG.Next(str.Length)];
        
        public static void Draw(this IEntity e, VirtualScreen scr)
        {
            e.UnderChar = scr.Read(e.Position.X, e.Position.Y);
            
            scr.Write(e.Position.X, e.Position.Y, e.Graphic);
        }
    
        public static void Add<T>(this LinkedList<T> list, T value)
        {
            list.AddLast(new LinkedListNode<T>(value));
        }
    }
}