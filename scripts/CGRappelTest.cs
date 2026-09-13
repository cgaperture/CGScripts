using System;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;

namespace CGRappelTest
{
    public class CGRappelTest : Script
    {
        Vector3 dropOffPos = new Vector3(-498.2090f, -241.5667f, 35.9642f + 18.0f);
        Vehicle maverick;

        bool playable = true;
        bool onTheWayObjective = false;

        enum MissionState
        {
            NotActive,
            OnTheWay,
            Rappel
        }

        MissionState currentMissionState = MissionState.NotActive;

        public CGRappelTest()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (currentMissionState == MissionState.NotActive) return;

            Vector3 playerPos = Game.Player.Character.Position;

            switch (currentMissionState)
            {
                case (MissionState.OnTheWay):
                    GTA.UI.Screen.ShowSubtitle("Wait for the ~b~pilot~w~ to reach the ~y~drop-zone~w~", 10);

                    if (!onTheWayObjective)
                    {
                        onTheWayObjective = true;

                        // TODO: Implement code here
                        VehicleHash maverickHash = VehicleHash.Maverick;
                        PedHash pilotHash = PedHash.Pilot;
                        Model maverickModel = new Model(maverickHash);
                        Model pilotModel = new Model(pilotHash);

                        maverickModel.Request();
                        while (!maverickModel.IsLoaded) Script.Wait(10);
                        pilotModel.Request();
                        while (!pilotModel.IsLoaded) Script.Wait(10);

                        maverick = Vehicle.Create(maverickModel, playerPos + Game.Player.Character.UpVector * 10.0f);
                        Ped pilot = Ped.Create(pilotModel, playerPos + Game.Player.Character.ForwardVector * 3.0f);

                        maverickModel.MarkAsNoLongerNeeded();
                        pilotModel.MarkAsNoLongerNeeded();

                        pilot.SetIntoVehicle(maverick, VehicleSeat.Driver);
                        Game.Player.Character.SetIntoVehicle(maverick, VehicleSeat.LeftRear);

                        maverick.IsEngineRunning = true;
                        Function.Call(Hash.SET_HELI_BLADES_FULL_SPEED, maverick.Handle);

                        pilot.Task.StartHeliMission(maverick, dropOffPos, VehicleMissionType.GoTo, 35.0f, 10.0f, 30, 20, -1, -1, HeliMissionFlags.DisableAllHeightMapAvoidance);
                    }

                    if (maverick != null && maverick.Exists() && maverick.Position.DistanceTo(dropOffPos) <= 10.0f)
                    {
                        currentMissionState = MissionState.Rappel;

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CHECKPOINT_PERFECT", "HUD_MINI_GAME_SOUNDSET", true);
                    }

                    break;
                case (MissionState.Rappel):
                    GTA.UI.Screen.ShowSubtitle("Rappel out of the heli", 10);

                    GTA.UI.Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to rappel.");

                    if (Game.IsControlJustPressed(GTA.Control.Context))
                    {
                        currentMissionState = MissionState.NotActive;

                        Game.Player.Character.Task.RappelFromHelicopter();

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CHECKPOINT_PERFECT", "HUD_MINI_GAME_SOUNDSET", true);
                    }

                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1 && currentMissionState == MissionState.NotActive && playable)
            {
                playable = false;
                currentMissionState = MissionState.OnTheWay;
            }
        }
    }
}
