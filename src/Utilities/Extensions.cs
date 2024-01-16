using System;
using System.Collections.Generic;
using CursesSharp;
using Zene.Structs;

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
            char c = scr.Read(e.Position.X, e.Position.Y);
            // Already drawn
            if (c == e.Graphic) { return; }
            e.UnderChar = c;
            
            scr.Write(e.Position.X, e.Position.Y, e.Graphic);
        }
        public static void UnDraw(this IEntity e, VirtualScreen scr)
        {
            scr.Write(e.Position.X, e.Position.Y, e.UnderChar);
        }
        
        public static void Add<T>(this LinkedList<T> list, T value)
        {
            list.AddLast(new LinkedListNode<T>(value));
        }
        public static bool Exists<T>(this IEnumerable<T> e, Predicate<T> test)
        {
            foreach (T t in e)
            {
                if (!test(t)) { continue; }
                return true;
            }
            return false;
        }
        public static void Fill<T>(this T[,] array, T value)
        {
            int w = array.GetLength(1);
            int h = array.GetLength(0);
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    array[j, i] = value;
                }
            }
        }
        
        public static bool IsNullFull<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null) { continue; }
                return false;
            }
            
            return true;
        }
        
        public static bool IsDirection(this int ch)
        {
            Direction d = (Direction)ch;
            
            return d == Direction.Down ||
                d == Direction.Up ||
                d == Direction.Left ||
                d == Direction.Right ||
                d == Direction.UpLeft ||
                d == Direction.UpRight ||
                d == Direction.DownLeft ||
                d == Direction.DownRight;
        }
        public static Vector2I GetOffset(this Direction d)
        {
            switch (d)
            {
                case Direction.Down:
                    return (0, 1);
                case Direction.Up:
                    return (0, -1);
                case Direction.Right:
                    return (1, 0);
                case Direction.Left:
                    return (-1, 0);
                case Direction.DownRight:
                    return (1, 1);
                case Direction.UpLeft:
                    return (-1, -1);
                case Direction.UpRight:
                    return (1, -1);
                case Direction.DownLeft:
                    return (-1, 1);
            }
            
            return 0;
        }
    }
}