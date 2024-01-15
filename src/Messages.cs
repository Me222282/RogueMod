namespace RogueMod
{
    public static class Messages
    {
        public const string Cursed = "You can't.  It appears to be cursed";
        public const string NoRings = "You aren't wearing any rings";
        public const string AllRings = "You already have a ring on each hand";
        public const string AlreadyWearing = "You are already wearing that";
        public const string NoArmour = "You aren't wearing any amour";
        public const string AlreadyArmour = "You are already wearing some. You'll have to take it off first.";
        
        public const string DoorLocked = "The door is locked!";
        public const string UnlockedDoor = "You unlocked the door";
        
        public static readonly string[] ControlVisual = new string[]
        {
            "F1     list of commands",
            "F2     list of symbols",
            "F3     repeat command",
            "F4     repeat message",
            "F5     rename something",
            "F6     recall what's been discovered",
            "F7     inventory of your possessions",
            "F8     <dir> identify trap type",
            "F9     The Any Key (definable)",
            "Alt F9 defines the Any Key",
            "F10    Supervisor Key (fake dos)",
            "Space  Clear -More- message",
            "◄┘     the Enter Key",
            "←      left",
            "↓      down",
            "↑      up",
            "→      right",
            "Home   up & left",
            "PgUp   up & right",
            "End    down & left",
            "PgDn   down & right",
            "Scroll Fast Play mode",
            ".      rest",
            ">      go down a staircase",
            "<      go up a staircase",
            "Esc    cancel command",
            "d      drop object",
            "e      eat food",
            "f      <dir> find something",
            "q      quaff potion",
            "r      read paper",
            "s      search for trap/secret door",
            "t      <dir> throw something",
            "w      wield a weapon",
            "z      <dir> zap with a wand",
            "B      run down & left",
            "H      run left",
            "J      run down",
            "K      run up",
            "L      run right",
            "N      run down & right",
            "U      run up & right",
            "Y      run up & left",
            "W      wear armor",
            "T      take armor off",
            "P      put on ring",
            "Q      quit",
            "R      remove ring",
            "S      save game",
            "^      identify trap",
            "?      help",
            "/      key",
            "+      throw",
            "-      zap",
            "Ctrl t terse message format",
            "Ctrl r repeat message",
            "Del    search for something hidden",
            "Ins    <dir> find something",
            "a      repeat command",
            "c      rename something",
            "i      inventory",
            "v      version number",
            "!      Supervisor Key (fake DOS)",
            "D      list what has been discovered",
        };
        public static readonly string[] SymbolVisual = new string[]
        {
            $"{(char)Draw.Floor}: the floor",
            $"{(char)Draw.Player}: the hero",
            $"{(char)Draw.Food}: some food",
            $"{(char)Draw.Amulet}: the amulet of yendor",
            $"{(char)Draw.Key}: a key",
            $"{(char)Draw.Scroll}: a scroll",
            $"{(char)Draw.Weapon}: a weapon",
            $"{(char)Draw.Armour}: a piece of armor",
            $"{(char)Draw.Gold}: some gold",
            $"{(char)Draw.Staff}: a magic staff",
            $"{(char)Draw.Potion}: a potion",
            $"{(char)Draw.Ring}: a magic ring",
            $"{(char)Draw.PassageB}: a passage",
            $"{(char)Draw.Door}: a door",
            $"{(char)Draw.WallTL}: an upper left corner",
            $"{(char)Draw.Trap}: a trap",
            $"{(char)Draw.WallH}: a horizontal wall",
            $"{(char)Draw.WallBR}: a lower right corner",
            $"{(char)Draw.WallBL}: a lower left corner",
            $"{(char)Draw.WallV}: a vertical wall",
            $"{(char)Draw.WallTR}: an upper right corner",
            $"{(char)Draw.Stairs}: a stair case",
            $"{(char)Draw.Magic},{(char)Draw.MagicB}: safe and perilous magic",
            "A-Z: 26 different monsters"
        };
        
    }
}