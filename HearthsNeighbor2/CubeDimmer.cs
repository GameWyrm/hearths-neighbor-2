using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class CubeDimmer : MonoBehaviour
    {
        public MeshRenderer mesh;

        private SingleInteractionVolume _interactVolume;
        private bool hasBeenRead;

        private void Awake()
        {
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += (() =>
            {
                if (!hasBeenRead) GlobalMessenger.AddListener("ExitConversation", () =>
                {
                    if (hasBeenRead) return;
                    mesh.materials[1].color = Color.black;
                    mesh.transform.GetChild(0).gameObject.SetActive(false);

                    hasBeenRead = true;
                });
            });
        }
    }
}
