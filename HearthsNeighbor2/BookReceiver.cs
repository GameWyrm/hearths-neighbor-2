using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor2
{
    public class BookReceiver : MonoBehaviour
    {
        public GameObject door;
        public List<GameObject> books;
        public float maxDistance = 3;
        public Color[] colorStages;

        private bool isOpen;
        private bool opening;
        private int bookCount = 0;
        private MeshRenderer rend;
        private AudioSource aud;

        private void Start()
        {
            rend = GetComponent<MeshRenderer>();
            aud = GetComponent<AudioSource>();
        }

        public void CheckBooks(GameObject obj, out bool disableSender)
        {
            disableSender = Vector3.Distance(transform.position, obj.transform.position) < maxDistance;
            if (!disableSender) return;
            bookCount++;
            aud.Play();
            rend.materials[1].color = colorStages[bookCount - 1];
            foreach (GameObject book in books)
            {
                if (Vector3.Distance(transform.position, book.transform.position) > maxDistance) return;
            }

            opening = true;
        }

        private void Update()
        {
            if (!isOpen && opening)
            {
                door.transform.localScale -= Vector3.one * Time.deltaTime;
                if (door.transform.localScale.x <= 0)
                {
                    door.SetActive(false);
                    opening = false;
                    isOpen = true;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}
