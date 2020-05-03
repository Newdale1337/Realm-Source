using System.Collections.Generic;

namespace Externals.Utilities
{
    public class RotmgUtils
    {
        public static string[] GuestNames = new string[]
        {
            "Darq","Deyst","Itani","Ehoni","Tiar",
            "Eango","Eashy","Eati","Eendi","Lorz",
            "Gharr","Iatho","Iawa","Idrae","Rilr",
            "Laen","Oalei","Oshyu","Odaru","Yimi",
            "Lauk","Radph","Oeti","Orothi","Rayr",
            "Queq","Saylt","Scheev","Serl","Vorv",
            "Tal","Iri","Sek","Ril","Seus","Uoro",
            "Vorck","Issz","Urake","Risrr","Drac",
            "Drol","Utanu","Yangu","Zhiar"
        };

        public static Dictionary<string, string> CharacterRestricitons = new Dictionary<string, string>()
        {
            {"Rogue", "unrestricted"},
            {"Assassin", "unrestricted"},
            {"Huntress", "unrestricted"},
            {"Mystic", "unrestricted"},
            {"Trickster", "unrestricted"},
            {"Sorcerer", "unrestricted"},
            {"Ninja", "unrestricted"},
            {"Archer", "unrestricted"},
            {"Wizard", "unrestricted"},
            {"Priest", "unrestricted"},
            {"Necromancer", "unrestricted"},
            {"Warrior", "unrestricted"},
            {"Knight", "unrestricted"},
            {"Paladin", "unrestricted"}
        };
    }
}