using System;
using System.Windows.Forms;

namespace CsGoToDiscordActivity
{
    public partial class GameStateUi : Form
    {
        public GameStateUi()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (GameProcessWatcher.gameRunning)
            {
                if (GameStateHttpServer.LastMessage != null)
                {
                    textBox1.Text = GameStateHttpServer.LastMessage.Replace("\n", "\r\n");
                }
            }
            else
            {
                textBox1.Text = "Game is not running";
            }
            
            
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }
    }
}
