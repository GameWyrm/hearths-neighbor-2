using UnityEngine;

namespace HearthsNeighbor2
{
    public class EndingManager : MonoBehaviour
    {
        public GameObject magister;
        public GameObject[] otherNpcs;
        public GameObject deathCube;


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
            }
        }

        private void OnDialogueConditionChanged(string condition, bool state)
        {
            if (condition == "HN2_ENDMAGISTER" && state == true) SetUpFinish();
            if (condition == "HN2_REDUCEDFRIGHT") PlayerData.SetPersistentCondition("HN2_P_REDUCEDFRIGHT", state);
        }

        // happens after talking to Magister
        public void SetUpFinish()
        {
            foreach (GameObject go in otherNpcs)
            {
                go.SetActive(true);
                foreach (Animator anim in go.GetComponentsInChildren<Animator>())
                {
                    anim.SetTrigger("Open");
                }
            }
            deathCube.SetActive(true);
        }
    }
}
