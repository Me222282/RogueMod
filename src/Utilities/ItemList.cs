using System.Collections;
using System.Collections.Generic;

namespace RogueMod
{
    public class ItemList : IEnumerable<IItem>
    {
        public ItemList(int cap)
        {
            Capacity = cap;
        }
        
        private Node _first;
        private Node _last;
        
        public int Length { get; private set; } = 0;
        public int Capacity { get; }
        
        private class Node
        {
            public Node(IItem i, Node n)
            {
                Item = i;
                Next = n;
            }
            
            public IItem Item;
            public Node Next;
        }
        
        public IItem this[char select]
        {
            get
            {
                Node item = _first;
                int end = select - 'a';
                int leng = Length / 2;
                if (end < leng)
                {
                    for (int i = 0; i < leng; i++)
                    {
                        item = item.Next;
                    }
                    return item.Item;
                }
                
                for (int i = Length - 1; i >= leng; i--)
                {
                    item = item.Next;
                }
                
                return item.Item;
            }
        }
        public bool this[IItem item]
        {
            get
            {
                Node node = _first;
                for (int i = 0; i < Length; i++)
                {
                    if (node.Item.Equals(item))
                    {
                        return true;
                    }
                    
                    node = node.Next;
                }
                
                return false;
            }
        }
        
        public bool Add(IItem item)
        {
            if (item is null) { return false; }
            if (!item.Stackable && Length >= Capacity) { return false; }
            
            Node last = null;
            Node n = _first;
            if (n is null)
            {
                _first = new Node(item, null);
                _last = _first;
                Length++;
                return true;
            }
            if (item.Stackable && n.Item.Equals(item))
            {
                n.Item.Quantity += item.Quantity;
                return true;
            }
            while (n.Item.Type < item.Type)
            {   
                last = n;
                n = n.Next;
                
                if (item.Stackable && n.Item.Equals(item))
                {
                    n.Item.Quantity += item.Quantity;
                    return true;
                }
            }
            
            if (Length >= Capacity) { return false; }
            Length++;
            Node w = new Node(item, n);
            
            last.Next = w;
            if (n == null) { _last = w; }
            return true;
        }
        public bool Remove(IItem item)
        {
            Node pre = null;
            Node n = _first;
            if (n is null) { return false; }
            for (int i = 0; i < Length; i++)
            {
                if (!n.Item.Equals(item))
                {
                    pre = n;
                    n = n.Next;
                    continue;
                }
                if (n.Item.Stackable && n.Item.Quantity > 1)
                {
                    n.Item.Quantity -= item.Quantity;
                    return true;
                }
                
                Length--;
                
                if (pre == null)
                {
                    _first = n;
                    return true;
                }
                pre.Next = n.Next;
                if (n.Next == null) { _last = pre; }
                return true;
            }
            
            return false;
        }
        public bool Remove(IItem item, int qauntity)
        {
            Node pre = null;
            Node n = _first;
            if (n is null) { return false; }
            for (int i = 0; i < Length; i++)
            {
                if (!n.Item.Equals(item))
                {
                    pre = n;
                    n = n.Next;
                    continue;
                }
                if (n.Item.Stackable && n.Item.Quantity > 1)
                {
                    n.Item.Quantity -= qauntity;
                    return true;
                }
                
                Length--;
                
                if (pre == null)
                {
                    _first = n;
                    return true;
                }
                pre.Next = n.Next;
                if (n.Next == null) { _last = pre; }
                return true;
            }
            
            return false;
        }
        
        public IEnumerator<IItem> GetEnumerator() => new Enumerator(_first);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_first);

        private class Enumerator : IEnumerator<IItem>
        {
            public Enumerator(Node first)
            {
                _current = new Node(null, first);
                _start = _current;
            }
            
            private Node _current;
            private Node _start;
            public IItem Current => _current.Item;
            object IEnumerator.Current => _current.Item;

            public void Dispose() { }

            public bool MoveNext()
            {
                _current = _current.Next;
                
                return _current is not null;
            }

            public void Reset()
            {
                _current = _start;
            }
        }
    }
}