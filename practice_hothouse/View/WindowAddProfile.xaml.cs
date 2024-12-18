using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace practice_hothouse.View
{
    public partial class WindowAddProfile : Window
    {
        DbHothouseContext db = new DbHothouseContext();

        public WindowAddProfile()
        {
            InitializeComponent();
            LoadPlots();  
        }

        private void LoadPlots()
        {
            var plots = db.Plots.ToList();
            plotIdComboBox.ItemsSource = plots;
            plotIdComboBox.DisplayMemberPath = "PlotName";  
            plotIdComboBox.SelectedValuePath = "PlotId";    
        }

        private bool ValidateFields()
        {
            if (!IsValidEmail(emailTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите правильный email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;
            int plotID = (int)plotIdComboBox.SelectedValue;
            var newUser = new User
            {
                PlotId = plotID,
                UserFname = firstNameTextBox.Text,
                UserLname = lastNameTextBox.Text,
                UserSname = secondNameTextBox.Text,
                JobTitle = jobTitleTextBox.Text,
                Phone = phoneTextBox.Text,
                Email = emailTextBox.Text,
                Login = loginTextBox.Text,
                Password = passwordTextBox.Text,
                IsArhive = 0 
            };

            try
            {
                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Сотрудник добавлен успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                WindowAllProfiles wAllProfiles = new WindowAllProfiles();
                wAllProfiles.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowAllProfiles wAllProfiles = new WindowAllProfiles();
            wAllProfiles.Show();
            Close();
        }

        private void MouseLeftButtonDown_profile(object sender, MouseButtonEventArgs e)
        {
            WindowAllProfiles wProfile = new WindowAllProfiles();
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
