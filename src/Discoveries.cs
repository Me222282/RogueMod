using System.Text;

namespace RogueMod
{
    public class Discoveries
    {
        public bool[] IsRings { get; } = new bool[(int)RingType.MaxValue];
        public string[] RingGuesses { get; } = new string[(int)RingType.MaxValue];
        
        public bool[] IsScrolls { get; } = new bool[(int)ScrollType.MaxValue];
        public string[] ScrollGuesses { get; } = new string[(int)ScrollType.MaxValue];
        
        public bool[] IsPotions { get; } = new bool[(int)PotionType.MaxValue];
        public string[] PotionGuesses { get; } = new string[(int)PotionType.MaxValue];
        
        public bool[] IsSticks { get; } = new bool[(int)StaffType.MaxValue];
        public string[] StickGuesses { get; } = new string[(int)StaffType.MaxValue];
        
        public string[] PotionNames { get; } = new string[(int)PotionType.MaxValue];
        public string[] RingNames { get; } = new string[(int)RingType.MaxValue];
        public string[] ScrollNames { get; } = new string[(int)ScrollType.MaxValue];
        public string[] StickNames { get; } = new string[(int)StaffType.MaxValue];
        public bool[] StickMaterial { get; } = new bool[(int)StaffType.MaxValue];
        
        public Discoveries()
        {
            InitPotions();
            InitRings();
            InitScrolls();
            InitStaffs();
        }
        private void InitPotions()
        {
            string[] names = new string[]
            {
                "amber",
                "aquamarine",
                "black",
                "blue",
                "brown",
                "clear",
                "crimson",
                "cyan",
                "ecru",
                "gold",
                "green",
                "grey",
                "magenta",
                "orange",
                "pink",
                "plaid",
                "purple",
                "red",
                "silver",
                "tan",
                "tangerine",
                "topaz",
                "turquoise",
                "vermilion",
                "violet",
                "white",
                "yellow"
            };
            
            bool[] used = new bool[names.Length];
            
            for (int i = 0; i < PotionNames.Length; i++)
            {
                int j;
                do
                {
                    j = Program.RNG.Next(names.Length);
                } while (used[j]);
                used[j] = true;
                
                PotionNames[i] = names[j];
            }
        }
        private void InitRings()
        {
            string[] names = new string[]
            {
                "agate",
                "alexandrite",
                "amethyst",
                "carnelian",
                "diamond",
                "emerald",
                "germanium",
                "granite",
                "garnet",
                "jade",
                "kryptonite",
                "lapis lazuli",
                "moonstone",
                "obsidian",
                "onyx",
                "opal",
                "pearl",
                "peridot",
                "ruby",
                "sapphire",
                "stibotantalite",
                "tiger eye",
                "topaz",
                "turquoise",
                "taaffeite",
                "zircon"
            };
            
            bool[] used = new bool[names.Length];
            
            for (int i = 0; i < RingNames.Length; i++)
            {
                int j;
                do
                {
                    j = Program.RNG.Next(names.Length);
                } while (used[j]);
                used[j] = true;
                
                RingNames[i] = names[j];
            }
        }
        private void InitScrolls()
        {
            string[] names = new string[(int)ScrollType.MaxValue];
            
            int nsyl;
            char[] sp;
            StringBuilder cp;
            int i, nwords;

            for (i = 0; i < names.Length; i++)
            {
                cp = new StringBuilder(20);
                nwords = Program.RNG.Next(4) + 2;
                while (nwords-- > 0)
                {
                    nsyl = Program.RNG.Next(2) + 1;
                    while (nsyl-- > 0)
                    {
                        // getsyl
                        sp = new char[]
                        {
                            _cSet.RndChar(),
                            _vSet.RndChar(),
                            _cSet.RndChar()
                        };
                        if ((cp.Length + sp.Length) > 20)
                        {
                            nwords = 0;
                            break;
                        }
                        cp.Append(sp);
                    }
                    cp.Append(' ');
                }
                cp = cp.Remove(cp.Length - 1, 1);
                
                ScrollNames[i] = cp.ToString();
            }
        }
        private static string _cSet = "bcdfghjklmnpqrstvwxyz";
        private static string _vSet = "aeiou";
        private void InitStaffs()
        {
            string[] nWood = new string[]
            {
                "avocado wood",
                "balsa",
                "bamboo",
                "banyan",
                "birch",
                "cedar",
                "cherry",
                "cinnibar",
                "cypress",
                "dogwood",
                "driftwood",
                "ebony",
                "elm",
                "eucalyptus",
                "fall",
                "hemlock",
                "holly",
                "ironwood",
                "kukui wood",
                "mahogany",
                "manzanita",
                "maple",
                "oaken",
                "persimmon wood",
                "pecan",
                "pine",
                "poplar",
                "redwood",
                "rosewood",
                "spruce",
                "teak",
                "walnut",
                "zebrawood"
            };
            string[] nMetal = new string[]
            {
                "aluminum",
                "beryllium",
                "bone",
                "brass",
                "bronze",
                "copper",
                "electrum",
                "gold",
                "iron",
                "lead",
                "magnesium",
                "mercury",
                "nickel",
                "pewter",
                "platinum",
                "steel",
                "silver",
                "silicon",
                "tin",
                "titanium",
                "tungsten",
                "zinc"
            };
            
            bool[] wUsed = new bool[nWood.Length];
            bool[] mUsed = new bool[nMetal.Length];
            
            for (int i = 0; i < StickNames.Length; i++)
            {
                int j;
                bool metal = Program.RNG.Next(2) == 0;
                bool[] u = metal ? mUsed : wUsed;
                string[] names = metal ? nMetal : nWood;
                
                do
                {
                    j = Program.RNG.Next(names.Length);
                } while (u[j]);
                u[j] = true;
                
                StickNames[i] = names[j];
                StickMaterial[i] = metal;
            }
        }
    }
}