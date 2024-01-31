using System;

namespace RogueMod
{
    public class Gold : IItem
    {
        public Gold(int q)
        {
            Quantity = q;
        }

        public ItemType Type => ItemType.Gold;
        public Damage Attack => Damage.Zero;

        public bool Cursed
        {
            get => false;
            set => throw new NotSupportedException();
        }
        public int Quantity { get; set; }
        public bool Stackable => true;
        public int Value => 1;
        
        char IEntity.Graphic => (char)Draw.Gold;
        
        public void Effect(ICharacter character, IRogue game)
             => throw new NotSupportedException();
        public bool IsKnown(Discoveries dics) => true;
        public void MakeKnown(Discoveries dics) => throw new NotSupportedException();
        public string ToString(Discoveries dics, bool plural) => throw new NotSupportedException();
        public IItem Copy() => throw new NotSupportedException();
        
        public static Gold Create(int level)
        {
            return new Gold(Program.RNG.Next(50 + (10 * level)) + level);
        }
    }
}