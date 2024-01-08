using System;
using Zene.Structs;

namespace RogueMod
{
    public enum EntityFlags
    {
        Blind           = 0x0001,
        Confused        = 0x0002,
        Running         = 0x0004,
        Invisable       = 0x0008,
        Regen           = 0x0010,
        SeeInvisable    = 0x0020,
        Slow            = 0x0040,
        Haste           = 0x0080,
        Fly             = 0x0100,
        Mean            = 0x0200,
        Greed           = 0x0400,
        // May not be used
        Held            = 0x0800,
        Found           = 0x1000,
        SeeMonst        = 0x2000,
        Cancelled       = 0x4000,
        CanConfuse      = 0x8000
    }
    
    public abstract class Character : IDisplayable, IEntity
    {
        public Character(char graphic, int packSize, int hp)
        {
            Graphic = graphic;
            
            Backpack = new Backpack(packSize);
            
            _maxHp = hp;
            HP = hp;
        }
        
        public double HP { get; set; }
        private double _maxHp;
        public double MaxHp
        {
            get => _maxHp;
            set
            {
                double p = HP / _maxHp;
                _maxHp = value;
                HP = p * _maxHp;
            }
        }
        
        public bool Seen { get; set; }
        
        public double Strength { get; set; } = 1;
        public int BaseArmour { get; set; } = 1;
        public int Level { get; set; } = 0;
        public EntityFlags Flags { get; set; }
        
        public Backpack Backpack { get; }
        
        public Vector2I Position { get; set; }
        public char Graphic { get; }
        public char UnderChar { get; set; }
        
        public int GetArmour()
        {
            Armour a = (Armour)Backpack.Wearing;
            
            if (a == null)
            {
                return BaseArmour;
            }
            
            return a.ArmourClass + a.Modifier;
        }
    }
}