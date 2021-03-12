using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework.Media;

namespace GameArchitectureExample.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {
        public PauseMenuScreen() : base("Paused")
        {
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");
            var optionsMenuEntry = new MenuEntry("Options");
            var restartGameMenuEntry = new MenuEntry("Restart Game");

            resumeGameMenuEntry.Selected += OnCancel;
            optionsMenuEntry.Selected += OptionsMenuEntry_Selected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            restartGameMenuEntry.Selected += RestartGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(restartGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        private void OptionsMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
             ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        private void RestartGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Stop();
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}
