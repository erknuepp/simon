namespace SimonGame
{
    using System;
    using System.Threading.Tasks;

    using Xamarin.Forms;

    internal sealed class GameDriver
    {
        private const int maxRoundLength = 20;
        private const int turnChangeDelayMilliseconds = 500;

        private int sequenceLength = 3;
        private int numberOfButtonPresses = 0;

        private Button[] _playerButtons = new Button[maxRoundLength];
        private Button[] _gameButtons = new Button[4];


        public GameDriver(Button[] buttons)
        {
            _gameButtons = buttons;
        }

        internal void AddButton(Button button)
        {
            _playerButtons[numberOfButtonPresses++] = button;
            Console.WriteLine(_playerButtons.ToString());
        }

        internal async Task RunSequence()
        {
            //TODO Disable clicking whilst sequence runs
            for(int i = 0; i < sequenceLength; i++)
            {
                var r = new System.Random();
                await ButtonGlowAnimation(_gameButtons[r.Next(0, 3)]);
                //TODO Save Sequence
                await Task.Delay(turnChangeDelayMilliseconds); //This will give the play 
            }
        }

        public async Task ButtonGlowAnimation(Button button)
        {
            //TODO Have the delay shrink on each round
            //TODO Add sound on button press
            await Task.Delay(100);
            await button.FadeTo(opacity: 0, length: 250);
            await Task.Delay(200);
            await button.FadeTo(opacity: 1, length: 250);
        }
    }
}
