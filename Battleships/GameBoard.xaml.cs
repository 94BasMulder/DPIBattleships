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
    /// Interaction logic for GameBoard.xaml
    /// Size will be added later due to performance issues generating the board.
    /// </summary>
    public partial class GameBoard : Window
    {
        private Game game { get; set; }

        public GameBoard(Game game)
        {
            battleshipContext context = new battleshipContext();
            InitializeComponent();
            this.Title = game.Id.ToString();
            this.game = context.Games.Where(t => t.Id == game.Id).First();
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
    }
}
