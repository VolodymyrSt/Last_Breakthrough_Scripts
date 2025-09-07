using UnityEngine;

namespace LastBreakthrought.Util
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;

        private void OnEnable() => _camera = Camera.main;

        private void Update()
        {
            Vector3 direction = _camera.transform.position - transform.position;
            direction.y = 0; 
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
