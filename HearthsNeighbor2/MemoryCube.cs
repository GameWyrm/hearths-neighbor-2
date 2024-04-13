using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HearthsNeighbor2
{
    public class MemoryCube : MonoBehaviour
    {
        public Color defaultColor = Color.white;
        public Color importantColor = new Color(1, 0.5f, 0, 1);
        public Color deactivatedColor = Color.black;
        // whether the cube has a ship log
        public bool isCritical;
        public event Action onFinishedReading;

        private CharacterDialogueTree dialogue;
        // whether the player has interacted with the cube at least once
        private bool hasBeenRead = false;
        // whether the cube can be interacted with 
        private bool isOn = true;
        // changes the color of the highlight
        private bool easyMode = false;
        // nauseu mode
        private bool followCube = true;
        private float transitionAmount = 0;
        private Material glow;
        private Light light;
        private SingleInteractionVolume _interactVolume;
        private Animator anim;

        private void Awake()
        {
            dialogue = GetComponentInChildren<CharacterDialogueTree>();
            glow = GetComponentInChildren<MeshRenderer>().materials[1];
            light = GetComponentInChildren<Light>();
            _interactVolume = GetComponentInChildren<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += Read;
            anim = GetComponentInChildren<Animator>();
            int positivity = Random.value < 0.5f ? -1 : 1;
            anim.SetFloat("Speed", Random.Range(0.8f, 1.2f) * positivity);
            HearthsNeighbor2.Main.configsUpdated += ReadConfigs;
            ReadConfigs();
        }

        private void Start()
        {
            ChangeActiveState(true);
        }

        private void Update()
        {
            if (hasBeenRead && transitionAmount < 1)
            {
                transitionAmount += Time.deltaTime;
                if (transitionAmount > 1) transitionAmount = 1;
                light.intensity = 1 - transitionAmount;
                if (light.intensity <= 0) light.gameObject.SetActive(false);
                glow.color = Color.Lerp((easyMode && isCritical) ? importantColor : defaultColor, deactivatedColor, transitionAmount);
            }
        }

        private void OnDestroy()
        {
            HearthsNeighbor2.Main.configsUpdated -= ReadConfigs;
        }

        /// <summary>
        /// Sets the cube to be on or off, without disabling its GameObject. Cubes that are off cannot be interacted with and will not move, but still exist in the world.
        /// </summary>
        public void ChangeActiveState(bool on)
        {
            if (on)
            {
                anim.SetBool("Active", true);
                _interactVolume.SetInteractionEnabled(true);
                if (!hasBeenRead)
                {
                    glow.color = (easyMode && isCritical) ? importantColor : defaultColor;
                    light.gameObject.SetActive(true);
                }
            }
            else
            {
                anim.SetBool("Active", false);
                _interactVolume.SetInteractionEnabled(false);
                glow.color = deactivatedColor;
                light.gameObject.SetActive(false);
            }
        }

        public void Read()
        {
            GlobalMessenger.AddListener("ExitConversation", OnRead);
        }


        private void OnRead()
        {
            hasBeenRead = true;

            onFinishedReading?.Invoke();

            GlobalMessenger.RemoveListener("ExitConversation", OnRead);
        }

        public void ReadConfigs()
        {
            easyMode = HearthsNeighbor2.Main.easyMode;
            followCube = HearthsNeighbor2.Main.focusOnCubes;
            if (isOn && !hasBeenRead)
            {
                glow.color = (easyMode && isCritical) ? importantColor : defaultColor;
            }
            if (followCube)
            {
                dialogue._attentionPoint = transform.Find("Mesh");
            }
            else
            {
                dialogue._attentionPoint = transform.Find("Dialogue");
            }
        }
    }
}
