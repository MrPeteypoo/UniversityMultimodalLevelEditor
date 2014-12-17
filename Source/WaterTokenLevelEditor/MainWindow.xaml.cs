using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        
        private uint                            m_gridWidth         = 10;                                   //!< The current width of the level grid.
        private uint                            m_gridHeight        = 10;                                   //!< The current height of the level grid.

        private const uint                       m_kTileSize         = 32;                                   //!< How wide and tall each displayed tile should be in the application, multiplied by the zoom factor.

        private bool                            m_unsavedChanges    = false;                                //!< Prompts the user to save when they risk losing data.
        
        private List<GameTile>                  m_data              = new List<GameTile>();                 //!< The list of GameTile data which is saved and loaded into XML.
        private ObservableCollection<Image[]>   m_images            = new ObservableCollection<Image[]>();  //!< The list of images which are displayed on the grid.

        #endregion


        #region Constructor and operators

        /// <summary>
        /// The entry point for the WPF application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
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


        #region Grid creation and maniuplation

        /// <summary>
        /// Creates a blank level grid, discarding all current data and starting afresh.
        /// </summary>
        private void CreateGrid()
        {
            // Start by clearing data.
            m_data.Clear();
            m_images.Clear();
            grd_levelGrid.ColumnDefinitions.Clear();
            grd_levelGrid.RowDefinitions.Clear();
            grd_levelGrid.Children.Clear();

            // Now start afresh.

        }

        #endregion


        /// <summary>
        /// Creates a test grid.
        /// </summary>
        private void MainWindowLoaded()
        {
            // Resize the grid.
            for (int x = 0; x < 24; ++x)
            {   
                grd_levelGrid.ColumnDefinitions.Add (new ColumnDefinition());
            }

            ObservableCollection<Image> collection = new ObservableCollection<Image>();
            
            for (int y = 0; y < 12; ++y)
            {
                grd_levelGrid.RowDefinitions.Add (new RowDefinition());
                for (int x = 0; x < 24; ++x)
                {
                    Image tile = new Image() {};// Source = new BitmapImage (new Uri (@"Images/alphaThing.png", UriKind.Relative)), Stretch = Stretch.Fill };
                    tile.SetValue (Grid.ColumnProperty, x);
                    tile.SetValue (Grid.RowProperty, y);
                    collection.Add (tile);
                }
            }

            for (int i = 0; i < collection.Count; ++i)
            {
                grd_levelGrid.Children.Add (collection[i]);
            }

            grd_levelGrid.Width = 32 * 24;
            grd_levelGrid.Height = 32 * 12;

            lbl_statusLabel.Content = "Level grid loaded...";
        }


        #region Utility functions



        #endregion


        #region WPF Events

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
            gridSize.gridWidth = m_gridWidth;
            gridSize.gridHeight = m_gridHeight;

            // Open the dialog box.
            gridSize.ShowDialog();

            // Check whether the user wants to change anything. Ignore the OK click if the values won't be changed.
            if (gridSize.DialogResult == true && (gridSize.gridWidth != m_gridWidth || gridSize.gridHeight != m_gridHeight))
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
            if (grd_levelGrid.Children.Count != 0)
            {
                // Adjust the zoom level of the grid.
                grd_levelGrid.Width = m_kTileSize * sdr_zoom.Value;
                grd_levelGrid.Height = m_kTileSize * sdr_zoom.Value;
            }
        }

        #endregion
    }
}
