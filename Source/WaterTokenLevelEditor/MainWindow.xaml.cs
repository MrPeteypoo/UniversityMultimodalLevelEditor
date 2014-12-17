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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Implementation data
        
        private LevelGrid   m_grid              = null;     //!< The management class for the displayable grid.

        private bool        m_unsavedChanges    = false;    //!< Prompts the user to save when they risk losing data.

        #endregion


        #region Constructor and operators

        /// <summary>
        /// The entry point for the WPF application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialise the grid system.
            m_grid = new LevelGrid (grd_levelGrid);
            sdr_zoom.Value = sdr_zoom.Value;
            m_grid.CreateGrid();

            lbl_statusLabel.Content = "Level grid loaded...";
        }

       
        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="window">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (MainWindow window)
        {
            return window != null;
        }

        #endregion


        #region Utility functions



        #endregion


        #region WPF events

        /// <summary>
        /// The button up event called when the new button is pressed. This will cause a completely blank level to be generated.
        /// </summary>
        private void Menu_NewClick (object sender, RoutedEventArgs e)
        {
            if (m_unsavedChanges)
            {
                string message = "There are unsaved changes, do you wish to save them before continuing?";
                MessageBoxResult result = MessageBox.Show (message, "Warning", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        //TODO: Save
                        break;
                    
                    case MessageBoxResult.No:
                        //Input
                        break;

                    case MessageBoxResult.Cancel:
                        break;                    
                }
            }

            else
            {

            }
        }


        /// <summary>
        /// The exit event which will close the window.
        /// </summary>
        private void Menu_ExitClick (object sender, RoutedEventArgs e)
        {
            if (m_unsavedChanges)
            {

            }

            win_mainWindow.Close();
        }


        /// <summary>
        /// Opens the grid-size dialog box for the user to modify the current grid.
        /// </summary>
        private void Menu_GridSizeClick (object sender, RoutedEventArgs e)
        {
            // Create the dialog box.
            GridSize gridSize = new GridSize();
            gridSize.Owner = this;

            // Copy over the current grid values.
            gridSize.gridWidth = m_grid.width;
            gridSize.gridHeight = m_grid.height;

            // Open the dialog box.
            gridSize.ShowDialog();

            // Check whether the user wants to change anything. Ignore the OK click if the values won't be changed.
            if (gridSize.DialogResult == true && (gridSize.gridWidth != m_grid.width || gridSize.gridHeight != m_grid.height))
            {
                string message = "Resizing the grid could result in the loss of data. Are you sure you wish to continue?";
                MessageBoxResult result = MessageBox.Show (message, "Warning", MessageBoxButton.OKCancel);

                switch (result)
                {
                    case MessageBoxResult.OK:
                        // TODO: Grid resizing.
                        break;
                    
                    case MessageBoxResult.Cancel:
                        // Do nothing.
                        break; 
                }
            }
        }


        /// <summary>
        /// Colours any given control a dark gray colour with aqua text.
        /// </summary>
        private void Button_MouseEnter (object sender, MouseEventArgs e)
        {
            // Cast the sender to a Control so we can modify the colour.
            Control control = sender as Control;

            // Check whether the sender was indeed a Control.
            if (control != null)
            {
                Color darkGray = Color.FromRgb (60, 60, 60);

                control.Background = new SolidColorBrush (darkGray);
                control.Foreground = Brushes.Aqua;
            }
        }


        /// <summary>
        /// Colours any given control transparent with black text.
        /// </summary>
        private void Button_MouseLeave (object sender, MouseEventArgs e)
        {
            // Cast the sender to a Control so we can modify the colour.
            Control control = sender as Control;

            // Check whether the sender was indeed a Control.
            if (control != null)
            {
                control.Background = Brushes.Transparent;
                control.Foreground = Brushes.Black;
            }
        }


        /// <summary>
        /// Handles the zooming functionality of the level grid.
        /// </summary>
        private void Slider_ZoomValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_grid)
            {
                // Ensure we convert it to a percentage.
                m_grid.zoomLevel = sdr_zoom.Value / 100.0;
            }
        }

        #endregion
    }
}
