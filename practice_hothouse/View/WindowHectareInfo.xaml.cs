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
using System.Xml;

namespace practice_hothouse.View
{
    /// <summary>
    /// Логика взаимодействия для WindowHectareInfo.xaml
    /// </summary>
    public partial class WindowHectareInfo : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        Hectare h;
        public WindowHectareInfo(Hectare currentHectare)
        {
            InitializeComponent();
            h = currentHectare;
            nameLabel.Content = "Вы выбрали " + h.HectareName + "!";
            var listViewData = (from sowing in db.Sowings
                                join hectare in db.Hectares on sowing.HectareId equals hectare.HectareId
                                join seed in db.Seeds on sowing.SeedId equals seed.SeedId
                                where hectare.HectareId == currentHectare.HectareId
                                select new
                                {
                                    sowing.SowingDate,
                                    seed.DaysToFirstHarvest,
                                    seed.HarvestFrequency,
                                    seed.NumberOfHarvests,
                                    SeedType = seed.SeedType,
                                    SeedVariety = seed.SeedVariety
                                })
                     .AsEnumerable() // Преобразуем в память, чтобы использовать локальную функцию
                     .Select(data => new
                     {
                         data.SeedType,
                         data.SeedVariety,
                         GreenhouseHarvestDate = CalculateHarvestDate(
                             data.SowingDate.HasValue ? data.SowingDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                             data.DaysToFirstHarvest,
                             data.HarvestFrequency,
                             data.NumberOfHarvests)
                     }).ToList();

            // Локальная функция для вычисления даты
            DateTime? CalculateHarvestDate(DateTime? sowingDate, int? daysToFirstHarvest, int? harvestFrequency, int? numberOfHarvests)
            {
                if (sowingDate == null || daysToFirstHarvest == null || harvestFrequency == null || numberOfHarvests == null)
                    return null;

                return sowingDate.Value
                    .AddDays(daysToFirstHarvest.Value)
                    .AddDays(harvestFrequency.Value * numberOfHarvests.Value);
            }

            lvHectare.ItemsSource = listViewData;

        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

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

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            WindowHectare wHectare = new WindowHectare(App.currentPlot);
            wHectare.Show();
            Close();
        }
    }
}
