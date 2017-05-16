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

namespace Battleships
{
    /// <summary>
    /// Interaction logic for RoomSelector.xaml
    /// </summary>
    public partial class RoomSelector : Window
    {
        public User user { get; set; }

        public RoomSelector()
        {
            InitializeComponent();

            loadListBox();
        }

        private void loadListBox()
        {
            battleshipContext context = new battleshipContext();
            if(context.Games.Where(t => t.usersInGame.Contains(user)) != null)
                lvGames.ItemsSource = context.Games.Where(t => t.usersInGame.Contains(user));

        }
       
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnInvite_Click(object sender, RoutedEventArgs e)
        {
             
        }
    }
}
