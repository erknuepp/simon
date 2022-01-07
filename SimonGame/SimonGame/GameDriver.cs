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
        private Button[][] _sequenceComparisionJaggedArray = new Button[2][];


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
                var randomButton = _gameButtons[new Random().Next(0, 4)]; // 0 is inclusive 4 is exclusive
                await GloomAnimation(randomButton);
                _simonSequenceButtons[i] = randomButton;
                await Task.Delay(500);
            }
            _sequenceComparisionJaggedArray[0] = _simonSequenceButtons;
        }

        internal async Task CapturePlayerSequenceTerm(Button button)
        {
            _playerSequenceButtons[_numberOfTermsEntered++] = button;
            _sequenceComparisionJaggedArray[1] = _playerSequenceButtons;
            if (IsValidSequence())
            {
                // if sequence is valid and more terms to enter
                if (_numberOfTermsEntered < _sequenceLength)
                {
                    return; // wait for next term entry
                }
                else // go to next round
                {

                    RoundNumber++;
                    UpdateRoundLabelText();
                    _sequenceLength++;
                    await Reset();
                    return;
                }
            }
            else
            {
                // else if sequence is invalid reset
                RoundNumber = initialRound;
                UpdateRoundLabelText();
                _sequenceLength = initialSequenceLength;
                await Reset();
                return;
            }
        }

        private async Task Reset()
        {
            await DisableButtons();
            _playerSequenceButtons = new Button[_sequenceLength];
            _simonSequenceButtons = new Button[_sequenceLength];
            _sequenceComparisionJaggedArray = new Button[2][];
            _numberOfTermsEntered = 0;
            await _playButton.FadeTo(1);
        }

        private void UpdateRoundLabelText()
        {
            _roundNumberLabel.Text = $"Round {RoundNumber}";
        }

        internal async Task GloomAnimation(VisualElement visualElement)
        {
            await Task.Delay(100);
            await visualElement.FadeTo(opacity: 0);
            await Task.Delay(200);
            await visualElement.FadeTo(opacity: 1);
        }

        internal async Task EnableButtons()
        {
            _gameButtons.ForEach(x => x.IsEnabled = true);
            await Task.CompletedTask;
        }

        internal async Task DisableButtons()
        {
            _gameButtons.ForEach(x => x.IsEnabled = false);
            await Task.CompletedTask;
        }

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
