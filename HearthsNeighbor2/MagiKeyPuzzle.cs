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

        private string[] conditionsToCheck =
        {
            "MAGISTARIUM",
            "StarshipCommunity",
            "TheBoiledEgg",
            "EggStar",
            "ALTTH",
            "Gravelrock",
            "FracturedHarmony",
            "FinisPlateau",
            "ModJamHub",
            "EchoHike",
            "Axiom",
            "ProjectionSimulation"
        };

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            bool questOver = PlayerData.GetPersistentCondition("HN2_END_QUEST");
            hintDialogue.GetComponentInChildren<Animator>().SetBool("IsOnline", questOver);
            newDialogue.GetComponentInChildren<Animator>().SetBool("IsOnline", questOver);
            hintDialogue.SetActive(!questOver);
            newDialogue.SetActive(questOver);
        }

        public void PlaceKeys()
        {
            if (PlayerData.GetPersistentCondition("HN2_END_QUEST")) return;
            KeyPlacer.PlaceKeys(magiKeyPrefab);
        }

        public void CompletePuzzle()
        {
            HearthsNeighbor2.Main.ModHelper.Console.WriteLine("HN0 puzzle complete! Check the Magister's Dorm.");
            PlayerData.SetPersistentCondition("HN2_END_QUEST", true);
            hintDialogue.SetActive(false);
            newDialogue.SetActive(true);
        }

        public void CheckPuzzle()
        {
            HearthsNeighbor2.Main.ModHelper.Console.WriteLine("Checking HN0 puzzle...");
            foreach (string condition in conditionsToCheck)
            {
                HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Checking {"HN2_KEY_" + condition}...");
                if (!PlayerData.GetPersistentCondition("HN2_KEY_" + condition)) return;
            }
            CompletePuzzle();
        }
    }
}
