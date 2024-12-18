using Microsoft.EntityFrameworkCore;
using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace practice_hothouse.View
{
    public partial class WindowEditHistory : Window
    {
        private DbHothouseContext db = new DbHothouseContext();
        private HarvestHistory currentHistory;

        public WindowEditHistory(HarvestHistory history)
        {
            InitializeComponent();
            currentHistory = history;
            LoadEmployees();
            LoadHistoryData();
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
        private void LoadHistoryData()
        {
            if (currentHistory != null)
            {
                // Выбор значения сотрудника
                employeeComboBox.SelectedValue = currentHistory.UserId;

                // Проверяем, если HarvestDate не пустая, то преобразуем в DateTime
                if (currentHistory.HarvestDate.HasValue)
                {
                    // Преобразуем DateOnly в DateTime
                    harvestDatePicker.SelectedDate = currentHistory.HarvestDate.Value.ToDateTime(TimeOnly.MinValue);
                }
                else
                {
                    // Если дата не установлена, делаем поле пустым
                    harvestDatePicker.SelectedDate = null;
                }

                // Заполняем количество собранного урожая
                harvestAmountTextBox.Text = currentHistory.HarvestedAmount.ToString();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                // Получаем ID выбранного сотрудника
                int userId = (int)(employeeComboBox.SelectedValue ?? 0);

                if (userId == 0)
                {
                    MessageBox.Show("Выберите сотрудника из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Обновляем информацию о сотруднике
                currentHistory.UserId = userId;

                // Проверяем, что дата была выбрана
                if (harvestDatePicker.SelectedDate.HasValue)
                {
                    currentHistory.HarvestDate = DateOnly.FromDateTime(harvestDatePicker.SelectedDate.Value);
                }
                else
                {
                    MessageBox.Show("Выберите дату сбора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                currentHistory.HarvestedAmount = decimal.Parse(harvestAmountTextBox.Text.Trim());

                db.Entry(currentHistory).State = EntityState.Modified;
                db.SaveChanges();

                MessageBox.Show("Запись успешно сохранена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                WindowHistory wHistory = new WindowHistory(App.currentHistory);
                wHistory.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
