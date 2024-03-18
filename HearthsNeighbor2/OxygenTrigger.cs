using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class OxygenTrigger : MonoBehaviour
    {
        public bool turnOnOxygen = false;
        public TimeStateManager timeManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                timeManager.SetOxygenState(turnOnOxygen);
            }
        }
    }
}
