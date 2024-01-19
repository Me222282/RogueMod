using System;
using System.Collections.Generic;
using CursesSharp;

namespace RogueMod
{
    public class MessageManager
    {
        public MessageManager() { }
        public MessageManager(IOutput o)
        {
            Output = o;
        }
        
        public IOutput Output { get; set; }
        
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
            
            if (message == null) { return; }
            
            Output.Write(0, 0, message, Attrs.NORMAL);
            _lastMessage = message;
            
            if (carry)
            {
                Out.InvertColours();
                Output.Append(" More ");
            }
        }
        public void PushLastMessage() => Push(_lastMessage);
        
        public void Clear() => Output.ClearLine(0);
    }
}