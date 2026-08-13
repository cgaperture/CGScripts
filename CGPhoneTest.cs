using GTA;
using iFruitAddon2;
using LemonUI;
using LemonUI.Menus;
using System;

namespace CGPhoneTest
{
    public class CGPhoneTest : Script
    {
        readonly CustomiFruit _iFruit;
        private ObjectPool pool = new ObjectPool();
        private NativeMenu callMenu;

        public CGPhoneTest()
        {
            _iFruit = new CustomiFruit();

            iFruitContact apertureContact = new iFruitContact("CG Aperture")
            {
                DialTimeout = 4000,
                Active = true,
                Icon = new ContactIcon("CHAR_GANGAPP")
            };
            apertureContact.Answered += ApertureContactAnswered;
            _iFruit.Contacts.Add(apertureContact);

            callMenu = new NativeMenu("CG Aperture", "CALL MENU", "Select an option");
            NativeItem notificationItem = new NativeItem("Send Notification", "Sends a Notification to You");

            callMenu.Add(notificationItem);
            pool.Add(callMenu);

            notificationItem.Activated += NotificationItem_Activated;

            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            _iFruit.Update();
            pool.Process();
        }

        private void ApertureContactAnswered(iFruitContact contact)
        {
            if (!callMenu.Visible)
            {
                callMenu.Visible = true;
            }

            _iFruit.Close(2000);
        }

        private void NotificationItem_Activated(object sender, EventArgs e)
        {
            GTA.UI.Notification.PostTicker("Notification Sent!", false);
        }
    }
}
