using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using GameArchitectureExample.StateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameArchitectureExample.Screens;

namespace GameArchitectureExample.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {

        private readonly MenuEntry _musicVolumeUp;
        private readonly MenuEntry _musicVolumeDown;
        private readonly MenuEntry _effectsVolumeUp;
        private readonly MenuEntry _effectsVolumeDown;
        private readonly MenuEntry _currentMusicVolume;
        private readonly MenuEntry _currentEffectsVolume;


       

        public OptionsMenuScreen() : base("Options")
        {
            

            _musicVolumeUp = new MenuEntry(string.Empty);
            _musicVolumeDown = new MenuEntry(string.Empty);
            _effectsVolumeUp = new MenuEntry(string.Empty);
            _effectsVolumeDown = new MenuEntry(string.Empty);
            _currentMusicVolume = new MenuEntry(string.Empty);
            _currentEffectsVolume = new MenuEntry(string.Empty);

         

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _musicVolumeUp.Selected += MusicVolumeUpEntrySelected;
            _musicVolumeDown.Selected += MusicVolumeDownEntrySelected;
            _effectsVolumeUp.Selected += EffectsVolumeUpEntrySelected;
            _effectsVolumeDown.Selected += EffectsVolumeDownEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(_currentMusicVolume);
            MenuEntries.Add(_musicVolumeUp);
            MenuEntries.Add(_musicVolumeDown);

            MenuEntries.Add(_currentEffectsVolume);
            MenuEntries.Add(_effectsVolumeUp);
            MenuEntries.Add(_effectsVolumeDown);
 
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _currentMusicVolume.Text = $"Current Music Volume: " +MediaPlayer.Volume.ToString();
            _musicVolumeUp.Text = $"Music Volume Up";
            _musicVolumeDown.Text = $"Music Volume Down";
            _currentEffectsVolume.Text = $"Current Effects Volume: "+ SoundEffect.MasterVolume.ToString();
            _effectsVolumeUp.Text = $"Effects Volume Up";
            _effectsVolumeDown.Text = $"Effects Volume Down";
        }

        private void MusicVolumeUpEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Volume += 0.1f;

            SetMenuEntryText();
        }

        private void MusicVolumeDownEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Volume-= 0.1f;

            SetMenuEntryText();
        }

        private void EffectsVolumeUpEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (SoundEffect.MasterVolume <= 0.9f)
            {
                SoundEffect.MasterVolume += 0.1f;
            }
            else
            {
                SoundEffect.MasterVolume = 1;
            }
            SetMenuEntryText();
        }

        private void EffectsVolumeDownEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (SoundEffect.MasterVolume >= 0.1f)
            {
                SoundEffect.MasterVolume -= 0.1f;
            }
            else
            {
                SoundEffect.MasterVolume = 0;
            }
            SetMenuEntryText();
        }

    }
}
