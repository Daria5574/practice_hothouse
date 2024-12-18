﻿using practice_hothouse.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace practice_hothouse.View
{
    public partial class WindowAllProfiles : Window
    {
        DbHothouseContext db = new DbHothouseContext();

        public WindowAllProfiles()
        {
            InitializeComponent();
            LoadCurrentUsers(); 
            ToggleButtonsVisibility(true); 
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
            WindowAddProfile wAddProfile = new WindowAddProfile();
            wAddProfile.Show();
            Close();
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = lvCurrentUsers.SelectedItem as User;

            if (selectedUser == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                WindowEditProfile editProfileWindow = new WindowEditProfile(selectedUser);
                editProfileWindow.Show();
                Close();
            }
        }

        private void Button_ShowArchive_Click(object sender, RoutedEventArgs e)
        {
            lvCurrentUsers.Visibility = Visibility.Collapsed;
            lvArchiveUsers.Visibility = Visibility.Visible;

            btnShowArchive.Visibility = Visibility.Collapsed;
            btnShowCurrent.Visibility = Visibility.Visible;

            ToggleButtonsVisibility(false); 

            LoadArchiveUsers();
        }

        private void Button_ShowCurrent_Click(object sender, RoutedEventArgs e)
        {
            lvCurrentUsers.Visibility = Visibility.Visible;
            lvArchiveUsers.Visibility = Visibility.Collapsed;

            btnShowArchive.Visibility = Visibility.Visible;
            btnShowCurrent.Visibility = Visibility.Collapsed;

            ToggleButtonsVisibility(true); 

            LoadCurrentUsers();
        }

        private void LoadCurrentUsers()
        {
            var currentUsers = from user in db.Users
                               where user.IsArhive == 0
                               select user;

            lvCurrentUsers.ItemsSource = currentUsers.ToList();
            lvCurrentUsers.Items.Refresh();
        }

        private void LoadArchiveUsers()
        {
            var archiveUsers = from user in db.Users
                               where user.IsArhive == 1
                               select user;
            lvArchiveUsers.ItemsSource = archiveUsers.ToList();
            lvArchiveUsers.Items.Refresh();
        }

        private void Button_Archive_Click(object sender, RoutedEventArgs e)
        {
            if (lvCurrentUsers.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника для архивации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedUser = lvCurrentUsers.SelectedItem as User;
            if (selectedUser != null)
            {
                selectedUser.IsArhive = 1;

                db.Users.Update(selectedUser);
                db.SaveChanges();

                LoadCurrentUsers();
                LoadArchiveUsers();

                MessageBox.Show("Сотрудник переведен в архив.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_RemoveFromArchive_Click(object sender, RoutedEventArgs e)
        {
            if (lvArchiveUsers.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления из архива.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedUser = lvArchiveUsers.SelectedItem as User;
            if (selectedUser != null)
            {
                db.Users.Update(selectedUser);
                selectedUser.IsArhive = 0;

                db.SaveChanges();

                LoadCurrentUsers();
                LoadArchiveUsers();

                MessageBox.Show("Сотрудник удален из архива.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ToggleButtonsVisibility(bool isCurrentUsers)
        {
            Button_Add.Visibility = isCurrentUsers ? Visibility.Visible : Visibility.Collapsed;
            Button_Edit.Visibility = isCurrentUsers ? Visibility.Visible : Visibility.Collapsed;
            btnArchive.Visibility = isCurrentUsers ? Visibility.Visible : Visibility.Collapsed;
            btnUnarchive.Visibility = !isCurrentUsers ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
