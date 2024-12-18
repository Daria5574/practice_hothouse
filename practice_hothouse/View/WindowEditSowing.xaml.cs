using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace practice_hothouse.View
{
    public partial class WindowEditSowing : Window
    {
        private readonly DbHothouseContext db = new DbHothouseContext();
        private readonly Sowing currentSowing;

        public WindowEditSowing(Sowing sowing)
        {
            InitializeComponent();
            currentSowing = sowing;
            LoadSeeds();
            LoadSowingDetails();
        }

        private void LoadSeeds()
        {
            var seeds = db.Seeds.ToList();
            if (seeds.Count > 0)
            {
                var seedList = seeds.Select(s => new
                {
                    s.SeedId,
                    DisplayText = $"{s.SeedType} - {s.SeedVariety}"
                }).ToList();

                seedComboBox.ItemsSource = seedList;
                seedComboBox.DisplayMemberPath = "DisplayText";
                seedComboBox.SelectedValuePath = "SeedId";
            }
            else
            {
                MessageBox.Show("Список семян пуст. Добавьте семена в систему.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadSowingDetails()
        {
            seedComboBox.SelectedValue = currentSowing.SeedId;
            sowingDatePicker.SelectedDate = currentSowing.SowingDate.HasValue
                ? currentSowing.SowingDate.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null; 
            numberOfPlantedSeedsTextBox.Text = currentSowing.NumberOfPlantedSeeds.ToString();
            seasonSowingOrderTextBox.Text = currentSowing.NumberInSeason.ToString();
            expectedYieldTextBox.Text = currentSowing.ExpectedYield.ToString();
            isHarvestClosedCheckBox.IsChecked = currentSowing.IsHarvestClosed == 1;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                var sowingToUpdate = db.Sowings.FirstOrDefault(s => s.SowingId == currentSowing.SowingId);
                if (sowingToUpdate == null)
                {
                    MessageBox.Show("Посев не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                sowingToUpdate.SeedId = (int)seedComboBox.SelectedValue;
                sowingToUpdate.SowingDate = DateOnly.FromDateTime(sowingDatePicker.SelectedDate.Value);
                sowingToUpdate.NumberOfPlantedSeeds = int.Parse(numberOfPlantedSeedsTextBox.Text);
                sowingToUpdate.NumberInSeason = int.Parse(seasonSowingOrderTextBox.Text);
                sowingToUpdate.ExpectedYield = decimal.Parse(expectedYieldTextBox.Text);
                sowingToUpdate.IsHarvestClosed = isHarvestClosedCheckBox.IsChecked == true ? 1 : 0;

                db.SaveChanges();

                MessageBox.Show("Посев успешно обновлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                WindowHectareInfo wHectareInfo = new WindowHectareInfo(App.currentHectare);
                wHectareInfo.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении посева: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFields()
        {
            if (seedComboBox.SelectedValue == null ||
                !int.TryParse(numberOfPlantedSeedsTextBox.Text, out _) ||
                !int.TryParse(seasonSowingOrderTextBox.Text, out _) ||
                sowingDatePicker.SelectedDate == null ||
                !double.TryParse(expectedYieldTextBox.Text, out _))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowHectareInfo wHectareInfo = new WindowHectareInfo(App.currentHectare);
            wHectareInfo.Show();
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
    }
}
