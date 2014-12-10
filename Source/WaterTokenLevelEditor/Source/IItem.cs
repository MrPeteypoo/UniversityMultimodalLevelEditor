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
    public sealed enum ItemType
    {
        Weapon = 0,
        Supply = 1
    }


    /// <summary>
    /// An interface to every Item class in the game, this is the interface used by weapons, supplies and accessories.
    /// </summary>
    public interface IItem
    {
        protected string    m_name      = "";   //!< The name of the item as it should be displayed in-game.
        protected uint      m_uses      = 1;    //!< How many times the item can be used by characters.
        protected uint      m_worth     = 2;    //!< How much the item is worth, therefore how much it can be bought and sold for.
        protected uint      m_weight    = 0;    //!< How quickly the item can be used by characters, effects turn order.


        /// <summary>
        /// Gets type of item the object is, useful for knowing how to cast.
        /// </summary>
        public abstract ItemType itemType;
    }
}
