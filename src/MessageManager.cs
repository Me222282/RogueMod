using System;
using System.Collections.Generic;
using CursesSharp;

namespace RogueMod
{
    public class MessageManager
    {
        private List<string> _messages = new List<string>();
        
        public void Push(string message) => _messages.Add(message);
        
        public void Manage()
        {
            int end = (_messages.Count - 1);
            for (int i = 0; i < _messages.Count; i++)
            {
                Print(_messages[i], i != end);
                if (i == end) { break; }
                
                int ch;
                do
                {
                    ch = Stdscr.GetChar();
                } while (ch != ' ');
            }
            _messages.Clear();
        }
        
        private string _lastMessage;
        private void Print(string message, bool carry)
        {
            Clear();
            
            Out.ColourNormal();
            
            Stdscr.Add(0, 0, message);
            _lastMessage = message;
            
            if (carry)
            {
                Out.InvertColours();
                Stdscr.Add(" More ");
            }
        }
        public void PushLastMessage() => Push(_lastMessage);
        
        public void Clear()
        {
            Stdscr.Move(0, 0);
            Stdscr.ClearToEol();
        }
    }
}