using System;
using CursesSharp;

namespace RogueMod
{
    public enum Colours
    {
        Normal,
        Red,
        Green,
        Brown,
        Blue,
        Magenta,
        Cyan,
        LightGrey,
        Grey,
        LightRed,
        LightGreen,
        Yellow,
        LightBlue,
        LightMagenta,
        LightCyan,
        White
    }
    
    public readonly struct Attribute
    {
        public Attribute(uint attr)
        {
            Value = attr;
        }
        
        public uint Value { get; }
        public bool Reversed => (Value & Attrs.REVERSE) == Attrs.REVERSE;
        public bool Bold => (Value & Attrs.BOLD) == Attrs.BOLD;
        
        public Attribute GetReversed()
        {
            if (Reversed)
            {
                return new Attribute(Value & ~Attrs.REVERSE);
            }
            
            return new Attribute(Value | Attrs.REVERSE);
        }

        public override bool Equals(object obj)
            => obj is Attribute a && a.Value == Value;
        public override int GetHashCode() => HashCode.Combine(Value);
        
        public static Attribute Normal { get; } = new Attribute(0);
        public static Attribute Red => GetAttribute(Colours.Red);
        public static Attribute Green => GetAttribute(Colours.Green);
        public static Attribute Brown => GetAttribute(Colours.Brown);
        public static Attribute Blue => GetAttribute(Colours.Blue);
        public static Attribute Magenta => GetAttribute(Colours.Magenta);
        public static Attribute Cyan => GetAttribute(Colours.Cyan);
        public static Attribute LightGrey => GetAttribute(Colours.LightGrey);
        public static Attribute Grey => GetAttribute(Colours.Grey);
        public static Attribute LightRed => GetAttribute(Colours.LightRed);
        public static Attribute LightGreen => GetAttribute(Colours.LightGreen);
        public static Attribute Yellow => GetAttribute(Colours.Yellow);
        public static Attribute LightBlue => GetAttribute(Colours.LightBlue);
        public static Attribute LightMagenta => GetAttribute(Colours.LightMagenta);
        public static Attribute LightCyan => GetAttribute(Colours.LightCyan);
        public static Attribute White => GetAttribute(Colours.White);
        
        public static Attribute GetAttribute(Colours colour, bool reverse = false, bool onGrey = false)
        {
            uint s = reverse ? Attrs.REVERSE : Attrs.NORMAL;
            uint b = colour > Colours.LightGrey ? Attrs.BOLD : Attrs.NORMAL;
            int a = onGrey ? 8 : 0;
            return Curses.COLOR_PAIR((int)colour & 0x7 + a) | b | s;
        }
        public static Attribute GetAttribute(Draw c, bool onGrey = false)
        {
            switch (c)
            {
                case Draw.Door:
                case Draw.WallTL:
                case Draw.WallH:
                case Draw.WallBR:
                case Draw.WallBL:
                case Draw.WallV:
                case Draw.WallTR:
                    return GetAttribute(Colours.Brown, false, onGrey);
                
                case Draw.Floor:
                    return GetAttribute(Colours.Green, false, onGrey);
                    
                case Draw.Player:
                case Draw.Gold:
                case Draw.Key:
                    return GetAttribute(Colours.Yellow, false, onGrey);
                
                case Draw.Staff:
                case Draw.Weapon:
                case Draw.Armour:
                case Draw.Amulet:
                case Draw.Ring:
                case Draw.Scroll:
                case Draw.Potion:
                    return GetAttribute(Colours.Blue, false, onGrey);
                    
                case Draw.Food:
                    return GetAttribute(Colours.Red, false, onGrey);
                    
                case Draw.Trap:
                    return GetAttribute(Colours.Magenta, false, onGrey);
                    
                case Draw.Stairs:
                    return GetAttribute(Colours.Green, true, onGrey);
                    
                case Draw.Passage:
                case Draw.PassageB:
                case Draw.Magic:
                case Draw.MagicB:
                    return GetAttribute(Colours.Grey);
            }
            
            return Attrs.NORMAL;
        }

        public static implicit operator Attribute(uint i)
            => new Attribute(i);
        public static bool operator ==(Attribute l, Attribute r) => l.Equals(r);
        public static bool operator !=(Attribute l, Attribute r) => !l.Equals(r);
    }
}