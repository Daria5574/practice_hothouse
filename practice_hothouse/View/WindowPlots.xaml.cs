using practice_hothouse.Models;
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
    /// Логика взаимодействия для WindowPlots.xaml
    /// </summary>
    public partial class WindowPlots : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        Plot p;
        public WindowPlots(User currentUser)
        {
            InitializeComponent();
        }
        private void MouseLeftButtonDown_plot1(object sender, MouseButtonEventArgs e)
        {
            p = db.Plots.Where(c => c.PlotId == 1).FirstOrDefault();
            App.currentPlot = p;

            WindowHectare wHectare = new WindowHectare(p);
            wHectare.Show();
            Close();
        }

        private void MouseLeftButtonDown_plot2(object sender, MouseButtonEventArgs e)
        {
            p = db.Plots.Where(c => c.PlotId == 2).FirstOrDefault();
            App.currentPlot = p;

            WindowHectare wHectare = new WindowHectare(p);
            wHectare.Show();
            Close();
        }

        private void MouseLeftButtonDown_plot3(object sender, MouseButtonEventArgs e)
        {
            p = db.Plots.Where(c => c.PlotId == 3).FirstOrDefault();
            App.currentPlot = p;

            WindowHectare wHectare = new WindowHectare(p);
            wHectare.Show();
            Close();
        }

        private void MouseLeftButtonDown_plot4(object sender, MouseButtonEventArgs e)
        {
            p = db.Plots.Where(c => c.PlotId == 4).FirstOrDefault();
            App.currentPlot = p;

            WindowHectare wHectare = new WindowHectare(p);
            wHectare.Show();
            Close();
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
            WindowAddPlot wAddPlot = new WindowAddPlot();
            wAddPlot.Show();
            Close();
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

