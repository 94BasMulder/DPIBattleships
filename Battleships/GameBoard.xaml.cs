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
        private Board Board { get; set; }
        private User user { get; set; }
        private User user2 { get; set; }
        private int currentUser;

        public GameBoard(Game game,User user) 
        {
            battleshipContext context = new battleshipContext();
            InitializeComponent();
            currentUser = user.Id;
            this.Title = game.Id.ToString();
            this.user = context.Users.Find(context.Games.Find(game.Id).User1.Id); ;
            this.user2 = context.Users.Find(context.Games.Find(game.Id).User2.Id);
            this.Game = context.Games.Find(game.Id);
            this.Board = context.Boards.Find(game.Id);
            disableFields();
            //generateField();
        }

        private void disableFields()
        {

            battleshipContext context = new battleshipContext();
            Tile t = null;

            if (this.Board.setTilesUser1 == 0 && user.Id == currentUser || this.Board.setTilesUser2 == 0 && user2.Id == currentUser)
            {
                gbYou.IsEnabled = false;
            }
            else
            {
                for (int i = 1; i < 6; i++)
                    for (int j = 1; j < 6; i++)
                    {
                        t = context.Tiles.SqlQuery("select * from Tiles where Board_Id = " + Board.Id + " and Owner_Id = " + currentUser + " and x = " + j + " and y = " + i).First();
                        if(t.Piece != null)
                        {
                            Control c = new Control();
                            c.FindName("btnY"+i+"_"+j);
                            ((Button)c).Content = "x";
                            c.IsEnabled = false;
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
                    b.Margin = new Thickness(x, y,0,0);
                    gameGrid.Children.Add(b);

                }
            }
        }

        private void btnClickEnemy(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();
            if (context.Boards.SqlQuery("select * from Boards where id = " + Board.Id).First().setTilesUser2 != 0 && context.Boards.SqlQuery("select * from Boards where id = " + Board.Id).First().setTilesUser1 != 0)
            {
                MessageBox.Show("Niet alle velden zijn geset.");
                return;
            }

            MessageQueue msq = null;
            if (MessageQueue.Exists(@".\private$\" + Game.Id))
            {
                msq = new MessageQueue(@".\private$\" + Game.Id);
                msq.Label = "testing";
            }
            else
            {
                MessageQueue.Create(@".\private$\" + Game.Id);
                msq = new MessageQueue(@".\private$\" + Game.Id);
                msq.Label = "new q";
            }

            msq.Send(new Message(new Move(), formatter));

        }

        private void btnClickYou(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();

            if (currentUser == user.Id)
                if (context.Boards.SqlQuery("select * from Boards where id = " + Board.Id).First().setTilesUser1 == 0) {return; }
            else
                if (context.Boards.SqlQuery("select * from Boards where id = " + Board.Id).First().setTilesUser2 == 0) { return; }


            Piece p = new Piece();
            p.pieceType = "Boat";
            context.Pieces.Add(p);
            Button button = (Button)sender;
            base.Content = "x";

            context.Tiles.SqlQuery("select * from Tiles where Board_Id = "+Board.Id+" and Owner_Id = "+user.Id+" and x = "+ button.Name[6]+" and y = " +button.Name[4]).First().Piece = p;
            if(currentUser == user.Id)
                context.Boards.SqlQuery("select * from Boards where id = " + Board.Id).First().setTilesUser1 -= 1;
            else
                context.Boards.SqlQuery("select * from Boards where id = " + Board.Id).First().setTilesUser2 -= 1;
            context.SaveChanges();
            button.IsEnabled = false;
        }
        
    }
}
