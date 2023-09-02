
using UnityEngine;

namespace Fusion114
{
    public class FirstPersonCamera : MonoBehaviour
    {
        public Transform target;
        public float mouseSensitivity = 2f;

        private float _verticalRotation;
        private float _horizontalRotation;

        private void LateUpdate()
        {
            if (ReferenceEquals(target, null))
            {
                return;
            }

            transform.position = target.position;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            _verticalRotation -= mouseY * mouseSensitivity;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -70f, 70f);

            _horizontalRotation += mouseX * mouseSensitivity;

            transform.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
        }
    }
}

