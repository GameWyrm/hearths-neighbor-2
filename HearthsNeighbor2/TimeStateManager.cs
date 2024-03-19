using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class TimeStateManager : MonoBehaviour
    {
        public TimeState timeState = TimeState.Normal;
        public DirectionalForceVolume gravity;
        public GameObject oxygen;
        public Material lampMaterial;
        public Animator deviceAnimator;
        public AudioSource deviceAudio;

        // time loop properties
        private readonly int lowGravTime = 8;
        private readonly int noGravTime = 14;
        private readonly int redLightTime = 19;
        private readonly int deadTime = 20;

        private INewHorizons nh;

        private GameObject player;
        private List<Light> lights;

        private void Start()
        {
            player = Locator.GetPlayerBody().gameObject;
            nh = HearthsNeighbor2.Main.newHorizons;
            lights = new();
            foreach (Light light in GetComponentsInChildren<Light>())
            {
                if (light.gameObject.name != "LightNoRed")
                {
                    lights.Add(light);
                    if (light.gameObject.name == "MagiLight")
                    {
                        light.GetComponentInParent<MeshRenderer>().sharedMaterials[2] = lampMaterial;
                    }
                }
            }
            lampMaterial.color = Color.white;
            transform.parent.Find("DeviceNomai").localScale = Vector3.one * 0.3f;
        }

        private void Update()
        {
            bool playerNear = Vector3.Distance(transform.position, player.transform.position) < 200;
            var time = TimeLoop.GetMinutesElapsed();
            if (timeState != TimeState.Dead)
            {
                float progress = Mathf.Clamp01((20 - time) / 20);
                deviceAnimator.SetFloat("Speed", progress * 4);
                deviceAudio.pitch = Mathf.Lerp(0.5f, 2.5f, (progress));
            }
            switch (timeState)
            {
                case TimeState.Normal:
                    if (time >= lowGravTime)
                    {
                        gravity.SetFieldMagnitude(8);
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(nh.GetTranslationForOtherText("$HN2TimeStateLowGrav")));
                        timeState = TimeState.LowGrav;
                    }
                    break;
                case TimeState.LowGrav:
                    if (time >= noGravTime)
                    {
                        gravity.gameObject.SetActive(false);
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(nh.GetTranslationForOtherText("$HN2TimeStateNoGrav")));
                        timeState = TimeState.NoGrav;
                    }
                    break;
                case TimeState.NoGrav:
                    if (time >= redLightTime)
                    {
                        foreach (Light light in lights)
                        {
                            light.color = Color.red;
                        }
                        lampMaterial.color = Color.red;
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(nh.GetTranslationForOtherText("$HN2TimeStateRedLights")));
                        timeState = TimeState.RedLights;
                    }
                    break;
                case TimeState.RedLights:
                    if (time >= deadTime)
                    {
                        foreach (Light light in lights)
                        {
                            light.gameObject.SetActive(false);
                        }
                        lampMaterial.color = Color.black;
                        oxygen.SetActive(false);
                        deviceAudio.Stop();
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(nh.GetTranslationForOtherText("$HN2TimeStateDead")));
                        timeState = TimeState.Dead;
                    }
                    break;
            }
        }

        /// <summary>
        /// Turns the ship's oxygen on or off, does not work if oxygen production is offline
        /// </summary>
        /// <param name="on"></param>
        public void SetOxygenState(bool on)
        {
            if (timeState != TimeState.Dead)
            {
                oxygen.gameObject.SetActive(on);
            }
        }
    }
}
