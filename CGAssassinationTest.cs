using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Windows.Forms;

namespace CGAssassinationTest
{
    public class Class1 : Script
    {
        int checkpoint = -1;
        Blip rooftopBlip;
        Blip targetPedBlip;
        Ped targetPed;
        Vector3 locationPos;

        bool disableWantedLevel = true;
        bool locationObjective = false;
        bool assassinateObjective = false;

        private enum HeistState
        {
            NotActive,
            Location,
            Assasinate,
            Leave,
            Money
        }

        private HeistState currentState = HeistState.NotActive;

        public Class1()
        {
            Tick += OnTick;
            KeyUp += OnKeyUp;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (currentState == HeistState.NotActive)
            {
                return;
            }

            if (disableWantedLevel)
            {
                Game.MaxWantedLevel = 0;

                if (Game.Player.WantedLevel > 0)
                {
                    Game.Player.WantedLevel = 0;
                }
            }

            Vector3 playerPos = Game.Player.Character.Position;
            float distance = locationPos.DistanceTo(playerPos);

            switch (currentState)
            {
                case (HeistState.Location):
                    GTA.UI.Screen.ShowSubtitle("Go to the ~y~Vantage Point~w~", 10);

                    if (!locationObjective)
                    {
                        locationObjective = true;
                        LocationObjective();
                    }

                    if (checkpoint != -1 && locationObjective)
                    {
                        if (distance <= 1.2f)
                        {
                            if (rooftopBlip != null && rooftopBlip.Exists())
                            {
                                rooftopBlip.Delete();
                                rooftopBlip = null;
                            }

                            if (checkpoint != -1)
                            {
                                Function.Call(GTA.Native.Hash.DELETE_CHECKPOINT, checkpoint);
                                checkpoint = -1;
                            }

                            currentState = HeistState.Assasinate;
                        }
                    }
                    break;
                case (HeistState.Assasinate):
                    GTA.UI.Screen.ShowSubtitle("Assassinate the ~r~Target~w~", 10);

                    if (!assassinateObjective)
                    {
                        assassinateObjective = true;
                        AssassinateObjective();
                    }

                    if (targetPed != null && targetPed.Exists())
                    {
                        if (targetPed.IsDead)
                        {
                            if (targetPedBlip != null && targetPedBlip.Exists())
                            {
                                targetPedBlip.Delete();
                                targetPedBlip = null;
                            }

                            targetPed = null;

                            currentState = HeistState.Leave;
                        }
                    }
                    break;
                case (HeistState.Leave):
                    if (distance < 50)
                    {
                        GTA.UI.Screen.ShowSubtitle("Leave the area", 10);
                    } else
                    {
                        currentState = HeistState.Money;
                    }
                    break;
                case (HeistState.Money):
                    AwardMoney();
                    CleanupBooleans();
                    currentState = HeistState.NotActive;
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1 && currentState == HeistState.NotActive)
            {
                currentState = HeistState.Location;
            }
        }

        private void LocationObjective()
        {
            locationPos = new Vector3(-336.4754f, -822.1486f, 54.7902f);
            Vector3 checkpointPos = new Vector3(locationPos.X, locationPos.Y, locationPos.Z - 0.95f);

            rooftopBlip = World.CreateBlip(locationPos);
            rooftopBlip.Sprite = BlipSprite.Standard;
            rooftopBlip.Name = "Vantage Point";
            rooftopBlip.Color = BlipColor.Yellow;
            rooftopBlip.ShowRoute = true;

            checkpoint = Function.Call<int>(GTA.Native.Hash.CREATE_CHECKPOINT,
                47,
                checkpointPos.X, checkpointPos.Y, checkpointPos.Z,
                locationPos.X, locationPos.Y, locationPos.Z,
                1.0f,
                240, 200, 80, 150,
                0
                );

            Function.Call(GTA.Native.Hash.SET_CHECKPOINT_CYLINDER_HEIGHT, checkpoint, 1.0f, 1.0f, 1.0f);
        }

        private void AssassinateObjective()
        {
            PedHash pedModel = PedHash.BallaOrig01GMY;
            Vector3 spawnPos = new Vector3(-310.4843f, -869.7059f, 31.6821f + 1.0f);

            Model model = new Model(pedModel);
            model.Request(500);

            if (model.IsInCdImage && model.IsValid)
            {
                while (!model.IsLoaded)
                {
                    Script.Yield();
                }

                targetPed = World.CreatePed(model, spawnPos);

                model.MarkAsNoLongerNeeded();

                if (targetPed != null)
                {
                    targetPedBlip = targetPed.AddBlip();

                    targetPedBlip.Sprite = BlipSprite.Standard;
                    targetPedBlip.Color = BlipColor.Red;
                    targetPedBlip.Name = "Target";

                    targetPed.SetConfigFlag(PedConfigFlagToggles.DisableShockingEvents, true);
                    targetPed.SetCombatAttribute(CombatAttributes.DisableFleeFromCombat, true);
                    targetPed.BlockPermanentEvents = true;
                }
            }
        }

        private void AwardMoney()
        {
            Game.Player.Money += 50000;
        }

        private void CleanupBooleans()
        {
            locationObjective = false;
            assassinateObjective = false;
        }
    }
}
