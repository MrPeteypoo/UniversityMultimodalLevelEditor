using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// The class representing a player spawn point, only one player character can spawn on this tile at once.
    /// </summary>
    public sealed class PlayerSpawn : Interactive
    {
        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public PlayerSpawn() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public PlayerSpawn (PlayerSpawn copy)
        {
            if (copy)
            {
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a PlayerSpawn object from a null pointer.");
            }
        }


        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.PlayerSpawn.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.PlayerSpawn;
        }
    }


    /// <summary>
    /// A regen point is a tile which replenishes the HP of whatever unit is stationed on it each turn.
    /// </summary>
    public sealed class RegenPoint : Interactive
    {
        public uint effect = 0; //!< How much HP is replenished per turn.


        /// <summary>
        /// The default constructor for the RegenPoint class.
        /// </summary>
        public RegenPoint() { }


        /// <summary>
        /// The copy constructor for the RegenPoint class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public RegenPoint (RegenPoint copy)
        {
            if (copy)
            {
                effect = copy.effect;
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a RegenPoint object from a null pointer.");
            }
        }

        
        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.RegenPoint.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.RegenPoint;
        }
    }


    /// <summary>
    /// The treasure tile is typically a treasure chest which gives the player an item.
    /// </summary>
    public sealed class Treasure : Interactive
    {
        public IItem item = null; //!< The item to give the player upon collecting the treasure.


        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public Treasure() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Treasure (Treasure copy)
        {
            if (copy)
            {
                item = copy.item;
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Treasure object from a null pointer.");
            }
        }


        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.Treasure.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.Treasure;
        }
    }


    /// <summary>
    /// Doors provide obstacles and access to different strategic points.
    /// </summary>
    public sealed class Door : Interactive
    {
        public bool thiefUnlockable = true; //!< Indicates whether the door can be unlocked by thieves or not.


        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public Door() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Door (Door copy)
        {
            if (copy)
            {
                thiefUnlockable = copy.thiefUnlockable;
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Door object from a null pointer.");
            }
        }


        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.Door.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.Door;
        }

    }



    public sealed class Village : Interactive
    {
        public uint sceneID = 0;    //!< Determines which visitation scene should be played when the player enters the terrain.


        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public Village() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Village (Village copy)
        {
            if (copy)
            {
                sceneID = copy.sceneID;
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Village object from a null pointer.");
            }
        }


        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.Village.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.Village;
        }
    }


    public sealed class Goal : Interactive
    {
        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public Goal() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Goal (Goal copy)
        {
            if (copy)
            {
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Goal object from a null pointer.");
            }
        }


        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.Goal.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.Goal;
        }
    }


    public sealed class Boundary : Interactive
    {
        /// <summary>
        /// The default constructor for the class.
        /// </summary>
        public Boundary() { }


        /// <summary>
        /// The copy constructor for the class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public Boundary (Boundary copy)
        {
            if (copy)
            {
                sprite = copy.sprite;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a Boundary object from a null pointer.");
            }
        }


        /// <summary>
        /// Obtains the enumeration for the type of interactive tile that the class is.
        /// </summary>
        /// <returns>InteractiveType.Boundary.</returns>
        public sealed override InteractiveType GetInteractiveType()
        {
            return InteractiveType.Boundary;
        }
    }
}
