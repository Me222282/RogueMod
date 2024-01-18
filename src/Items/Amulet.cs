using System;

namespace RogueMod
{
    public class Amulet : IItem
    {
        public ItemType Type => ItemType.Amulet;
        public Damage Attack => Damage.Zero;

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => false;

        public int Value => 1000;
        
        char IEntity.Graphic => (char)Draw.Amulet;
        
        public string ToString(IRogue game, bool plural) => "The Amulet of Yendor";
        
        public bool IsKnown(IRogue game) => true;
        public void MakeKnown(IRogue game) { }
        
        public override bool Equals(object obj) => obj is Amulet a;
        public override int GetHashCode() => HashCode.Combine(Type);
        
        public IItem Copy() => new Amulet();
        
        public void Effect(ICharacter character, IRogue game)
        {
            throw new System.NotSupportedException();
        }
    }
}