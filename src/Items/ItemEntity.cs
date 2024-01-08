using System;
using Zene.Structs;

namespace RogueMod
{
    public sealed class ItemEntity : IEntity
    {
        public ItemEntity(IItem item, Vector2I pos)
        {
            Item = item;
            Position = pos;
            SetGraphic(item.Type);
        }
        public ItemEntity(IItem item)
        {
            Item = item;
            SetGraphic(item.Type);
        }
        
        public IItem Item { get; }
        
        public Vector2I Position { get; set; }
        public char Graphic { get; private set; }
        public char UnderChar { get; set; }
        public bool Seen { get; set; } = false;
        
        private void SetGraphic(ItemType type)
        {
            switch (type)
            {
                case ItemType.Food:
                    Graphic = (char)Draw.Food;
                    return;
                    
                case ItemType.Armour:
                    Graphic = (char)Draw.Armour;
                    return;
                    
                case ItemType.Key:
                    Graphic = (char)Draw.Key;
                    return;
                
                case ItemType.Amulet:
                    Graphic = (char)Draw.Amulet;
                    return;
                    
                case ItemType.Potion:
                    Graphic = (char)Draw.Potion;
                    return;
                    
                case ItemType.Ring:
                    Graphic = (char)Draw.Ring;
                    return;
                    
                case ItemType.Scroll:
                    Graphic = (char)Draw.Scroll;
                    return;
                    
                case ItemType.Staff:
                    Graphic = (char)Draw.Staff;
                    return;
                    
                case ItemType.Weapon:
                    Graphic = (char)Draw.Weapon;
                    return;
            }
        }
    }
}