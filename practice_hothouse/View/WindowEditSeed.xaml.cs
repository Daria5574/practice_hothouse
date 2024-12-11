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
    /// Логика взаимодействия для WindowEditSeed.xaml
    /// </summary>
    public partial class WindowEditSeed : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        private Seed _seedToEdit;

        public WindowEditSeed(Seed seed)
        {
            InitializeComponent();
            _seedToEdit = seed;
            typeTextBox.Text = seed.SeedType;
            varietyTextBox.Text = seed.SeedVariety;
            daysToFirstHarvestTextBox.Text = seed.DaysToFirstHarvest.ToString();
            harvestFrequencyTextBox.Text = seed.HarvestFrequency.ToString();
            numberOfHarvestsTextBox.Text = seed.NumberOfHarvests.ToString();
            additionalNotesTextBox.Text = seed.AdditionalNotes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _seedToEdit.SeedType = typeTextBox.Text.Trim();
            _seedToEdit.SeedVariety = varietyTextBox.Text.Trim();
            _seedToEdit.DaysToFirstHarvest = int.Parse(daysToFirstHarvestTextBox.Text.Trim());
            _seedToEdit.HarvestFrequency = int.Parse(harvestFrequencyTextBox.Text.Trim());
            _seedToEdit.NumberOfHarvests = int.Parse(numberOfHarvestsTextBox.Text.Trim());
            _seedToEdit.AdditionalNotes = additionalNotesTextBox.Text.Trim();
            var existingSeed = db.Seeds.FirstOrDefault(s => s.SeedId == _seedToEdit.SeedId);

            if (existingSeed != null)
            {
                db.Entry(existingSeed).CurrentValues.SetValues(_seedToEdit);
            }
            else
            {
                MessageBox.Show("Не удалось найти семена для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            db.SaveChanges();

            MessageBox.Show("Семена успешно обновлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowSeeds wSeeds = new WindowSeeds();
            wSeeds.Show();
            this.Close();
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
