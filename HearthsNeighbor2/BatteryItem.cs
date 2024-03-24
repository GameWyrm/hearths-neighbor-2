using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class BatteryItem : OWItem
    {
        public override string GetDisplayName()
        {
            return HearthsNeighbor2.Main.newHorizons.GetTranslationForOtherText("$HN2ItemArtifact");
        }

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            transform.localScale = Vector3.one * 0.3f;
            HearthsNeighbor2.Main.hasBattery = true;
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            transform.localScale = Vector3.one;
            HearthsNeighbor2.Main.hasBattery = false;
        }
    }
}
