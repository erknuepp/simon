﻿namespace SimonGame
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
        private Button[] _buttons = new Button[4];
        private GameDriver _game;
        public MainPage()
        {
            InitializeComponent();

            _buttons[0] = greenButton;
            _buttons[1] = blueButton;
            _buttons[2] = redButton;
            _buttons[3] = yellowButton;

            _game = new GameDriver(_buttons);
            _game.DisableButtons();
        }

        public async void GreenButtonClicked(object sender, EventArgs e)
        {
            GameButtonClicked(sender as Button);
        }

        public async void RedButtonClicked(object sender, EventArgs e)
        {
            GameButtonClicked(sender as Button);
        }

        public async void YellowButtonClicked(object sender, EventArgs e)
        {
            GameButtonClicked(sender as Button);
        }

        public async void BlueButtonClicked(object sender, EventArgs e)
        {
            GameButtonClicked(sender as Button);
        }

        public async void PlayButtonClicked(object sender, EventArgs e)
        {
            playButton.Text = playButton.Text == "Play" ? "Pause Game" : "Play";
            _ = _game.RunSequence();
            _game.EnableButtons();
        }

        private async void GameButtonClicked(Button b)
        {
            _game.CapturePlayerSequenceTerm(b);
            _ = _game.ButtonGlowAnimation(b);
        }

        
    }
}
