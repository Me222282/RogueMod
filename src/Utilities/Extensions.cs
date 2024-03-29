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
        public static bool Exists<T>(this ReadOnlySpan<T> e, Predicate<T> test)
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
        
        public static Vector2I GetRandomPosition(this IRogue game, IRoom room)
            => game.EntityManager.GetRandomPosition(room.InnerBounds);
        
        public static void Centre(this IOutput ouput, int y, string str, Attribute attr)
        {
            ouput.Write((ouput.Size.X - str.Length) / 2, y, str, attr);
        }
        public static void RenderBoxS(this IOutput ouput, int x, int y, int w, int h)
        {
            ouput.Write(x, y, '┌', Attribute.Green);
            ouput.RenderLineH(x + 1, y, '─', w - 2, Attribute.Green);
            ouput.Append('┐', Attribute.Green);
            ouput.RenderLineV(x, y + 1, '│', h - 2, Attribute.Green);
            ouput.RenderLineV(x + w - 1, y + 1, '│', h - 2, Attribute.Green);
            ouput.Write(x, y + h - 1, '└', Attribute.Green);
            ouput.RenderLineH(x + 1, y + h - 1, '─', w - 2, Attribute.Green);
            ouput.Append('┘', Attribute.Green);
        }
        public static void RenderBoxD(this IOutput output, int x, int y, int w, int h)
        {
            output.Write(x, y, (char)Draw.WallTL, Attribute.Brown);
            output.RenderLineH(x + 1, y, (char)Draw.WallH, w - 2, Attribute.Brown);
            output.Write(x + w - 1, y, (char)Draw.WallTR, Attribute.Brown);
            output.RenderLineV(x, y + 1, (char)Draw.WallV, h - 2, Attribute.Brown);
            output.RenderLineV(x + w - 1, y + 1, (char)Draw.WallV, h - 2, Attribute.Brown);
            output.Write(x, y + h - 1, (char)Draw.WallBL, Attribute.Brown);
            output.RenderLineH(x + 1, y + h - 1, (char)Draw.WallH, w - 2, Attribute.Brown);
            output.Write(x + w - 1, y + h - 1, (char)Draw.WallBR, Attribute.Brown);
        }
        public static void RenderBoxD(this IOutput output, RectangleI bounds)
            => RenderBoxD(output, bounds.X, bounds.Y, bounds.Width, bounds.Height);
        public static int PrintList(this IOutput output, string[] list, bool vertical, Func<int, bool> exit = null)
        {
            if (exit is null)
            {
                exit = i => false;
            }
            
            int hw = output.Size.X / 2;
            for (int i = 0, j = 0; j < list.Length; i++, j++)
            {
                int x = 0;
                int y = i;
                if (!vertical)
                {
                    x = i % 2 == 0 ? 0 : hw;
                    y = i / 2;
                }
                
                if (y >= (output.Size.Y - 2))
                {
                    output.Write(0, output.Size.Y - 1, "--Press space for more, Esc to continue--", Attribute.Normal);
                    while (true)
                    {
                        int ch = output.ReadKeyInput();
                        if (ch == Keys.ESC || exit(ch)) { return ch; }
                        if (ch != ' ') { continue; }
                        break;
                    }
                    Stdscr.Clear();
                    i = -1;
                    j--;
                    continue;
                }
                
                if (list[j] == null) { i--; continue; }
                
                WriteAttr(output, x, y, list[j]);
            }
            
            output.Write(0, output.Size.Y - 1, "--Press space to continue--", Attribute.Normal);
            int ex;
            do
            {
                ex = output.ReadKeyInput();
            } while (ex != Keys.ESC && ex != ' ' && !exit(ex));
            return ex;
        }
        private static void WriteAttr(IOutput output, int x, int y, ReadOnlySpan<char> str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                output.Write(x + i, y, str[i], Attribute.GetAttribute((Draw)str[i]));
            }
        }
        
        public static void Animate(this IOutput output, Vector2I start, Vector2I end, char c, Attribute a)
        {
            Vector2I diff = end - start;
            int length = Math.Abs(diff.X) | Math.Abs(diff.Y);
            diff /= length;
            
            Vector2I last = start - diff;
            char uc = ' ';
            Attribute ua = 0;
            for (int i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    output.Write(last.X, last.Y, uc, ua);
                }
                
                uc = output.Read(start.X, start.Y);
                ua = output.ReadAttribute(start.X, start.Y);
                output.Write(start.X, start.Y, c, a);
                
                start += diff;
                last += diff;
                
                output.Pause(Program.Properties.ThrowTime);
            }
            
            output.Write(last.X, last.Y, uc, ua);
        }
    }
}