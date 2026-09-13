using GTA;
using GTA.Graphics;
using GTA.Math;
using GTA.UI;
using System;
using System.Windows.Forms;

namespace CGDrivingCarTest
{
    public class CGDrivingCarTest : Script
    {
        bool active = false;

        public CGDrivingCarTest()
        {
            KeyUp += OnKeyUp;
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!active) return;

            if (active)
            {
                GTA.UI.Screen.ShowSubtitle("Trail the car", 10);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1 && !active)
            {
                active = true;

                TextureAsset asset = new TextureAsset("CHAR_LESTER", "CHAR_LESTER");

                Notification.PostMessageText(
                    "The driver has pulled up. Follow him.",
                    asset,
                    false,
                    FeedTextIcon.Blank,
                    "Lester",
                    "Mission"
                    );

                SpawnCarAndPed();
            }
        }

        private void SpawnCarAndPed()
        {
            Vector3 spawnCoords = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5.0f;
            float spawnHeading = Game.Player.Character.Heading + 90.0f;
            Vector3 destinationCoords = new Vector3(175.2839f, 6624.4492f, 31.6926f);
            float destinationHeading = 43.3616f;

            Vehicle vehicle = Vehicle.Create(VehicleHash.Baller, spawnCoords, spawnHeading);
            vehicle.CanTiresBurst = false;
            vehicle.Mods.PrimaryColor = VehicleColor.MatteBlack;
            vehicle.Mods.SecondaryColor = VehicleColor.Chrome;
            vehicle.PlaceOnGround();
            vehicle.Mods.LicensePlate = "FFN";

            vehicle.AddBlip();
            vehicle.AttachedBlip.Sprite = BlipSprite.GetawayCar;
            vehicle.AttachedBlip.Color = BlipColor.BlueDark;
            vehicle.AttachedBlip.Name = "Enemy Vehicle";

            PedHash pedHash = PedHash.BallaOrig01GMY;
            Model pedModel = new Model(pedHash);

            pedModel.Request();
            while (!pedModel.IsLoaded)
            {
                Script.Wait(10);
            }

            Ped driver = Ped.Create(pedModel, spawnCoords + new Vector3(0.0f, 0.0f, 2.0f));
            driver.SetIntoVehicle(vehicle, VehicleSeat.Driver);

            //Game.Player.Character.SetIntoVehicle(vehicle, VehicleSeat.LeftRear);

            driver.Task.DriveTo(vehicle, destinationCoords, 20.0f, VehicleDrivingFlags.DrivingModeAvoidVehiclesReckless, 30.0f);
        }
    }
}
