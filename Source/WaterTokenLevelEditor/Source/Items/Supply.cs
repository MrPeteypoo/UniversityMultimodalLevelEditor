using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// A type of item which is used to provide positive effects to a character, these can be temporary or permenant.
    /// </summary>
    public sealed class Supply : IItem
    {
        #region Implementation data

        private SupplyType  m_supplyType    = SupplyType.Healing;   //!< The type of supply that the item is, this changes how the effects are applied to characters.
        private Stats       m_effect        = new Stats();          //!< The effects that using the supply has.

        #endregion


        #region Constructors

        /// <summary>
        /// The default constructor for the Supply class.
        /// </summary>
        public Supply() { }


        /// <summary>
        /// The default copy constructor for the Supply class. This will make a copy of all data instead of sharing pointers.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Supply (Supply copy)
        {
            if (copy)
            {
                m_supplyType = copy.m_supplyType;
                m_effect = new Stats (copy.m_effect);

                m_name = copy.m_name;
                m_uses = copy.m_uses;
                m_worth = copy.m_worth;
                m_weight = copy.m_weight;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Supply object from a null pointer.");
            }
        }

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Gets type of item the object is, useful for knowing how to cast.
        /// </summary>
        /// <returns>ItemType.Supply</returns>
        public override ItemType GetItemType()
        {
            return ItemType.Supply;
        }


        /// <summary>
        /// Gets or sets the type of supply that the item is, this changes how the effects are applied to characters.
        /// </summary>
        public SupplyType supplyType
        {
            get { return m_supplyType; }
            set { m_supplyType = value; }
        }


        public Stats effect
        {
            get { return m_effect; }
            set
            {
                if (value)
                {
                    m_effect = value;
                }

                else
                {
                    throw new ArgumentNullException ("Attempt to set Supply.effect to null.");
                }
            }
        }

        #endregion
    }
}
