using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace practice_hothouse.View
{
    public partial class WindowSeeds : Window
    {
        DbHothouseContext db = new DbHothouseContext();

        public WindowSeeds()
        {
            InitializeComponent();

            var listViewData = from seed in db.Seeds
                               select seed;
            lvSeed.ItemsSource = listViewData.ToList();
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
            var selectedSeed = lvSeed.SelectedItem as Seed;

            if (selectedSeed == null)
            {
                MessageBox.Show("Выберите тип семени для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                App.currentSeed = selectedSeed;
                WindowEditSeed editSeedWindow = new WindowEditSeed(App.currentSeed);
                editSeedWindow.Show();
                Close();
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedSeed = lvSeed.SelectedItem as Seed;

            if (selectedSeed == null)
            {
                MessageBox.Show("Выберите семя для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить семя {selectedSeed.SeedType} ({selectedSeed.SeedVariety})?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.Seeds.Remove(selectedSeed);
                        db.SaveChanges();

                        lvSeed.ItemsSource = db.Seeds.ToList();

                        MessageBox.Show("Семя успешно удалено!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении семени: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
