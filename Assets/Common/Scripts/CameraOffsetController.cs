using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    [GenerateAuthoringComponent]
    public class CameraOffsetController : MonoBehaviour
    {
        public GameObject Camera;
        public GameObject Player;

        public Vector3 Offset;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void LateUpdate()
        {
            var camPos = this.Camera.transform.position;
            var offsetVal = this.Player.transform.position + this.Offset;
            this.Camera.transform.position = new Vector3(offsetVal.x, camPos.y, offsetVal.z);
        }
    }
}
