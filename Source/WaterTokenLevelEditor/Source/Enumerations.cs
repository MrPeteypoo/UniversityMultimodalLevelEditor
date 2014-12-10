using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterTokenLevelEditor
{
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
    /// An enumerator of each available weapon rank in the game. Determines whether characters have enough skill to weild the weapon or not.
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
