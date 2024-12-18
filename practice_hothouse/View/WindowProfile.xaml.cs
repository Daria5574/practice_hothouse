using practice_hothouse.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace practice_hothouse.View
{
    /// <summary>
    /// Логика взаимодействия для WindowProfile.xaml
    /// </summary>
    public partial class WindowProfile : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        User u;

        public WindowProfile(User currentUser)
        {
            InitializeComponent();
            u = currentUser;

            if (u == null)
            {
                lNameLabel.Content = "Имя: Дарья";
                fNameLabel.Content = "Должность: Администратор";
                sNameLabel.Content = string.Empty;
                jobTitleLablel.Content = string.Empty;
                phoneLablel.Content = string.Empty;
                numberLabel.Content = string.Empty;
            }
            else
            {
                lNameLabel.Content = "Фамилия: " + u.UserLname;
                fNameLabel.Content = "Имя: " + u.UserFname;
                sNameLabel.Content = "Отчество: " + u.UserSname;
                jobTitleLablel.Content = "Должность: " + u.JobTitle;
                phoneLablel.Content = "Номер телефона: " + u.Phone;
                numberLabel.Content = "Номер участка: " + u.PlotId;

                if (App.UserRole != 3)
                {
                    AddButton.Visibility = Visibility.Collapsed; 
                    ControlPersonButton.Visibility = Visibility.Collapsed;
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

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }

        private void ControlButton_Click(object sender, RoutedEventArgs e)
        {
            WindowSeeds wSeeds = new WindowSeeds();
            wSeeds.Show();
            Close();
        }

        private void ControlPersonButton_Click(object sender, RoutedEventArgs e)
        {
            WindowAllProfiles wAllProfiles = new WindowAllProfiles();
            wAllProfiles.Show();
            Close();
        }
    }
}
