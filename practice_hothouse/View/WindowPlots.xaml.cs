using practice_hothouse.Models;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace practice_hothouse.View
{
    /// <summary>
    /// Логика взаимодействия для WindowPlots.xaml
    /// </summary>
    public partial class WindowPlots : Window
    {
        DbHothouseContext db = new DbHothouseContext();
        Plot p;

        public WindowPlots(User currentUser)
        {
            InitializeComponent();
        }

        private void MouseLeftButtonDown_plot1(object sender, MouseButtonEventArgs e)
        {
            HandlePlotAccess(1);
        }

        private void MouseLeftButtonDown_plot2(object sender, MouseButtonEventArgs e)
        {
            HandlePlotAccess(2);
        }

        private void MouseLeftButtonDown_plot3(object sender, MouseButtonEventArgs e)
        {
            HandlePlotAccess(3);
        }

        private void MouseLeftButtonDown_plot4(object sender, MouseButtonEventArgs e)
        {
            HandlePlotAccess(4);
        }

        private void HandlePlotAccess(int plotId)
        {
            p = db.Plots.FirstOrDefault(c => c.PlotId == plotId);
            if (p == null)
            {
                MessageBox.Show("Участок не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (App.UserRole == 3 || App.currentUser.PlotId == plotId)
            {
                App.currentPlot = p;
                WindowHectare wHectare = new WindowHectare(p);
                wHectare.Show();
                Close();
            }
            else
            {
                MessageBox.Show(
                    "У вас нет доступа к этому участку.",
                    "Доступ запрещен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
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

    }
}
