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
    
    public interface ICharacter : IEntity
    {
        public double HP { get; set; }
        public int Level { get; }
        public double Strength { get; set; }
        public int ArmourClass { get; }
        public EntityFlags Flags { get; set; }
        public bool PickupItems { get; }
        
        public Backpack Backpack { get; }
        
        bool IEntity.Character => true;
        public Vector2I Position { get; set; }
        
        public void Action(IRogue game);
        public void Damage(IItem source, bool thrown);
    }
}