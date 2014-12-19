using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// An interface to every interactive layer class in the game.
    /// </summary>
    public sealed class Interactive : TileLayer
    {

        #region Implmentation data

        private InteractiveType m_type      = InteractiveType.Boundary; //!< The type of interactive tile.
        private uint            m_effect    = 0;                        //!< This could be the HP regenerated from a RegenPoint or it could be the sceneID of the village.

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
                m_type = copy.m_type;
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
            get { return m_type; }
            set { m_type = value; }
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

    }
}
