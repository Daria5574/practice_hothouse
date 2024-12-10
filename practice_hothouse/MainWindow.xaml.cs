using practice_hothouse.Models;
using practice_hothouse.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace practice_hothouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User authUser = null;
        private DbHothouseContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new DbHothouseContext();
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "Это поле введено некорректно.";
                textBoxLogin.Background = Brushes.LightCoral;
            }
            else if (pass.Length < 6 || !pass.Any(char.IsUpper) || !pass.Any(char.IsDigit) || !pass.Any(c => "!@#$%^.".Contains(c)))
            {
                passBox.ToolTip = "Это поле введено некорректно.";
                passBox.Background = Brushes.LightCoral;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                if (login == "adminka" && pass == "Admin5574!")
                {
                    App.UserRole = 2;

                    WindowPlots wPlots = new WindowPlots(App.currentUser);
                    wPlots.Show();
                    Close();
                }
                else if (char.IsLetter(login[0]))
                {
                    authUser = db.Users.Where(c => c.Login == login && c.Password == pass).FirstOrDefault();

                    if (authUser != null)
                    {
                        int userRole = 0; 

                        if (login.StartsWith("b", StringComparison.OrdinalIgnoreCase))
                        {
                            userRole = 1;
                        }
                        else if (login.StartsWith("o", StringComparison.OrdinalIgnoreCase))
                        {
                            userRole = 2; 
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Ошибка: Неопределённая роль для пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; 
                        }

                        App.UserRole = userRole;
                        App.currentUser = authUser;

                        WindowPlots wPlots = new WindowPlots(authUser);
                        wPlots.Show();
                        Close();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Пользователь не найден. Проверьте правильность введённых данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }
    }
}
