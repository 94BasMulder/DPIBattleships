using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Messaging;
using System.Diagnostics;

namespace Battleships
{
    /// <summary>
    /// Interaction logic for RoomSelector.xaml
    /// </summary>
    public partial class RoomSelector : Window
    {
        public User User { get; set; }


        private readonly BinaryMessageFormatter formatter = new BinaryMessageFormatter();

        public RoomSelector(User user)
        {
            InitializeComponent();
            this.User = user;
            this.Title = user.UserName;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            loadListBox();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                MessageQueue msq = new MessageQueue(@".\private$\" + User.UserName);
                Message[] messages = msq.GetAllMessages();
                foreach (Message m in messages)
                {
                    Invite inv = (Invite) formatter.Read(m);
                    MessageBoxResult mboxres = MessageBox.Show(inv.Message,"A CHALLENGER APROACHES!", MessageBoxButton.YesNo);
                    if(mboxres == MessageBoxResult.Yes)
                    {
                        addGame(inv);
                    }
                }
                msq.Purge();
            }
            catch (Exception exc)
            {
                Debug.Write(exc.Message);
                Debug.Write(exc.InnerException);
                Debug.Write(exc.Source);
            }
        }

        private void addGame(Invite inv)
        {
            battleshipContext context = new battleshipContext();
            Board b = new Board(inv.Size);
            context.Boards.Add(b);
            context.SaveChanges();
            User user1 = context.Users.Where(t => t.Id == User.Id).First();
            User user2 = context.Users.Where(t => t.Id == inv.User.Id).First();

            context.Games.Add(new Game(user1,user2, b));
            context.SaveChanges();

        }

        private void loadListBox()
        {
            battleshipContext context = new battleshipContext();
            if(context.Games.Any(t => t.User1.Id== User.Id || t.User2.Id == User.Id))
               lvGames.ItemsSource = context.Games.Where(t => t.User1.Id == User.Id || t.User2.Id == User.Id).ToList();
        }

        private void btnInvite_Click(object sender, RoutedEventArgs e)
        {
            newGameDialog ngd = new newGameDialog(this.User);
            ngd.ShowDialog();
            loadListBox();
        }

        private void lvGames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GameBoard gameboard = new GameBoard((Game)lvGames.SelectedValue);
            gameboard.Show();
        }
    }
}
