using System;
using CursesSharp;

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
        
        public bool IsWeilding => Weilding is not null;
        public bool IsWearing => Wearing is not null;
        public bool IsLeftRing => LeftRing is not null;
        public bool IsRightRing => RightRing is not null;
        
        public bool IsFull => _holding.Length == _holding.Capacity;
        public bool IsEmpty => _holding.Length == 0;
        
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
        
        private static string Vowels(string next)
        {
            switch (next[0])
            {
                case 'a': case 'A':
                case 'e': case 'E':
                case 'i': case 'I':
                case 'o': case 'O':
                case 'u': case 'U':
                    return "An";
                
                default:
                    return "A";
            }
        }
        public string[] GetItemList(Rogue game)
        {
            string[] l = new string[_holding.Length];
            
            char a = 'a';
            foreach (IItem item in _holding)
            {
                l[a - 'a'] = $"{a}) {GetItemString(item, game)}";
                a++;
            }
            
            return l;
        }
        private string GetItemString(IItem item, Rogue game)
        {
            bool pl = item.Quantity > 1;
            string value = item.ToString(game, pl);
            
            if (item.Type != ItemType.Amulet)
            {
                return pl ? $"{item.Quantity} {value}" : $"{Vowels(value)} {value}";
            }
            return value;
        }
        public string[] GetFilteredItemList(Rogue game, ItemType type)
        {
            string[] l = new string[_holding.Length];
            
            char a = 'a';
            foreach (IItem item in _holding)
            {
                if (item.Type != type)
                {
                    a++;
                    continue;
                }
                
                l[a - 'a'] = $"{a} ) {GetItemString(item, game)}";
                a++;
            }
            
            return l;
        }
        public string[] GetFilteredItemList(Rogue game, Func<IItem, bool> filter)
        {
            string[] l = new string[_holding.Length];
            
            char a = 'a';
            foreach (IItem item in _holding)
            {
                if (!filter(item))
                {
                    a++;
                    continue;
                }
                
                l[a - 'a'] = $"{a} ) {GetItemString(item, game)}";
                a++;
            }
            
            return l;
        }
    }
}