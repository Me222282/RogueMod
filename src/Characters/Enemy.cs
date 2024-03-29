using System;
using Zene.Structs;

namespace RogueMod
{
    public abstract class Enemy : ICharacter
    {
        public Enemy(char graphic, int packSize, int hp)
        {
            Graphic = graphic;
            
            Backpack = new Backpack(packSize);
            
            MaxHp = hp;
            HP = hp;
        }
        
        public double HP { get; set; }
        public double MaxHp { get; set; }
        
        public double Strength { get; set; } = 1;
        public int BaseArmour { get; set; }
        public int ArmourClass
        {
            get
            {
                Armour a = (Armour)Backpack.Wearing;
                
                if (a == null)
                {
                    return BaseArmour;
                }
                
                return a.ArmourClass + a.Modifier;
            }
        }
        public int Level { get; set; } = 0;
        public EntityFlags Flags { get; set; }
        public bool PickupItems { get; set; }
        
        public Backpack Backpack { get; }
        
        public char Graphic { get; }
        public Vector2I Position { get; set; }
        
        public abstract void Action(IRogue game);
        public abstract void Damage(IItem source, bool thrown);
    }
}