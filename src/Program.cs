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
            Out.ColourNormal();
            Output.Write(0, 0, "Do you wish to end your quest now (");
            Out.InvertColours();
            Output.Append('Y');
            Out.InvertColours();
            Output.Append("es/");
            Out.InvertColours();
            Output.Append('N');
            Out.InvertColours();
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
            Out.RenderBoxD(0, 0, Out.Width, Out.Height);
            
            Out.SetColour(Colours.Normal, true);
            Out.Centre(2, "ROGUE:  The Adventure Game");
            Out.SetColour(Colours.Magenta);
            Out.Centre(4, "The game of Rogue was designed by:");
            Out.SetColour(Colours.White);
            Out.Centre(6, "Michael Toy and Glenn Wichman");
            Out.SetColour(Colours.Magenta);
            Out.Centre(9, "Various implementations by:");
            Out.SetColour(Colours.White);
            Out.Centre(11, "Ken Arnold, Jon Lane, Michael Toy and Owen Fraser");
            Out.SetColour(Colours.Magenta);
            Out.Centre(14, "Adapted for the IBM PC by:");
            Out.SetColour(Colours.White);
            Out.Centre(16, "A.I. Design");
            Out.SetColour(Colours.Yellow);
            Out.Centre(19, "(C)Copyright 1985");
            Out.SetColour(Colours.White);
            Out.Centre(20, "Epyx Incorporated");
            Out.SetColour(Colours.Yellow);
            Out.Centre(21, "All Rights Reserved");
            
            Out.SetColour(Colours.Brown);
            Stdscr.AddW(22, 0, '╠');
            Out.RenderLineH(1, 22, (char)Draw.WallH, Out.Width - 2);
            Stdscr.AddW('╣');
            Stdscr.Standend();
            
            Stdscr.Refresh();
            
            Stdscr.Add(23, 2, "Rogue's Name? ");
            Out.SetColour(Colours.White);
            Curses.Echo = true;
            Curses.CursorVisibility = 1;
            string name =  Stdscr.GetString(23);
            Curses.Echo = false;
            Curses.CursorVisibility = 0;
            return name;
        }
        private static void Curtain(Rogue game)
        {
            int delay = Properties.CurtainTime / Out.Height;
            
            Out.RenderBoxS(0, 0, Out.Width, Out.Height);
            Stdscr.Refresh();
            Curses.NapMs(delay);
            
            // Lower curtain
            Out.SetColour(Colours.Yellow);
            for (int l = 1; l < Out.Height - 1; l++)
            {
                Out.RenderLineH(1, l, (char)Draw.Passage, Out.Width - 2);
                Stdscr.Refresh();
                Curses.NapMs(delay);
            }
            Curses.NapMs(delay);
            
            // Raise curtain
            
            for (int l = Out.Height - 1; l >= 0; l--)
            {
                game.Out.PrintLine(l);
                Stdscr.Refresh();
                Curses.NapMs(delay);
            }
        }
    }
}
