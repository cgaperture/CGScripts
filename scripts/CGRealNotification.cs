using System.Windows.Forms;
using GTA;
using GTA.Graphics;
using GTA.UI;

namespace CGRealNotification
{
    public class CGRealNotification : Script
    {
        public CGRealNotification()
        {
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                // TODO: Implement Notification Code
                TextureAsset asset = new TextureAsset("CHAR_LESTER", "CHAR_LESTER");

                Notification.PostMessageText(
                    "Get over to the warehouse!",
                    asset,
                    false,
                    FeedTextIcon.Blank,
                    "Lester",
                    "Heist Setup"
                    );
            }
        }
    }
}
