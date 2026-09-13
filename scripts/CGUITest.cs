using System;
using System.Windows.Forms;
using GTA;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Scaleform;

namespace CGUITest
{
    public class CGUITest : Script
    {
        private ObjectPool pool = new ObjectPool();
        private NativeMenu menu;
        private BigMessage currentMessage;

        public CGUITest()
        {
            menu = new NativeMenu("UI Test", "Menu", "Multiple options to test out LemonUI");

            NativeItem planeItem = new NativeItem("Plane", "Shows/Hides Plane Banner");
            NativeItem missionPassedItem = new NativeItem("Mission Passed", "Shows/Hides Mission Passed Oldgen Banner");

            menu.Add(planeItem);
            menu.Add(missionPassedItem);

            pool.Add(menu);

            planeItem.Activated += PlaneItem_Activated;
            missionPassedItem.Activated += MissionPassedItem_Activated;

            Tick += OnTick;
            KeyUp += OnKeyUp;
        }

        private void PlaneItem_Activated(object sender, EventArgs e)
        {
            // Toggle or create the plane message
            if (currentMessage != null)
            {
                pool.Remove(currentMessage);
                currentMessage = null;
            }
            else
            {
                currentMessage = new BigMessage("Plane Title", "Plane Message", MessageType.Plane);
                currentMessage.Visible = true;
                pool.Add(currentMessage);
            }
        }

        private void MissionPassedItem_Activated(object sender, EventArgs e)
        {
            // Toggle or create the mission passed message
            if (currentMessage != null)
            {
                pool.Remove(currentMessage);
                currentMessage = null;
            }
            else
            {
                currentMessage = new BigMessage("Mission Title", "Mission Passed", MessageType.MissionPassedOldGen);
                currentMessage.Visible = true;
                pool.Add(currentMessage);
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                if (!menu.Visible) menu.Visible = true;
            }
        }
    }
}
