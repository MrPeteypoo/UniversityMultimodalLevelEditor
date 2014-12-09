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
        /// <summary>
        /// The entry point for the WPF application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }


        #region WPF Events


        /// <summary>
        /// Colours any given control a dark gray colour with aqua text.
        /// </summary>
        private void btn_MouseEnter (object sender, MouseEventArgs e)
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
        private void btn_MouseLeave (object sender, MouseEventArgs e)
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


        #endregion

        private void win_mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Image> collection = new ObservableCollection<Image>();
            
            for (int y = 0; y < 12; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    collection.Add (new Image() { Source = new BitmapImage (new Uri (@"Images/alphaThing.png", UriKind.Relative)), Height = 32, Width = 32 });
                }

            }

            lst_levelGrid.ItemsSource = collection;
            //lst_levelGrid.it
        }
    }
}
