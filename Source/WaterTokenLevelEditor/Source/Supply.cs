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


        #region Constructors and operators

        /// <summary>
        /// The default constructor for the Supply class.
        /// </summary>
        public Supply() { }


        /// <summary>
        /// The default copy constructor for the supply class. This will make a copy of all data instead of sharing pointers.
        /// </summary>
        /// <param name="copy"></param>
        public Supply (Supply copy)
        {
            m_supplyType = copy.m_supplyType;

            m_effect = new Stats (copy.m_effect);
        }


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="supply">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (Supply supply)
        {
            return supply != null;
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
