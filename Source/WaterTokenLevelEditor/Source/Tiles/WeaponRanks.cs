using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// A simple container for the weapon ranks that are associated with characters.
    /// </summary>
    public sealed class WeaponRanks
    {
        #region Implementation data

        public WeaponRank   sword   = WeaponRank.E;     //!< The actual rank for the sword.
        public WeaponRank   axe     = WeaponRank.Null;  //!< The actual rank for the axe.
        public WeaponRank   lance   = WeaponRank.Null;  //!< The actual rank for the lance.
        public WeaponRank   bow     = WeaponRank.Null;  //!< The actual rank for the bow.
        public WeaponRank   fire    = WeaponRank.Null;  //!< The actual rank for the fire.
        public WeaponRank   thunder = WeaponRank.Null;  //!< The actual rank for the thunder.
        public WeaponRank   wind    = WeaponRank.Null;  //!< The actual rank for the wind.
        public WeaponRank   light   = WeaponRank.Null;  //!< The actual rank for the light.
        public WeaponRank   staff   = WeaponRank.Null;  //!< The actual rank for the staff.
        public WeaponRank   knife   = WeaponRank.Null;  //!< The actual rank for the knife.

        #endregion


        #region Constructors and operators

        /// <summary>
        /// The default constructor for the WeaponRanks class.
        /// </summary>
        public WeaponRanks() { }


        /// <summary>
        /// The copy constructor for the WeaponRanks class.
        /// </summary>
        /// <param name="copy">The object to copy data from.</param>
        public WeaponRanks (WeaponRanks copy)
        {
            if (copy)
            {
                sword = copy.sword;
                axe = copy.axe;
                lance = copy.lance;
                bow = copy.bow;
                fire = copy.fire;
                thunder = copy.thunder;
                wind = copy.wind;
                light = copy.light;
                staff = copy.staff;
                knife = copy.knife;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a WeaponRanks object with a null pointer");
            }
        }


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="weaponRanks">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (WeaponRanks weaponRanks)
        {
            return weaponRanks != null;
        }

        #endregion


        #region XML functionality

        /// <summary>
        /// Converts the data stored within the object into XML.
        /// </summary>
        /// <returns>An XML version of the object.</returns>
        public XElement ToXElement()
        {
            XElement element = new XElement ("WeaponRanks");

            element.Add (   new XAttribute ("Sword", (int) sword),
                            new XAttribute ("Axe", (int) axe),
                            new XAttribute ("Lance", (int) lance),
                            new XAttribute ("Bow", (int) bow),
                            new XAttribute ("Fire", (int) fire),
                            new XAttribute ("Thunder", (int) thunder),
                            new XAttribute ("Wind", (int) wind),
                            new XAttribute ("Light", (int) light),
                            new XAttribute ("Staff", (int) staff),
                            new XAttribute ("Knife", (int) knife));

            return element;
        }


        /// <summary>
        /// Creates a WeaponRanks object from an XElement node.
        /// </summary>
        /// <param name="element">The XML of the object.</param>
        /// <returns>The duplicated object.</returns>
        public static WeaponRanks FromXElement (XElement element)
        {
            // Create a stats object.
            WeaponRanks ranks   = new WeaponRanks();
            
            // Fill it with data. Use properties to handle corrupt data.
            ranks.sword     = (WeaponRank) Convert.ToInt32 (element.Attribute ("Sword").Value);
            ranks.axe       = (WeaponRank) Convert.ToInt32 (element.Attribute ("Axe").Value);
            ranks.lance     = (WeaponRank) Convert.ToInt32 (element.Attribute ("Lance").Value);
            ranks.bow       = (WeaponRank) Convert.ToInt32 (element.Attribute ("Bow").Value);
            ranks.fire      = (WeaponRank) Convert.ToInt32 (element.Attribute ("Fire").Value);
            ranks.thunder   = (WeaponRank) Convert.ToInt32 (element.Attribute ("Thunder").Value);
            ranks.wind      = (WeaponRank) Convert.ToInt32 (element.Attribute ("Wind").Value);
            ranks.light     = (WeaponRank) Convert.ToInt32 (element.Attribute ("Light").Value);
            ranks.staff     = (WeaponRank) Convert.ToInt32 (element.Attribute ("Staff").Value);
            ranks.knife     = (WeaponRank) Convert.ToInt32 (element.Attribute ("Knife").Value);
           
            // Return the processed object.
            return ranks;
        }

        #endregion

    }
}
