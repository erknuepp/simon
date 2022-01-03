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

        internal int RoundNumber { get; set; }
        private int _sequenceLength;
        private int _numberOfTermsEntered;

        private Button[] _playerSequenceButtons;
        private Button[] _gameButtons = new Button[4];
        private Button[] _simonSequenceButtons;
        private Button _playButton;
        private Label _roundNumberLabel;

        /// <summary>
        /// A jagged array with two rows to hold.
        /// Row 0: Simon's sequence
        /// Row 1: Player's sequence
        /// </summary>
        private Button[][] _sequenceComparisionJaggedArray =  new Button[2][];


        public GameDriver(Button[] buttons, Button playButton, Label roundNumberLabel)
        {
            _gameButtons = buttons;
            _sequenceLength = initialSequenceLength;
            _simonSequenceButtons = new Button[_sequenceLength];
            _playerSequenceButtons = new Button[_sequenceLength];
            _numberOfTermsEntered = 0;
            RoundNumber = initialRound;
            _playButton = playButton;
            _roundNumberLabel = roundNumberLabel;
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
            _playerSequenceButtons[_numberOfTermsEntered++] = button;
            _sequenceComparisionJaggedArray[1] = _playerSequenceButtons;
            if (IsValidSequence())
            {
                
                //What to do if sequence is valid
                if (_numberOfTermsEntered < _sequenceLength)
                {
                    return; //(wait for next term entry)
                }
                else
                {
                    //  go to next round
                    RoundNumber++;
                    UpdateRoundLabelText();
                    _sequenceLength++;
                    DisableButtons();
                    await _playButton.FadeTo(1);
                    _playerSequenceButtons = new Button[_sequenceLength];
                    _simonSequenceButtons = new Button[_sequenceLength];
                    _sequenceComparisionJaggedArray = new Button[2][];
                    _numberOfTermsEntered = 0;
                    return;
                }
            }
            else
            {
                //TODO What to do if sequence is invalid
                RoundNumber = initialRound;
                DisableButtons();
                await _playButton.FadeTo(1);
                UpdateRoundLabelText();
                _sequenceLength = initialSequenceLength;
                return;
            }
        }

        private void UpdateRoundLabelText()
        {
            _roundNumberLabel.Text = $"Round {RoundNumber}";
        }

        internal async Task GloomAnimation(VisualElement visualElement)//TODO Possibly have this use generics?
        {
            await Task.Delay(100);
            await visualElement.FadeTo(opacity: 0);
            await Task.Delay(200);
            await visualElement.FadeTo(opacity: 1);
        }

        internal void EnableButtons() { _gameButtons.ForEach(x => x.IsEnabled = true); }
        internal void DisableButtons() { _gameButtons.ForEach(x => x.IsEnabled = false); }

        private bool IsValidSequence()
        {
            for (int i = 0; i < _numberOfTermsEntered; i++)
            {
                if (_sequenceComparisionJaggedArray[0][i].Text != _sequenceComparisionJaggedArray[1][i].Text)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
