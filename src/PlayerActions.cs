using System;
using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public static class PlayerActions
    {
        public static void Drop(IRogue game)
        {
            IItem item = SelectItem(game, ItemType.Any);
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
        
        public static void Wear(IRogue game)
        {
            if (game.Player.Backpack.IsWearing)
            {
                Program.Message.Push(Messages.AlreadyArmour);
                return;
            }
            char wear = SelectItemChar(game, ItemType.Armour);
            try
            {
                game.Player.Backpack.Wear(wear);
                IItem w = game.Player.Backpack.Wearing;
                w.MakeKnown(game);
                Program.Message.Push($"You are now wearing {w.ToString(game, false)}");
            }
            catch (Exception) { }
        }
        public static void Wield(IRogue game)
        {
            if (game.Player.Backpack.IsWielding && game.Player.Backpack.Wielding.Cursed)
            {
                Program.Message.Push(Messages.Cursed);
                return;
            }
            
            char wield = SelectWeaponChar(game);
            try
            {
                game.Player.Backpack.Wield(wield);
                IItem w = game.Player.Backpack.Wielding;
                Program.Message.Push($"You are now wielding {w.ToString(game, false)} ({wield})");
            }
            catch (Exception) { }
        }
        public static void TakeOff(IRogue game)
        {
            if (!game.Player.Backpack.IsWearing)
            {
                Program.Message.Push(Messages.NoArmour);
                return;
            }
            IItem oldWear = game.Player.Backpack.Wearing;
            if (!game.Player.Backpack.Wear('\0'))
            {
                Program.Message.Push(Messages.Cursed);
                return;
            }
            char c = game.Player.Backpack.GetChar(oldWear);
            Program.Message.Push($"You used to be wearing {c}) {oldWear.ToString(game, false)}");
        }
        public static void PutOnRing(IRogue game)
        {
            if (game.Player.Backpack.IsLeftRing && game.Player.Backpack.IsRightRing)
            {
                Program.Message.Push(Messages.AllRings);
                return;
            }
            char wear = SelectItemChar(game, ItemType.Ring);
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
                Program.Message.Push(Messages.AlreadyWearing);
                return;
            }
            
            if (!game.Player.Backpack.IsRightRing && !game.Player.Backpack.IsLeftRing)
            {
                Out.ColourNormal();
                Stdscr.Add(0, 0, "Left hand or right hand? ");
                char ch;
                do
                {
                    ch = Char.ToLower((char)Stdscr.GetChar());
                    if (ch == Keys.ESC) { return; }
                } while (ch != 'l' && ch != 'r');
                left = ch == 'l';
                Program.Message.Clear();
            }
            
            try
            {
                game.Player.Backpack.WearRing(wear, left);
                IItem w = left ? game.Player.Backpack.LeftRing : game.Player.Backpack.RightRing;
                Program.Message.Push($"You are now wearing {w.ToString(game, false)} ({wear})");
            }
            catch (Exception) { }
        }
        public static void TakeOffRing(IRogue game)
        {
            if (!game.Player.Backpack.IsLeftRing && !game.Player.Backpack.IsRightRing)
            {
                Program.Message.Push(Messages.NoRings);
                return;
            }
            bool left = game.Player.Backpack.IsLeftRing;
            if (game.Player.Backpack.IsRightRing && game.Player.Backpack.IsLeftRing)
            {
                Out.ColourNormal();
                Stdscr.Add(0, 0, "Left hand or right hand? ");
                char ch;
                do
                {
                    ch = Char.ToLower((char)Stdscr.GetChar());
                    if (ch == Keys.ESC) { return; }
                } while (ch != 'l' && ch != 'r');
                left = ch == 'l';
            }
            
            IItem oldWear = left ? game.Player.Backpack.LeftRing : game.Player.Backpack.RightRing;
            if (!game.Player.Backpack.WearRing('\0', left))
            {
                Program.Message.Push(Messages.Cursed);
                return;
            }
            char c = game.Player.Backpack.GetChar(oldWear);
            Program.Message.Push($"You used to be wearing {c}) {oldWear.ToString(game, false)}");
        }
        public static void Throw(IRogue game)
        {
            if (game.Player.Backpack.IsEmpty)
            {
                Program.Message.Push("You are empty handed");
                return;
            }
            
            Out.ColourNormal();
            Stdscr.Add(0, 0, "Pick a direction");
            int ch;
            do
            {
                ch = Stdscr.GetChar();
                if (ch == Keys.ESC) { return; }
            } while (!ch.IsDirection());
            
            char th = SelectWeaponChar(game);
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
        
        public static IItem SelectItem(IRogue game, ItemType type)
        {
            char c = SelectItemChar(game, type);
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
        public static char SelectItemChar(IRogue game, ItemType type)
        {   
            if (game.Player.Backpack.IsEmpty)
            {
                Program.Message.Push("You are empty handed");
                return '\0';
            }
            
            char c = '\0';
            
            string[] list = game.Player.Backpack.GetFilteredItemList(game, type);
            
            if (list.IsNullFull())
            {
                Program.Message.Push("You don't have anything appropriate");
                return '\0';
            }
            
            Stdscr.Clear();
            Out.PrintList(list, true, ch =>
            {
                if (ch == Keys.ESC) { return Out.Return.Return; }
                if (ch == ' ') { return Out.Return.Break; }
                if (char.IsLetter((char)ch))
                {
                    c = char.ToLower((char)ch);
                    return Out.Return.Break;
                }
                
                return Out.Return.Continue;
            });
            game.Out.PrintAll();
            
            return c;
        }
        public static char SelectWeaponChar(IRogue game)
        {   
            if (game.Player.Backpack.IsEmpty)
            {
                Program.Message.Push("You are empty handed");
                return '\0';
            }
            
            char c = '\0';
            
            string[] list = game.Player.Backpack.GetFilteredItemList(game,
                i => i.Attack != Damage.Zero);
            
            if (list.IsNullFull())
            {
                Program.Message.Push("You don't have anything appropriate");
                return '\0';
            }
            
            Stdscr.Clear();
            Out.PrintList(list, true, ch =>
            {
                if (ch == Keys.ESC) { return Out.Return.Return; }
                if (ch == ' ') { return Out.Return.Break; }
                if (char.IsLetter((char)ch))
                {
                    c = char.ToLower((char)ch);
                    return Out.Return.Break;
                }
                
                return Out.Return.Continue;
            });
            game.Out.PrintAll();
            
            return c;
        }
    }
}