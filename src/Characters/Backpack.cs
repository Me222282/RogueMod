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
        
        public IItem Wielding { get; private set; }
        public IItem Wearing { get; private set; }
        public IItem LeftRing { get; private set; }
        public IItem RightRing { get; private set; }
        
        public bool IsWielding => Wielding is not null;
        public bool IsWearing => Wearing is not null;
        public bool IsLeftRing => LeftRing is not null;
        public bool IsRightRing => RightRing is not null;
        
        public bool IsFull => _holding.Length == _holding.Capacity;
        public bool IsEmpty => _holding.Length == 0;
        
        public IItem this[char select] => _holding[select];
        public bool this[IItem item] => _holding[item];
        
        public bool Wield(char select)
        {
            if (IsWielding && Wielding.Cursed) { return false; }
            
            if (select == '\0')
            {
                Wielding = null;
                return true;
            }
            
            Wielding = _holding[select];
            
            return true;
        }
        public bool Wear(char select)
        {
            if (IsWearing && Wearing.Cursed) { return false; }
            
            if (select == '\0')
            {
                Wearing = null;
                return true;
            }
            
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
            if (r is not null && r.Cursed) { return false; }
            
            IItem item = select =='\0' ? null : _holding[select];
            if (select !='\0' && item.Type != ItemType.Ring) { return false; }
            
            if (left)
            {
                LeftRing = item;
                return true;
            }
            
            RightRing = item;
            return true;
        }
        
        public bool Add(IItem item)
        {
            if (item.Quantity < 1)
            {
                item.Quantity = 1;
            }
            
            if (item.Type == ItemType.Gold)
            {
                Gold += item.Quantity;
                return true;
            }
            
            return _holding.Add(item);
        }
        public bool Remove(IItem item) => _holding.Remove(item);
        public bool DropOne(IItem item) => _holding.Remove(item, 1);
        public bool DropAll(IItem item) => _holding.Remove(item);
        
        public char GetChar(IItem item) => _holding.IndexOf(item);
        
        internal static string Vowels(string next)
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
        public string[] GetItemList(IRogue game)
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
        private string GetItemString(IItem item, IRogue game)
        {
            bool pl = item.Quantity > 1;
            string value = item.ToString(game, pl);
            
            string mod = "";
            if (item.Equals(Wielding))
            {
                mod = " (weapon in hand)";
            }
            else if (item.Equals(Wearing))
            {
                mod = " (being worn)";
            }
            else if (item.Equals(LeftRing))
            {
                mod = " (on left hand)";
            }
            else if (item.Equals(RightRing))
            {
                mod = " (on right hand)";
            }
            
            if (item.Type != ItemType.Amulet &&
                item.Type != ItemType.Armour &&
                item.Type != ItemType.Food)
            {
                return pl ? $"{item.Quantity} {value}{mod}" : $"{Vowels(value)} {value}{mod}";
            }
            return value + mod;
        }
        public string[] GetFilteredItemList(IRogue game, ItemType type)
        {
            string[] l = new string[_holding.Length];
            
            char a = 'a';
            foreach (IItem item in _holding)
            {
                if (type != ItemType.Any && item.Type != type)
                {
                    a++;
                    continue;
                }
                
                l[a - 'a'] = $"{a}) {GetItemString(item, game)}";
                a++;
            }
            
            return l;
        }
        public string[] GetFilteredItemList(IRogue game, Func<IItem, bool> filter)
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
                
                l[a - 'a'] = $"{a}) {GetItemString(item, game)}";
                a++;
            }
            
            return l;
        }
    }
}