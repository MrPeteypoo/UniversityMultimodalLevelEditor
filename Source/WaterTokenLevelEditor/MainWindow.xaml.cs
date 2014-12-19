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
        
        private LevelGrid       m_grid              = null;                         //!< The management class for the displayable grid.

        private string          m_workingDirectory  = "";                           //!< Used for setting the sprite location of tiles.
        private const string    defaultTerrain      = "Defaults/terrain.png";       //!< The default terrain image.
        private const string    defaultInteractive  = "Defaults/interactive.png";   //!< The default interactive image.
        private const string    defaultCharacter    = "Defaults/character.png";     //!< The default character image.
        
        private Control         m_selectedControl   = null;                         //!< The currently selected UI control.
        private int             m_selectedTile      = -1;                            //!< The currently selected grid tile.

        private ContextMenu     m_tileMenu          = null;                         //!< The context menu used when tiles are right-clicked.

        private bool            m_unsavedChanges    = false;                        //!< Prompts the user to save when they risk losing data.
        private bool            m_updateData        = true;                         //!< Used to prevent events from firing whilst loading in data.

        #endregion


        #region Constructor and operators

        /// <summary>
        /// The entry point for the WPF application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // Now initialise the window itself.
            m_workingDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            // Initialise the grid system.
            m_grid = new LevelGrid (grd_levelGrid, Image_MouseLeftButtonDown, Image_MouseRightButtonDown, m_workingDirectory, defaultTerrain);
            m_grid.CreateGrid();

            // Force the zoom level to update.
            sdr_zoom.Value = sdr_zoom.Value;

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
                DeselectTile (m_selectedTile);
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


        /// <summary>
        /// Calculates the tile number of the given FrameworkElement.
        /// </summary>
        /// <param name="element">An object which should exist within a grid.</param>
        /// <returns>The calculated value.</returns>
        private int CalcGridTile (FrameworkElement element)
        {
            if (element != null)
            {
                int column = (int) element.GetValue (Grid.ColumnProperty);
                int row = (int) element.GetValue (Grid.RowProperty);

                return column + row * (int) m_grid.width;
            }

            return -1;
        }


        /// <summary>
        /// Using the tag of the parent of the selected control we calculate the correct LayerType selected.
        /// </summary>
        /// <returns>The LayerType of the selected control.</returns>
        private LayerType CalcSelectedLayerType()
        {
            if (m_selectedControl == null)
            {
                throw new NullReferenceException ("Attempt to call MainWindow.CalcSelectedLayerType() when there is no control selected.");
            }

            // We can use the tag of the parent object to determine the layer type.
            int enumValue = Convert.ToInt32 (m_selectedControl.Parent.GetValue (Control.TagProperty));

            return (LayerType) enumValue;
        }

        
        /// <summary>
        /// Displays a message box to confirm that the user is OK with overwriting a tile.
        /// </summary>
        /// <param name="layer">The layer name, this is displayed in the message.</param>
        /// <returns>Whether to proceed or not.</returns>
        private bool ConfirmTilePlacement (string layer)
        {
            string message          = "This tile already contains " + layer + " layer, this will modify the current layer.";
            MessageBoxResult result = MessageBox.Show (this, message, "Warning", MessageBoxButton.OKCancel);

            return result == MessageBoxResult.OK;
        }


        /// <summary>
        /// Sets the given expanders enabled and expanded values to the given value.
        /// </summary>
        /// <param name="expander">The expander to modify.</param>
        /// <param name="enabledAndExpanded">Whether it should be expanded and enabled.</param>
        private void SetExpanderStatus (Expander expander, bool enabledAndExpanded)
        {
            if (expander != null)
            {
                expander.IsExpanded = enabledAndExpanded;
                expander.IsEnabled = enabledAndExpanded;
            }
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
                            DeselectTile (m_selectedTile);
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
                    DeselectTile (m_selectedTile);
                    m_grid.ResizeGrid (gridSize.gridWidth, gridSize.gridHeight);

                    lbl_statusLabel.Content = "Grid resized...";
                }
            }
        }

        #endregion


        #region GameObject selection

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

            // Ensure no tile is selected.
            DeselectTile (m_selectedTile);
        }

        #endregion


        #region Level grid events

        /// <summary>
        /// The main handler of all tile selection and placement of object.
        /// </summary>
        private void Image_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            // We need to start by calculating the current tile.
            FrameworkElement element = sender as FrameworkElement;
            int tile = CalcGridTile (element);

            // We must first check if we're supposed to be placing a tile.
            if (m_selectedControl != null)
            {
                // Place the selected object.
                switch (CalcSelectedLayerType())
                {
                    case LayerType.Terrain:
                        PlaceTerrain (m_grid.GetGameTile (tile));
                        break;

                    case LayerType.Interactive:
                        PlaceInteractive (m_grid.GetGameTile (tile), tile);
                        break;

                    case LayerType.Character:
                        PlaceCharacter (m_grid.GetGameTile (tile), tile);
                        break;

                    default:
                        throw new NotImplementedException ("Attempt to place unhandled LayerType in MainWindow.Image_MouseLeftButtonDown().");
                }
            }

            // We must be selecting a tile so do that instead!
            else
            {
                SelectTile (tile);
            }
        }


        private void Image_MouseRightButtonDown (object sender, MouseButtonEventArgs e)
        {

        }

        #endregion


        #region Tile placement

        /// <summary>
        /// Places a terrain object at the given tile.
        /// </summary>
        /// <param name="tile">The tile to place the terrain at.</param>
        private void PlaceTerrain (GameTile tile)
        {
            // Obtain the terrain tile.
            Terrain terrain = tile.terrain;
            
            // Calculate the correct terrain type.
            TerrainType type = (TerrainType) Convert.ToInt32 (m_selectedControl.Tag);

            // Swap over the values. TODO: Have the default terrain values set via the application configuration.
            terrain.terrainType = type;
        }


        /// <summary>
        /// Places an interactive object at the given tile.
        /// </summary>
        /// <param name="tile">The tile to place the object at.</param>
        /// <param name="tileNumber">The tile number of the object, used if intialising a new object is required.</param>
        private void PlaceInteractive (GameTile tile, int tileNumber = -1)
        {
            // Check if we need to create the Interactive tile or just modify the existing tile.
            Interactive interactive = tile.interactive;
            bool writeData = true;

            if (!interactive)
            {
                // We need to create the tile with a default image.
                interactive = new Interactive();

                if (m_grid.IsValid (tileNumber) && File.Exists (m_workingDirectory + defaultInteractive))
                {
                    Image image = m_grid.GetImageLayer (tileNumber, LayerType.Interactive);

                    image.Source = new BitmapImage (new Uri (m_workingDirectory + defaultInteractive, UriKind.RelativeOrAbsolute));
                    interactive.sprite = defaultInteractive;
                }
            }

            else
            {
                // Check whether they want to overwrite data.
                writeData = ConfirmTilePlacement ("an interactive");
            }

            if (writeData)
            {
                // Carry on where we left off.
                InteractiveType type = (InteractiveType) Convert.ToInt32 (m_selectedControl.Tag);

                interactive.interactiveType = type;

                tile.interactive = interactive;
            }            
        }


        /// <summary>
        /// Places a character object at the given tile.
        /// </summary>
        /// <param name="tile">The tile to place the character at.</param>
        /// <param name="tileNumber">The tile number of the object, used if intialising a new object is required.</param>
        private void PlaceCharacter (GameTile tile, int tileNumber = -1)
        {
            // Check if we need to create the Interactive tile or just modify the existing tile.
            Character character = tile.character;
            bool writeData = true;

            if (!character)
            {
                // We need to create the tile with a default image.
                character = new Character();

                if (m_grid.IsValid (tileNumber) && File.Exists (m_workingDirectory + defaultCharacter))
                {
                    Image image = m_grid.GetImageLayer (tileNumber, LayerType.Character);

                    image.Source = new BitmapImage (new Uri (m_workingDirectory + defaultCharacter, UriKind.RelativeOrAbsolute));
                    character.sprite = defaultCharacter;
                }
            }

            else
            {
                // Check whether they want to overwrite data.
                writeData = ConfirmTilePlacement ("a character");
            }

            if (writeData)
            {
                // Carry on where we left off.
                CharacterType type = (CharacterType) Convert.ToInt32 (m_selectedControl.Tag);

                character.characterType = type;
                
                // TODO: NEEDZ MOR DEFOLTZ!
                tile.character = character;
            }
        }

        #endregion


        #region Tile selection

        /// <summary>
        /// Selects the given tile, this enables the properties panel and gives it the correct data for the user to modify.
        /// </summary>
        /// <param name="tile">The tile to select.</param>
        private void SelectTile (int tile)
        {
            // Stop the updating of data.
            m_updateData = false;

            // First check if we should actually be deselecting the tile.
            if (tile == m_selectedTile)
            {
                DeselectTile (tile);
            }

            else
            {
                // Ensure the value is valid before populating the properties panel.
                if (m_grid.IsValid (tile))
                {
                    // Visually set the tile.
                    m_grid.SetTileBorder (tile, new Thickness (5), Brushes.Aqua);

                    // Fill the properties panel with lovely data.
                    GameTile data = m_grid.GetGameTile (tile);
                    
                    FillTerrainProperties (data.terrain);
                    FillInteractiveProperties (data.interactive);
                    FillCharacterProperties (data.character);

                    // Set the expanders status to avoid potential errors from arising. 
                    SetExpanderStatus (exp_terrainProp, data.terrain);
                    SetExpanderStatus (exp_interProp, data.interactive);
                    SetExpanderStatus (exp_charProp, data.character);

                    // We have to deselect the old tile after selecting the current to ensure expanders don't unnecessarily close.
                    int oldTile = m_selectedTile;
                    m_selectedTile = tile;

                    DeselectTile (oldTile);
                }
            }

            // Allow updating again.
            m_updateData = true;
        }


        /// <summary>
        /// Deselects the given tile, this will remove all borders and disable all property controls.
        /// </summary>
        /// <param name="tile">The tile to disable.</param>
        private void DeselectTile (int tile)
        {
            if (tile != -1)
            {            
                // We can assume the set function will check if the tile is valid.
                m_grid.SetTileBorder (tile, new Thickness (0), Brushes.Transparent);

                // Close the properties window since it is no longer useful.
                if (tile == m_selectedTile)
                {
                    SetExpanderStatus (exp_terrainProp, false);
                    SetExpanderStatus (exp_interProp, false);
                    SetExpanderStatus (exp_charProp, false);

                    m_selectedTile = -1;
                }
            }
        }


        /// <summary>
        /// Simply extracts the data from a given Terrain object and displays it in the properties panel.
        /// </summary>
        /// <param name="data">The data to retrieve.</param>
        private void FillTerrainProperties (Terrain data)
        {
            if (data)
            {
                // Simply move the data across.
                txt_terrainSprite.Text          = data.sprite;

                cmb_terrainType.SelectedIndex   = (int) data.terrainType;

                sdr_terrainDEF.Value            = data.defenseBonus;
                sdr_terrainRES.Value            = data.resistanceBonus;
                sdr_terrainEVA.Value            = data.evasionBonus * 100.0;
                sdr_terrainMOV.Value            = data.moveCost;
            }
        }


        /// <summary>
        /// Simply extracts the data from a given Interactive object and displays it in the properties panel.
        /// </summary>
        /// <param name="data">The data to retrieve.</param>
        private void FillInteractiveProperties (Interactive data)
        {
            if (data)
            {
                // Not much to move really.....
                txt_interSprite.Text        = data.sprite;

                cmb_interType.SelectedIndex = (int) data.interactiveType;

                sdr_interEffect.Value       = data.effect;
            }
        }


        /// <summary>
        /// Simply extracts the data from a given Character object and displays it in the properties panel.
        /// </summary>
        /// <param name="data">The data to retrieve.</param>
        private void FillCharacterProperties (Character data)
        {
            if (data)
            {
                // We have a lot of data to deal with so lets just call some functions.
                FillCharacterPropertiesCore (data);
                FillCharacterPropertiesStats (data.stats);
                FillCharacterPropertiesWeaponRanks (data.ranks);
            }
        }


        /// <summary>
        /// Only updates the core character properties.
        /// </summary>
        /// <param name="data">The data to read from.</param>
        private void FillCharacterPropertiesCore (Character data)
        {
            if (data)
            {
                txt_charSprite.Text             = data.sprite;

                cmb_charType.SelectedIndex      = (int) data.characterType;
                cmb_charClass.SelectedIndex     = (int) data.characterClass;
                cmb_charAffinity.SelectedIndex  = (int) data.affinity;
                
                sdr_charLevel.Value             = data.level;
            }
        }


        /// <summary>
        /// Only updates the stats part of the character properties.
        /// </summary>
        /// <param name="data">The data to read from.</param>
        private void FillCharacterPropertiesStats (Stats data)
        {
            if (data)
            {
                sdr_charHP.Value    = data.hp;
                sdr_charSTR.Value   = data.strength;
                sdr_charMAG.Value   = data.magic;
                sdr_charSKL.Value   = data.skill;
                sdr_charSPD.Value   = data.speed;
                sdr_charLCK.Value   = data.luck;
                sdr_charDEF.Value   = data.defense;
                sdr_charRES.Value   = data.resistance;
                sdr_charCON.Value   = data.constitution;
                sdr_charWEI.Value   = data.weight;
                sdr_charMOV.Value   = data.movement;
            }
        }


        /// <summary>
        /// Only updates the weapon ranks part of the character properties.
        /// </summary>
        /// <param name="data"></param>
        private void FillCharacterPropertiesWeaponRanks (WeaponRanks data)
        {
            if (data)
            {
                cmb_charRankSword.SelectedIndex     = (int) data.sword;
                cmb_charRankAxe.SelectedIndex       = (int) data.axe;
                cmb_charRankLance.SelectedIndex     = (int) data.lance;
                cmb_charRankBow.SelectedIndex       = (int) data.bow;
                cmb_charRankFire.SelectedIndex      = (int) data.fire;
                cmb_charRankThunder.SelectedIndex   = (int) data.thunder;
                cmb_charRankWind.SelectedIndex      = (int) data.wind;
                cmb_charRankLight.SelectedIndex     = (int) data.light;
                cmb_charRankStaff.SelectedIndex     = (int) data.staff;
                cmb_charRankKnife.SelectedIndex     = (int) data.knife;
            }
        }

        #endregion


        #region Terrain properties panel

        /// <summary>
        /// This function fills the terrain layer of the currently selected tile with all the available information.
        /// </summary>
        private void UpdateTerrainLayer()
        {
            if (IsInitialized && m_selectedTile != -1 && m_updateData)
            {
                try
                {
                    // Obtain the correct layer.
                    Terrain layer           = m_grid.GetGameTile (m_selectedTile).terrain;

                    // Set the current values.
                    layer.terrainType       = (TerrainType) cmb_terrainType.SelectedIndex;
                    layer.defenseBonus      = (uint) sdr_terrainDEF.Value;
                    layer.resistanceBonus   = (uint) sdr_terrainRES.Value;

                    // Avoid divide-by-zero errors.
                    layer.evasionBonus      = sdr_terrainEVA.Value != 0.0 ? sdr_terrainEVA.Value / 100.0 : 0.0;
                    layer.moveCost          = (uint) sdr_terrainMOV.Value;

                    m_unsavedChanges = true;
                }

                catch (Exception error)
                {
                    lbl_statusLabel.Content = "Error updating terrain tile: " + error.Message;
                }
            }
        }


        /// <summary>
        /// This simply requests that the terrain information be updated.
        /// </summary>
        private void ComboBox_UpdateTerrain (object sender, SelectionChangedEventArgs e)
        {
            UpdateTerrainLayer();
        }


        /// <summary>
        /// This simply requests that the terrain information be updated.
        /// </summary>
        private void Slider_UpdateTerrain (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateTerrainLayer();
        }

        #endregion


        #region Interactive properties panel

        /// <summary>
        /// This function fills the interactive layer of the currently selected tile with all the available information.
        /// </summary>
        private void UpdateInteractiveLayer()
        {
            if (IsInitialized && m_selectedTile != -1 && m_updateData)
            {
                try
                {
                    // Obtain the correct layer.
                    Interactive layer       = m_grid.GetGameTile (m_selectedTile).interactive;

                    // Set the current values.
                    layer.interactiveType   = (InteractiveType) cmb_interType.SelectedIndex;
                    layer.effect            = (uint) sdr_interEffect.Value;

                    m_unsavedChanges = true;
                }

                catch (Exception error)
                {
                    lbl_statusLabel.Content = "Error updating interactive tile: " + error.Message;
                }
            }
        }

        
        /// <summary>
        /// This should simply request the interactive values to be updated.
        /// </summary>
        private void ComboBox_UpdateInteractive (object sender, SelectionChangedEventArgs e)
        {
            UpdateInteractiveLayer();
        }


        /// <summary>
        /// This should simply request the interactive values to be updated.
        /// </summary>
        private void TextBox_UpdateInteractive (object sender, TextChangedEventArgs e)
        {
            UpdateInteractiveLayer();
        }


        /// <summary>
        /// This should simply request the interactive values to be updated.
        /// </summary>
        private void Slider_UpdateInteractive (object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateInteractiveLayer();
        }

        #endregion
                
        
        #region Character properties panel
        
        /// <summary>
        /// This function fills the character tile with all the information currently available.
        /// </summary>
        private void UpdateCharacterLayer()
        {
            if (IsInitialized && m_selectedTile != -1 && m_updateData)
            {
                try
                {
                    // Obtain the character.
                    Character character = m_grid.GetGameTile (m_selectedTile).character;

                    UpdateCharacterCore (character);
                    UpdateCharacterStats (character);
                    UpdateCharacterWeaponRanks (character);

                    m_unsavedChanges = true;
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
            if (IsInitialized && m_selectedTile != -1 && m_updateData)
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
                        if (data)
                        {
                            sprite.Source = new BitmapImage (new Uri (@location, UriKind.RelativeOrAbsolute));
                            data.sprite = text;
                        }
                    }

                    else
                    {
                        if (data)
                        {
                            sprite.Source = null;
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
