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
        
        char IEntity.Graphic => (char)Draw.Key;
        
        public string ToString(Discoveries dics, bool plural)
        {
            string p = plural ? "s" : "";
            return $"key{p}";
        }
        
        public bool IsKnown(Discoveries dics) => true;
        public void MakeKnown(Discoveries dics) { }
        
        public override bool Equals(object obj)
        {
            return obj is Key k &&
                k.Cursed == Cursed;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Cursed, Quantity);
        }
        
        public void Effect(ICharacter character, IRogue game)
        {
            throw new System.NotSupportedException();
        }
        
        public IItem Copy() => new Key();
    }
}