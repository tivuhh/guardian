﻿using Guardian.Utilities;
using System.IO;
using System.Text;
using UnityEngine;

namespace Guardian.Features.Properties
{
    class PropertyManager : FeatureManager<Property>
    {
        private string _dataPath = Mod.RootDir + "\\GameSettings.txt";

        // Master Client
        public Property<bool> EndlessTitans = new Property<bool>("MC_EndlessTitans", new string[0], false);
        public Property<bool> InfiniteRoom = new Property<bool>("MC_InfiniteRoom", new string[0], true);
        public Property<bool> OGPunkHair = new Property<bool>("MC_OGPunkHair", new string[0], true);
        public Property<bool> DeadlyHooks = new Property<bool>("MC_DeadlyHooks", new string[0], false);

        // Player
        public Property<bool> UseRawInput = new Property<bool>("Player_RawTPS-WOWInput", new string[0], true);
        public Property<bool> DoubleTapBurst = new Property<bool>("Player_DoubleTapBurst", new string[0], true);
        public Property<bool> AlternateIdle = new Property<bool>("Player_AHSSIdle", new string[0], false);
        public Property<bool> AlternateBurst = new Property<bool>("Player_CrossBurst", new string[0], false);
        public Property<bool> HideHookArrows = new Property<bool>("Player_HideHookArrows", new string[0], false);
        public Property<bool> HoldForBladeTrails = new Property<bool>("Player_HoldForBladeTrails", new string[0], true);
        public Property<bool> Interpolation = new Property<bool>("Player_Interpolation", new string[0], true);
        public Property<float> ReelOutScrollSmoothing = new Property<float>("Player_ReelOutScrollSmoothing", new string[0], 0.2f);
        public Property<float> OpacityOfOwnName = new Property<float>("Player_OpacityOfOwnName", new string[0], 1.0f);
        public Property<float> OpacityOfOtherNames = new Property<float>("Player_OpacityOfOtherNames", new string[0], 1.0f);
        public Property<bool> DirectionalFlares = new Property<bool>("Player_DirectionalFlares", new string[0], true);
        public Property<string> SuicideMessage = new Property<string>("Player_SuicideMessage", new string[0], "[FFFFFF]Suicide[-]");
        public Property<string> LavaDeathMessage = new Property<string>("Player_LavaDeathMessage", new string[0], "[FF0000]Lava[-]");
        public Property<int> LocalMinDamage = new Property<int>("Player_LocalMinDamage", new string[0], 10);

        // Chat
        public Property<int> MaxChatLines = new Property<int>("Chat_MaxMessages", new string[0], 100);
        public Property<bool> ChatBackground = new Property<bool>("Chat_DrawBackground", new string[0], true);

        public Property<bool> TranslateIncoming = new Property<bool>("Chat_TranslateIncoming", new string[0], false);
        public Property<string> IncomingLanguage = new Property<string>("Chat_IncomingLanguage", new string[0], "auto");
        public Property<bool> TranslateOutgoing = new Property<bool>("Chat_TranslateOutgoing", new string[0], false);
        public Property<string> OutgoingLanguage = new Property<string>("Chat_OutgoingLanguage", new string[0], Mod.SystemLanguage);

        public Property<string> JoinMessage = new Property<string>("Chat_JoinMessage", new string[0], string.Empty);
        public Property<string> ChatName = new Property<string>("Chat_UserName", new string[0], string.Empty);
        public Property<bool> BoldName = new Property<bool>("Chat_BoldName", new string[0], false);
        public Property<bool> ItalicName = new Property<bool>("Chat_ItalicName", new string[0], false);
        public Property<string> TextColor = new Property<string>("Chat_TextColor", new string[0], string.Empty);
        public Property<string> TextPrefix = new Property<string>("Chat_TextPrefix", new string[0], string.Empty);
        public Property<string> TextSuffix = new Property<string>("Chat_TextSuffix", new string[0], string.Empty);
        public Property<bool> BoldText = new Property<bool>("Chat_BoldText", new string[0], false);
        public Property<bool> ItalicText = new Property<bool>("Chat_ItalicText", new string[0], false);

        // Visual [Render]
        public Property<int> DrawDistance = new Property<int>("Visual_DrawDistance", new string[0], 1500);
        public Property<bool> Fog = new Property<bool>("Visual_Fog", new string[0], false);
        public Property<bool> SoftShadows = new Property<bool>("Visual_SoftShadows", new string[0], false);
        // Visual [Misc]
        public Property<string> Flare1Color = new Property<string>("Visual_Flare1Color", new string[0], "00FF007B");
        public Property<string> Flare2Color = new Property<string>("Visual_Flare2Color", new string[0], "FF00007B");
        public Property<string> Flare3Color = new Property<string>("Visual_Flare3Color", new string[0], "00000087");
        public Property<bool> FPSCamera = new Property<bool>("Visual_FPSCamera", new string[0], false);
        public Property<bool> MultiplayerNapeMeat = new Property<bool>("Visual_MultiplayerNapeMeat", new string[0], false);

        // Misc
        public Property<string> CustomAppId = new Property<string>("Misc_AppId", new string[0], string.Empty);
        public Property<bool> UseRichPresence = new Property<bool>("Misc_DiscordPresence", new string[0], true);

        // Logging
        public Property<int> MaxLogLines = new Property<int>("Log_MaxEntries", new string[0], 100);
        public Property<bool> ShowLog = new Property<bool>("Log_ShowLog", new string[0], true);
        public Property<bool> LogBackground = new Property<bool>("Log_DrawBackground", new string[0], true);

