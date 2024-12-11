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
    /// Логика взаимодействия для WindowAddSeed.xaml
    /// </summary>
    public partial class WindowAddSeed : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        public WindowAddSeed()
        {
            InitializeComponent();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string seedType = typeTextBox.Text.Trim();
            string seedVariety = varietyTextBox.Text.Trim();
            int daysToFirstHarvest = int.Parse(daysToFirstHarvestTextBox.Text.Trim());
            int harvestFrequency = int.Parse(harvestFrequencyTextBox.Text.Trim());
            int numberOfHarvests = int.Parse(numberOfHarvestsTextBox.Text.Trim());
            string additionalNotes = additionalNotesTextBox.Text.Trim();

            Seed newSeed = new Seed
            {
                SeedType = seedType,
                SeedVariety = seedVariety,
                DaysToFirstHarvest = daysToFirstHarvest,
                HarvestFrequency = harvestFrequency,
                NumberOfHarvests = numberOfHarvests,
                AdditionalNotes = additionalNotes
            };

            db.Seeds.Add(newSeed);
            db.SaveChanges();

            MessageBox.Show("Семена добавлены успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            WindowSeeds wSeeds = new WindowSeeds();
            wSeeds.Show();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowSeeds wSeeds = new WindowSeeds();
            wSeeds.Show();
            Close();
        }
        private void DaysToFirstHarvestTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void HarvestFrequencyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void NumberOfHarvestsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
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

