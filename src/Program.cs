using System;
using Zene.Structs;
using CursesSharp;

namespace RogueMod
{
    public class Program
    {
        public static Random RNG = new Random();
        public static IMessageManager Message;
        public static Properties Properties;
        public static IOutput Output;
        
        private static void Main(string[] args)
        {
            Properties = new Properties();
            Output = new Output(Properties.Size);
            Message = new MessageManager(Output);
            
            Output.Clear();
            
            Splash();
            
            Rogue game = new Rogue(Properties.Size);
            
            game.Render();
            
            Curtain(game);
            game.Out.DirectOut = Output;
            
            while (true)
            {                
                int cki = Output.ReadKeyInput();
                if (cki == 'Q')
                {
                    if (IsQuit()) { break; }
                    
                    continue;
                }
                
                Message.Clear();
                ManageKeyInput(cki, game);
                Message.Manage();
            }
        }
        private static bool IsQuit()
        {
            Attribute rev = Attribute.Normal.GetReversed();
            
            Output.Write(0, 0, "Do you wish to end your quest now (", Attribute.Normal);
            Output.Append('Y', rev);
            Output.Append("es/", Attribute.Normal);
            Output.Append('N', rev);
            Output.Append("o) ?", Attribute.Normal);
            
            int cki = Output.ReadKeyInput();
            Output.ClearLine(0);
            return char.ToLower((char)cki) == 'y';
        }
        
        public static string GetMod(int i)
        {
            if (i < 0)
            {
                return $"-{Math.Abs(i)}";
            }
            
            return $"+{i}";
        }
        
        private static int _lastAction;
        private static void ManageKeyInput(int ch, Rogue game)
        {
            if (ch != Controls.Repeat &&
                ch != Controls.RepeatB)
            {
                _lastAction = ch;
            }
            
            switch (ch)
            {
                case Controls.LastMessage:
                case Controls.LastMessageB:
                    Message.PushLastMessage();
                    return;
                case Controls.ControlVis:
                    Stdscr.Clear();
                    Output.PrintList(Messages.ControlVisual, (Output.Size.X / 2) < 37);
                    game.Out.PrintAll();
                    return;
                case Controls.SymbolVis:
                    Stdscr.Clear();
                    Output.PrintList(Messages.SymbolVisual, (Output.Size.X / 2) < 37);
                    game.Out.PrintAll();
                    return;
                case Controls.Inventory:
                    PlayerActions.SelectItemChar(game, ItemType.Any, Message, Output);
                    return;
                case Controls.Wear:
                    PlayerActions.Wear(game, Message, Output);
                    return;
                case Controls.Wield:
                    PlayerActions.Wield(game, Message, Output);
                    return;
                case Controls.TakeOff:
                    PlayerActions.TakeOff(game, Message);
                    return;
                case Controls.Drop:
                    PlayerActions.Drop(game, Message, Output);
                    return;
                case Controls.PutOnRing:
                    PlayerActions.PutOnRing(game, Message, Output);
                    return;
                case Controls.RemoveRing:
                    PlayerActions.TakeOffRing(game, Message, Output);
                    return;
                case Controls.RepeatB:
                case Controls.Repeat:
                    ManageKeyInput(_lastAction, game);
                    return;
                case (int)Direction.Up:
                case (int)Direction.Down:
                case (int)Direction.Left:
                case (int)Direction.Right:
                case (int)Direction.UpLeft:
                case (int)Direction.UpRight:
                case (int)Direction.DownLeft:
                case (int)Direction.DownRight:
                    game.TryMovePlayer((Direction)ch);
                    return;
                case 'l':
                    Room room = game.CurrentRoom as Room;
                    if (room is null) { return; }
                    room.Dark = false;
                    game.Eluminate(room);
                    return;
            }
        }
        
        private static string Splash()
        {   
            Output.RenderBoxD(0, 0, Output.Size.X, Output.Size.Y);
            
            Output.Centre(2, "ROGUE:  The Adventure Game", Attribute.Normal.GetReversed());
            Output.Centre(4, "The game of Rogue was designed by:", Attribute.Magenta);
            Output.Centre(6, "Michael Toy and Glenn Wichman", Attribute.White);
            Output.Centre(9, "Various implementations by:", Attribute.Magenta);
            Output.Centre(11, "Ken Arnold, Jon Lane, Michael Toy and Owen Fraser", Attribute.White);
            Output.Centre(14, "Adapted for the IBM PC by:", Attribute.Magenta);
            Output.Centre(16, "A.I. Design", Attribute.White);
            Output.Centre(19, "(C)Copyright 1985", Attribute.Yellow);
            Output.Centre(20, "Epyx Incorporated", Attribute.White);
            Output.Centre(21, "All Rights Reserved", Attribute.Yellow);
            
            Output.Write(22, 0, '╠', Attribute.Brown);
            Output.RenderLineH(1, 22, (char)Draw.WallH, Output.Size.X - 2, Attribute.Brown);
            Output.Append('╣', Attribute.Brown);
            //Stdscr.Standend();
            
            //Stdscr.Refresh();
            
            Output.Write(23, 2, "Rogue's Name? ");
            return Output.ReadString(23);
        }
        private static void Curtain(Rogue game)
        {
            int delay = Properties.CurtainTime / Output.Size.Y;
            
            Output.RenderBoxS(0, 0, Output.Size.X, Output.Size.Y);
            Output.Pause(delay);
            
            // Lower curtain
            for (int l = 1; l < Output.Size.Y - 1; l++)
            {
                Output.RenderLineH(1, l, (char)Draw.Passage, Output.Size.X - 2, Attribute.Yellow);
                Output.Pause(delay);
            }
            Output.Pause(delay);
            
            // Raise curtain
            
            for (int l = Output.Size.Y - 1; l >= 0; l--)
            {
                game.Out.PrintLine(l);
                Output.Pause(delay);
            }
        }
    }
}
