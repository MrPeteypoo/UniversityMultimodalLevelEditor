using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// A simple range class which holds a minimum and maximum value.
    /// </summary>
    public sealed class Range<T> where T : struct, IComparable<T>
    {
        private T m_minimum;    //!< The minimum value of the range.
        private T m_maximum;    //!< The maximum value of the range.


        #region Constructors and operators

        /// <summary>
        /// The constructor for the Range class. Clamps the minimum and maximum values to avoid incorrect values.
        /// </summary>
        /// <param name="minimum">The minimum value of the range.</param>
        /// <param name="maximum">The maximum value of the range.</param>
        public Range (T minimum, T maximum)
        {
            m_minimum = MathMin (minimum, maximum);
            m_maximum = MathMax (minimum, maximum);            
        }


        /// <summary>
        /// The default copy constructor for the Range class.
        /// </summary>
        /// <param name="copy">The object to copy from.</param>
        public Range (Range<T> copy)
        {
            if (copy)
            {
                m_minimum = copy.m_minimum;
                m_maximum = copy.m_maximum;
            }

            else
            {
                throw new ArgumentNullException ("Attempt to use the Range copy constructor with a null object.");
            }
        }


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="range">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (Range<T> range)
        {
            return range != null;
        }

        #endregion


        #region Getters, setters & properties

        /// <summary>
        /// Gets or sets the minimum value of the range, cannot exceed the maximum or the value will be clamped.
        /// </summary>
        public T minimum 
        {
            get { return m_minimum; }
            set 
            {
                m_minimum = MathMin (value, m_maximum);
            }
        }


        /// <summary>
        /// Gets or sets the maximum value of the range, cannot be less than the minimum or the value will be clamped.
        /// </summary>
        public T maximum
        {
            get { return m_maximum; }
            set 
            {
                m_maximum = MathMax (value, m_minimum);
            }
        }

        #endregion


        #region Utility

        /// <summary>
        /// An implementation of the Math.Min function for the Range template.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>The minimum value from the two given parameters.</returns>
        private T MathMin (T left, T right)
        {
            return left.CompareTo (right) >= 0 ? left : right;
        }

        
        /// <summary>
        /// An implementation of the Math.Max function for the Range template.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        /// <returns>The maximum value from the two given parameters.</returns>
        private T MathMax (T left, T right)
        {
            return left.CompareTo (right) <= 0 ? left : right;
        }

        #endregion
    }
}
