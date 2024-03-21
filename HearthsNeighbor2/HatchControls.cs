using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class HatchControls : MonoBehaviour
    {
        public Animator hatchAnim;
        public GameObject[] objectsToToggle;
        public bool hatchOpen = true;

        private SingleInteractionVolume _interactVolume;

        private void Awake()
        {
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += OnPressInteract;
        }

        private void OnPressInteract()
        {
            hatchOpen = !hatchOpen;
            hatchAnim.SetBool("HatchOpen", hatchOpen);
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(hatchOpen);
            }
        }
    }
}
