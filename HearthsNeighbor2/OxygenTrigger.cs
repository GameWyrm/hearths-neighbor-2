using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    [RequireComponent(typeof(OWTriggerVolume))]
    public class OxygenTrigger : SectoredMonoBehaviour
    {
        public bool turnOnOxygen = false;
        public TimeStateManager timeManager;

        private OWTriggerVolume _triggerVolume;

        public override void Awake()
        {
            base.Awake();
            _triggerVolume = this.GetRequiredComponent<OWTriggerVolume>();
            _triggerVolume.OnEntry += OnTriggerVolumeEntry;
        }

        public void OnTriggerVolumeEntry(GameObject hitObj)
        {
            if (hitObj.CompareTag("PlayerDetector"))
            {
                timeManager.SetOxygenState(turnOnOxygen);
            }
        }
    }
}
