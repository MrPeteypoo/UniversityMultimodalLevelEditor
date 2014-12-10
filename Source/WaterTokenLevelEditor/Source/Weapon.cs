using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// The data representation of a Weapon item, these are used by characters in the game to cause damage.
    /// </summary>
    public sealed class Weapon : IItem
    {
        #region Implementation data

        private WeaponType  m_weaponType    = WeaponType.Sword;         //!< Indicates what type the weapon is, and therefore, who can use it.
        private WeaponRank  m_rank          = WeaponRank.E;             //!< The skill level required to actually use the weapon.
        
        private uint        m_might         = 0;                        //!< How strong the weapon is, adds to the base damage.
        
        private double      m_hitRate       = 1.0;                      //!< The hit rate in percentage of the weapon.
        private double      m_criticalRate  = 0.0;                      //!< How likely the weapon is to critically hit another character.

        private Range<uint> m_range         = new Range<uint> (1,1);    //!< The range at which the weapon can hit characters.

        private uint        m_experience    = 1;                        //!< How much experience a character gains on their weapon rank from using the weapon.

        #endregion


        #region Constructors and operators

        /// <summary>
        /// The default constructor for the Weapon class.
        /// </summary>
        public Weapon() { }


        /// <summary>
        /// The default copy constructor for the Weapon class.
        /// </summary>
        /// <param name="weapon">The object to copy data from.</param>
        public Weapon (Weapon weapon)
        {
            if (weapon)
            {
                m_weaponType = weapon.m_weaponType;
                m_rank = weapon.m_rank;

                m_might = weapon.m_might;

                m_hitRate = weapon.m_hitRate;
                m_criticalRate = weapon.m_criticalRate;

                m_range = new Range<uint> (weapon.m_range);

                m_experience = weapon.m_experience;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Weapon object using the copy constructor with a null pointer.");
            }
        }


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="range">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (Weapon weapon)
        {
            return weapon != null;
        }

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Gets type of item the object is, useful for knowing how to cast.
        /// </summary>
        public override ItemType GetItemType()
        {
            return ItemType.Weapon;
        }

        /// <summary>
        /// Gets or sets the weapon type of the weapon.
        /// </summary>
        public WeaponType weaponType
        {
            get { return m_weaponType; }
            set { m_weaponType = value; }
        }


        /// <summary>
        /// Gets or sets the rank of the weapon. WeaponRank.Null values will be silently ignored.
        /// </summary>
        public WeaponRank rank
        {
            get { return m_rank; }
            set 
            { 
                if (value != WeaponRank.Null)
                {
                    m_rank = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the might value of the weapon, this effects how powerful the weapon is.
        /// </summary>
        public uint might
        {
            get { return m_might; }
            set { m_might = value; }
        }


        /// <summary>
        /// Gets or sets the hit rate value of the weapon, this should be in percentage where 1.0 == 100%. Values below 0.0 will be clamped.
        /// </summary>
        public double hitRate
        {
            get { return m_hitRate; }
            set { m_hitRate = Math.Max (value, 0.0); }
        }


        /// <summary>
        /// Gets or sets the critical hit rate of the weapon, this will be clamped between 0.0 and 1.0.
        /// </summary>
        public double criticalRate
        {
            get { return m_criticalRate; }
            set
            {
                if (value < 0.0)
                {
                    m_criticalRate = 0.0;
                }

                else if (value > 1.0)
                {
                    m_criticalRate = 1.0;
                }

                else
                {
                    m_criticalRate = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the range of the weapon. The range cannot have a minimum or maximum of 0.
        /// </summary>
        public Range<uint> range
        {
            get { return m_range; }
            set
            {
                if (value)
                {
                    if (value.minimum != 0 && value.maximum != 0)
                    {
                        m_range = value;
                    }
                }

                else
                {
                    throw new ArgumentNullException ("Attempt to set Weapon.range to null.");
                }
            }
        }
    

        /// <summary>
        /// Gets or sets the experience value of the weapon, this effects the rate at which a characters weapon rank increases.
        /// </summary>
        public uint experience
        {
            get { return m_experience; }
            set { m_experience = value; }
        }

        #endregion

    }
}
