using System.Collections.Generic;

namespace RogueMod
{
    public enum ItemType
    {
        Food,
        Scroll,
        Key,
        Ring,
        Staff,
        Potion,
        Armour,
        Weapon,
        Amulet,
        Gold,
        MaxValue,
        Any
    }
    
    public interface IItem : IEntity
    {
        public ItemType Type { get; }
        public Damage Attack { get; }
        public bool Cursed { get; set; }
        public int Quantity { get; set; }
        public bool Stackable { get; }
        
        public int Value { get; }
        
        public bool IsKnown(Discoveries dics);
        public void MakeKnown(Discoveries dics);
        
        public void Effect(ICharacter character, IRogue game);
        public string ToString(Discoveries dics, bool plural);
        
        public IItem Copy();
        
        bool IEntity.Character => false;
        
        public class Comparer : IComparer<IItem>
        {
            public int Compare(IItem x, IItem y)
            {
                if (x is null)
                {
                    if (y is null) { return 0; }
                    
                    return 1;
                }
                if (y is null) { return -1; }
                
                if (x.Type < y.Type)
                {
                    return -1;
                }
                if (x.Type > y.Type)
                {
                    return 1;
                }
                
                //Comparer<string> c = Comparer<string>.Default;
                //return c.Compare(x.Name, y.Name);
                
                return 0;
            }
        }
    }
}