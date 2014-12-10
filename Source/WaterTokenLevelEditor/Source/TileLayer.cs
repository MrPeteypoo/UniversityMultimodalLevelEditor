using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// A base class for all layer types to be used in a GameTile.
    /// </summary>
    public abstract class TileLayer
    {
        public string   sprite  = "";   //!< The location of the sprite when used in the game.


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="tileLayer">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (TileLayer tileLayer)
        {
            return tileLayer != null;
        }


        /// <summary>
        /// Obtains the type of the children layer classes.
        /// </summary>
        /// <returns>The specific enumeratior for that layer class.</returns>
        public abstract LayerType GetLayerType();
    }
}
