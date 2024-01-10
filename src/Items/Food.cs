using System;

namespace RogueMod
{
    public class Food : IItem
    {
        public Food(bool t)
        {
            FoodType = t;
        }
        
        public ItemType Type => ItemType.Food;
        public Damage Attack => Damage.Zero;

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;

        public bool Stackable => true;
        public int Value => 1;
        
        public bool FoodType { get; }
        
        public void Effect(ICharacter character, Rogue game)
        {
            if (character is Player p)
            {
                p.Food = Program.Properties.StomachSize;
            }
        }

        public bool IsKnown(Rogue game) => true;
        public void MakeKnown(Rogue game) { }

        public string ToString(Rogue game, bool plural)
        {
            if (FoodType)
            {
                string fruit = Program.Properties.Fruit;
                return plural ? $"{Quantity} {fruit}s"
                    : $"{Backpack.Vowels(fruit)} {fruit}";
            }
            
            if (plural)
            {
                return $"{Quantity} rations of food";
            }
            return "some food";
        }
        
        public IItem Copy() => new Food(FoodType);
                
        public override bool Equals(object obj)
        {
            return obj is Food s && s.FoodType == FoodType;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(FoodType, Value, Cursed, Quantity);
        }
        
        public static Food Create() => new Food(Program.RNG.Next(2) == 1);
    }
}