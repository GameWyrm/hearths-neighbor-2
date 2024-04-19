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
        public GameObject endngPumpMusic;
        public GameObject suction;
        public Material lampMaterial;
        public MeshRenderer magistrationRenderer;
        public Color fullGravityColor;
        public Color lowGravityColor;
        public Animator deviceAnimator;
        public AudioSource deviceAudio;
        public GameObject owlkLanterns;
        public GameObject endingSector;
        public GameObject RingInner;
        public GameObject RingMiddle;
        public GameObject RingOuter;
        public GameObject endBlackHole;
        public GameObject endWhiteHole;
        public GameObject devDoor;
        public GameObject devCube;
        public GameObject laceWeaver;
        public GameObject shoe;
        public EndingManager endingManager;

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

            endingSector = transform.parent.Find("Ending").gameObject;

            RingInner = transform.parent.Find("RingInner").gameObject;
            RingMiddle = transform.parent.Find("RingMiddle").gameObject;
            RingOuter = transform.parent.Find("RingOuter").gameObject;

            endBlackHole = transform.Find("EndingBlackHole").gameObject;
            endWhiteHole = endingSector.transform.Find("EndingWhiteHole").gameObject;
            endBlackHole.SetActive(false);
            endWhiteHole.SetActive(false);

            endingManager = endingSector.GetComponentInChildren<EndingManager>();
            endingSector.SetActive(false);

            if (PlayerData.GetPersistentCondition("HN2_P_HORRORDOOR")) EnableDevRoom(PlayerData.GetPersistentCondition("HN2_P_REDUCEDFRIGHT"));
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

                float ringSpeed = progress * Time.deltaTime * 30;
                RingInner.transform.localEulerAngles = new Vector3(RingInner.transform.localEulerAngles.x, RingInner.transform.localEulerAngles.y + ringSpeed, RingInner.transform.localEulerAngles.z);
                RingMiddle.transform.localEulerAngles = new Vector3(RingMiddle.transform.localEulerAngles.x, RingMiddle.transform.localEulerAngles.y, RingMiddle.transform.localEulerAngles.z + ringSpeed);
                RingOuter.transform.localEulerAngles = new Vector3(RingOuter.transform.localEulerAngles.x, RingOuter.transform.localEulerAngles.y + ringSpeed, RingOuter.transform.localEulerAngles.z);
            }
            switch (timeState)
            {
                case TimeState.Normal:
                    if (time >= lowGravTime)
                    {
                        gravity.SetFieldMagnitude(8);
                        if (time < noGravTime && playerNear)
                        {
                            NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateLowGrav"), 15));
                            Locator.GetShipLogManager().RevealFact("HN2_EG_Rumor");
                            Locator.GetShipLogManager().RevealFact("HN2_Intro3");
                        }

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
                        if (time < redLightTime && playerNear)
                        {
                            NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateNoGrav"), 15));
                            Locator.GetShipLogManager().RevealFact("HN2_EG_Rumor");
                            Locator.GetShipLogManager().RevealFact("HN2_Intro3");
                        }
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
                        if (time < deadTime && playerNear)
                        {
                            NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateRedLights"), 15));
                            Locator.GetShipLogManager().RevealFact("HN2_EG_Rumor");
                            Locator.GetShipLogManager().RevealFact("HN2_Intro4");
                        }
                        foreach (OWEmissiveRenderer emissiveRenderer in owlkLanterns.GetComponentsInChildren<OWEmissiveRenderer>())
                        {
                            if (emissiveRenderer.gameObject.name == "CandleCover_Paper") emissiveRenderer.SetEmissiveScale(0.5f);
                        }
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
                        suction.SetActive(false);
                        deviceAudio.Stop();
                        if (playerNear)
                        {
                            NotificationManager.s_instance.PostNotification(new(NotificationTarget.All, nh.GetTranslationForOtherText("$HN2TimeStateDead"), 15));
                            Locator.GetShipLogManager().RevealFact("HN2_EG_Rumor");
                            Locator.GetShipLogManager().RevealFact("HN2_Intro5");
                        }

                        foreach (OWEmissiveRenderer emissiveRenderer in owlkLanterns.GetComponentsInChildren<OWEmissiveRenderer>())
                        {
                            if (emissiveRenderer.gameObject.name == "CandleCover_Paper") emissiveRenderer.SetEmissiveScale(0);
                        }

                        electricity.SetActive(false);
                        timeState = TimeState.Dead;
                    }
                    break;
            }
            // TODO add requirement for ship logs and make more efficient
            if (HearthsNeighbor2.Main.hasBattery && !PlayerState.IsWearingSuit() && Vector3.Distance(player.transform.position, transform.position) < 200)
            {
                endngPumpMusic.SetActive(true);
            }
        }

        private void EnableDevRoom(bool reducedFright)
        {
            devCube.SetActive(false);
            devDoor.SetActive(false);
            laceWeaver.SetActive(!reducedFright);
            shoe.SetActive(reducedFright);
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

        public void FireDevice()
        {
            endBlackHole.SetActive(true);
            endWhiteHole.SetActive(true);
            endingSector.SetActive(true);
            foreach (Animator anim in endingManager.magister.GetComponentsInChildren<Animator>())
            {
                anim.SetTrigger("Close");
            }
            Locator.GetShipLogManager().RevealFact("HN2_Device1");
        }
    }
}
