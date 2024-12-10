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
    /// Логика взаимодействия для WindowHectare.xaml
    /// </summary>
    public partial class WindowHectare : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        Plot p;
        Hectare h;
        public WindowHectare(Plot currentPlot)
        {
            InitializeComponent();
            p = currentPlot;
            Hectare h = new Hectare();
            string[] plotNames = { "первый", "второй", "третий", "четвёртый" };

            int plotID = p.PlotId;

            if (plotID >= 1 && plotID <= plotNames.Length)
            {
                plotNameLabel.Content = $"Вы выбрали {plotNames[plotID - 1]} участок!";
            }

            var listViewData = from plot in db.Plots
                               join hectare in db.Hectares on plot.PlotId equals hectare.PlotId
                               where plot.PlotId == hectare.PlotId && hectare.PlotId == currentPlot.PlotId
                               select new Hectare
                               {
                                   HectareId = hectare.HectareId,
                                   HectareName = hectare.HectareName,

                               };
            lvHectare.ItemsSource = listViewData.ToList();
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem listViewItem && listViewItem.Content is Hectare selectedItem)
            {
                int idHectare = selectedItem.HectareId;
                h = db.Hectares.FirstOrDefault(x => x.HectareId == idHectare);
                App.currentHectare = h;
                WindowHectareInfo wHectareInfo = new WindowHectareInfo(App.currentHectare);
                wHectareInfo.Show();
                Close();
            }
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

    }
}