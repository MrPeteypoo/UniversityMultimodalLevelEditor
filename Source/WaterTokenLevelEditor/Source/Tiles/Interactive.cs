using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// An interface to every interactive layer class in the game.
    /// </summary>
    public sealed class Interactive : TileLayer
    {

        #region Implmentation data

        private InteractiveType m_interactiveType   = InteractiveType.Boundary; //!< The type of interactive tile.
        private uint            m_effect            = 0;                        //!< This could be the HP regenerated from a RegenPoint or it could be the sceneID of the village.

        #endregion


        #region Constructors

        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public Interactive() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Interactive (Interactive copy)
        {
            if (copy)
            {
                sprite = copy.sprite;
                m_interactiveType = copy.m_interactiveType;
                m_effect = copy.m_effect;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise an Interactive object from a null pointer.");
            }
        }

        #endregion

        #region Getters, setters and properties

        /// <summary>
        /// Obtains the enumeration of the type for this class.
        /// </summary>
        /// <returns>LayerType.Interactive.</returns>
        public sealed override LayerType GetLayerType()
        {
            return LayerType.Interactive;
        }


        /// <summary>
        /// Gets or sets the type of interactive tile the object represents.
        /// </summary>
        public InteractiveType interactiveType
        {
            get { return m_interactiveType; }
            set { m_interactiveType = value; }
        }


        /// <summary>
        /// Gets or sets the effect value of the tile. This is only used for special types which have an effect such as a Regen Point or a Village where it represents the scene ID.
        /// </summary>
        public uint effect
        {
            get { return m_effect; }
            set { m_effect = value; }
        }

        #endregion


        #region XML functionality

        /// <summary>
        /// Converts the data stored within the object into XML.
        /// </summary>
        /// <returns>An XML version of the object.</returns>
        public sealed override XElement ToXElement()
        {
            XElement element = new XElement ("Interactive");

            element.Add (   new XAttribute ("Sprite", sprite),
                            new XAttribute ("Type", (int) m_interactiveType),
                            new XAttribute ("Effect", m_effect));

            return element;
        }


        /// <summary>
        /// Creates an Interactive object from an XElement node.
        /// </summary>
        /// <param name="element">The XML of the object.</param>
        /// <returns>The duplicated object.</returns>
        public static Interactive FromXElement (XElement element)
        {
            // Create a tile object.
            Interactive tile        = new Interactive();
            
            // Fill it with data. Use properties to handle corrupt data.
            tile.sprite             =                                       element.Attribute ("Sprite").Value;
            tile.interactiveType    = (InteractiveType) Convert.ToInt32 (   element.Attribute ("Type").Value);
            tile.effect             = Convert.ToUInt32 (                    element.Attribute ("Effect").Value);
           
            // Return the processed object.
            return tile;
        }

        #endregion

    }
}
