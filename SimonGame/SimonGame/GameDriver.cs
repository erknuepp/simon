namespace SimonGame
{
    using System;

    using Xamarin.Forms;

    internal sealed class GameDriver
    {
        const int maxRoundLength = 20;
        Button[] playerButtons = new Button[maxRoundLength];
        int numberOfButtonPresses = 0;

        public GameDriver()
        {

        }

        internal void AddButton(Button button)
        {
            playerButtons[numberOfButtonPresses++] = button;
            Console.WriteLine(playerButtons.ToString());
        }


    }
}
