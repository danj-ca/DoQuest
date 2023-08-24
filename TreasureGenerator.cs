static class TreasureGenerator
{
    static Random _rng = new Random();
    // TODO Define constants for treasures... figure out how to randomly choose between

    enum TreasureTypes
    {
        Jewel,
        Jewelry,
        Weapon,
        Armor
    };

    enum TreasureRarities
    {
        Common,
        Uncommon,
        Rare,
        Unique
    };

    static List<string> JewelQualities = new List<string>
    {
        "Broken",
        "Flawed",
        "Discoloured",
        "Ordinary",
        "Excellent",
        "Flawless",
        "Perfect"
    };

    static List<string> Jewels = new List<string>
    {
        "Ruby",
        "Emerald",
        "Topaz",
        "Saphire",
        "Tourmaline",
        "Opal",
        "Onyx",
        "Diamond",
        "Amethyst",
        "Quartz",
        "Obsidian",
        "Jade"
    };

    static List<string> JewelryTypes = new List<string>
    {
        "Ring",
        "Necklace",
        "Collar",
        "Torq",
        "Bracelet",
        "Earring",
        "Nose Ring",
        "Mask",
        "Tiara",
        "Crown",
        "Circlet",
        "Bellybutton Stud",
        "Anklet",
        "Cuff",
        "Chain",
        "Locket",
        "Pendant",
        "Sceptre",
        "Orb",
        "Claw"
    };

    static List<string> WeaponArmorQualities = new List<string>
    {
        "Rusty",
        "Tarnished",
        "Ordinary",
        "Decent",
        "Gleaming",
        "Excellent",
        "Peerless",
        "Heroic",
        "Legendary"
    };

    static List<string> WeaponArmorMaterials = new List<string>
    { 
        "Wooden",
        "Iron",
        "Steel",
        "Mithril",
        "Silver",
        "Gold",
        "Obsidian",
        "Diamond"
    };

    static List<string> Weapons = new List<string>
    { 
        // Blades
        "Dagger",
        "Short Sword",
        "Broadsword",
        "Longsword",
        "Bastard Sword",
        "Claymore",
        "Zweihander",
        "Greatsword",
        "Rapier",
        "Sabre",
        "Falchion",
        "Scimitar",
        // Spears
        "Spear",
        "Poleaxe",
        "Glaive",
        "War Scythe",
        // Axes
        "Handaxe",
        "Battleaxe",
        "War Axe",
        "Greataxe",
        // Thrown/Shot
        "Short Bow",
        "Longbow",
        "Great Bow",
        "Crossbow",
        "Javelin"
    };

    static List<string> Armors = new List<string>
    {
        // Shields
        "Buckler",
        "Shield",
        "Tower Shield",
        // Body Armor
        "Gauntlets",
        "Boots",
        "Pauldrons",
        "Cuirass",
        // Helmets
        "Helm",
        "Greathelm"
    };

    static List<string> WeaponArmorPrefixes = new List<string>
    {
        "King's",
        "Queen's",
        "Marquise's",
        "Marquess's",
        "Lady's",
        "Lord's",
        "Rogue's",
        "Knight's",
        "Blackguard's",
        "Hero's",
        "Demon's",
        "Angel's",
        "Devil's",
        "Serpent's",
        "Raven's",
        "Eagle's",
        "Crow's",
        "Owl's",
        "Wilf's",
        "Wolf's",
        "Cat's",
        "Cthulhu's"
    };

    static List<string> WeaponArmorSuffixes = new List<string>
    {
        "of Frost",
        "of Flame",
        "of Stone",
        "of Wind",
        "of Radiance",
        "of Darkness",
        "of Transmutation",
        "of Annihilation",
        "of the Sun",
        "of the Moon",
        "of the Stars",
        "of the Sky",
        "of the Forest",
        "of the Sea",
        "of the Pit",
        "of Haste",
        "of Strength",
        "of Mirth",
        "of Wisdom",
        "of Tolerance",
        "of Spite",
        "of Blight",
        "of Wounding",
        "of Healing",
        "of Sleep",
        "of Dreams",
        "of Nightmares",
        "of Life",
        "of Death",
        "of Doom",
        "of Destiny"
    };

    // TODO Wait, we don't pick the treasures here, we generate them to go in the database :P
    static TreasureTypes PickTreasureType()
    {
        var pick = _rng.Next(0, 4);
        return (TreasureTypes)pick;
    }

    static T PickRandomListEntry<T>(List<T> choices)
    {
        var pick = _rng.Next(0, choices.Count);
        return choices[pick];
    }

    //static string GetTreasure()
}