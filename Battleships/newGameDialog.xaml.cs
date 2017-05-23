using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
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
    /// Interaction logic for newGameDialog.xaml
    /// </summary>
    public partial class newGameDialog : Window
    {
        private readonly BinaryMessageFormatter formatter = new BinaryMessageFormatter();

        public User user { get; set; }

        public newGameDialog(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            battleshipContext dbContext = new battleshipContext();
            if(dbContext.Users.Any(t => t.UserName == txtUsername.Text))
            {
                MessageQueue msq = null;
                if (MessageQueue.Exists(@".\private$\" + txtUsername.Text))
                {
                    msq = new MessageQueue(@".\private$\" + txtUsername.Text);
                    msq.Label = "testing";
                }
                else
                {
                    MessageQueue.Create(@".\private$\" + txtUsername.Text);
                    msq = new MessageQueue(@".\private$\" + txtUsername.Text);
                    msq.Label = "new q";
                }

                msq.Send(new Message(new Invite(getSize(),user.UserName + " heeft u uitgedaagd voor een game! (Boardsize: "+getSize()+")", user),formatter));
            }
            else {MessageBox.Show("Gebruiker bestaat niet!");}
        }

        private int getSize()
        {
            try
            {
                if (Convert.ToInt32(txtBoardsize.Text) > 5)
                    return Convert.ToInt32(txtBoardsize.Text);
                else return 5;
            }
            catch { return 5; }
        }
    }
}
