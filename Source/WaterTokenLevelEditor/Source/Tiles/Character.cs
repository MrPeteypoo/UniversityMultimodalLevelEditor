using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// The class used to represent a character layer in a GameTile, this is the top-most layer and contains all required data for NPCs.
    /// </summary>
    public sealed class Character : TileLayer
    {
        #region Implementation data

        private CharacterType   m_characterType = CharacterType.Grunt;  //!< The type of character that the object is, this effects how the character will behave in-game.
        
        private string          m_name          = "Bobby-five";         //!< The name which will be displayed in-game for the character.
        
        private Class           m_class         = Class.Princess;       //!< Which class the character should be, if everyone is a Princess then something has gone wrong!
        private Element         m_affinity      = Element.Anima;        //!< The elemental affinity of the character, effects relationships.
        private uint            m_level         = 1;                    //!< The level of the character, along with the class this effects experience gained for the players.
        private Stats           m_stats         = new Stats();          //!< The stats of the character, this is the effectiveness of the character in-game.
        private WeaponRanks     m_ranks         = new WeaponRanks();    //!< Manages the available weapons to the character.
        
        //private IItem[]         m_inventory     = new IItem[4];         //!< The inventory of the character, can hold up to four items.
        //private double          m_dropChance    = 0.0;                  //!< The chance of the character dropping an item upon death.

        #endregion


        #region Constructors

        /// <summary>
        /// The default constructor for the Character class.
        /// </summary>
        public Character() { }


        /// <summary>
        /// The copy constructor for the Character class. All data will be copied, pointers will not be shared.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Character (Character copy)
        {
            if (copy)
            {
                m_characterType = copy.m_characterType;
        
                m_name = copy.m_name;
                
                m_class = copy.m_class;
                m_affinity = copy.m_affinity;
                m_level = copy.m_level;
                m_stats = new Stats (copy.m_stats);
                m_ranks = new WeaponRanks (copy.m_ranks);

                //m_inventory = new IItem[4] { copy.m_inventory[0], copy.m_inventory[1], copy.m_inventory[2], copy.m_inventory[3] };
                //m_dropChance = copy.m_dropChance;

                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Character object with a null pointer.");
            }
        }

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Obtains the type of the children layer classes.
        /// </summary>
        /// <returns>LayerType.Character</returns>
        public sealed override LayerType GetLayerType()
        {
            return LayerType.Character;
        }


        /// <summary>
        /// Gets or sets the character type value, this effects how the character will behave in-game.
        /// </summary>
        public CharacterType characterType
        {
            get { return m_characterType; }
            set { m_characterType = value; }
        }


        /// <summary>
        /// Gets or sets the name which will be displayed in-game for the character.
        /// </summary>
        public string name
        {
            get { return m_name; }
            set { m_name = value; }
        }


        /// <summary>
        /// Gets or sets which class the character should be.
        /// </summary>
        public Class characterClass
        {
            get { return m_class; }
            set { m_class = value; }
        }


        /// <summary>
        /// Gets or sets the elemental affinity of the character, effects relationships.
        /// </summary>
        public Element affinity
        {
            get { return m_affinity; }
            set { m_affinity = value; }
        }


        /// <summary>
        /// Gets or sets level of the character, along with the class this effects experience gained for the players. Will be clamped between 1 and 20.
        /// </summary>
        public uint level
        {
            get { return m_level; }
            set 
            {
                if (value > 20)
                {
                    m_level = 20;
                }

                else if (value < 1)
                {
                    m_level = 1;
                }

                else
                {
                    m_level = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the stats of the character, this is the effectiveness of the character in-game.
        /// </summary>
        public Stats stats
        {
            get { return m_stats; }
            set
            {
                if (value)
                {
                    m_stats = value;
                }

                else
                {
                    throw new ArgumentNullException ("Attempt to set Character.stats to null.");
                }
            }
        }


        /// <summary>
        /// Gets or sets the weapon ranks of the character, this manages the available weapons.
        /// </summary>
        public WeaponRanks ranks
        {
            get { return m_ranks; }
            set
            {
                if (value)
                {
                    m_ranks = value;
                }

                else
                {
                    throw new ArgumentNullException ("Attempt to set Character.ranks to null");
                }
            }
        }


        /*/// <summary>
        /// Gets or sets the inventory of the character, can hold up to four items. Any arrays used for setting must have a length of 4.
        /// </summary>
        public IItem[] inventory
        {
            get { return m_inventory; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException ("Attempt to set Character.inventory to null.");
                }

                if (value.Length != 4)
                {
                    throw new ArgumentException ("Attempt to set Character.inventory to an inventory with an invalid size.");
                }

                m_inventory = value;
            }
        }


        /// <summary>
        /// Gets or set the chance of the character dropping an item upon death. This will be clamped between 0.0 and 1.0.
        /// </summary>
        public double dropChance
        {
            get { return m_dropChance; }
            set 
            { 
                if (value > 1.0)
                {
                    m_dropChance = 1.0;
                }

                else if (value < 0.0)
                {
                    m_dropChance = 0.0;
                }

                else
                {
                    m_dropChance = value;
                }
            }
        }*/

        #endregion


        #region XML functionality

        /// <summary>
        /// Converts the data stored within the object into XML.
        /// </summary>
        /// <returns>An XML version of the object.</returns>
        public override XElement ToXElement()
        {
            XElement element = new XElement ("Character");

            element.Add (   new XAttribute ("Sprite", sprite),
                            new XAttribute ("Type", (int) m_characterType),
                            new XAttribute ("Name", m_name),
                            new XAttribute ("Class", (int) m_class),
                            new XAttribute ("Affinity", (int) m_affinity),
                            new XAttribute ("Level", m_level),
                            
                            m_stats.ToXElement(),
                            m_ranks.ToXElement());

            return element;
        }

        #endregion

    }
}