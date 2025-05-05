using OWML.Common;
using OWML.ModHelper;
using System;
using System.IO;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

namespace HearthsNeighbor2
{
    public class HearthsNeighbor2 : ModBehaviour
    {
        public static HearthsNeighbor2 Main 
        { get 
            { 
                if (instance == null) instance = FindObjectOfType<HearthsNeighbor2>(); 
                return instance;
            }
        }

        public INewHorizons newHorizons;
        public IOWVoiceMod voiceMod;

        public bool isInSystem = false;
        public bool hasBattery = false;

        public bool easyMode = false;
        public bool focusOnCubes = true;
        public bool vaNotifications = false;

        public event Action configsUpdated;

        private static HearthsNeighbor2 instance;
        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        private void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(HearthsNeighbor2)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            newHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            newHorizons.LoadConfigs(this);

            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

            };

            newHorizons.GetStarSystemLoadedEvent().AddListener((system) => 
            { 
                isInSystem = system == "Jam3"; 
                if (isInSystem)
                {
                    if (PlayerData.GetShipLogFactSave("HN_POD_RESOLUTION") != null && PlayerData.GetShipLogFactSave("HN_POD_RESOLUTION").revealOrder > -1)
                    {
                        ModHelper.Console.WriteLine("Player has finished Hearth's Neighbor 1! Special treat for you!", MessageType.Success);
                        StartCoroutine(RegisterConnectionLog());
                    }

                    hasBattery = false;
                }
            });
            // Voice mod stuff
            voiceMod = ModHelper.Interaction.TryGetModApi<IOWVoiceMod>("Krevace.VoiceMod");
            if (voiceMod != null)
            {
                ModHelper.Console.WriteLine("Voice Mod detected, registering voice lines.", MessageType.Success);
                voiceMod.RegisterAssets(Path.Combine(ModHelper.Manifest.ModFolderPath, "assets\\VoiceActing"));
            }

            // Load settings
            easyMode = ModHelper.Config.GetSettingsValue<bool>("EasyMode");
            focusOnCubes = ModHelper.Config.GetSettingsValue<bool>("FocusOnCubes");
        }

        public override void Configure(IModConfig config)
        {
            easyMode = config.GetSettingsValue<bool>("EasyMode");
            focusOnCubes = config.GetSettingsValue<bool>("FocusOnCubes");
            string shouldVANotifs = config.GetSettingsValue<string>("VaNotifications");
            switch (shouldVANotifs)
            {
                case "On":
                    vaNotifications = true; break;
                case "Off":
                    vaNotifications = false; break;
                default:
                    vaNotifications = ModHelper.Interaction.ModExists("Krevace.VoiceMod"); break;
            }

            configsUpdated?.Invoke();
        }

        IEnumerator RegisterConnectionLog()
        {
            yield return new WaitForEndOfFrame();
            ShipLogManager manager = GameObject.FindObjectOfType<ShipLogManager>();
            manager.RevealFact("HN2_HN1");
            manager.RevealFact("HN2_HN2");
        }
    }

}
