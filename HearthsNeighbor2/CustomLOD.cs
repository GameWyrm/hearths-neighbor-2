using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class CustomLOD : MonoBehaviour
    {
        public Sector sector;

        private MeshRenderer rend;
        private bool mainPlanetLoaded;
        private List<GameObject> spriteObjects;

        private void Awake()
        {
            spriteObjects = new List<GameObject>();
            GameObject magistration = transform.parent.Find("Magistration").gameObject;
            foreach (SpriteRenderer sprite in  magistration.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteObjects.Add(sprite.gameObject);
            }
        }

        private void Start()
        {
            rend = GetComponent<MeshRenderer>();
            sector = GetComponentInParent<Sector>();
            sector.OnSectorOccupantsUpdated += () => CheckStatus(sector.GetOccupants().Count);
        }

        private void CheckStatus(int occupants)
        {
            if (occupants <= 0)
            {
                rend.enabled = true;
                mainPlanetLoaded = false;
            }
            else if (!mainPlanetLoaded)
            {
                mainPlanetLoaded = true;
                StartCoroutine(HideLODPlanet());
            }
        }

        // Hides the LOD version, and also fixes an issue where sprite renderers render incorrectly when loaded in via the sector
        IEnumerator HideLODPlanet()
        {
            yield return new WaitForSeconds(2);
            if (mainPlanetLoaded)
            {
                rend.enabled = false;
                foreach (GameObject sprite in spriteObjects)
                {
                    sprite.SetActive(false);
                }
                yield return new WaitForSeconds(0.1f);
                foreach (GameObject sprite in spriteObjects)
                {
                    sprite.SetActive(true);
                }
            }
        }
    }
}
