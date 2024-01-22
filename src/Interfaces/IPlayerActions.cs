namespace RogueMod
{
    public interface IPlayerActions
    {
        public void Drop();
        public void Wear();
        public void Wield();
        public void TakeOff();
        public void PutOnRing();
        public void TakeOffRing();
        public void Throw();
        
        public IItem SelectItem(ItemType type);
        public char SelectItemChar(ItemType type);
        public char SelectWeaponChar();
    }
}