using UnityEngine;

namespace HearthsNeighbor2
{
    public class MagiKeyInteraction : MonoBehaviour
    {
        public string conditionKey;
        public string shipLogEntry;

        private SingleInteractionVolume _interactVolume;
        private float frontPoint = -1f;
        private float backPoint = -1f;
        private float speed;

        [SerializeField]
        private LineRenderer line;

        private void Awake()
        {
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += OnPressInteract;
        }

        private void Start()
        {
            _interactVolume.ChangePrompt(HearthsNeighbor2.Main.newHorizons.GetTranslationForOtherText("$HN2InteractKey"));
        }

        private void OnPressInteract()
        {
            _interactVolume.DisableInteraction();
            GetComponent<Animator>().SetTrigger("Grab");
            if (!string.IsNullOrEmpty(conditionKey))
            {
                HearthsNeighbor2.Main.ModHelper.Console.WriteLine($"Key collected, saving Persistent Condition {conditionKey}");
                PlayerData.SetPersistentCondition(conditionKey, true);
                MagiKeyPuzzle.Instance.CheckPuzzle();
                Locator.GetShipLogManager().RevealFact(shipLogEntry);
            }
        }

        private void Update()
        {
            Vector3 targetPoint = Vector3.zero;
            if (frontPoint > 0f)
            {
                targetPoint = Vector3.Lerp(MagiKeyPuzzle.Instance.MagiCube.position, transform.position, frontPoint);
                frontPoint -= speed * Time.deltaTime;
                if (frontPoint < 0f) frontPoint = 0f;
                line.SetPosition(0, targetPoint);
            }
            if (backPoint > 0f)
            {
                targetPoint = Vector3.Lerp(MagiKeyPuzzle.Instance.MagiCube.position, transform.position, backPoint);
                backPoint -= speed * Time.deltaTime;
                if (backPoint < 0f) backPoint = 0f;
                line.SetPosition(1, targetPoint);
            }
        }

        public void StartLine(float speed)
        {
            frontPoint = 1f;
            this.speed = 1f / speed;
        }

        public void EndLine(float speed)
        {
            backPoint = 1f;
            this.speed = 1f / speed;
        }

        public void DisableKey()
        {
            gameObject.SetActive(false);
        }
    }
}
