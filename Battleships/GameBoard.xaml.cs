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

namespace Battleships
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// Size will be added later due to performance issues generating the board.
    /// </summary>
    public partial class GameBoard : Window
    {
        private Game Game { get; set; }
        private Board Board { get; set; }
        private User user { get; set; }

        public GameBoard(Game game,User user) 
        {
            battleshipContext context = new battleshipContext();
            InitializeComponent();
            this.Title = game.Id.ToString();
            this.user = user;
            this.Game = context.Games.Where(t => t.Id == game.Id).First();
            this.Board = context.Games.Where(t => t.Id == game.Id).First().Board;
            //generateField();
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

        }

        private void btnClickYou(object sender, RoutedEventArgs e)
        {
            battleshipContext context = new battleshipContext();
            Piece p = new Piece();
            context.Pieces.Add(p);
            Button button = (Button)sender;
            //List<Tile> tiles = context.Tiles.Where(t => t.IsHit == false && t.x == button.Name[6] && t.y == button.Name[4] && t.Owner.Id == this.user.Id).ToList();
        }
        
    }
}
