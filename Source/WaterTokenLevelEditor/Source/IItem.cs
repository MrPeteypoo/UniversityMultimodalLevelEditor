using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// An abtract base class for every Item class in the game, this is the interface used by weapons, supplies and accessories.
    /// </summary>
    public abstract class IItem
    {
        #region Implementation data

        protected string    m_name      = "";   //!< The name of the item as it should be displayed in-game.
        protected uint      m_uses      = 1;    //!< How many times the item can be used by characters.
        protected uint      m_worth     = 2;    //!< How much the item is worth, therefore how much it can be bought and sold for.
        protected uint      m_weight    = 0;    //!< How much the item effects a characters attack speed, if at all.

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Gets type of item the object is, useful for knowing how to cast.
        /// </summary>
        public abstract ItemType GetItemType();


        /// <summary>
        /// Gets or sets the name of the Item, this is displayed in-game.
        /// </summary>
        public string name
        {
            get { return m_name; }
            set { m_name = value; }
        }


        /// <summary>
        /// Gets or sets how many times the item can be used, this will always be more than 0.
        /// </summary>
        public uint uses
        {
            get { return m_uses; }
            set { m_uses = Math.Max (1, value); }
        }


        /// <summary>
        /// Gets or sets how much the item can be bought for, this effects the selling value and will always be more than 1.
        /// </summary>
        public uint worth
        {
            get { return m_worth; }
            set { m_worth = Math.Max (2, value); }
        }


        /// <summary>
        /// Gets or sets the weight of the item. This effects a characters attack speed.
        /// </summary>
        public uint weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }

        #endregion
    }
}
