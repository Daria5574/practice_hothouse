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

namespace practice_hothouse.View
{
    /// <summary>
    /// Логика взаимодействия для WindowSeeds.xaml
    /// </summary>
    public partial class WindowSeeds : Window
    {
        public WindowSeeds()
        {
            InitializeComponent();
        }
        private void MouseLeftButtonDown_profile(object sender, MouseButtonEventArgs e)
        {
            WindowProfile wProfile = new WindowProfile(App.currentUser);
            wProfile.Show();
            Close();
        }
        public void MouseLeftButtonDown_main(object sender, MouseButtonEventArgs e)
        {
            WindowPlots wPlots = new WindowPlots(App.currentUser);
            wPlots.Show();
            Close();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            WindowAddSeed wAddSeed = new WindowAddSeed();
            wAddSeed.Show();
            Close();
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
