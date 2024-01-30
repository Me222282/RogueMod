using System;
using System.Collections;
using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public class EntityManager : IEntityManager
    {
        public EntityManager(Vector2I size)
        {
            Size = size;
            _map = new Node[size.Y, size.X];
        }
        
        public Vector2I Size { get; }
        private Node[,] _map;
        //private LinkedList<IEntity> _entities = new LinkedList<IEntity>();
        
        public void Delete(int x, int y, bool top = false)
        {
            Node n = _map[y, x];
            
            if (top && n.Character is not null)
            {
                _map[y, x].Character = null;
                return;
            }
            
            _map[y, x].Item = null;
        }
        public bool Place(Vector2I pos, IEntity entity)
        {
            (int x, int y) = (pos.X, pos.Y);
            
            if (entity.Character)
            {
                if (_map[y, x].Character is not null) { return false; }
                ICharacter c = (ICharacter)entity;
                c.Position = pos;
                _map[y, x].Character = c;
                //_entities.Add(entity);
                return true;
            }
            
            if (_map[y, x].Item is not null) { return false; }
            _map[y, x].Item = (IItem)entity;
            //_entities.Add(entity);
            return true;
        }
        public bool Shift(Vector2I start, Vector2I end)
        {
            if (_map[start.Y, start.X].Character is not null)
            {
                if (_map[end.Y, end.X].Character is not null) { return false; }
                ICharacter c = _map[start.Y, start.X].Character;
                _map[end.Y, end.X].Character = c;
                c.Position = end;
                _map[start.Y, start.X].Character = null;
                return true;
            }
            
            if (_map[end.Y, end.X].Item is not null) { return false; }
            _map[end.Y, end.X].Item = _map[start.Y, start.X].Item;
            _map[start.Y, start.X].Item = null;
            return true;
        }
        
        public ICharacter GetCharacter(int x, int y) => _map[y, x].Character;
        public IItem GetItem(int x, int y) => _map[y, x].Item;
        public IEntity GetTopEntity(int x, int y)
        {
            Node n = _map[y, x];
            
            if (n.Character is not null)
            {
                return n.Character;
            }
            
            return n.Item;
        }
        
        public ICharacter GetHitCast(Vector2I start, Direction dir, Vector2I end)
        {
            Vector2I offset = dir.GetOffset();
            
            ICharacter c;
            while ((c = _map[start.Y, start.X].Character) is null && start != end)
            {
                start += offset;
            }
            
            return c;
        }
        public Vector2I GetRandomPosition(RectangleI bounds)
        {
            int x = 0, y = 0;
            
            do
            {
                x = Program.RNG.Next(bounds.Left, bounds.Right);
                y = Program.RNG.Next(bounds.Y, bounds.Y + bounds.Height);
            } while (_map[y, x].Item is not null || _map[y, x].Character is not null);
            
            return (x, y);
        }
        
        public void Render(RectangleI bounds, IOutput scr)
        {
            int bot = bounds.Y + bounds.Height;
            for (int y = bounds.Y; y < bot; y++)
            {
                for (int x = bounds.X; x < bounds.Right; x++)
                {
                    Node n = _map[y, x];
                    if (n.Character is not null)
                    {
                        scr.Write(x, y, n.Character.Graphic);
                        continue;
                    }
                    if (n.Item is null) { continue; }
                    
                    scr.Write(x, y, n.Item.Graphic);
                }
            }
        }
        public void Hide(int x, int y, IOutput scr)
        {
            throw new NotImplementedException();
        }
        public void Show(int x, int y, IOutput scr)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerator<IEntity> GetEnumerator() => new Enumerator(_map);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_map);
        
        private struct Node
        {
            public IItem Item;
            public ICharacter Character;
        }
        private class Enumerator : IEnumerator<IEntity>
        {
            public Enumerator(Node[,] n)
            {
                _nodes = n;
                _size = (n.GetLength(1), n.GetLength(0));
                _pos = 0;
                _shifted = true;
            }
            
            private Vector2I _size;
            private Node[,] _nodes;
            private Vector2I _pos;
            private bool _shifted;
            
            public IEntity Current { get; private set; }
            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (_pos.Y >= _size.Y) { return false; }
                
                while (Current is null)
                {
                    Current = Next();
                    if (_pos.Y >= _size.Y) { return false; }
                }
                
                return true;
            }
            private void Shift()
            {
                _pos.X++;
                if (_pos.X >= _size.X)
                {
                    _pos.Y++;
                    _pos.X = 0;
                }
            }
            private IEntity Next()
            {
                Node n = _nodes[_pos.Y, _pos.X];
                if (_shifted)
                {
                    _shifted = false;
                    return n.Item;
                }
                Shift();
                _shifted = true;
                return n.Character;
            }
            
            public void Reset()
            {
                _pos = 0;
                Current = null;
            }
        }
    }
}