using Inventory_Management_System_PC.Models;
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

namespace Inventory_Management_System_PC.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        InventoryDBContext db = new InventoryDBContext();
        public LoginView()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = db.Users.Find(UsernameTextBox.Text);
            if (user != null)
            {
                if(user.Password == PasswordBox.Password)
                {
                    MainWindow MW = new MainWindow();
                    Hide();
                    SessionManager.SetUser(user);
                    MW.ShowDialog();
                    Show();
                }
                else
                {
                    MessageBox.Show("Incorrect Password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No User Found", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UsernameTextBox.Clear();
            PasswordBox.Clear();
        }
    }
}
