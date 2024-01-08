using System;

namespace RogueMod
{
    public struct Dice
    {
        public Dice(int r, int s)
        {
            Rolls = r;
            Sides = s;
        }
        
        public int Rolls { get; }
        public int Sides { get; }
        
        public int NewRoll()
        {
            if (Sides == 0 || Rolls == 0) { return 0; }
            if (Sides == 1) { return Rolls; }
            
            int sum = 0;
            
            for (int i = 0; i < Rolls; i++)
            {
                sum += Program.RNG.Next(1, Sides + 1);
            }
            
            return sum;
        }
        
        public static implicit operator Dice(string d)
        {
            d = d.Trim();
            int i = d.IndexOf('d');
            int r = int.Parse(d.Remove(i));
            int s = int.Parse(d.Remove(0, i + 1));
            
            return new Dice(r, s);
        }
        
        public static bool operator ==(Dice l, Dice r) => l.Equals(r);
        public static bool operator !=(Dice l, Dice r) => !l.Equals(r);

        public override bool Equals(object obj)
        {
            return obj is Dice d &&
                d.Rolls == Rolls &&
                d.Sides == Sides;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rolls, Sides);
        }
    }
}