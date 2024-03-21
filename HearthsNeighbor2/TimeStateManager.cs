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
        public GameObject electricity;
        public Material lampMaterial;
        public MeshRenderer magistrationRenderer;
        public Color fullGravityColor;
        public Color lowGravityColor;
        public Animator deviceAnimator;
        public AudioSource deviceAudio;

        // time loop properties
        private readonly int lowGravTime = 6;
        private readonly int noGravTime = 12;
        private readonly int redLightTime = 17;
        private readonly int deadTime = 20;

        private INewHorizons nh;

        private GameObject player;
        private List<Light> lights;
        private Material gravityFloor;

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
            foreach (Material mat in magistrationRenderer.sharedMaterials)
            {
                if (mat.name.Contains("MagistrationFloor"))
                {
                    mat.SetColor("_EmissionColor", fullGravityColor);
                    break;
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
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateLowGrav"), 15));

                        foreach (Material mat in magistrationRenderer.sharedMaterials)
                        {
                            if (mat.name.Contains("MagistrationFloor"))
                            {
                                mat.SetColor("_EmissionColor", lowGravityColor);
                                break;
                            }
                        }


                        timeState = TimeState.LowGrav;
                    }
                    break;
                case TimeState.LowGrav:
                    if (time >= noGravTime)
                    {
                        gravity.gameObject.SetActive(false);
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateNoGrav"), 15)); 
                        foreach (Material mat in magistrationRenderer.sharedMaterials)
                        {
                            if (mat.name.Contains("MagistrationFloor"))
                            {
                                mat.SetColor("_EmissionColor", Color.black);
                                break;
                            }
                        }
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
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateRedLights"), 15));
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
                        if (playerNear) NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateDead"), 15));
                        electricity.SetActive(false);
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
