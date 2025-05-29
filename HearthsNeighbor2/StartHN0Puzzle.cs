using UnityEngine;

namespace HearthsNeighbor2
{
    public class StartHN0Puzzle : MonoBehaviour
    {
        private SingleInteractionVolume _interactVolume;
        private string[] rumors =
        {

        };

        private void Start()
        {
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += OnPressInteract;
        }

        private void OnPressInteract()
        {
            if (PlayerData.GetPersistentCondition("HN2_START_QUEST")) return;
            PlayerData.SetPersistentCondition("HN2_START_QUEST", true);
            MagiKeyPuzzle.Instance.PlaceKeys();
        }
    }
}
