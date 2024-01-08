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
        
        public static void Draw(this IEntity e)
        {
            e.UnderChar = Curses.StdScr.ReadOutputCharW(e.Position.X, e.Position.Y);
            
            Out.Write(e.Position.X, e.Position.Y, e.Graphic, true);
        }
    
        public static void Add<T>(this LinkedList<T> list, T value)
        {
            list.AddLast(new LinkedListNode<T>(value));
        }
    }
}