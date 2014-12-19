using System;
using System.Collections.Generic;
using System.IO;
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
        
        private LevelGrid   m_grid              = null;                 //!< The management class for the displayable grid.

        private string      m_workingDirectory  = "";                   //!< Used for setting the sprite location of tiles.
        
        private Control     m_selectedControl   = null;                 //!< The currently selected UI control.
        private int         m_selectedTile      = 0;                    //!< The currently selected grid tile.

        private ContextMenu m_tileMenu          = null;                 //!< The context menu used when tiles are right-clicked.

        private bool        m_unsavedChanges    = false;                //!< Prompts the user to save when they risk losing data.

        #endregion


        #region Constructor and operators

        /// <summary>
        /// The entry point for the WPF application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initialise the grid system.
            m_grid = new LevelGrid (grd_levelGrid, Image_MouseLeftButtonDown, Image_MouseRightButtonDown);
            m_grid.CreateGrid();

            // Force the zoom level to update.
            sdr_zoom.Value = sdr_zoom.Value;

            m_workingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            lbl_statusLabel.Content = "Application ready...";
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

        /// <summary>
        /// Clears the current level grid and makes the editor appear as new for the user.
        /// </summary>
        /// <param name="gridSize">The GridSize object with the data required. Null values are silently ignored and nothing will happen.</param>
        private void CreateNewLevel (GridSize gridSize)
        {
            // Ensure we actually need to create a new level.
            if (gridSize)
            {
                m_grid.CreateGrid (gridSize.gridWidth, gridSize.gridHeight);

                lbl_statusLabel.Content = "Level ready for editing...";
            }
        }


        /// <summary>
        /// Opens a Grid Size window, waits for the user to choose a size and returns the object if they do. Null will be returned if no changes are required.
        /// </summary>
        private GridSize ProcessGridSize()
        {
            // Create the dialog box.
            GridSize gridSize = new GridSize();
            gridSize.Owner = this;

            // Copy over the current grid values.
            gridSize.gridWidth = m_grid.width;
            gridSize.gridHeight = m_grid.height;

            // Open the dialog box.
            gridSize.ShowDialog();

            // Check whether the user wants to change anything.
            if (gridSize.DialogResult == true)
            {
                return gridSize;
            }

            // No changes required? Just return null.
            return null;
        }


        /// <summary>
        /// Gets the context menu ready for usage.
        /// </summary>
        private void SetupContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            // Create the menu items.
            MenuItem reset = new MenuItem () { Header = "_Reset to default", Tag = 0 };
            MenuItem interactive = new MenuItem () { Header = "_Remove interactive object", Tag = 1 };
            MenuItem character = new MenuItem () { Header = "_Remove character object", Tag = 2  };
        }

        #endregion


        #region Menu events

        /// <summary>
        /// The button up event called when the new button is pressed. This will cause a completely blank level to be generated.
        /// </summary>
        private void Menu_NewClick (object sender, RoutedEventArgs e)
        {
            if (m_unsavedChanges)
            {
                string message = "There are unsaved changes, do you wish to save before continuing?";
                MessageBoxResult result = MessageBox.Show (message, "Warning", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        //TODO: Save
                        // Just pass in the returned value in case the user decides to cancel their action.
                        m_unsavedChanges = false;
                        CreateNewLevel (ProcessGridSize());
                        break;
                    
                    case MessageBoxResult.No:
                        // Just pass in the returned value in case the user decides to cancel their action.
                        m_unsavedChanges = false;
                        CreateNewLevel (ProcessGridSize());
                        break;

                    case MessageBoxResult.Cancel:
                        break;                    
                }
            }

            else
            {
                // Just pass in the returned value in case the user decides to cancel their action.
                CreateNewLevel (ProcessGridSize());
            }
        }


        /// <summary>
        /// The exit event which will close the window.
        /// </summary>
        private void Menu_ExitClick (object sender, RoutedEventArgs e)
        {
            if (m_unsavedChanges)
            {
                string message = "There are unsaved changes, do you wish to save before exiting?";
                MessageBoxResult result = MessageBox.Show (message, "Warning", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        //TODO: Save
                        win_mainWindow.Close();
                        break;
                    
                    case MessageBoxResult.No:
                        win_mainWindow.Close();
                        break;

                    case MessageBoxResult.Cancel:
                        break;                    
                }
            }

            else
            {
                win_mainWindow.Close();
            }
        }


        /// <summary>
        /// Opens the grid-size dialog box for the user to modify the current grid.
        /// </summary>
        private void Menu_GridSizeClick (object sender, RoutedEventArgs e)
        {
            // Run through the Grid Size menu.
            GridSize gridSize = ProcessGridSize();
            
            // Check whether any changes are required.
            if (gridSize && (gridSize.gridWidth != m_grid.width || gridSize.gridHeight != m_grid.height))
            {
                // Output a warning message if data will be lost.
                if (gridSize.gridWidth < m_grid.width || gridSize.gridHeight < m_grid.height)
                {
                    string message = "Resizing the grid will result in the loss of some data. Are you sure you wish to continue?";
                    MessageBoxResult result = MessageBox.Show (message, "Warning", MessageBoxButton.OKCancel);

                    switch (result)
                    {
                        case MessageBoxResult.OK:                            
                            m_grid.ResizeGrid (gridSize.gridWidth, gridSize.gridHeight);
                            
                            lbl_statusLabel.Content = "Grid resized...";
                            break;
                    
                        case MessageBoxResult.Cancel:
                            // Do nothing.
                            break; 
                    }
                }
                
                else
                {
                    m_grid.ResizeGrid (gridSize.gridWidth, gridSize.gridHeight);

                    lbl_statusLabel.Content = "Grid resized...";
                }
            }
        }

        #endregion


        #region GameObject placement events

        /// <summary>
        /// Colours any given control a dark gray colour with aqua text.
        /// </summary>
        private void Label_MouseEnter (object sender, MouseEventArgs e)
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
        private void Label_MouseLeave (object sender, MouseEventArgs e)
        {
            // Cast the sender to a Control so we can modify the colour.
            Control control = sender as Control;

            // Check whether the sender was indeed a Control.
            if (control != null && control != m_selectedControl)
            {
                control.Background = Brushes.Transparent;
                control.Foreground = Brushes.Black;
            }
        }


        /// <summary>
        /// Handles the selection and deselection of the labels. Maintains the currently selected object.
        /// </summary>
        private void Label_MouseDoubleClick (object sender, MouseButtonEventArgs e)
        {
            // Attempt to cast the label.
            Label label = sender as Label;
            Label current = m_selectedControl as Label;
            
            // Selection
            if (label != current)
            {
                // Assign the new control and deselect the previous.
                m_selectedControl = label;
                Label_MouseLeave (current, null);
            }

            // Deselection
            else
            {
                m_selectedControl = null;
            }
        }

        private void Image_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {

        }


        private void Image_MouseRightButtonDown (object sender, MouseButtonEventArgs e)
        {

        }

        #endregion
                
        
        #region Character properties
        
        /// <summary>
        /// This nasty function creates a character tile from all the information currently available and assigns it to the selected tile.
        /// </summary>
        private void UpdateCharacterLayer()
        {
            if (IsInitialized)
            {
                try
                {
                    // Obtain the character.
                    Character character = m_grid.GetGameTile (m_selectedTile).character;

                    UpdateCharacterCore (character);
                    UpdateCharacterStats (character);
                    UpdateCharacterWeaponRanks (character);
                }

                catch (Exception error)
                {
                    lbl_statusLabel.Content = "Error updating character tile: " + error.Message;
                }
            }
        }


        /// <summary>
        /// Simply updates the core character information from the properties panel.
        /// </summary>
        /// <param name="character">The character object to modify.</param>
        private void UpdateCharacterCore (Character character)
        {
            character.name              = txt_charName.Text.Trim();
            character.characterType     = (CharacterType) cmb_charType.SelectedIndex;
            character.characterClass    = (Class) cmb_charClass.SelectedIndex;
            character.affinity          = (Element) cmb_charAffinity.SelectedIndex;
            character.level             = (uint) sdr_charLevel.Value;
            
        }


        /// <summary>
        /// Simply updates the stats information from the properties panel.
        /// </summary>
        /// <param name="character">The character object to modify.</param>
        private void UpdateCharacterStats (Character character)
        {
            // Obtain and modify the stats values.
            Stats stats         = character.stats;
                    
            stats.hp            = (uint) sdr_charHP.Value;
            stats.strength      = (uint) sdr_charSTR.Value;
            stats.magic         = (uint) sdr_charMAG.Value;
            stats.skill         = (uint) sdr_charSKL.Value;
            stats.speed         = (uint) sdr_charSPD.Value;
            stats.luck          = (uint) sdr_charLCK.Value;
            stats.defense       = (uint) sdr_charDEF.Value;
            stats.resistance    = (uint) sdr_charRES.Value;
            stats.constitution  = (uint) sdr_charCON.Value;
            stats.weight        = (uint) sdr_charWEI.Value;
            stats.movement      = (uint) sdr_charMOV.Value;
            
        }


        /// <summary>
        /// Simply updates the weapon rank information from the properties panel.
        /// </summary>
        /// <param name="character">The character object to modify.</param>
        private void UpdateCharacterWeaponRanks (Character character)
        {
            // Obtain and modify the weapon rank values.
            WeaponRanks ranks   = character.ranks;
                
            ranks.sword     = (WeaponRank) cmb_charRankSword.SelectedIndex;
            ranks.axe       = (WeaponRank) cmb_charRankAxe.SelectedIndex;
            ranks.lance     = (WeaponRank) cmb_charRankLance.SelectedIndex;
            ranks.bow       = (WeaponRank) cmb_charRankBow.SelectedIndex;
            ranks.fire      = (WeaponRank) cmb_charRankFire.SelectedIndex;
            ranks.thunder   = (WeaponRank) cmb_charRankThunder.SelectedIndex;
            ranks.wind      = (WeaponRank) cmb_charRankWind.SelectedIndex;
            ranks.light     = (WeaponRank) cmb_charRankLight.SelectedIndex;
            ranks.staff     = (WeaponRank) cmb_charRankStaff.SelectedIndex;
            ranks.knife     = (WeaponRank) cmb_charRankKnife.SelectedIndex;
        }


        /// <summary>
        /// This should simply request the characters values to be updated.
        /// </summary>
        private void ComboBox_UpdateChar (object sender, SelectionChangedEventArgs e)
        {
            UpdateCharacterLayer();
        }


        /// <summary>
        /// This is called whenever any character related TextBox is updated.
        /// </summary>
        private void TextBox_UpdateChar (object sender, TextChangedEventArgs e)
        {
            UpdateCharacterLayer();
        }


        /// <summary>
        /// This is called whenever any character related Slider is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_UpdateChar (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateCharacterLayer();
        }

        #endregion


        #region Misc events

        /// <summary>
        /// Manages the displaying of sprites in the level grid as well as setting sprite variable of each tile.
        /// </summary>
        private void TextBox_SpriteBox (object sender, TextChangedEventArgs e)
        {
            // If any exceptions are thrown then the given location is not valid for the editor.
            try
            {
                TextBox textBox = sender as TextBox;

                // Ensure text is valid.
                string text = textBox.Text.Trim();
                textBox.Text = text;

                // Obtain the required data.
                LayerType layer = (LayerType) Convert.ToInt32 (textBox.Tag);

                Image sprite = m_grid.GetImageLayer (m_selectedTile, layer);
                TileLayer data = m_grid.GetGameTile (m_selectedTile).GetLayer (layer);

                string location = m_workingDirectory + text;

                // Attempt to set the correct values.
                if (File.Exists (location))
                {                   
                    sprite.Source = new BitmapImage (new Uri (@location, UriKind.RelativeOrAbsolute));
                    
                    if (data)
                    {
                        data.sprite = text;
                    }
                }

                else
                {
                    sprite.Source = null;

                    if (data)
                    {
                        data.sprite = "";
                    }
                }
            }

            catch (Exception error)
            {
                lbl_statusLabel.Content = "Unable to load sprite: " + error.Message;
            }

            m_unsavedChanges = true;
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
