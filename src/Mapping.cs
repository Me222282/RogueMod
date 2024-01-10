using System;
using System.Text;

namespace RogueMod
{
    public class Mapping
    {
        public string[] Potions { get; } = new string[(int)PotionType.MaxValue];
        public string[] Rings { get; } = new string[(int)RingType.MaxValue];
        public string[] Scrolls { get; } = new string[(int)ScrollType.MaxValue];
        public string[] Sticks { get; } = new string[(int)StaffType.MaxValue];
        public bool[] StickMaterial { get; } = new bool[(int)StaffType.MaxValue];
        
        public Mapping()
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
            
            for (int i = 0; i < Potions.Length; i++)
            {
                int j;
                do
                {
                    j = Program.RNG.Next(names.Length);
                } while (used[j]);
                used[j] = true;
                
                Potions[i] = names[j];
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
            
            for (int i = 0; i < Rings.Length; i++)
            {
                int j;
                do
                {
                    j = Program.RNG.Next(names.Length);
                } while (used[j]);
                used[j] = true;
                
                Rings[i] = names[j];
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
                cp.Remove(cp.Length - 1, 1);
                
                Scrolls[i] = cp.ToString();
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
            
            for (int i = 0; i < Sticks.Length; i++)
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
                
                Sticks[i] = names[j];
                StickMaterial[i] = metal;
            }
        }
    }
}