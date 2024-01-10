using System;

namespace RogueMod
{
    public class Key : IItem
    {
        public ItemType Type => ItemType.Key;
        public Damage Attack => Damage.Default;

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => true;

        public int Value => 500;
        
        public string ToString(Rogue game, bool plural)
        {
            string p = plural ? "s" : "";
            return $"key{p}";
        }
        
        public bool IsKnown(Rogue game) => true;
        public void MakeKnown(Rogue game) { }
        
        public override bool Equals(object obj)
        {
            return obj is Key k &&
                k.Cursed == Cursed;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Cursed, Quantity);
        }
        
        public void Effect(ICharacter character, Rogue game)
        {
            throw new System.NotSupportedException();
        }
        
        public IItem Copy() => new Key();
    }
}