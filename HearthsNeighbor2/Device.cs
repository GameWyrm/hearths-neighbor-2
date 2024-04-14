using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class Device : MonoBehaviour
    {
        public TimeStateManager StateManager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && HearthsNeighbor2.Main.hasBattery && !PlayerState.IsWearingSuit())
            {
                // Makes the player drop the battery and disables it
                ItemTool item = Locator.GetPlayerCamera().GetComponentInChildren<ItemTool>();
                if (item.GetHeldItem() != null)
                {
                    item.DropItemInstantly(GetComponentInParent<Sector>(), transform);
                    item.gameObject.SetActive(false);
                }
                StateManager.FireDevice();
            }
        }
    }
}
