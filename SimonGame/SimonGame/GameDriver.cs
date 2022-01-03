namespace SimonGame
{
    using System;
    using System.Threading.Tasks;

    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    internal sealed class GameDriver
    {
        private const int initialSequenceLength = 3;
        private const int turnChangeDelayMilliseconds = 500;
        private int _highScrore;

        private Round _round;
        private int _sequenceLength;
        private int _numberOfButtonPresses;

        private Button[] _playerSequenceButtons;
        private Button[] _gameButtons = new Button[4];
        private Button[] _sequenceButtons;

        /// <summary>
        /// A jagged array with two rows to hold.
        /// Row 0: Simon's sequence
        /// Row 1: Player's sequence
        /// </summary>
        private Button[][] _sequenceComparisionJaggedArray =  new Button[2][];


        public GameDriver(Button[] buttons, int highScrore = 0)
        {
            _gameButtons = buttons;
            _sequenceLength = initialSequenceLength;
            _sequenceButtons = new Button[_sequenceLength];
            _playerSequenceButtons = new Button[_sequenceLength];
            _numberOfButtonPresses = 0;
            _round = new Round(1);
            _highScrore = highScrore;
        }

        internal async Task RunSequence()
        {
            //TODO Disable clicking whilst sequence runs
            for (int i = 0; i < _sequenceLength; i++)
            {
                var random = new Random();
                var randomButton = _gameButtons[random.Next(0, 3)];
                await ButtonGlowAnimation(randomButton);
                _sequenceButtons[i] = randomButton;
                await Task.Delay(turnChangeDelayMilliseconds); //This will give variable delay 
            }
            _sequenceComparisionJaggedArray[0] = _sequenceButtons;
        }

        internal async Task CapturePlayerSequenceTerm(Button button)
        {
            //TODO Detect when the player has entered the right sequence
            //TODO If the player has the full correct sequence then either go immediatley to next round/turn
            _playerSequenceButtons[_numberOfButtonPresses++] = button;
            Console.WriteLine(_playerSequenceButtons.ToString());
            _sequenceComparisionJaggedArray[1] = _playerSequenceButtons;
            var isValidSequence = IsValidSequence();
            if (isValidSequence)
            {
                
                //TODO What to do is sequence is valid
                if (_numberOfButtonPresses < _sequenceLength)
                {
                    return; //(wait for next sequence entry)
                }
                else
                {
                    //  go to next round
                    
                    _round.Number++;
                    _sequenceLength++;
                    await RunSequence();
                }

            }
            else
            {
                //TODO What to do if sequence is invalid - maybe flash the screen red
                _round.Number = 1;
                _sequenceLength = initialSequenceLength;
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

        

        public async Task ButtonGlowAnimation(Button button) //TODO Possibly have this use generics?
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
