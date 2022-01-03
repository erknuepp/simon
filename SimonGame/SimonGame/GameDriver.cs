namespace SimonGame
{
    using System;
    using System.Threading.Tasks;

    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    internal sealed class GameDriver
    {
        private const int initialSequenceLength = 3;
        private const int initialRound = 1;
        private const int turnChangeDelayMilliseconds = 500;

        internal int RoundNumber { get; private set; }
        private int _sequenceLength;
        private int _numberOfTermsEntered;

        private Button[] _playerSequenceButtons;
        private Button[] _gameButtons = new Button[4];
        private Button[] _simonSequenceButtons;

        /// <summary>
        /// A jagged array with two rows to hold.
        /// Row 0: Simon's sequence
        /// Row 1: Player's sequence
        /// </summary>
        private Button[][] _sequenceComparisionJaggedArray =  new Button[2][];


        public GameDriver(Button[] buttons)
        {
            _gameButtons = buttons;
            _sequenceLength = initialSequenceLength;
            _simonSequenceButtons = new Button[_sequenceLength];
            _playerSequenceButtons = new Button[_sequenceLength];
            _numberOfTermsEntered = 0;
            RoundNumber = initialRound;
        }

        internal async Task RunSequence()
        {
            
            //TODO Disable clicking whilst sequence runs
            for (int i = 0; i < _sequenceLength; i++)
            {
                var randomButton = _gameButtons[new Random().Next(0, 3)];
                await GloomAnimation(randomButton);
                _simonSequenceButtons[i] = randomButton;
                await Task.Delay(turnChangeDelayMilliseconds); //This will give variable delay 
            }
            _sequenceComparisionJaggedArray[0] = _simonSequenceButtons;
        }

        internal async Task CapturePlayerSequenceTerm(Button button)
        {
            //TODO Detect when the player has entered the right sequence
            //TODO If the player has the full correct sequence then either go immediatley to next round/turn
            _playerSequenceButtons[_numberOfTermsEntered++] = button;
            Console.WriteLine(_playerSequenceButtons.ToString());
            _sequenceComparisionJaggedArray[1] = _playerSequenceButtons;
            var isValidSequence = IsValidSequence();
            if (isValidSequence)
            {
                
                //TODO What to do is sequence is valid
                if (_numberOfTermsEntered < _sequenceLength)
                {
                    return; //(wait for next term entry)
                }
                else
                {
                    //  go to next round
                    RoundNumber++;
                    _sequenceLength++;
                    await RunSequence();
                }
            }
            else
            {
                //TODO What to do if sequence is invalid - maybe flash the screen red
                RoundNumber = initialRound;
                _sequenceLength = initialSequenceLength;
            }
        }

        internal async Task GloomAnimation(VisualElement visualElement)//TODO Possibly have this use generics?
        {
            await Task.Delay(100);
            await visualElement.FadeTo(opacity: 0, length: 250);
            await Task.Delay(200);
            await visualElement.FadeTo(opacity: 1, length: 250);
        }

        internal void EnableButtons() { _gameButtons.ForEach(x => x.IsEnabled = true); }
        internal void DisableButtons() { _gameButtons.ForEach(x => x.IsEnabled = false); }

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
    }
}
