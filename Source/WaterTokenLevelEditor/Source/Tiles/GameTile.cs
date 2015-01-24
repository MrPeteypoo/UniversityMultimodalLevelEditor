using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// A container for each game tile in the level. A GameTile contains a terrain, interactive and character tile. Only the terrain tile is manditory, the other are completely optional.
    /// </summary>
    public sealed class GameTile
    {
        #region Implementation data

        private Terrain     m_terrain       = new Terrain();    //!< The mandatory tile, absolutely required by the game at all times.
        private Interactive m_interactive   = null;             //!< The interactive tile is completely optional and not required at all.
        private Character   m_character     = null;             //!< The character tile is completely optional and has the highest z-index in-game.

        #endregion


        #region Operators

        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="gameTile">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (GameTile gameTile)
        {
            return gameTile != null;
        }

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Gets the TileLayer associated with the given LayerType. This can return null if the layer does not exist.
        /// </summary>
        /// <param name="layer">The layer to return.</param>
        /// <returns>The desired layer, null if it does not exist.</returns>
        public TileLayer GetLayer (LayerType layer)
        {
            switch (layer)
            {
                case LayerType.Terrain:
                    return m_terrain;

                case LayerType.Interactive:
                    return m_interactive;

                case LayerType.Character:
                    return m_character;

                default:
                    throw new ArgumentException ("Attempt to obtain a tile on layer \"" + layer + "\", this is not handled by GameTile.GetLayer().");
            }
        }


        /// <summary>
        /// Gets or sets the terrain tile, this can never be null and any attempt to set to null will throw an ArgumentNullException.
        /// </summary>
        public Terrain terrain
        {
            get { return m_terrain; }
            set
            {
                if (value)
                {
                    m_terrain = value;
                }

                else
                {
                    throw new ArgumentNullException ("Attempt to set GameTile.terrain to null.");
                }
            }
        }


        /// <summary>
        /// Gets or sets the interactive tile.
        /// </summary>
        public Interactive interactive
        {
            get { return m_interactive; }
            set { m_interactive = value; }
        }


        /// <summary>
        /// Gets or sets the character tile.
        /// </summary>
        public Character character
        {
            get { return m_character; }
            set { m_character = value; }
        }

        #endregion


        #region XML functionality

        /// <summary>
        /// Creates an XElement containing all the data of the GameTile and it's composed data.
        /// </summary>
        /// <returns>An XElement.</returns>
        public XElement ToXElement()
        {
            XElement element = new XElement("GameTile");

            element.Add (m_terrain.ToXElement());

            if (m_interactive)
            {
                element.Add (m_interactive.ToXElement());
            }

            if (m_character)
            {
                element.Add (m_character.ToXElement());
            }

            return element;
        }


        /// <summary>
        /// Creates a GameTile object from an XElement node.
        /// </summary>
        /// <param name="element">The XML of the object.</param>
        /// <returns>The duplicated tile.</returns>
        public static GameTile FromXElement (XElement element)
        {
            // Terrain tiles are guaranteed.
            GameTile tile = new GameTile();
            tile.terrain = Terrain.FromXElement (element.Element ("Terrain"));

            // We need to actually check for interactive tiles.
            XElement optional = element.Element ("Interactive");

            if (optional != null)
            {
                tile.interactive = Interactive.FromXElement (optional);
            }

            // Same with character tiles.
            optional = element.Element ("Character");

            if (optional != null)
            {
                tile.character = Character.FromXElement (optional);
            }

            return tile;
        }

        #endregion

    }
}
