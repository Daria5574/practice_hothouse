using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace practice_hothouse.View
{
    public partial class WindowAddHistory : Window
    {
        private DbHothouseContext db = new DbHothouseContext();
        private int currentUserId;

        public WindowAddHistory()
        {
            InitializeComponent();
            LoadEmployees(); 
            harvestDatePicker.SelectedDate = DateTime.Today;
        }

        private void LoadEmployees()
        {
            var employees = db.Users
                              .Where(u => u.IsArhive == 0 && u.JobTitle != "Бригадир")  
                              .Select(u => new
                              {
                                  u.UserId,
                                  FullName = $"{u.UserLname} {u.UserFname} {u.UserSname}" 
                              })
                              .ToList();

            employeeComboBox.ItemsSource = employees;
            employeeComboBox.DisplayMemberPath = "FullName"; 
            employeeComboBox.SelectedValuePath = "UserId";   
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            var currentSowing = db.Sowings.FirstOrDefault(s => s.SowingId == App.currentSowing.SowingId);
            var selectedEmployee = (dynamic)employeeComboBox.SelectedItem;
            int userId = selectedEmployee?.UserId ?? 0;

            if (userId == 0)
            {
                MessageBox.Show("Выберите сотрудника из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (!harvestDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Выберите дату сбора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newHistory = new HarvestHistory
                {
                    UserId = userId,
                    SowingId = App.currentSowing.SowingId,
                    HarvestDate = DateOnly.FromDateTime(harvestDatePicker.SelectedDate.Value),
                    HarvestedAmount = decimal.Parse(harvestAmountTextBox.Text.Trim()),
                    IsArhive = 0
                };

                db.HarvestHistories.Add(newHistory);
                db.SaveChanges();

                MessageBox.Show("Запись о сборе успешно добавлена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                WindowHistory wHistory = new WindowHistory(App.currentHistory);
                wHistory.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(harvestAmountTextBox.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowHistory wHistory = new WindowHistory(App.currentHistory);
            wHistory.Show();
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
