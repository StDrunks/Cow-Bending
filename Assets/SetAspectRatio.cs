using UnityEngine;

public class SetAspectRatio : MonoBehaviour
{
        void Start()
        {
                SetRatio();
        }
        public void SetRatio()
        {
                GetComponent<Camera>().aspect = 1;
        }
}
