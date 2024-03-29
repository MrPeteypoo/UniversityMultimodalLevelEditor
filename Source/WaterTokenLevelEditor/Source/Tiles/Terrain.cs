﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// The class used to represent a character layer in a GameTile, this is the bottom-most layer which typically contains the graphical representation of the level.
    /// </summary>
    public sealed class Terrain : TileLayer
    {
        #region Implementation data

        private TerrainType m_terrainType       = TerrainType.Normal;   //!< Indicates the type of tile, useful for knowing what to render in-game.
        private uint        m_defenseBonus      = 0;                    //!< The defense bonus applied to units in the terrain.
        private uint        m_resistanceBonus   = 0;                    //!< The resistance bonus applied to units in the terrain.
        private double      m_evasionBonus      = 0.0;                  //!< The evasion bonus applied to units in the terrain.
        private uint        m_moveCost          = 0;                    //!< How many extra movement points are spent moving through the terrain.

        #endregion


        #region Constructors

        /// <summary>
        /// The default constructor for the Terrain class.
        /// </summary>
        public Terrain() { }


        /// <summary>
        /// The copy constructor for the Terrain class. All data is copied, pointers are not shared.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Terrain (Terrain copy)
        {
            if (copy)
            {
                sprite = copy.sprite;

                m_terrainType = copy.m_terrainType;
                m_defenseBonus = copy.m_defenseBonus;
                m_resistanceBonus = copy.m_resistanceBonus;
                m_evasionBonus = copy.m_evasionBonus;
                m_moveCost = copy.m_moveCost;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Terrain object with a null pointer.");
            }
        }

        #endregion


        #region Getters, setters, properties

        /// <summary>
        /// Gets the enumeration represting the type of derived class.
        /// </summary>
        /// <returns>LayerType.Terrain.</returns>
        public sealed override LayerType GetLayerType()
        {
            return LayerType.Terrain;
        }


        /// <summary>
        /// Gets or sets the type of terrain, used for displaying the name in-game properly.
        /// </summary>
        public TerrainType terrainType
        {
            get { return m_terrainType; }
            set { m_terrainType = value; }
        }


        /// <summary>
        /// Gets or sets the defense bonus that is added to units stationed in the tile.
        /// </summary>
        public uint defenseBonus
        {
            get { return m_defenseBonus; }
            set { m_defenseBonus = value; }
        }


        /// <summary>
        /// Gets or sets the resistance bonus that is applied to units stationed in the tile.
        /// </summary>
        public uint resistanceBonus
        {
            get { return m_resistanceBonus; }
            set { m_resistanceBonus = value; }
        }


        /// <summary>
        /// Gets or sets the evasion bonus that is applied to units station in the tile. Values are clamped between 0.0 and 1.0.
        /// </summary>
        public double evasionBonus
        {
            get { return m_evasionBonus; }
            set 
            { 
                if (value > 1.0)
                {
                    m_evasionBonus = 1.0;
                }

                else if (value < 0.0)
                {
                    m_evasionBonus = 0.0;
                }

                else
                {
                    m_evasionBonus = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the number of extra movement points that are spent moving through the terrain.
        /// </summary>
        public uint moveCost
        {
            get { return m_moveCost; }
            set { m_moveCost = value; }
        }

        #endregion


        #region XML functionality

        /// <summary>
        /// Converts the data stored within the object into XML.
        /// </summary>
        /// <returns>An XML version of the object.</returns>
        public sealed override XElement ToXElement()
        {
            XElement element = new XElement ("Terrain");

            element.Add (   new XAttribute ("Sprite", sprite),
                            new XAttribute ("Type", (int) m_terrainType),
                            new XAttribute ("Defense", m_defenseBonus),
                            new XAttribute ("Resistance", m_resistanceBonus),
                            new XAttribute ("Evasion", m_evasionBonus),
                            new XAttribute ("MovementCost", m_moveCost));

            return element;
        }


        /// <summary>
        /// Creates a Terrain object from an XElement node.
        /// </summary>
        /// <param name="element">The XML of the object.</param>
        /// <returns>The duplicated object.</returns>
        public static Terrain FromXElement (XElement element)
        {
            // Create a tile object.
            Terrain tile        = new Terrain();
            
            // Fill it with data. Use properties to handle corrupt data.
            tile.sprite             =                                   element.Attribute ("Sprite").Value;
            tile.terrainType        = (TerrainType) Convert.ToInt32 (   element.Attribute ("Type").Value);
            tile.defenseBonus       = Convert.ToUInt32 (                element.Attribute ("Defense").Value);
            tile.resistanceBonus    = Convert.ToUInt32 (                element.Attribute ("Resistance").Value);
            tile.evasionBonus       = Convert.ToDouble (                element.Attribute ("Evasion").Value);
            tile.moveCost           = Convert.ToUInt32 (                element.Attribute ("MovementCost").Value);
           
            // Return the processed object.
            return tile;
        }

        #endregion
    }
}
