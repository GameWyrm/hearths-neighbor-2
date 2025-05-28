using UnityEngine;

namespace HearthsNeighbor2
{
    public class MagiKeyPuzzle : MonoBehaviour
    {
        public static MagiKeyPuzzle Instance;

        public GameObject hintDialogue;
        public GameObject newDialogue;
        public Transform MagiCube;
        public GameObject magiKeyPrefab;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            hintDialogue.GetComponentInChildren<Animator>().SetBool("IsOnline", false);
            newDialogue.GetComponentInChildren<Animator>().SetBool("IsOnline", true);
            newDialogue.SetActive(false);
        }

        public void PlaceKeys()
        {
            KeyPlacer.PlaceKeys(magiKeyPrefab);
        }

        public void CompletePuzzle()
        {
            hintDialogue.SetActive(false);
            newDialogue.SetActive(true);
        }
    }
}
