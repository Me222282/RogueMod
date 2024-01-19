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
            Out.Output = (Output)Output;
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
            Output.DefaultAttribute = rev;
            Output.Append('Y');
            Output.DefaultAttribute = Attribute.Normal;
            Output.Append("es/");
            Output.DefaultAttribute = rev;
            Output.Append('N');
            Output.DefaultAttribute = Attribute.Normal;
            Output.Append("o) ?");
            
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
                    Out.PrintList(Messages.ControlVisual, (Out.Width / 2) < 37, Out.DefaultOnChar);
                    game.Out.PrintAll();
                    return;
                case Controls.SymbolVis:
                    Stdscr.Clear();
                    Out.PrintList(Messages.SymbolVisual, (Out.Width / 2) < 37, Out.DefaultOnChar);
                    game.Out.PrintAll();
                    return;
                case Controls.Inventory:
                    PlayerActions.SelectItemChar(game, ItemType.Any);
                    return;
                case Controls.Wear:
                    PlayerActions.Wear(game);
                    return;
                case Controls.Wield:
                    PlayerActions.Wield(game);
                    return;
                case Controls.TakeOff:
                    PlayerActions.TakeOff(game);
                    return;
                case Controls.Drop:
                    PlayerActions.Drop(game);
                    return;
                case Controls.PutOnRing:
                    PlayerActions.PutOnRing(game);
                    return;
                case Controls.RemoveRing:
                    PlayerActions.TakeOffRing(game);
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
            Output.RenderBoxD(0, 0, Out.Width, Out.Height);
            
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
            Output.RenderLineH(1, 22, (char)Draw.WallH, Out.Width - 2);
            Output.Append('╣');
            //Stdscr.Standend();
            
            //Stdscr.Refresh();
            
            Output.Write(23, 2, "Rogue's Name? ");
            Output.DefaultAttribute = Attribute.White;
            return Output.ReadString(23);
        }
        private static void Curtain(Rogue game)
        {
            int delay = Properties.CurtainTime / Out.Height;
            
            Output.RenderBoxS(0, 0, Out.Width, Out.Height);
            Output.Pause(delay);
            
            // Lower curtain
            Out.SetColour(Colours.Yellow);
            for (int l = 1; l < Out.Height - 1; l++)
            {
                Output.RenderLineH(1, l, (char)Draw.Passage, Out.Width - 2);
                Output.Pause(delay);
            }
            Output.Pause(delay);
            
            // Raise curtain
            
            for (int l = Out.Height - 1; l >= 0; l--)
            {
                game.Out.PrintLine(l);
                Output.Pause(delay);
            }
        }
    }
}
