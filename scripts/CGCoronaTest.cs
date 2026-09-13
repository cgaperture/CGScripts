using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using GTA.UI;

namespace CGCoronaTest
{
    public class Class1 : Script
    {
        private Vector3 checkpointPos = new Vector3(492.7659f, -1518.788f, 29.2897f);
        private int checkpoint = -1;
        Blip myBlip = null;

        private bool isActive = false;

        public Class1()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (isActive)
            {
                GTA.UI.Screen.ShowSubtitle("Get to the ~y~location!~w~", 10);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1 && isActive == false)
            {
                isActive = true;
                StartHeist();
            } else if (e.KeyCode == Keys.NumPad1 && isActive == true)
            {
                CleanUpMissionAssets();
            }
        }

        private void StartHeist()
        {
            Notification.PostTicker("CG CORONA TEST ENABLED", false);

            // Parking lot location

            myBlip = World.CreateBlip(checkpointPos);

            myBlip.Sprite = BlipSprite.Standard;
            myBlip.Color = BlipColor.Yellow;
            myBlip.Scale = 1.0f;
            myBlip.Name = "Location";

            Function.Call(GTA.Native.Hash.SET_BLIP_ROUTE, myBlip.Handle, true);
            Function.Call(GTA.Native.Hash.SET_BLIP_ROUTE_COLOUR, myBlip.Handle, 60);


            checkpoint = Function.Call<int>(GTA.Native.Hash.CREATE_CHECKPOINT,
                47,
                checkpointPos.X, checkpointPos.Y, checkpointPos.Z - 0.95f,
                checkpointPos.X, checkpointPos.Y, checkpointPos.Z,
                2.0f,
                240, 200, 80, 150,
                0
                );

            Function.Call(GTA.Native.Hash.SET_CHECKPOINT_CYLINDER_HEIGHT, checkpoint, 4.0f, 4.0f, 2.0f);
        }

        private void CleanUpMissionAssets()
        {
            if (myBlip != null && myBlip.Exists())
            {
                myBlip.Delete();
                myBlip = null;
            }

            if (checkpoint != -1)
            {
                Function.Call(GTA.Native.Hash.DELETE_CHECKPOINT, checkpoint);
                checkpoint = -1;
            }

            Notification.PostTicker("CG CORONA TEST DISABLED", false);
            isActive = false;
        }
    }
}
