using UnityEngine;
using Unity.Netcode;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : NetworkBehaviour
{
    public GameObject gameOverUI;  // Reference to the Game Over UI

    void Start()
    {
        // Ensure gameOverUI is initially disabled
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void NotifyGameOverServerRpc(ulong loserClientId)
    {
        Debug.Log($"NotifyGameOverServerRpc called for Client ID: {loserClientId}");

        // Trigger the game over for the losing client
        GameOverClientRpc(loserClientId, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { loserClientId }
            }
        });
    }

    [ClientRpc]
    private void GameOverClientRpc(ulong loserClientId, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log($"GameOverClientRpc called for Client ID: {loserClientId}");
        Debug.Log($"Local Client ID: {NetworkManager.Singleton.LocalClientId}");

        if (NetworkManager.Singleton.LocalClientId == loserClientId)
        {
            Debug.Log($"Game Over for Local Client ID: {loserClientId}");
            // Display the Game Over UI
            ShowGameOverUI();
        }
        else
        {
            Debug.Log("Client ID does not match, no Game Over for this client.");
        }
    }

    private void ShowGameOverUI()
    {
        Debug.Log("Showing Game Over UI");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);  // Make the Game Over screen visible
            StartCoroutine(WaitForExit());
        }
    }

    private IEnumerator WaitForExit()
    {
        Debug.Log("Waiting for exit input...");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        StopGame();
    }

    private void StopGame()
    {
        Debug.Log("Stopping the game...");

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;  // Stop play mode in the Unity Editor
#else
        Application.Quit();  // Quit the application if it's a build
#endif
    }
}