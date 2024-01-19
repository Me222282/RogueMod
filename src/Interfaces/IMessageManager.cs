namespace RogueMod
{
    public interface IMessageManager
    {
        public IOutput Output { get; set; }
        
        public void Push(string message);
        public void Manage();
        public void PushLastMessage();
        public void Clear();
    }
}