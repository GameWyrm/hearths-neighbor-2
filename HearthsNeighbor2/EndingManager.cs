using UnityEngine;

namespace HearthsNeighbor2
{
    public class EndingManager : MonoBehaviour
    {
        public GameObject magister;
        public GameObject[] otherNpcs;
        public GameObject deathCube;
        public GameObject music;


        private void Start()
        {
            GlobalMessenger<string, bool>.AddListener("DialogueConditionChanged", OnDialogueConditionChanged);
            HearthsNeighbor2.Main.ModHelper.Console.WriteLine("Subbed!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (Animator anim in magister.GetComponentsInChildren<Animator>())
                {
                    anim.SetTrigger("Open");
                }
                music.SetActive(true);
            }
        }

        private void OnDialogueConditionChanged(string condition, bool state)
        {
            if (condition == "HN2_ENDMAGISTER" && state == true) SetUpFinish();
            if (condition == "HN2_REDUCEDFRIGHT")
            {
                HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Reduced Fright {(state ? "ENABLED" : "DISABLED")}");
                PlayerData.SetPersistentCondition("HN2_P_REDUCEDFRIGHT", state);
            }
        }

        // happens after talking to Magister
        public void SetUpFinish()
        {
            foreach (GameObject go in otherNpcs)
            {
                if (go == null) continue;
                go.SetActive(true);
            }
            deathCube.SetActive(true);
        }
    }
}
