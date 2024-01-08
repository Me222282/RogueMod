using System;

namespace RogueMod
{
    public class Backpack
    {
        public Backpack(int size = 23)
        {
            BackpackSize = size;
            
            _holding = new ItemList(BackpackSize);
        }
        
        public int BackpackSize { get; }
        
        public int Gold { get; set; }
        
        private ItemList _holding;
        
        public IItem Weilding { get; private set; }
        public IItem Wearing { get; private set; }
        public IItem LeftRing { get; private set; }
        public IItem RightRing { get; private set; }
        
        public IItem this[char select] => _holding[select];
        public bool this[IItem item] => _holding[item];
        
        public bool Weild(char select)
        {
            if (Weilding.Cursed) { return false; }
            
            Weilding = _holding[select];
            
            return true;
        }
        public bool Wear(char select)
        {
            if (Wearing.Cursed) { return false; }
            
            IItem item = _holding[select];
            if (item.Type != ItemType.Armour)
            {
                return false;
            }
            
            Wearing = item;
            
            return true;
        }
        public bool WearRing(char select, bool left)
        {
            IItem r = left ? LeftRing : RightRing;
            if (r.Cursed) { return false; }
            
            IItem item = _holding[select];
            if (item.Type != ItemType.Ring)
            {
                return false;
            }
            
            if (left)
            {
                LeftRing = item;
                return true;
            }
            
            RightRing = item;
            return true;
        }
        
        public bool Add(IItem item) => _holding.Add(item);
        public bool Remove(IItem item) => _holding.Remove(item);
        
        private string Vowels(string next)
        {
            switch (next[0])
            {
                case 'a': case 'A':
                case 'e': case 'E':
                case 'i': case 'I':
                case 'o': case 'O':
                case 'u': case 'U':
                    return "An ";
                
                default:
                    return "A ";
            }
        }
        public void PrintItemList(Rogue game)
        {
            char a = 'a';
            foreach (IItem item in _holding)
            {
                PrintItem(item, game, a);
                a++;
            }
        }
        private void PrintItem(IItem item, Rogue game, char a)
        {
            bool pl = item.Quantity > 1;
            string value = item.ToString(game, pl);
            
            Console.Write(a);
            Console.Write(" ) ");
            if (item.Type != ItemType.Amulet)
            {
                Console.Write(pl ? $"{item.Quantity} " : Vowels(value));
            }
            Console.Write(value);
            Console.WriteLine();
        }
        public void PrintFilteredItemList(Rogue game, ItemType type)
        {
            char a = 'a';
            foreach (IItem item in _holding)
            {
                if (item.Type != type)
                {
                    a++;
                    continue;
                }
                
                PrintItem(item, game, a);
                a++;
            }
        }
        public void PrintFilteredItemList(Rogue game, Func<IItem, bool> filter)
        {
            char a = 'a';
            foreach (IItem item in _holding)
            {
                if (!filter(item))
                {
                    a++;
                    continue;
                }
                
                PrintItem(item, game, a);
                a++;
            }
        }
    }
}