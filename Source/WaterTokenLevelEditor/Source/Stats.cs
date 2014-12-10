using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    public sealed class Stats
    {
        #region Implementation data
        
        private uint    m_hp            = 1;    //!< How much damage a character can take.
        private uint    m_strength      = 1;    //!< The amount of damage a character can deal physically.
        private uint    m_magic         = 1;    //!< The amount of damage a character can deal with magic.
        private uint    m_skill         = 1;    //!< The likelihood of a character hitting another and using a critical hit.
        private uint    m_speed         = 1;    //!< How many strikes a character can make as well as the evasiveness of the character.
        private uint    m_luck          = 1;    //!< How lucky the character is, effects random item pickups, hit rate, evasion rate, critical hit rate, etc.
        private uint    m_defense       = 1;    //!< How much damage a character absorbs when physically hit.
        private uint    m_resistance    = 1;    //!< How much damage a character absorbs when magically hit.
        private uint    m_constitution  = 1;    //!< How large a character is, effects ability to reduce the weight of weapons and who they can pick up.
        private uint    m_weight        = 1;    //!< How much a character weighs, effects their ability to rescue and shove others.
        private uint    m_movement      = 1;    //!< How many cells a character can move each turn.
        
        #endregion


        #region Constructors and operators

        /// <summary>
        /// The default constructor for the Stats class.
        /// </summary>
        public Stats() { }


        /// <summary>
        /// The copy constructor for the Stats class.
        /// </summary>
        /// <param name="copy"></param>
        public Stats (Stats copy)
        {
            if (copy)
            {
                m_hp = copy.m_hp;
                m_strength = copy.m_strength;
                m_magic = copy.m_magic;
                m_skill = copy.m_skill;
                m_speed = copy.m_speed;
                m_luck = copy.m_luck;
                m_defense = copy.m_defense;
                m_resistance = copy.m_resistance;
                m_constitution = copy.m_constitution;
                m_weight = copy.m_weight;
                m_movement = copy.m_movement;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Stats object from a null pointer.");
            }
        }


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="stats">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (Stats stats)
        {
            return stats != null;
        }

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Gets or sets the HP value, this will never be below 1.
        /// </summary>
        public uint hp
        {
            get { return m_hp; }
            set { m_hp = Math.Max (1, value); }
        }


        /// <summary>
        /// Gets or sets the strength value, this effects how much physical damage a character can make.
        /// </summary>
        public uint strength
        {
            get { return m_strength; }
            set { m_strength = value; }
        }


        /// <summary>
        /// Gets or sets the magic value, this effects how much magical damage a character can make.
        /// </summary>
        public uint magic
        {
            get { return m_magic; }
            set { m_magic = value; }
        }


        /// <summary>
        /// Gets or sets the skill value, this impacts the hit rate and critical hit rate of a character.
        /// </summary>
        public uint skill
        {
            get { return m_skill; }
            set { m_skill = value; }
        }


        /// <summary>
        /// Gets or sets the speed value, this effects how many strikes a character can make as well as the evasiveness of the character.
        /// </summary>
        public uint speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }


        /// <summary>
        /// Gets or sets the luck value, effects random item pickups, hit rate, evasion rate, critical hit rate, etc.
        /// </summary>
        public uint luck
        {
            get { return m_luck; }
            set { m_luck = value; }
        }


        /// <summary>
        /// Gets or sets the defense value, this effects how much physical damage is absorbed.
        /// </summary>
        public uint defense
        {
            get { return m_defense; }
            set { m_defense = value; }
        }


        /// <summary>
        /// Gets or sets the resistance value, this effects how much magical damage is absorbed.
        /// </summary>
        public uint resistance
        {
            get { return m_resistance; }
            set { m_resistance = value; }
        }


        /// <summary>
        /// Gets or sets the constitution value, effects ability to reduce the weight of weapons and who they can pick up.
        /// </summary>
        public uint constitution
        {
            get { return m_constitution; }
            set { m_constitution = value; }
        }


        /// <summary>
        /// Gets or sets the weight value, effects a characters ability to rescue and shove others.
        /// </summary>
        public uint weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }


        /// <summary>
        /// Gets or sets the movement value, this determines how many tiles a character can move during a single turn.
        /// </summary>
        public uint movement
        {
            get { return m_movement; }
            set { m_movement = value; }
        }

        #endregion
    }
}
