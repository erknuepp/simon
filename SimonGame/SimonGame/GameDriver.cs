namespace SimonGame
{
    using System;
    using System.Threading.Tasks;

    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    internal sealed class GameDriver
    {
        private const int maxRoundLength = 20;
        private const int turnChangeDelayMilliseconds = 500;

        private int _sequenceLength = 3;
        private int _numberOfButtonPresses = 0;

        private Button[] _playerButtons = new Button[maxRoundLength];
        private Button[] _gameButtons = new Button[4];
        private Button[] _sequenceButtons;

        /// <summary>
        /// A jagged array with two rows to hold.
        /// Row 0: Simon's sequence
        /// Row 1: Player's sequence
        /// </summary>
        private Button[][] _sequenceComparisionJaggedArray =  new Button[2][];


        public GameDriver(Button[] buttons)
        {
            _gameButtons = buttons;
            _sequenceButtons = new Button[_sequenceLength];
        }

        internal void AddButton(Button button)
        {
            _playerButtons[_numberOfButtonPresses++] = button;
            Console.WriteLine(_playerButtons.ToString());
            _sequenceComparisionJaggedArray[1] = _playerButtons;
            var isValidSequence = IsValidSequence();
            if (isValidSequence)
            {
                //TODO What to do is sequence is valid
                //if has another turn i.e. _numberOfButtonsPress < _sequenceLength
                //  return (wait for next sequence entry)
                //else
                //  go to next round
            }
            else
            {
                //TODO What to do if sequence is invlid
                //restart from round 1 turn 1
            }
        }

        private bool IsValidSequence()
        {
            for (int i = 0; i < _sequenceLength; i++)
            {
                if (_sequenceComparisionJaggedArray[0][i] != _sequenceComparisionJaggedArray[1][i])
                {
                    return false;
                }
            }
            return true;
        }

        internal async Task RunSequence()
        {
            //TODO Disable clicking whilst sequence runs
            for(int i = 0; i < _sequenceLength; i++)
            {
                var random = new Random();
                var randomButton = _gameButtons[random.Next(0, 3)];
                await ButtonGlowAnimation(randomButton);
                _sequenceButtons[i] = randomButton;
                await Task.Delay(turnChangeDelayMilliseconds); //This will give the play 
            }
            _sequenceComparisionJaggedArray[0] = _sequenceButtons;
        }

        public async Task ButtonGlowAnimation(Button button)
        {
            //TODO Have the delay shrink on each round
            //TODO Add sound upon button gloom
            await Task.Delay(100);
            await button.FadeTo(opacity: 0, length: 250);
            await Task.Delay(200);
            await button.FadeTo(opacity: 1, length: 250);
        }

        public void EnableButtons() { _gameButtons.ForEach(x => x.IsEnabled = true); }
        public void DisableButtons() { _gameButtons.ForEach(x => x.IsEnabled = false); }
    }
}
