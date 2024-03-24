using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class BookItem : OWItem
    {
        public BookReceiver receiver;
        public string factReveal;

        private bool isReturned = false;
        private INewHorizons nh;

        private void Start()
        {
            nh = HearthsNeighbor2.Main.newHorizons;
            receiver.books.Add(gameObject);
        }

        public override string GetDisplayName()
        {
            return nh.GetTranslationForOtherText("$HN2ItemBook");
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            transform.localScale = Vector3.one;
            receiver.CheckBooks(gameObject, out isReturned);
            if (isReturned)
            {
                _interactable = false;
            }
        }

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            transform.localScale = Vector3.one * 0.4f;
            Locator.GetShipLogManager().RevealFact(factReveal);
        }
    }
}
