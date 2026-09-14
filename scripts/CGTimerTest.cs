using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using LemonUI;
using LemonUI.Menus;
using LemonUI.TimerBars;

namespace CGTimerTest
{
    public class CGTimerTest : Script
    {
        ObjectPool pool;
        NativeMenu menu;

        TimerBarCollection timerBarCollection;
        TimerBarProgress timerBarProgress;

        float maxTime = 30f;
        float timeRemaining;

        public CGTimerTest()
        {
            GTA.UI.Notification.Show("Script Loaded!", false);

            pool = new ObjectPool();
            menu = new NativeMenu("", "CG MENU", "Menu for CG Tests", null);
            timerBarCollection = new TimerBarCollection();

            NativeItem item = new NativeItem("Test Item", "This is a test item");

            menu.Add(item);
            pool.Add(menu);

            timerBarProgress = new TimerBarProgress("MISSION TIMER")
            {
                Progress = 100f,
                Color = Color.FromArgb(200, 50, 50)
            };

            timerBarCollection.Add(timerBarProgress);

            timeRemaining = maxTime;

            Tick += OnTick;
            KeyUp += OnKeyUp;
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();

            if (timeRemaining > 0)
            {
                // Subtract elapsed time since the last frame
                timeRemaining -= Game.LastFrameTime;

                // Calculate percentage (0.0 to 100.0)
                float percentage = (timeRemaining / maxTime) * 100f;

                // Update the LemonUI progress bar value
                timerBarProgress.Progress = Math.Max(0f, percentage);

                // 5. Draw the timer bar collection onto the screen
                timerBarCollection.Process();
            }
            else
            {
                // Timer has finished
                timerBarProgress.Progress = 0f;

                // Optional: Trigger custom completion logic here (e.g., Fail Mission)
                GTA.UI.Notification.Show("Time's Up!");

                // Turn off the tick or remove from collection to stop rendering
                Tick -= OnTick;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                menu.Visible = !menu.Visible;
            }
        }
    }
}