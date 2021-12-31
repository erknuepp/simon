namespace SimonGame
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xamarin.Forms;

    public partial class MainPage : ContentPage
    {
        private List<Button> _buttons = new List<Button>();
        private GameDriver _game;
        public MainPage()
        {
            InitializeComponent();
            _buttons.Add(greenButton);
            _buttons.Add(blueButton);
            _buttons.Add(redButton);
            _buttons.Add(yellowButton);

            _game = new GameDriver();            
        }

        public async void GreenButtonClicked(object sender, EventArgs e)
        {
            _game.AddButton(sender as Button);
            _ = StartAnimation();
        }

        public void RedButtonClicked(object sender, EventArgs e)
        {
            _game.AddButton(sender as Button);
        }

        public void YellowButtonClicked(object sender, EventArgs e)
        {
            _game.AddButton(sender as Button);
        }

        public void BlueButtonClicked(object sender, EventArgs e)
        {
            _game.AddButton(sender as Button);
        }

        public void PlayButtonClicked(object sender, EventArgs e)
        {
            playButton.Text = playButton.Text == "Play" ? "Pause Game" : "Play";
        }

        private async Task StartAnimation()
        {
            await Task.Delay(200);
            await greenButton.FadeTo(0, 250);
            await Task.Delay(200);
            await greenButton.FadeTo(1, 250);
        }
    }
}
