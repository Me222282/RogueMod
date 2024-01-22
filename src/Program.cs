using System;
using Zene.Structs;
using CursesSharp;

namespace RogueMod
{
    public class Program
    {
        public static Random RNG = new Random();
        public static Properties Properties;
        
        private static void Main(string[] args)
        {
            Properties = new Properties();
            
            Program program = new Program();
            program.Run();
        }
        
        public static string GetMod(int i)
        {
            if (i < 0)
            {
                return $"-{Math.Abs(i)}";
            }
            
            return $"+{i}";
        }
        
        public Program()
        {
            Output = new Output(Properties.Size);
            Message = new MessageManager(Output);
        }
        
        public IMessageManager Message { get; }
        public IOutput Output { get; }
        public IPlayerActions Actions { get; private set; }
        public IRogue Game { get; private set; }
        
        private int _lastAction;
        
        public void Run()
        {
            Output.Clear();
            
            string name = Splash();
            
            Game = new Rogue(Properties.Size, Message);
            Actions = new PlayerActions(Output, Message, Game);
            
            Game.Render();
            
            Curtain();
            Game.Out.DirectOut = Output;
            
            while (true)
            {                
                int cki = Output.ReadKeyInput();
                if (cki == 'Q')
                {
                    if (IsQuit()) { break; }
                    
                    continue;
                }
                
                Message.Clear();
                ManageKeyInput(cki);
                Message.Manage();
            }
        }
        
        private void ManageKeyInput(int ch)
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
                    Game.Out.PrintAll();
                    return;
                case Controls.SymbolVis:
                    Stdscr.Clear();
                    Output.PrintList(Messages.SymbolVisual, (Output.Size.X / 2) < 37);
                    Game.Out.PrintAll();
                    return;
                case Controls.Inventory:
                    Actions.SelectItemChar(ItemType.Any);
                    return;
                case Controls.Wear:
                    Actions.Wear();
                    return;
                case Controls.Wield:
                    Actions.Wield();
                    return;
                case Controls.TakeOff:
                    Actions.TakeOff();
                    return;
                case Controls.Drop:
                    Actions.Drop();
                    return;
                case Controls.PutOnRing:
                    Actions.PutOnRing();
                    return;
                case Controls.RemoveRing:
                    Actions.TakeOffRing();
                    return;
                case Controls.RepeatB:
                case Controls.Repeat:
                    ManageKeyInput(_lastAction);
                    return;
                case (int)Direction.Up:
                case (int)Direction.Down:
                case (int)Direction.Left:
                case (int)Direction.Right:
                case (int)Direction.UpLeft:
                case (int)Direction.UpRight:
                case (int)Direction.DownLeft:
                case (int)Direction.DownRight:
                    Game.TryMovePlayer((Direction)ch);
                    return;
                case 'l':
                    Room room = Game.CurrentRoom as Room;
                    if (room is null) { return; }
                    room.Dark = false;
                    ((Rogue)Game).Eluminate(room);
                    return;
            }
        }
        
        private bool IsQuit()
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
        
        private string Splash()
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
        private void Curtain()
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
                Game.Out.PrintLine(l);
                Output.Pause(delay);
            }
        }
    }
}
