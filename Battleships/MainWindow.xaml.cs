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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Battleships
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();
            if (context.Users.Any(t => t.UserName == txtUsername.Text && t.Password == txtPassword.Password))
            {
                RoomSelector rs = new RoomSelector(context.Users.Where(t => t.UserName == txtUsername.Text && t.Password == txtPassword.Password).FirstOrDefault());
                rs.Show();
                this.Close();
            }
            else
                lblMessage.Content = "Failed to log in. :(";
            
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();
            User user = new User(txtUsername.Text, txtPassword.Password);
            context.Users.Add(user);
            context.SaveChanges();
            lblMessage.Content = "Nieuwe gebruiker aangemaakt.";
        }
    }
}
