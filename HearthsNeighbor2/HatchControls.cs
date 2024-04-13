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
        public MemoryCube hatchCube;
        public bool hatchOpen = true;

        private void Start()
        {
            GetComponentInParent<MemoryCube>().onFinishedReading += ToggleHatchControls;
        }

        private void ToggleHatchControls()
        {
            hatchOpen = !hatchOpen;
            hatchAnim.SetBool("HatchOpen", hatchOpen);
            HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Hatch controls set to {(hatchOpen ? "OPEN" : "CLOSED")}", OWML.Common.MessageType.Info);
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(hatchOpen);
            }
            hatchCube.ChangeActiveState(false);
        }
    }
}
