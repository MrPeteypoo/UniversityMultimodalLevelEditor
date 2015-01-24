using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{


    /// <summary>
    /// An enumeration used to specify what layer a particular object is to be displayed on.
    /// </summary>
    public enum LayerType
    {
        Terrain     = 0,
        Interactive = 1,
        Character   = 2
    }


    /// <summary>
    /// Used to specify how a character should behave in-game.
    /// </summary>
    public enum CharacterType
    {
        Boss    = 0,
        Grunt   = 1,
        NPC     = 2
    }


    /// <summary>
    /// Used to specify what the interactive tile is actually capable of doing.
    /// </summary>
    public enum InteractiveType
    {
        PlayerSpawn = 0,
        RegenPoint  = 1,
        Treasure    = 2,
        Door        = 3,
        Village     = 4,
        Goal        = 5,
        Boundary    = 6
    }


    /// <summary>
    /// Used to specify the type of terrain tile in place and determines who can walk on them in-game.
    /// </summary>
    public enum TerrainType
    {
        Normal      = 0,
        Sand        = 1,
        Forest      = 2,
        Mountain    = 3,
        River       = 4,
        Sea         = 5,
        Bridge      = 6,
        Village     = 7,
        Throne      = 8,
        Pillar      = 9
    }


    /// <summary>
    /// An enumeration of every class that exists in the game. This effects what the character has access to.
    /// </summary>
    public enum Class
    {
        Ranger          = 0,
        Lord            = 1,
        Hero            = 2,
        Myrmidon        = 3,
        Swordmaster     = 4,
        Soldier         = 5,
        Halberdier      = 6,
        Fighter         = 7,
        Warrior         = 8,
        Archer          = 9,
        Sniper          = 10,
        Knight          = 11,
        General         = 12,
        SwordKnight     = 13,
        LanceKnight     = 14,
        AxeKnight       = 15,
        BowKnight       = 16,
        Paladin         = 17,
        PegasusKnight   = 18,
        FalconKnight    = 19,
        Princess        = 20,
        WyvernRider     = 21,
        WyvernLord      = 22,
        King            = 23,
        Mage            = 24,
        Sage            = 25,
        Priest          = 26,
        Bishop          = 27,
        Cleric          = 28,
        Valkyrie        = 29,
        Thief           = 30,
        Assassin        = 31,
        Bandit          = 32,
        Berserker       = 33,
        Civilian        = 34,
        Child           = 35
    }


    /// <summary>
    /// An enumeration of every element in the game. This is used for character elemental affinity.
    /// </summary>
    public enum Element
    {
        Anima   = 0,
        Dark    = 1,
        Fire    = 2,
        Ice     = 3,
        Light   = 4,
        Thunder = 5,
        Wind    = 6,
        Water   = 7,
        Earth   = 8,
        Heaven  = 9
    }

    /// <summary>
    /// Defines the type of item, useful for knowing what type to cast down to without run-time type checking.
    /// </summary>
    public enum ItemType
    {
        Weapon  = 0,
        Supply  = 1
    }
    
    
    /// <summary>
    /// The type of each weapon in the game, required by the game so that only cerain characters can use weapons they're able to.
    /// </summary>
    public enum WeaponType
    {
        Sword   = 0,
        Axe     = 1, 
        Lance   = 2,
        Bow     = 3,
        Fire    = 4,
        Thunder = 5,
        Wind    = 6,
        Light   = 7,
        Staff   = 8,
        Knife   = 9
    }


    /// <summary>
    /// The type of supply an item is, this changes the impact that the effects have.
    /// </summary>
    public enum SupplyType
    {
        Healing     = 0,
        Growth      = 1,
        Temporary   = 2
    }


    /// <summary>
    /// An enumeration of each available weapon rank in the game. Determines whether characters have enough skill to weild the weapon or not.
    /// </summary>
    public enum WeaponRank
    {
        Null    = 0,
        E       = 1,
        D       = 2,
        C       = 3,
        B       = 4,
        A       = 5,
        S       = 6
    }
}
