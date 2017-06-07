using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Messaging;
using System.Windows.Threading;

namespace Battleships
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// Size will be added later due to performance issues generating the board.
    /// </summary>
    public partial class GameBoard : Window
    {
        private readonly BinaryMessageFormatter formatter = new BinaryMessageFormatter();
        private Game Game { get; set; }
        private int currentUser;
        private int otherUser;

        public GameBoard(Game game, User user)
        {
            battleshipContext context = new battleshipContext();
            this.Game = context.Games.Find(game.Id);
            if (Game.Board.HitsUser1 == 7 || Game.Board.HitsUser1 == 7) { MessageBox.Show("Dit spel is al geeindigd"); this.Close(); }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            InitializeComponent();
            currentUser = user.Id;
            context.Configuration.LazyLoadingEnabled = true;
            this.Title = game.Id.ToString();
            this.Game = context.Games.Find(game.Id);

            if (currentUser == Game.User1.Id)
                otherUser = Game.User2.Id;
            else
                otherUser = Game.User1.Id;
            disableFields();
            //generateField();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            battleshipContext context = new battleshipContext();
            MessageQueue msq = new MessageQueue(@".\private$\" + Game.Id + "/" + currentUser);
            try
            {
                Message[] messages = msq.GetAllMessages();
                foreach (Message m in messages)
                {
                    object o = formatter.Read(m);
                    try
                    {
                        Move move = (Move)o;
                        Tile t = context.Tiles.SqlQuery("select * from Tiles where Board_Id = " + Game.Board.Id + " and Owner_Id = " + currentUser + " and x = " + move.x + " and y = " + move.y).First();
                        t.IsHit = true;
                        if (t.Piece != null)
                        {
                            if (currentUser == Game.User1.Id)
                            {
                                context.Boards.Find(Game.Board.Id).HitsUser1 += 1;
                                if (context.Boards.Find(Game.Board.Id).HitsUser1 == 7) { sendMSMQ(new Response(true, false)); MessageBox.Show("U heeft verloren!");this.Close();  }
                                else sendMSMQ(new Response(true));
                            }
                            else
                            {
                                context.Boards.Find(Game.Board.Id).HitsUser2 += 1;
                                if (context.Boards.Find(Game.Board.Id).HitsUser2 == 7) { sendMSMQ(new Response(true, false)); MessageBox.Show("U heeft verloren!"); this.Close(); }
                                else sendMSMQ(new Response(true));
                            }
                            
                        }
                        else sendMSMQ(new Response(false));
                        context.Games.Find(Game.Id).Turn = context.Users.Find(currentUser);
                        context.SaveChanges();
                    }
                    catch (Exception){}
                    try
                    {
                        Response r = (Response)o;
                        MessageBox.Show(r.displayMessage());
                        if (!r.Continue)
                        {
                            MessageBox.Show("u heeft gewonnen!");
                            this.Close();
                        }
                    }
                    catch (Exception){}
                }
                msq.Purge();
            }
            catch
            { }
        }

        private void disableFields()
        {

            battleshipContext context = new battleshipContext();
            Tile t = null;

            for (int i = 1; i < 6; i++)
                for (int j = 1; j < 6; j++)
                {
                    t = context.Tiles.SqlQuery("select * from Tiles where Board_Id = " + Game.Board.Id + " and Owner_Id = " + currentUser + " and x = " + j + " and y = " + i).FirstOrDefault();
                    if (t != null)
                        if (t.Piece != null)
                        {
                            foreach (Control c in gYou.Children)
                            {
                                if (c.Name == "btnY" + j + "_" + i)
                                {
                                    ((Button)c).Content = "x";
                                    c.IsEnabled = false;
                                    break;
                                }
                            }

                        }
                }
            for (int i = 1; i < 6; i++)
                for (int j = 1; j < 6; j++)
                {
                    t = context.Tiles.SqlQuery("select * from Tiles where Board_Id = " + Game.Board.Id + " and Owner_Id = " + otherUser + " and x = " + j + " and y = " + i).FirstOrDefault();
                    if (t != null)
                        if (t.IsHit)
                        {
                            foreach (Control c in gEnemy.Children)
                            {
                                if (c.Name == "btnE" + j + "_" + i)
                                {
                                    ((Button)c).Content = "x";
                                    c.IsEnabled = false;
                                    break;
                                }
                            }

                        }
                }
        }

        private void generateField()
        {
            ////TODO: SIZE implementations

            int x = 0, y = 0;
            for (int i = 0; i <= 5; i++)
            {
                y += 30;
                for (int j = 0; j <= 5; i++)
                {
                    x += 30;
                    Button b = new Button();
                    string buttonName = "btn" + i + "_" + j;
                    b.Name = buttonName;
                    b.Content = buttonName;
                    b.Margin = new Thickness(x, y, 0, 0);
                    gameGrid.Children.Add(b);

                }
            }
        }

        private void btnClickEnemy(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();
            if (Game.Board.setTilesUser1 != 0 && Game.Board.setTilesUser2 != 0)
            {
                MessageBox.Show("Niet alle velden zijn geset.");
                return;
            }

            if (context.Games.Find(Game.Id).Turn.Id != currentUser)
            {
                MessageBox.Show("U bent niet aan de beurt.");
                return;
            }
            context.Games.Find(Game.Id).Turn = null;
            context.SaveChanges();
            Button button = (Button)sender;
            sendMSMQ(new Move(Convert.ToInt32(button.Name[6]) - 48, Convert.ToInt32(button.Name[4]) - 48));
            button.IsEnabled = false;
        }

        private void btnClickYou(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();

            if (currentUser == Game.User1.Id)
            {
                if (context.Boards.SqlQuery("select * from Boards where id = " + Game.Board.Id).First().setTilesUser1 == 0) { return; }
            }
            else
                if (context.Boards.SqlQuery("select * from Boards where id = " + Game.Board.Id).First().setTilesUser2 == 0) { return; }

            Piece p = new Piece();
            p.pieceType = "Boat";
            context.Pieces.Add(p);
            Button button = (Button)sender;
            button.Content = "x";

            context.Tiles.SqlQuery("select * from Tiles where Board_Id = " + Game.Board.Id + " and Owner_Id = " + currentUser + " and x = " + button.Name[6] + " and y = " + button.Name[4]).First().Piece = p;
            if (currentUser == Game.User1.Id)
                context.Boards.SqlQuery("select * from Boards where id = " + Game.Board.Id).First().setTilesUser1 -= 1;
            else
                context.Boards.SqlQuery("select * from Boards where id = " + Game.Board.Id).First().setTilesUser2 -= 1;
            context.SaveChanges();
            button.IsEnabled = false;
            button.Content = "x";
        }


        public void sendMSMQ(object attachment)
        {
            MessageQueue msq = null;
            if (MessageQueue.Exists(@".\private$\" + Game.Id + "/" + otherUser))
            {
                msq = new MessageQueue(@".\private$\" + Game.Id + "/" + otherUser);
                msq.Label = "testing";
            }
            else
            {
                MessageQueue.Create(@".\private$\" + Game.Id + "/" + otherUser);
                msq = new MessageQueue(@".\private$\" + Game.Id + "/" + otherUser);
                msq.Label = "new q";
            }
            msq.Send(new Message(attachment, formatter));
        }
    }
}
