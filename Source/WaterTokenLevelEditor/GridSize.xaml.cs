using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WaterTokenLevelEditor
{
    /// <summary>
    /// Interaction logic for GridSize.xaml
    /// </summary>
    public partial class GridSize : Window
    {
        #region Constructor and operators

        /// <summary>
        /// Initialises the window.
        /// </summary>
        public GridSize()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="grid">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (GridSize grid)
        {
            return grid != null;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the grid-width slider value. This will return zero if not initialised and the value will be clamped to the maximum and minimum values of the slider.
        /// </summary>
        public uint gridWidth
        {
            get
            {
                if (sdr_width.IsInitialized)
                {
                    return (uint) sdr_width.Value;
                }

                return 0;
            }

            set
            {
                // Ensure the value is within the given range.
                if (sdr_width.IsInitialized)
                {
                    if (value > sdr_width.Maximum)
                    {
                        sdr_width.Value = sdr_width.Maximum;
                    }

                    else if (value < sdr_width.Minimum)
                    {
                        sdr_width.Value = sdr_width.Minimum;
                    }

                    else
                    {
                        sdr_width.Value = value;
                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets the grid-height slider value. This will return zero if not initialised and the value will be clamped to the maximum and minimum values of the slider.
        /// </summary>
        public uint gridHeight
        {
            get
            {
                if (sdr_height.IsInitialized)
                {
                    return (uint) sdr_height.Value;
                }

                return 0;
            }

            set
            {
                // Ensure the value is within the given range.
                if (sdr_height.IsInitialized)
                {
                    if (value > sdr_height.Maximum)
                    {
                        sdr_height.Value = sdr_height.Maximum;
                    }

                    else if (value < sdr_height.Minimum)
                    {
                        sdr_height.Value = sdr_height.Minimum;
                    }

                    else
                    {
                        sdr_height.Value = value;
                    }
                }
            }
        }

        #endregion


        #region Button events

        /// <summary>
        /// This will set the result as invalid so we know that no decision was made.
        /// </summary>
        private void Button_CancelClick (object sender, RoutedEventArgs e)
        {
            // Just cancel any changes made.
            DialogResult = false;
        }


        /// <summary>
        /// Indicate that changes have indeed been made and the grid size should be changed to the values specified in this window.
        /// </summary>
        private void Button_AcceptClick (object sender, RoutedEventArgs e)
        {
            // The data can obtained from the gridWidth and gridHeight properties.
            DialogResult = true;
        }

        #endregion
    }
}
