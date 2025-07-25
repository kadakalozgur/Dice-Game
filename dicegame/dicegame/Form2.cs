using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace dicegame
{
    public partial class Form2 : Form
    {

        Image[] diceimages = new Image[6];

        PictureBox pictureBoxButton;
        PictureBox pictureBoxPlayer;
        PictureBox pictureBoxComputer;
        
        Image buttonNormal;
        Image buttonHover;
        Image emptyDice;

        Image mainMenuNormal = Resources.mainmenu;
        Image mainMenuHover  = Resources.mainmenuhover;

        Label labelResult;
        Label labelScore;

        SoundPlayer diceRollSound;

        Random random = new Random();

        int animationTicks = 0;

        public Form2()
        {
            InitializeComponent();

            Stream cursorStream = new MemoryStream(Resources.cursorGauntlet_blue);
            this.Cursor = new Cursor(cursorStream);

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            diceimages[0] = Resources.dice_1;
            diceimages[1] = Resources.dice_2;
            diceimages[2] = Resources.dice_3;
            diceimages[3] = Resources.dice_4;
            diceimages[4] = Resources.dice_5;
            diceimages[5] = Resources.dice_6;

            emptyDice = Resources.dice_question;

            this.DoubleBuffered = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            setupgame();

        }

        private void setupgame()
        {

            //ROLL BUTTON//

            buttonNormal = Resources.buttonLong_greyROLL;
            buttonHover = Resources.buttonLong_blueROLL;

            pictureBoxButton = new PictureBox();
            pictureBoxButton.Image = buttonNormal;
            pictureBoxButton.Size = new Size(250, 100);

            int x = (this.ClientSize.Width - pictureBoxButton.Width) / 2;
            int y = this.ClientSize.Height - 200;
            pictureBoxButton.Location = new Point(x,y);

            pictureBoxButton.SizeMode = PictureBoxSizeMode.StretchImage;
            
            pictureBoxButton.MouseEnter += (s, e) => pictureBoxButton.Image = buttonHover;
            pictureBoxButton.MouseLeave += (s, e) => pictureBoxButton.Image = buttonNormal;
            pictureBoxButton.Click += pictureBoxButton_Click;

            this.Controls.Add(pictureBoxButton);

            //ROLLS BOX//

            int diceSize = 250;
            int yPosition = 300;

            pictureBoxPlayer = new PictureBox();
            pictureBoxPlayer.Size = new Size(diceSize, diceSize);
            pictureBoxPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPlayer.Location = new Point(this.ClientSize.Width / 4 - diceSize / 2, yPosition);
            pictureBoxPlayer.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBoxPlayer);

            pictureBoxComputer = new PictureBox();
            pictureBoxComputer.Size = new Size(diceSize, diceSize);
            pictureBoxComputer.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxComputer.Location = new Point(this.ClientSize.Width * 3 / 4 - diceSize / 2, yPosition);
            pictureBoxComputer.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBoxComputer);

            pictureBoxPlayer.Image = emptyDice;
            pictureBoxComputer.Image = emptyDice;

            //LABEL BOX//

            labelResult = new Label();
            labelResult.Text = "\"Press the button to roll the dice\"";
            labelResult.Font = new Font("Arial", 22, FontStyle.Bold);
            labelResult.AutoSize = true;
            labelResult.ForeColor = Color.Black;
            labelResult.BackColor = Color.DarkGreen;
            int labelX = (this.ClientSize.Width - labelResult.PreferredWidth) / 2;
            labelResult.Location = new Point(labelX, 100);
            this.Controls.Add(labelResult);

            //LABEL SCORE//

            labelScore = new Label();
            labelScore.Text = "";
            labelScore.Font = new Font("Arial", 28, FontStyle.Bold);
            labelScore.AutoSize = true;
            labelScore.ForeColor = Color.White;
            labelScore.BackColor = Color.Transparent;

            int _x = (pictureBoxPlayer.Location.X + pictureBoxComputer.Location.X + pictureBoxPlayer.Width) / 2 - labelScore.PreferredWidth / 2;
            int _y = pictureBoxPlayer.Location.Y + pictureBoxPlayer.Height + 20;

            labelScore.Location = new Point(_x, _y);
            this.Controls.Add(labelScore);

            //YOU LABEL//
            Label labelPlayer = new Label();
            labelPlayer.Text = "You";
            labelPlayer.Font = new Font("Arial", 24, FontStyle.Bold);
            labelPlayer.AutoSize = true;
            labelPlayer.ForeColor = Color.Black;
            labelPlayer.BackColor = Color.DarkGreen;
            labelPlayer.Location = new Point(pictureBoxPlayer.Location.X + (pictureBoxPlayer.Width - labelPlayer.PreferredWidth) / 2, pictureBoxPlayer.Location.Y - labelPlayer.PreferredHeight - 10);
            this.Controls.Add(labelPlayer);

            // COMPUTER LABEL//
            Label labelComputer = new Label();
            labelComputer.Text = "Computer";
            labelComputer.Font = new Font("Arial", 24, FontStyle.Bold);
            labelComputer.AutoSize = true;
            labelComputer.ForeColor = Color.Black;
            labelComputer.BackColor = Color.DarkGreen;
            labelComputer.Location = new Point(pictureBoxComputer.Location.X + (pictureBoxComputer.Width - labelComputer.PreferredWidth) / 2,pictureBoxComputer.Location.Y - labelComputer.PreferredHeight - 10);
            this.Controls.Add(labelComputer);

            //ROLL VOİCE//

            diceRollSound = new SoundPlayer(Resources.dicesound);

            //MENU BUTTON//

            PictureBox pictureBoxBack = new PictureBox();
            pictureBoxBack.Size = new Size(100, 100);
            pictureBoxBack.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxBack.Image = mainMenuNormal;

            pictureBoxBack.MouseEnter += (s, e) => pictureBoxBack.Image = mainMenuHover;
            pictureBoxBack.MouseLeave += (s, e) => pictureBoxBack.Image = mainMenuNormal;

            pictureBoxBack.Location = new Point(this.ClientSize.Width - pictureBoxBack.Width - 20, 20);

            pictureBoxBack.Click += (s, e) =>
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide(); 
            };

            this.Controls.Add(pictureBoxBack);
        }


        private void timerRoll_Tick(object sender, EventArgs e)
        {

            animationTicks++;

            int playerDiceIndex = random.Next(0, 6);
            int computerDiceIndex = random.Next(0, 6);

            pictureBoxPlayer.Image = diceimages[playerDiceIndex];
            pictureBoxComputer.Image = diceimages[computerDiceIndex];

            if (animationTicks >= 15)
            {
                timerRoll.Stop();


                if (playerDiceIndex > computerDiceIndex)
                {
                    labelScore.Text = "You Win!";
               
                    int newX = (pictureBoxPlayer.Location.X + pictureBoxComputer.Location.X + pictureBoxPlayer.Width) / 2 - labelScore.PreferredWidth / 2;
                    labelScore.Location = new Point(newX, labelScore.Location.Y);

                }

                else if (playerDiceIndex < computerDiceIndex)
                {
                    labelScore.Text = "Computer Wins!";

                    int newX = (pictureBoxPlayer.Location.X + pictureBoxComputer.Location.X + pictureBoxPlayer.Width) / 2 - labelScore.PreferredWidth / 2;
                    labelScore.Location = new Point(newX, labelScore.Location.Y);

                }

                else
                {
                    int newX = (pictureBoxPlayer.Location.X + pictureBoxComputer.Location.X + pictureBoxPlayer.Width) / 2 - labelScore.PreferredWidth / 2;
                    labelScore.Location = new Point(newX, labelScore.Location.Y);

                    labelScore.Text = "It's a Tie!";
                }
            }
        }
        private void pictureBoxButton_Click(object sender, EventArgs e)
        {
            if (timerRoll.Enabled)
            {

                return;

            }

            animationTicks = 0;
            labelScore.Text = "Rolling...";

            int newX = (pictureBoxPlayer.Location.X + pictureBoxComputer.Location.X + pictureBoxPlayer.Width) / 2 - labelScore.PreferredWidth / 2;
            labelScore.Location = new Point(newX, labelScore.Location.Y);

            timerRoll.Start();
            diceRollSound.Play();
        }
    }
}
