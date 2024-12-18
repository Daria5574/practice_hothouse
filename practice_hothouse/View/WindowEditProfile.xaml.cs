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
    /// Логика взаимодействия для WindowEditProfile.xaml
    /// </summary>
    public partial class WindowEditProfile : Window
    {
        private DbHothouseContext db = new DbHothouseContext();
        private User _userToEdit;

        public WindowEditProfile(User user)
        {
            InitializeComponent();
            _userToEdit = user;

            var availablePlots = db.Plots.ToList();
            plotIdComboBox.ItemsSource = availablePlots;
            plotIdComboBox.SelectedValue = user.PlotId;  

            firstNameTextBox.Text = user.UserFname;
            lastNameTextBox.Text = user.UserLname;
            secondNameTextBox.Text = user.UserSname;
            jobTitleTextBox.Text = user.JobTitle;
            phoneTextBox.Text = user.Phone;
            emailTextBox.Text = user.Email;
            loginTextBox.Text = user.Login;
            passwordTextBox.Text = user.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _userToEdit.PlotId = (int)plotIdComboBox.SelectedValue;
            _userToEdit.UserFname = firstNameTextBox.Text.Trim();
            _userToEdit.UserLname = lastNameTextBox.Text.Trim();
            _userToEdit.UserSname = secondNameTextBox.Text.Trim();
            _userToEdit.JobTitle = jobTitleTextBox.Text.Trim();
            _userToEdit.Phone = phoneTextBox.Text.Trim();
            _userToEdit.Email = emailTextBox.Text.Trim();
            _userToEdit.Login = loginTextBox.Text.Trim();
            _userToEdit.Password = passwordTextBox.Text.Trim();

            var existingUser = db.Users.FirstOrDefault(u => u.UserId == _userToEdit.UserId);

            if (existingUser != null)
            {
                db.Entry(existingUser).CurrentValues.SetValues(_userToEdit);
            }
            else
            {
                MessageBox.Show("Не удалось найти профиль для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            db.SaveChanges();
            MessageBox.Show("Профиль успешно обновлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            WindowAllProfiles wAllProfiles = new WindowAllProfiles();
            wAllProfiles.Show();
            Close();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowAllProfiles wAllProfiles = new WindowAllProfiles();
            wAllProfiles.Show();
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
