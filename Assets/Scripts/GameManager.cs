using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //making variable for turn 
    public NetworkVariable<int> _currentTurn = new NetworkVariable<int>(0);

    public static GameManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null && Instance != this)
        {
            /*Destroy(gameObject);*/
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            Debug.Log(clientId + "joined");
            if(NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClients.Count == 2)
            {
                SpwanBoard();
            }
        }; 
    }
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    [SerializeField] private GameObject BoardPrefab;
    private void SpwanBoard()
    {
        GameObject newBoard = Instantiate(BoardPrefab);
        newBoard.GetComponent<NetworkObject>().Spawn();
    }
}
