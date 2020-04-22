using UnityEngine;

namespace Assets.Scripts
{
    public class FinishScript : MonoBehaviour
    {
        public GameObject Finish;

        void Awake()
        {
            var tempPos = this.transform.position;
            tempPos.y = 0.5001f;
            Instantiate(Finish, tempPos, Quaternion.Euler(0, 180, 0));
        }

        private void OnTriggerEnter(Collider other)
        {
            GameManager.Instance.FinishLevel();
        }
    }
}
