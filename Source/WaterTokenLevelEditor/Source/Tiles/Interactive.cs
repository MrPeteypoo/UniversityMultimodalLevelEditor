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
    public abstract class Interactive : TileLayer
    {
        /// <summary>
        /// Obtains the enumeration of the type for this class.
        /// </summary>
        /// <returns>LayerType.Interactive.</returns>
        public sealed override LayerType GetLayerType()
        {
            return LayerType.Interactive;
        }


        /// <summary>
        /// Obtains the enumeration for the particular sub-class of the interactive layer.
        /// </summary>
        /// <returns>The child-specific enumeration.</returns>
        public abstract InteractiveType GetInteractiveType();
    }
}
