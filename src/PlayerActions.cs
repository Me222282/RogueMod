using System;
using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public class PlayerActions : IPlayerActions
    {
        public PlayerActions(IOutput output, IMessageManager messages, IRogue game)
        {
            _output = output;
            _message = messages;
            _game = game;
        }
        
        private IOutput _output;
        public IOutput Ouput => _output;
        private IMessageManager _message;
        public IMessageManager Message => _message;
        private IRogue _game;
        public IRogue Game => _game;
        
        public void Drop()
        {
            IItem item = SelectItem(ItemType.Any);
            if (item is null) { return; }
            IItem drop = item.Copy();
            if (item is Weapon && item.Stackable)
            {
                _game.Player.Backpack.DropAll(item);
            }
            else
            {
                _game.Player.Backpack.DropOne(item);
            }
            _game.EntityManager.Place(_game.Player.Position, drop);
        }
        
        public void Wear()
        {
            if (_game.Player.Backpack.IsWearing)
            {
                _message.Push(Messages.AlreadyArmour);
                return;
            }
            char wear = SelectItemChar(ItemType.Armour);
            try
            {
                _game.Player.Backpack.Wear(wear);
                IItem w = _game.Player.Backpack.Wearing;
                w.MakeKnown(_game.Discoveries);
                _message.Push($"You are now wearing {w.ToString(_game.Discoveries, false)}");
            }
            catch (IndexOutOfRangeException) { }
        }
        public void Wield()
        {
            if (_game.Player.Backpack.IsWielding && _game.Player.Backpack.Wielding.Cursed)
            {
                _message.Push(Messages.Cursed);
                return;
            }
            
            char wield = SelectWeaponChar();
            try
            {
                _game.Player.Backpack.Wield(wield);
                IItem w = _game.Player.Backpack.Wielding;
                _message.Push($"You are now wielding {w.ToString(_game.Discoveries, false)} ({wield})");
            }
            catch (IndexOutOfRangeException) { }
        }
        public void TakeOff()
        {
            if (!_game.Player.Backpack.IsWearing)
            {
                _message.Push(Messages.NoArmour);
                return;
            }
            IItem oldWear = _game.Player.Backpack.Wearing;
            if (!_game.Player.Backpack.Wear('\0'))
            {
                _message.Push(Messages.Cursed);
                return;
            }
            char c = _game.Player.Backpack.GetChar(oldWear);
            _message.Push($"You used to be wearing {c}) {oldWear.ToString(_game.Discoveries, false)}");
        }
        public void PutOnRing()
        {
            if (_game.Player.Backpack.IsLeftRing && _game.Player.Backpack.IsRightRing)
            {
                _message.Push(Messages.AllRings);
                return;
            }
            char wear = SelectItemChar(ItemType.Ring);
            bool left = _game.Player.Backpack.IsRightRing;
            
            IItem select;
            try
            {
                select = _game.Player.Backpack[wear];
            }
            catch (IndexOutOfRangeException) { return; }
            
            if (select == _game.Player.Backpack.LeftRing ||
                select == _game.Player.Backpack.RightRing)
            {
                _message.Push(Messages.AlreadyWearing);
                return;
            }
            
            if (!_game.Player.Backpack.IsRightRing && !_game.Player.Backpack.IsLeftRing)
            {
                _output.Write(0, 0, "Left hand or right hand? ", Attribute.Normal);
                char ch;
                do
                {
                    ch = Char.ToLower((char)_output.ReadKeyInput());
                    if (ch == Keys.ESC) { return; }
                } while (ch != 'l' && ch != 'r');
                left = ch == 'l';
                _message.Clear();
            }
            
            try
            {
                _game.Player.Backpack.WearRing(wear, left);
                IItem w = left ? _game.Player.Backpack.LeftRing : _game.Player.Backpack.RightRing;
                _message.Push($"You are now wearing {w.ToString(_game.Discoveries, false)} ({wear})");
            }
            catch (Exception) { }
        }
        public void TakeOffRing()
        {
            if (!_game.Player.Backpack.IsLeftRing && !_game.Player.Backpack.IsRightRing)
            {
                _message.Push(Messages.NoRings);
                return;
            }
            bool left = _game.Player.Backpack.IsLeftRing;
            if (_game.Player.Backpack.IsRightRing && _game.Player.Backpack.IsLeftRing)
            {
                _output.Write(0, 0, "Left hand or right hand? ", Attribute.Normal);
                char ch;
                do
                {
                    ch = Char.ToLower((char)_output.ReadKeyInput());
                    if (ch == Keys.ESC) { return; }
                } while (ch != 'l' && ch != 'r');
                left = ch == 'l';
            }
            
            IItem oldWear = left ? _game.Player.Backpack.LeftRing : _game.Player.Backpack.RightRing;
            if (!_game.Player.Backpack.WearRing('\0', left))
            {
                _message.Push(Messages.Cursed);
                return;
            }
            char c = _game.Player.Backpack.GetChar(oldWear);
            _message.Push($"You used to be wearing {c}) {oldWear.ToString(_game.Discoveries, false)}");
        }
        public void Throw()
        {
            if (_game.Player.Backpack.IsEmpty)
            {
                _message.Push("You are empty handed");
                return;
            }
            
            _output.Write(0, 0, "Pick a direction", Attribute.Normal);
            int ch;
            do
            {
                ch = _output.ReadKeyInput();
                if (ch == Keys.ESC) { return; }
            } while (!ch.IsDirection());
            
            char th = SelectWeaponChar();
            IItem item = null;
            try
            {
                item = _game.Player.Backpack[th];
            }
            catch (IndexOutOfRangeException) { }
            if (item is null) { return; }
            
            Vector2I offset = ((Direction)ch).GetOffset();
            Vector2I hitPos = _game.RoomManager.GetHitCast(_game.Player.Position, (Direction)ch);
            ICharacter hit = _game.EntityManager.GetHitCast(_game.Player.Position + offset, (Direction)ch, hitPos);
            if (hit is not null)
            {
                hitPos = hit.Position;
            }
            hitPos -= offset;
            
            _game.Player.Backpack.DropOne(item);
            char graphic = item.Graphic;
            
            // Add graphical offset
            _output.Animate(_game.Player.Position + offset + (0, 1), hitPos + (0, 1),
                graphic, Attribute.GetAttribute((Draw)graphic));
            
            if (hit is not null)
            {
                hit.Damage(item, true);
                return;
            }
            
            // If it cant place it, then who cares, player misses out
            _game.EntityManager.Place(hitPos, item.Copy());
            _game.EntityManager.Render(new RectangleI(hitPos, Vector2I.One), _game.Out[1]);
        }
        
        public IItem SelectItem(ItemType type)
        {
            char c = SelectItemChar(type);
            if (c == '\0') { return null; }
            
            IItem item = null;
            try
            {
                item = _game.Player.Backpack[c];
            }
            catch (IndexOutOfRangeException) { }
            
            if (item is null || (type != ItemType.Any && item.Type != type))
            {
                return null;
            }
            
            return item;
        }
        public char SelectItemChar(ItemType type)
        {   
            if (_game.Player.Backpack.IsEmpty)
            {
                _message.Push("You are empty handed");
                return '\0';
            }
            
            int c = 0;
            
            string[] list = _game.Player.Backpack.GetFilteredItemList(_game, type);
            
            if (list.IsNullFull())
            {
                _message.Push("You don't have anything appropriate");
                return '\0';
            }
            
            _output.Clear();
            c = _output.PrintList(list, true, ch => char.IsLetter((char)ch));
            _game.Out.PrintAll();
            Message.Clear();
            
            return c == ' ' || c == Keys.ESC ? '\0' : (char)c;
        }
        public char SelectWeaponChar()
        {   
            if (_game.Player.Backpack.IsEmpty)
            {
                _message.Push("You are empty handed");
                return '\0';
            }
            
            int c = 0;
            
            string[] list = _game.Player.Backpack.GetFilteredItemList(_game,
                i => i.Attack != Damage.Zero);
            
            if (list.IsNullFull())
            {
                _message.Push("You don't have anything appropriate");
                return '\0';
            }
            
            _output.Clear();
            c = _output.PrintList(list, true, ch => char.IsLetter((char)ch));
            _game.Out.PrintAll();
            Message.Clear();
            
            if (!list.Exists(s => s != null && s[0] == c)) { return '\0'; }
            
            return c == ' ' || c == Keys.ESC ? '\0' : (char)c;
        }
    }
}