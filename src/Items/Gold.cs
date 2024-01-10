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
        public int Value => 10;

        public void Effect(ICharacter character, Stats tStats, Rogue game)
             => throw new NotSupportedException();
        public bool IsKnown(Rogue game) => true;
        public void MakeKnown(Rogue game) => throw new NotSupportedException();
        public string ToString(Rogue game, bool plural) => throw new NotSupportedException();
    }
}