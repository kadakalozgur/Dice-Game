using System.IO;
using System.Media;
using System.Resources;

namespace dicegame
{
    public partial class Form1 : Form
    {

        SoundPlayer menuSound;
        public Form1()
        {
            InitializeComponent();

            menuSound = new SoundPlayer(Resources.menuvoice);
            menuSound.PlayLooping();

            Stream cursorStream = new MemoryStream(Resources.cursorGauntlet_blue);
            this.Cursor = new Cursor(cursorStream);

        }

        private PictureBox setmainmenubutton(Image defaultImg, Image hoverImg)
        {

            PictureBox button = new PictureBox();

            button.SizeMode = PictureBoxSizeMode.AutoSize;
            button.Image = defaultImg;

            button.MouseEnter += (s, e) =>
            {

                button.Image = hoverImg;

            };

            button.MouseLeave += (s, e) =>
            {

                button.Image = defaultImg;

            };

            return button;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.DoubleBuffered = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            var button1 = setmainmenubutton(Resources.buttonLong_beigeplay, Resources.buttonLong_brownplay);
            var button2 = setmainmenubutton(Resources.buttonLong_beigequit, Resources.buttonLong_brownquit);

            button1.Click += (s, e) =>
            {

                menuSound.Stop();

                var gameForm = new Form2();

                gameForm.FormClosed += (s2, e2) =>
                {
                    this.Close();  
                };

                gameForm.Show();
                this.Hide(); 
            };

            button2.Click += (s, e) =>
            {

                menuSound.Stop();

                Application.Exit();

            };

            this.Controls.Add(button1);
            this.Controls.Add(button2);

            int spacing = 20;
            int verticalOffset = 250;

            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button1.Top = (this.ClientSize.Height - button1.Height * 2 - spacing) / 2 + verticalOffset;

            button2.Left = (this.ClientSize.Width - button2.Width) / 2;
            button2.Top = button1.Bottom + spacing;

            Label creditLabel = new Label();
            creditLabel.Text = "Made by Özgür Kadakal";
            creditLabel.Font = new Font("Arial", 18, FontStyle.Bold);
            creditLabel.ForeColor = Color.White;
            creditLabel.BackColor = Color.Transparent;
            creditLabel.AutoSize = true;

            creditLabel.Left = (this.ClientSize.Width - creditLabel.PreferredWidth) / 2;
            creditLabel.Top = button2.Bottom + 40; 

            this.Controls.Add(creditLabel);


        }
    }
}
