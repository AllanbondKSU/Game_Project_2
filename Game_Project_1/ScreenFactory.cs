using System;
using System.Collections.Generic;
using System.Text;
using GameArchitectureExample.StateManagement;

namespace Game_Project_2
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            // All of our screens have empty constructors so we can just use Activator
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
