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
    /// Логика взаимодействия для WindowAddSowing.xaml
    /// </summary>
    public partial class WindowAddSowing : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        public WindowAddSowing()
        {
            InitializeComponent();
            LoadSeeds();
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

        private bool ValidateFields()
        {

            if (seedComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, выберите семена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(numberOfPlantedSeedsTextBox.Text, out int seedsCount) || seedsCount <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество семян.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(seasonSowingOrderTextBox.Text, out int seasonOrder) || seasonOrder <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректный номер посева в сезоне.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (sowingDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату посева.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(expectedYieldTextBox.Text, out double expectedYield) || expectedYield <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное значение ожидаемого урожая (кг).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                var newSowing = new Sowing
                {
                    HectareId = App.currentHectare.HectareId,
                    SeedId = (int)seedComboBox.SelectedValue,
                    SowingDate = DateOnly.FromDateTime(sowingDatePicker.SelectedDate.Value),
                    NumberOfPlantedSeeds = int.Parse(numberOfPlantedSeedsTextBox.Text),
                    NumberInSeason = int.Parse(seasonSowingOrderTextBox.Text),
                    ExpectedYield = decimal.Parse(expectedYieldTextBox.Text), // Сохраняем ожидаемый урожай
                    ActualYield = 0,
                    IsHarvestClosed = 0,
                    IsArhive = 0
                };

                db.Sowings.Add(newSowing);
                db.SaveChanges();

                MessageBox.Show("Посев успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                WindowHectareInfo wHectareInfo = new WindowHectareInfo(App.currentHectare);
                wHectareInfo.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении посева: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
