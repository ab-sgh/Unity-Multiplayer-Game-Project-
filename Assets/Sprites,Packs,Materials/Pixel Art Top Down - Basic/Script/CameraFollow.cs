using UnityEngine;
using Unity.Netcode;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 5.0f; // Adjust the speed as needed

        private void Start()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            }
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            // Smoothly interpolate the camera's position towards the player's position
            Vector3 targetPos = target.position;
            targetPos.z = transform.position.z; // Preserve the camera's z position

            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            Debug.Log($"Target set to: {target.name}");
        }

        private void OnClientConnected(ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId == clientId)
            {
                FindLocalPlayer();
            }
        }

        private void FindLocalPlayer()
        {
            foreach (var player in FindObjectsOfType<NetworkBehaviour>())
            {
                if (player.IsLocalPlayer)
                {
                    SetTarget(player.transform);
                    break;
                }
            }
        }
    }
}
