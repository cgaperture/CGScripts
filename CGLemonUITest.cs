using System;
using System.Windows.Forms;
using GTA;
using LemonUI;
using LemonUI.Menus;

namespace CGLemonUITest
{
    public class CGLemonUITest : Script
    {
        private ObjectPool pool = new ObjectPool();
        private NativeMenu mainMenu;

        public CGLemonUITest()
        {
            mainMenu = new NativeMenu("CG Menu", "MAIN MENU", "Select an option");

            NativeItem notificationItem = new NativeItem("Send Notification", "Sends a Notification to You");
            NativeItem vehicleItem = new NativeItem("Spawn Vehicle", "Spawns a Vehicle");

            mainMenu.Add(notificationItem);
            mainMenu.Add(vehicleItem);
            pool.Add(mainMenu);

            notificationItem.Activated += NotificationItem_Activated;
            vehicleItem.Activated += VehicleItem_Activated;

            Tick += OnTick;
            KeyDown += OnKeyDown;
        }

        private void NotificationItem_Activated(object sender, EventArgs e)
        {
            GTA.UI.Notification.PostTicker("Notification Sent!", false);
        }

        private void VehicleItem_Activated(object sender, EventArgs e)
        {
            GTA.UI.Notification.PostTicker("Vehicle Spawned!", false);

            Model fmj2Model;

            unchecked
            {
                fmj2Model = new Model((int)3287642921u);
            }

            if (fmj2Model.IsValid)
            {
                fmj2Model.Request();

                while (!fmj2Model.IsLoaded)
                {
                    Script.Yield();
                }

                Vehicle vehicle = World.CreateVehicle(fmj2Model, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, Game.Player.Character.Heading + 90);
                vehicle.CanTiresBurst = false;
                vehicle.Mods.PrimaryColor = VehicleColor.MatteBlack;
                vehicle.Mods.SecondaryColor = VehicleColor.MatteWhite;
                vehicle.PlaceOnGround();
                vehicle.Mods.LicensePlate = "FFN";

                Blip carBlip = vehicle.AddBlip();

                carBlip.Sprite = BlipSprite.GetawayCar;
                carBlip.Color = BlipColor.BlueDark;
                carBlip.IsShortRange = false;
                carBlip.Name = "FMJ MK V";

                fmj2Model.MarkAsNoLongerNeeded();
            } else
            {
                GTA.UI.Notification.PostTicker("Error: FMJ MK V model not found in game files.", false);
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                mainMenu.Visible = !mainMenu.Visible;
            }
        }
    }
}