        public override void Load()
        {
            // Gameplay
            base.Add(EndlessTitans);
            base.Add(InfiniteRoom);
            base.Add(OGPunkHair);
            base.Add(DeadlyHooks);

            // Player
            base.Add(UseRawInput);
            base.Add(DoubleTapBurst);
            base.Add(AlternateIdle);
            base.Add(AlternateBurst);
            base.Add(HideHookArrows);
            base.Add(HoldForBladeTrails);

            Interpolation.OnValueChanged = () =>
            {
                HERO myHero = IN_GAME_MAIN_CAMERA.Gametype == GameType.Singleplayer ?
                    FengGameManagerMKII.Instance.Heroes[0] : GameHelper.GetHero(PhotonNetwork.player);

                if (myHero != null)
                {
                    myHero.rigidbody.interpolation = Interpolation.Value ? RigidbodyInterpolation.Interpolate
                        : RigidbodyInterpolation.None;
                }
            };
            base.Add(Interpolation);

            base.Add(ReelOutScrollSmoothing);

            OpacityOfOwnName.OnValueChanged = () =>
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    foreach (HERO hero in FengGameManagerMKII.Instance.Heroes)
                    {
                        if (hero.photonView.isMine && hero.myNetWorkName != null)
                        {
                            hero.myNetWorkName.GetComponent<UILabel>().alpha = Mod.Properties.OpacityOfOwnName.Value;
                        }
                    }
                }
            };
            base.Add(OpacityOfOwnName);

            OpacityOfOtherNames.OnValueChanged = () =>
            {
                if (IN_GAME_MAIN_CAMERA.Gametype == GameType.Multiplayer)
                {
                    foreach (HERO hero in FengGameManagerMKII.Instance.Heroes)
                    {
                        if (!hero.photonView.isMine && hero.myNetWorkName != null)
                        {
                            hero.myNetWorkName.GetComponent<UILabel>().alpha = Mod.Properties.OpacityOfOtherNames.Value;
                        }
                    }
                }
            };
            base.Add(OpacityOfOtherNames);

            base.Add(DirectionalFlares);
            base.Add(SuicideMessage);
            base.Add(LavaDeathMessage);
            base.Add(LocalMinDamage);

            // Chat
            base.Add(MaxChatLines);
            base.Add(ChatBackground);

            base.Add(TranslateIncoming);
            base.Add(IncomingLanguage);
            base.Add(TranslateOutgoing);
            base.Add(OutgoingLanguage);

            base.Add(JoinMessage);
            base.Add(ChatName);
            base.Add(TextColor);
            base.Add(BoldName);
            base.Add(ItalicName);
            base.Add(TextPrefix);
            base.Add(TextSuffix);
            base.Add(BoldText);
            base.Add(ItalicText);

            // Visual [Render]
            DrawDistance.OnValueChanged = () =>
            {
                if (Camera.main != null)
                {
                    Camera.main.farClipPlane = DrawDistance.Value;
                }
            };
            base.Add(DrawDistance);

            Fog.OnValueChanged = () =>
            {
                RenderSettings.fogColor = 0x222222FF.ToColor();
                RenderSettings.fog = Fog.Value;
            };
            base.Add(Fog);

            SoftShadows.OnValueChanged = () =>
            {
                GameObject mainLightSource = GameObject.Find("mainLight");
                if (mainLightSource != null)
                {
                    Light mainLight = mainLightSource.GetComponent<Light>();

                    if (SoftShadows.Value)
                    {
                        QualitySettings.shadowCascades = 5;
                        mainLight.shadowBias = 0.04f;
                        mainLight.shadowSoftness = 32f;
                    } else
                    {
                        QualitySettings.shadowCascades = 4;
                        mainLight.shadowBias = 0.15f;
                        mainLight.shadowSoftness = 4f;
                    }
                }
            };
            base.Add(SoftShadows);

            // Visual [Misc]
            base.Add(Flare1Color);
            base.Add(Flare2Color);
            base.Add(Flare3Color);
            base.Add(FPSCamera);
            base.Add(MultiplayerNapeMeat);

            // Misc
            CustomAppId.OnValueChanged = () =>
            {
                Networking.PhotonApplication.Custom.Id = CustomAppId.Value;
            };
            base.Add(CustomAppId);
            base.Add(UseRichPresence);

            // Logging
            base.Add(MaxLogLines);
            base.Add(ShowLog);
            base.Add(LogBackground);

            LoadFromFile();
            Save();
        }

        public void LoadFromFile()
        {
            GameHelper.TryCreateFile(_dataPath, false);

            foreach (string line in File.ReadAllLines(_dataPath))
            {
                string[] data = line.Split(new char[] { '=' }, 2);
                Property property = Find(data[0]);

                if (property != null)
                {
                    if (property.Value is bool)
                    {
                        if (bool.TryParse(data[1], out bool result))
                        {
                            ((Property<bool>)property).Value = result;
                        }
                    }
                    else if (property.Value is int)
                    {
                        if (int.TryParse(data[1], out int result))
                        {
                            ((Property<int>)property).Value = result;
                        }
                    }
                    else if (property.Value is float)
                    {
                        if (float.TryParse(data[1], out float result))
                        {
                            ((Property<float>)property).Value = result;
                        }
                    }
                    else if (property.Value is string)
                    {
                        ((Property<string>)property).Value = data[1];
                    }
                }
            }
        }

        public override void Save()
        {
            GameHelper.TryCreateFile(_dataPath, false);

            StringBuilder builder = new StringBuilder();
            base.Elements.ForEach(property => builder.AppendLine($"{property.Name}={property.Value}"));

            File.WriteAllText(_dataPath, builder.ToString());
        }
    }
}
