using practice_hothouse.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace practice_hothouse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User currentUser = null;
        public static int UserRole;
        public static Plot currentPlot = null;
        public static Hectare currentHectare = null;
        public static Seed currentSeed = null;
        public static Sowing currentSowing = null;
    }

}
