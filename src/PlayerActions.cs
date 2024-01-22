using System;
using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public static class PlayerActions
    {
        public static void Drop(IRogue game, IMessageManager message, IOutput direct)
        {
            IItem item = SelectItem(game, ItemType.Any, message, direct);
            IItem drop = item.Copy();
            if (item is null) { return; }
            if (item is Weapon && item.Stackable)
            {
                game.Player.Backpack.DropAll(item);
            }
            else
            {
                game.Player.Backpack.DropOne(item);
            }
            game.EntityManager.Place(game.Player.Position, drop);
        }
        
        public static void Wear(IRogue game, IMessageManager message, IOutput direct)
        {
            if (game.Player.Backpack.IsWearing)
            {
                message.Push(Messages.AlreadyArmour);
                return;
            }
            char wear = SelectItemChar(game, ItemType.Armour, message, direct);
            try
            {
                game.Player.Backpack.Wear(wear);
                IItem w = game.Player.Backpack.Wearing;
                w.MakeKnown(game);
                message.Push($"You are now wearing {w.ToString(game, false)}");
            }
            catch (Exception) { }
        }
        public static void Wield(IRogue game, IMessageManager message, IOutput direct)
        {
            if (game.Player.Backpack.IsWielding && game.Player.Backpack.Wielding.Cursed)
            {
                message.Push(Messages.Cursed);
                return;
            }
            
            char wield = SelectWeaponChar(game, message, direct);
            try
            {
                game.Player.Backpack.Wield(wield);
                IItem w = game.Player.Backpack.Wielding;
                message.Push($"You are now wielding {w.ToString(game, false)} ({wield})");
            }
            catch (Exception) { }
        }
        public static void TakeOff(IRogue game, IMessageManager message)
        {
            if (!game.Player.Backpack.IsWearing)
            {
                message.Push(Messages.NoArmour);
                return;
            }
            IItem oldWear = game.Player.Backpack.Wearing;
            if (!game.Player.Backpack.Wear('\0'))
            {
                message.Push(Messages.Cursed);
                return;
            }
            char c = game.Player.Backpack.GetChar(oldWear);
            message.Push($"You used to be wearing {c}) {oldWear.ToString(game, false)}");
        }
        public static void PutOnRing(IRogue game, IMessageManager message, IOutput direct)
        {
            if (game.Player.Backpack.IsLeftRing && game.Player.Backpack.IsRightRing)
            {
                message.Push(Messages.AllRings);
                return;
            }
            char wear = SelectItemChar(game, ItemType.Ring, message, direct);
            bool left = game.Player.Backpack.IsRightRing;
            
            IItem select;
            try
            {
                select = game.Player.Backpack[wear];
            }
            catch (Exception) { return; }
            
            if (select == game.Player.Backpack.LeftRing ||
                select == game.Player.Backpack.RightRing)
            {
                message.Push(Messages.AlreadyWearing);
                return;
            }
            
            if (!game.Player.Backpack.IsRightRing && !game.Player.Backpack.IsLeftRing)
            {
                direct.Write(0, 0, "Left hand or right hand? ", Attribute.Normal);
                char ch;
                do
                {
                    ch = Char.ToLower((char)direct.ReadKeyInput());
                    if (ch == Keys.ESC) { return; }
                } while (ch != 'l' && ch != 'r');
                left = ch == 'l';
                message.Clear();
            }
            
            try
            {
                game.Player.Backpack.WearRing(wear, left);
                IItem w = left ? game.Player.Backpack.LeftRing : game.Player.Backpack.RightRing;
                message.Push($"You are now wearing {w.ToString(game, false)} ({wear})");
            }
            catch (Exception) { }
        }
        public static void TakeOffRing(IRogue game, IMessageManager message, IOutput direct)
        {
            if (!game.Player.Backpack.IsLeftRing && !game.Player.Backpack.IsRightRing)
            {
                message.Push(Messages.NoRings);
                return;
            }
            bool left = game.Player.Backpack.IsLeftRing;
            if (game.Player.Backpack.IsRightRing && game.Player.Backpack.IsLeftRing)
            {
                direct.Write(0, 0, "Left hand or right hand? ", Attribute.Normal);
                char ch;
                do
                {
                    ch = Char.ToLower((char)direct.ReadKeyInput());
                    if (ch == Keys.ESC) { return; }
                } while (ch != 'l' && ch != 'r');
                left = ch == 'l';
            }
            
            IItem oldWear = left ? game.Player.Backpack.LeftRing : game.Player.Backpack.RightRing;
            if (!game.Player.Backpack.WearRing('\0', left))
            {
                message.Push(Messages.Cursed);
                return;
            }
            char c = game.Player.Backpack.GetChar(oldWear);
            message.Push($"You used to be wearing {c}) {oldWear.ToString(game, false)}");
        }
        public static void Throw(IRogue game, IMessageManager message, IOutput direct)
        {
            if (game.Player.Backpack.IsEmpty)
            {
                message.Push("You are empty handed");
                return;
            }
            
            direct.Write(0, 0, "Pick a direction", Attribute.Normal);
            int ch;
            do
            {
                ch = direct.ReadKeyInput();
                if (ch == Keys.ESC) { return; }
            } while (!ch.IsDirection());
            
            char th = SelectWeaponChar(game, message, direct);
            IItem item = null;
            try
            {
                item = game.Player.Backpack[th];
            }
            catch (Exception) { }
            if (item is null) { return; }
            
            Vector2I hitPos = game.RoomManager.GetHitCast(game.Player.Position, (Direction)ch);
            ICharacter hit = game.EntityManager.GetHitCast(game.Player.Position, (Direction)ch, hitPos);
            if (hit is not null)
            {
                hitPos = hit.Position;
            }
            hitPos -= ((Direction)ch).GetOffset();
            
            game.Player.Backpack.DropOne(item);
            char graphic = item.Graphic;
            
        }
        
        public static IItem SelectItem(IRogue game, ItemType type, IMessageManager message, IOutput direct)
        {
            char c = SelectItemChar(game, type, message, direct);
            if (c == '\0') { return null; }
            
            IItem item = null;
            try
            {
                item = game.Player.Backpack[c];
            }
            catch (Exception) { }
            
            if (item is null || (type != ItemType.Any && item.Type != type))
            {
                return null;
            }
            
            return item;
        }
        public static char SelectItemChar(IRogue game, ItemType type, IMessageManager message, IOutput direct)
        {   
            if (game.Player.Backpack.IsEmpty)
            {
                message.Push("You are empty handed");
                return '\0';
            }
            
            int c = 0;
            
            string[] list = game.Player.Backpack.GetFilteredItemList(game, type);
            
            if (list.IsNullFull())
            {
                message.Push("You don't have anything appropriate");
                return '\0';
            }
            
            direct.Clear();
            c = direct.PrintList(list, true, ch => char.IsLetter((char)ch));
            game.Out.PrintAll();
            
            return c == ' ' || c == Keys.ESC ? '\0' : (char)c;
        }
        public static char SelectWeaponChar(IRogue game, IMessageManager message, IOutput direct)
        {   
            if (game.Player.Backpack.IsEmpty)
            {
                message.Push("You are empty handed");
                return '\0';
            }
            
            int c = 0;
            
            string[] list = game.Player.Backpack.GetFilteredItemList(game,
                i => i.Attack != Damage.Zero);
            
            if (list.IsNullFull())
            {
                message.Push("You don't have anything appropriate");
                return '\0';
            }
            
            direct.Clear();
            c = direct.PrintList(list, true, ch => char.IsLetter((char)ch));
            game.Out.PrintAll();
            
            return c == ' ' || c == Keys.ESC ? '\0' : (char)c;
        }
    }
}