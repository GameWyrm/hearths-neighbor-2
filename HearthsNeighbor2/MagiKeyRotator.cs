using UnityEngine;

namespace HearthsNeighbor2
{
    public class MagiKeyRotator : MonoBehaviour
    {
        private Transform child;
        private float speed;

        private void Start ()
        {
            transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            speed = Random.Range(60f, 180f);
            child = transform.GetChild(0);
        }

        private void Update()
        {
            float rot = child.localEulerAngles.y;
            child.localEulerAngles = new Vector3(0, rot + (Time.deltaTime * 120), 0);
        }
    }
}
