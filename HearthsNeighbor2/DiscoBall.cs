using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class DiscoBall : MonoBehaviour
    {
        public float speed = 90;

        private SingleInteractionVolume _interactVolume;
        private bool spin = false;
        private INewHorizons nh;
        private Transform child;

        private void Awake()
        {
            nh = HearthsNeighbor2.Main.newHorizons;
            child = transform.GetChild(0);
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += () =>
            {
                spin = !spin;
                child.GetChild(0).gameObject.SetActive(spin);
            };
        }

        private void Start()
        {
            _interactVolume.ChangePrompt(nh.GetTranslationForUI("$HN2PartyPrompt"));
        }

        private void Update()
        {
            if (spin)
            {
                transform.localEulerAngles = new(0, transform.localEulerAngles.y + (speed * Time.deltaTime), 0);
            }
        }
    }
}
