using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Blaster.Player.Core 
{
    public class LookCamera : NetworkBehaviour
    {
        [SerializeField] private Transform verticalCamera = null;
        [SerializeField] private Transform horizontalCamera = null;
        [SerializeField] private Transform cameraSocket = null;
        private Vector2 currentLook = Vector2.zero;

        public override void OnNetworkSpawn() {
            if (!IsOwner) {
                Destroy(cameraSocket.GetComponentInChildren<Camera>().gameObject);
            }
        }

        public Quaternion ApplyLook(Vector2 lookInput) {
            currentLook += lookInput;

            currentLook.y = Mathf.Clamp(currentLook.y, -90f, 90f);

            cameraSocket.transform.rotation = Quaternion.Euler(-currentLook.y, currentLook.x, 0f);
            return Quaternion.Euler(0f, currentLook.x, 0f);
        }

        public void DOVerticalOffSet(float offset) {
            verticalCamera.transform.Rotate(new Vector3(offset, 0f, 0f));
        }

        public void DOHorizontalOffSet(float offset) {
            horizontalCamera.transform.Rotate(new Vector3(0f, offset, 0f));
        }

        public Vector3 GetForwardVector() {
            return Quaternion.Euler(0f, currentLook.x, 0f) * Vector3.forward;
        }
    }
}
