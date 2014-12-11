using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

    }
}
