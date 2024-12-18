using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace practice_hothouse.View
{
    /// <summary>
    /// Логика взаимодействия для WindowHistory.xaml
    /// </summary>
    public partial class WindowHistory : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        HarvestHistory h = new HarvestHistory();

        public WindowHistory(HarvestHistory h)
        {
            InitializeComponent();

            // Получаем информацию о семени для текущего посева (Sowing)
            var seedData = (from sowing in db.Sowings
                            join seed in db.Seeds on sowing.SeedId equals seed.SeedId
                            where sowing.SowingId == App.currentSowing.SowingId
                            select new
                            {
                                seed.SeedType,
                                seed.SeedVariety
                            }).FirstOrDefault();

            // Если информация о семени найдена, обновляем лейбл
            if (seedData != null)
            {
                titleLabel.Content = $"{seedData.SeedType} \"{seedData.SeedVariety}\" {App.currentSowing.NumberInSeason} посев";
            }

            // Запрос для данных истории сбора урожая
            var listViewData = (from history in db.HarvestHistories
                                join user in db.Users on history.UserId equals user.UserId
                                join sowingItem in db.Sowings on history.SowingId equals sowingItem.SowingId
                                where history.SowingId == App.currentSowing.SowingId
                                select new
                                {
                                    HarvestHistoryId = history.HarvestHistoryId,
                                    HarvestDate = history.HarvestDate,
                                    HarvestAmount = history.HarvestedAmount,
                                    FIO_User = $"{user.UserLname} {user.UserFname} {user.UserSname}",
                                    IsHarvestClosed = sowingItem.IsHarvestClosed
                                }).ToList();

            lvHistory.ItemsSource = listViewData;

            // Если роль пользователя 3, то не нужно проверять PlotId
            if (App.UserRole == 3)
            {
                addButton.Visibility = Visibility.Visible;
                editButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                // Получаем PlotId, с которым связан текущий Sowing через Hectare
                var userPlotId = App.currentUser.PlotId;

                // Получаем PlotId, связанный с текущим Sowing через Hectare
                var currentSowingPlotId = db.Sowings
                                            .Where(s => s.SowingId == App.currentSowing.SowingId)
                                            .Join(db.Hectares, sowing => sowing.HectareId, hectare => hectare.HectareId, (sowing, hectare) => hectare.PlotId)
                                            .FirstOrDefault();

                // Показываем кнопки, если роль пользователя 1 и PlotId совпадает
                if (App.UserRole == 1 && userPlotId == currentSowingPlotId)
                {
                    addButton.Visibility = Visibility.Visible;
                    editButton.Visibility = Visibility.Visible;
                    deleteButton.Visibility = Visibility.Visible;
                }
                else
                {
                    addButton.Visibility = Visibility.Collapsed;
                    editButton.Visibility = Visibility.Collapsed;
                    deleteButton.Visibility = Visibility.Collapsed;
                }
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

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            WindowPlantingInfo wPlantingInfo = new WindowPlantingInfo(App.currentSowing);
            wPlantingInfo.Show();
            Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var sowing = db.Sowings.FirstOrDefault(s => s.SowingId == App.currentSowing.SowingId);

            if (sowing != null && sowing.IsHarvestClosed == 1 && App.UserRole != 3) 
            {
                MessageBox.Show("Сбор закрыт, изменения невозможны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            WindowAddHistory wAddHistory = new WindowAddHistory();
            wAddHistory.Show();
            Close();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

            var sowing = db.Sowings.FirstOrDefault(s => s.SowingId == App.currentSowing.SowingId);

            if (sowing != null && sowing.IsHarvestClosed == 1 && App.UserRole != 3) 
            {
                MessageBox.Show("Сбор закрыт, изменения невозможны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedItem = lvHistory.SelectedItem as dynamic;

            if (selectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                int selectedHistoryId = selectedItem.HarvestHistoryId;
                var selectedHistory = db.HarvestHistories
                                        .FirstOrDefault(h => h.HarvestHistoryId == selectedHistoryId);

                if (selectedHistory == null)
                {
                    MessageBox.Show("Не удалось найти запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    WindowEditHistory editHistoryWindow = new WindowEditHistory(selectedHistory);
                    editHistoryWindow.Show();
                    Close();
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var sowing = db.Sowings.FirstOrDefault(s => s.SowingId == App.currentSowing.SowingId);

            if (sowing != null && sowing.IsHarvestClosed == 1 && App.UserRole != 3) 
            {
                MessageBox.Show("Сбор закрыт, изменения невозможны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedItem = lvHistory.SelectedItem as dynamic;
            if (selectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                int selectedHistoryId = selectedItem.HarvestHistoryId;
                var selectedHistory = db.HarvestHistories.FirstOrDefault(h => h.HarvestHistoryId == selectedHistoryId);

                if (selectedHistory != null)
                {
                    var result = MessageBox.Show("Вы действительно хотите удалить запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        db.HarvestHistories.Remove(selectedHistory);
                        db.SaveChanges();
                        MessageBox.Show("Запись успешно удалена.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                        UpdateListView();
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось найти запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateListView()
        {
            var listViewData = (from history in db.HarvestHistories
                                join user in db.Users on history.UserId equals user.UserId
                                join sowingItem in db.Sowings on history.SowingId equals sowingItem.SowingId
                                where history.SowingId == App.currentSowing.SowingId
                                select new
                                {
                                    HarvestHistoryId = history.HarvestHistoryId,
                                    HarvestDate = history.HarvestDate,
                                    HarvestAmount = history.HarvestedAmount,
                                    FIO_User = $"{user.UserLname} {user.UserFname} {user.UserSname}",
                                    IsHarvestClosed = sowingItem.IsHarvestClosed
                                }).ToList();

            lvHistory.ItemsSource = listViewData;
        }
    }
}
