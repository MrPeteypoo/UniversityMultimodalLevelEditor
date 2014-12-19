using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace WaterTokenLevelEditor
{
    /// <summary>
    /// A management class which controls the controls displayed to the user in a level grid.
    /// </summary>
    public sealed class LevelGrid
    {

        #region Implementation data
        
        private const uint                      tileSize            = 32;                                       //!< How wide and tall each displayed tile should be in the application, multiplied by the zoom factor.
        
        private uint                            m_width             = 10;                                       //!< How many tiles wide the level grid is.
        private uint                            m_height            = 10;                                       //!< How many tiles tall the level grid is.
        private uint                            m_tileCount         = 100;                                      //!< The total tile count.

        private double                          m_zoomLevel         = 1.0;                                      //!< Is used to control the width and height value of the level grid.

        private string                          m_workingDirectory  = "";                                       //!< The working directory for default images.
        private string                          m_terrainInit       = "";                                       //!< The relative file location for terrain images.

        private Grid                            m_grid              = null;                                     //!< A pointer to the Grid control which is displayed to the user.
        private MouseButtonEventHandler         m_leftMouse         = null;                                     //!< The event handler for the left mouse-down event.
        private MouseButtonEventHandler         m_rightMouse        = null;                                     //!< The event handler for the right mouse-down event.
        
        private List<GameTile>                  m_data              = new List<GameTile>();                     //!< The list of GameTile data which is saved and loaded into XML.
        private List<Tuple<Rectangle, Image[]>> m_images            = new List<Tuple<Rectangle, Image[]>>();    //!< The list of images which are displayed on the grid. A rectangle is used to handle mouse events in case the image has dimensions of 0 x 0.

        #endregion


        #region Constructor and operators

        /// <summary>
        /// The default constructor for the LevelGrid class. LevelGrid required a Grid object so that it may perform itss functionality.
        /// </summary>
        /// <param name="grid">The Grid which the LevelGrid object should manage.</param>
        /// <param name="leftMouse"></param>
        /// <param name="rightMouse"></param>
        /// <param name="workingDirectory">The working directory to search for default images.</param>
        /// <param name="defaultTerrainImage">The default file that every image should be initialised to. This should be relative to the working directory.</param>
        public LevelGrid (Grid grid, MouseButtonEventHandler leftMouse, MouseButtonEventHandler rightMouse, string workingDirectory, string defaultTerrainImage)
        {
            if (grid != null && leftMouse != null && rightMouse != null)
            {
                m_grid = grid;
                m_leftMouse = leftMouse;
                m_rightMouse = rightMouse;

                SetDefaultTerrainImage (workingDirectory, defaultTerrainImage);
            }

            else
            {
                throw new ArgumentNullException ("Attempt to initialise a LevelGrid with a null argument.");
            }
        }


        /// <summary>
        /// A simple operator overload for the "if (variable) ;" shorthand.
        /// </summary>
        /// <param name="levelGrid">The object to check.</param>
        /// <returns>Whether the object is a null pointer.</returns>
        public static implicit operator bool (LevelGrid levelGrid)
        {
            return levelGrid != null;
        }

        #endregion


        #region Getters, setters and properties

        /// <summary>
        /// Gets the GameTile corresponding to a particular tile. This gives access to the actual tile data which will be written to a file.
        /// </summary>
        /// <param name="tile">The tile to obtain.</param>
        /// <returns>The GameTile object, null if the tile does not exist.</returns>
        public GameTile GetGameTile (int tile)
        {
            return IsValid (tile) ? m_data[tile] : null;
        }


        /// <summary>
        /// Gets the desired Image object which corresponds to a particular layer. This is what is displayed on the level grid.
        /// </summary>
        /// <param name="tile">The tile to obtain.</param>
        /// <param name="layer">The image layer to obtain.</param>
        /// <returns>The Image object, null if the tile does not exist.</returns>
        public Image GetImageLayer (int tile, LayerType layer)
        {
            return IsValid (tile) ? m_images[tile].Item2[(int) layer] : null;
        }


        /// <summary>
        /// Sets the default terrain image that every newly create terrain tile is initialised to.
        /// </summary>
        /// <param name="workingDirectory">The working directory to search for default images.</param>
        /// <param name="defaultTerrainImage">The default file that every image should be initialised to. This should be relative to the working directory.</param>
        public void SetDefaultTerrainImage (string workingDirectory, string defaultTerrainImage)
        {
            if (File.Exists (workingDirectory + defaultTerrainImage))
            {
                m_workingDirectory = workingDirectory;
                m_terrainInit = defaultTerrainImage;
            }
        }


        /// <summary>
        /// Configures the border around each layer of a particular tile.
        /// </summary>
        /// <param name="tile">The tile to colour.</param>
        /// <param name="borderThickness">The thickness values for the tile.</param>
        /// <param name="colour">The colour of the border.</param>
        public void SetTileBorder (int tile, Thickness borderThickness, Brush colour)
        {
            if (IsValid (tile))
            {
                // Scale the thickness with the zoom level.
                borderThickness.Left *= m_zoomLevel;
                borderThickness.Top *= m_zoomLevel;
                borderThickness.Right *= m_zoomLevel;
                borderThickness.Bottom *= m_zoomLevel;
                
                // Obtain the data.
                Tuple<Rectangle, Image[]> tuple = m_images[tile];
                
                // Colour the rectangle.
                tuple.Item1.SetValue (Border.BorderThicknessProperty, borderThickness);
                tuple.Item1.SetValue (Border.BorderBrushProperty, colour);

                // We don't know which images will actual be displayed so just give them all a border.
                foreach (Image image in tuple.Item2)
                {
                    image.SetValue (Border.BorderThicknessProperty, borderThickness);
                    image.SetValue (Border.BorderBrushProperty, colour);
                }
            }
        }

        
        /// <summary>
        /// Gets the how wide the level grid is in number of tiles.
        /// </summary>
        public uint width
        {
            get { return m_width; }
        }


        /// <summary>
        /// Gets the how tall the level grid is in number of tiles.
        /// </summary>
        public uint height
        {
            get { return m_height; }
        }


        /// <summary>
        /// Gets or sets the value for the current zoom level. Setting this value will cause an update of the grid visuals.
        /// </summary>
        public double zoomLevel
        {
            get { return m_zoomLevel; }
            set
            {
                if (value >= 0.0)
                {
                    m_zoomLevel = value;
                    UpdateGridVisuals();
                }

                else
                {
                    throw new ArgumentException ("Attempt to set LevelGrid.zoomLevel to a negative value.");
                }
            }
        }


        /// <summary>
        /// Gets the total number of tiles on the current level grid.
        /// </summary>
        public uint tileCount
        {
            get { return m_tileCount; }
        }
        
        #endregion


        #region Grid creation

        /// <summary>
        /// Creates a blank level grid, discarding all current data and starting afresh.
        /// </summary>
        public void CreateGrid()
        {
            // Start by clearing data.
            ClearData();

            // Now start afresh.
            CreateColumnsRows();
            ConstructData();
            UpdateGridVisuals();
        }


        /// <summary>
        /// Discards all data and creates a blank level grid. Allows for new width and height values to be specified.
        /// </summary>
        /// <param name="newWidth">The new value for the grid width.</param>
        /// <param name="newHeight">The new value for the grid height.</param>
        public void CreateGrid (uint newWidth, uint newHeight)
        {
            if (newWidth == 0 || newHeight == 0)
            {
                ResetToZero();
            }

            else
            {
                UpdateDimensions (newWidth, newHeight);
                CreateGrid();
            }
        }


        /// <summary>
        /// Clears all data relating to the level grid from memory.
        /// </summary>
        private void ClearData()
        {
            m_data.Clear();
            m_images.Clear();

            m_grid.Children.Clear();
            m_grid.ColumnDefinitions.Clear();
            m_grid.RowDefinitions.Clear();
        }


        /// <summary>
        /// Initialises the column and row definitions for the grid.
        /// </summary>
        private void CreateColumnsRows()
        {
            if (m_tileCount != 0)
            {
                // Create the columns.
                for (uint column = 0; column < m_width; ++column)
                {
                    m_grid.ColumnDefinitions.Add (new ColumnDefinition());
                }

                // Create the rows.
                for (uint row = 0; row < m_height; ++row)
                {
                    m_grid.RowDefinitions.Add (new RowDefinition());
                }
            }
        }


        /// <summary>
        /// Fills the observable collection and list with data.
        /// </summary>
        private void ConstructData()
        {
            if (m_tileCount != 0)
            {   
                m_data.Capacity = (int) (m_width * m_height);
                m_images.Capacity = (int) (m_width * m_height);

                for (uint y = 0; y < m_height; ++y)
                {
                    for (uint x = 0; x < m_width; ++x)
                    {
                        // Fill the containers with blank data!
                        m_data.Add (CreateBlankGameTile());
                        m_images.Add (CreateBlankVisualTile ((int) x, (int) y));
                    }
                }
            }
        }

        #endregion


        #region Grid manipulation

        /// <summary>
        /// Sets every terrain to the image source given.
        /// </summary>
        /// <param name="imageSource">The file location of the image source. This must be an absolute location.</param>
        public void FillTerrainWithImage (string imageSource)
        {
            if (File.Exists (imageSource))
            {
                for (int i = 0; i < m_data.Count; ++i)
                {
                    m_data[i].terrain.sprite = imageSource;
                    m_images[i].Item2[0].Source = new BitmapImage (new Uri (@imageSource, UriKind.RelativeOrAbsolute));
                }
            }
        }


        /// <summary>
        /// Attempts to resize the level grid whilst maintaining as much data as possible. Slower than the CreateGrid function.
        /// </summary>
        /// <param name="newWidth">The new width value of the level grid.</param>
        /// <param name="newHeight">The new height value of the level grid.</param>
        public void ResizeGrid (uint newWidth, uint newHeight)
        {
            // Account for zero values.
            if (newWidth == 0 || newHeight == 0)
            {
                ResetToZero();
            }

            else if (newWidth != m_width || newHeight != m_height)
            {
                // Start by preparing the grid.
                m_grid.Children.Clear();
                ResizeColumnDefinitions (newWidth);
                ResizeRowDefinitions (newHeight);

                // Begin the transfer process.
                FillResizedGrid (newWidth, newHeight);
                UpdateDimensions (newWidth, newHeight);
                UpdateGridVisuals();
            }
        }


        /// <summary>
        /// Removes or inserts the required column definitions to ensure the count matches the parameter given.
        /// </summary>
        /// <param name="count">How many column definitions there should be.</param>
        private void ResizeColumnDefinitions (uint count)
        {
            if (count < m_width)
            {
                // Reduce the number of columns.
                int difference = (int) (m_width - count);

                m_grid.ColumnDefinitions.RemoveRange ((int) (count - 1), difference);
            }

            else if (count > m_width)
            {
                // Increase the amount of columns, ensure no infinite loops occur.
                while (m_grid.ColumnDefinitions.Count != count && m_grid.ColumnDefinitions.Count != int.MaxValue)
                {
                    m_grid.ColumnDefinitions.Add (new ColumnDefinition());
                }
            }
        }


        /// <summary>
        /// Removes or inserts the required row definitions to ensure the count matches the parameter given.
        /// </summary>
        /// <param name="count">How many row definitions there should be.</param>
        private void ResizeRowDefinitions (uint count)
        {
            if (count < m_height)
            {
                // Reduce the number of columns.
                int difference = (int) (m_height - count);

                m_grid.RowDefinitions.RemoveRange ((int) (count - 1), difference);
            }

            else if (count > m_height)
            {
                // Increase the amount of rows, ensure no infinite loops occur.
                while (m_grid.RowDefinitions.Count != count && m_grid.RowDefinitions.Count != int.MaxValue)
                {
                    m_grid.RowDefinitions.Add (new RowDefinition());
                }
            }
        }


        private void FillResizedGrid (uint newWidth, uint newHeight)
        {
            // Create the new containers.
            List<GameTile>                  newData = new List<GameTile>();
            List<Tuple<Rectangle, Image[]>> newImages = new List<Tuple<Rectangle, Image[]>>();
            
            newData.Capacity = (int) (newWidth * newHeight);
            newImages.Capacity = (int) (newWidth * newHeight);


            // Move the data.
            for (uint y = 0; y < newHeight; ++y)
            {
                for (uint x = 0; x < newWidth; ++x)
                {
                    // Copy over current data if possible.
                    if (x < m_width && y < m_height)
                    {
                        // Calculate where the data should be moved to.
                        int offset = (int) (x + y * m_width);

                        // Move the images data and add it to the grid again.
                        Tuple<Rectangle, Image[]> tile = m_images[offset];

                        m_grid.Children.Add (tile.Item1);

                        foreach (Image image in tile.Item2)
                        {
                            m_grid.Children.Add (image);
                        }
                        

                        // Move the data.
                        newData.Add (m_data[offset]);
                        newImages.Add (m_images[offset]);
                    }

                    // Otherwise create blank data.
                    else
                    {
                        newData.Add (CreateBlankGameTile());
                        newImages.Add (CreateBlankVisualTile ((int) x, (int) y));
                    }
                }
            }

            m_data = newData;
            m_images = newImages;
        }

        #endregion


        #region Utility functions
        
        /// <summary>
        /// Tests whether the input is valid and won't cause range-errors.
        /// </summary>
        /// <param name="tile">The tile value to check.</param>
        /// <returns>The validity of the tile value.</returns>
        public bool IsValid (int tile)
        {
            return tile >= 0 && tile < tileCount;
        }


        /// <summary>
        /// Creates a blank GameTile object with a default terrain sprite.
        /// </summary>
        /// <returns>The created object.</returns>
        private GameTile CreateBlankGameTile()
        {
            GameTile tile = new GameTile();
            tile.terrain.sprite = m_terrainInit;

            return tile;
        }


        /// <summary>
        /// Constructs a new array of Images which act as a layer in the level grid.
        /// </summary>
        /// <param name="column">The column where the layer should exist.</param>
        /// <param name="row">The row where the layer should exist.</param>
        /// <returns>A tuple of a clickable rectangle and the image layers.</returns>
        private Tuple<Rectangle, Image[]> CreateBlankVisualTile (int column, int row)
        {
            // We need three images, one for each layer. The rectangle provides the clickable area for events.
            Tuple<Rectangle, Image[]> tile = new Tuple<Rectangle, Image[]>
            (
                new Rectangle () { Fill = Brushes.Black, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch },
                new Image[3] 
                { 
                    new Image() { Stretch = Stretch.Fill, Source = new BitmapImage (new Uri (m_workingDirectory + m_terrainInit, UriKind.RelativeOrAbsolute)) },
                    new Image() { Stretch = Stretch.Fill },
                    new Image() { Stretch = Stretch.Fill }
                }
            );

            // Place the rectangle in the right area.
            tile.Item1.SetValue (Grid.ColumnProperty, column);
            tile.Item1.SetValue (Grid.RowProperty, row);
            tile.Item1.SetValue (Grid.ZIndexProperty, -1);

            m_grid.Children.Add (tile.Item1);

            Image[] layers = tile.Item2;

            // Initialise each layer.
            for (int i = 0; i < layers.Length; ++i)
            {
                // Position the images correctly.
                layers[i].SetValue (Grid.ColumnProperty, column);
                layers[i].SetValue (Grid.RowProperty, row);
                layers[i].SetValue (Grid.ZIndexProperty, i);
            
                layers[i].MouseLeftButtonDown += m_leftMouse;
                layers[i].MouseRightButtonDown += m_rightMouse;
                
                m_grid.Children.Add (layers[i]);
            }

            // Ensure there is always a rectangle available for clicking so image malfunctions don't break the application.
            tile.Item1.MouseLeftButtonDown += m_leftMouse;
            tile.Item1.MouseRightButtonDown += m_rightMouse;
            
            return tile;
        }


        /// <summary>
        /// Hopefully a function that will never be used, will set the width and height to zero as well as clearing all data.
        /// </summary>
        private void ResetToZero()
        {
            ClearData();
            UpdateDimensions (0, 0);
            UpdateGridVisuals();
        }


        /// <summary>
        /// Simply updates the width, height and tile count values to reflect the current situation.
        /// </summary>
        /// <param name="newWidth">The value to set the width to.</param>
        /// <param name="newHeight">The value to set the height to.</param>
        private void UpdateDimensions (uint newWidth, uint newHeight)
        {
            m_width = newWidth;
            m_height = newHeight;
            m_tileCount = newWidth * newHeight;
        }


        /// <summary>
        /// Resizes the visual height and width of the grid. Useful when changing zoom level or the size of the grid itself.
        /// </summary>
        private void UpdateGridVisuals()
        {
            uint actualTileSize = (uint) (tileSize * m_zoomLevel);
                
            m_grid.Width = actualTileSize * m_width;
            m_grid.Height = actualTileSize * m_height;
        }

        #endregion


        #region XML functionality

        /// <summary>
        /// Goes through the entirity of the grids data and creates a saveable XML document.
        /// </summary>
        /// <returns>A complete XML document of all level data.</returns>
        public XDocument GenerateXML()
        {
            // Create the document and add the grid parameters.
            XDocument xml = new XDocument();

            XElement grid =   new XElement ("Grid", 
                                            new XAttribute ("Width", m_width), new XAttribute ("Height", m_height),
                                            new XAttribute ("WorkingDirectory", m_workingDirectory), new XAttribute ("TerrainDefault", m_terrainInit)
                                        );

            // Traverse the deep maze of game tiles producing the XML.
            XElement gameTiles = new XElement ("GameTiles");

            foreach (GameTile gameTile in m_data)
            {
                gameTiles.Add (gameTile.ToXElement());
            }

            // Finally construct the XML document, ready to return.
            grid.Add (gameTiles);
            xml.Add (grid);

            return xml;
        }

        #endregion

    }
}
