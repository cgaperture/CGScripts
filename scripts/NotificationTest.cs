using GTA;
using GTA.Native;
using GTA.UI;
using System;
using System.Windows.Forms;

namespace CGFirstMod
{
    public class Class1 : Script
    {
        public Class1()
        {
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                SendIconNotification("CHAR_LAMAR", "Lamar", "Welcome", "Yo whatup!");
            }
        }

        public void SendIconNotification(string textureDict, string sender, string subject, string message)
        {
            // 1. Begin the notification text block
            Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message);

            // 2. Draw the notification with an icon
            // Parameters: textureDict, textureName, iconType (1 = Chat Box), flash (bool), title, subtitle
            Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, textureDict, textureDict, true, 1, sender, subject);

            // 3. Complete the post to display it on screen
            Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, false, false);
        }
    }
}
