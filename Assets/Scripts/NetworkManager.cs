using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

    // Used to change gamestate
    public GameObject Mainmenu;
    public GameObject gameLobby;
    public GameObject inGame;

    // Main menu
    string registeredGameName = "ProjectWarlock_Simon_Mikkel";
    float refreshRequestLenght = 1f;
    HostData[] hostData;
    public InputField enterName;
    public Button[] btnPrefabs = new Button[10];

    // Game lobby
    public Button[] playersInLobby = new Button[8];
    private int players = 0;

    // Ingame
    public GameObject playerPrefab;


    void Awake()
    {
        // Start in main menu
        Mainmenu.SetActive(true);
        gameLobby.SetActive(false);
        inGame.SetActive(false);
        Screen.showCursor = false;
        Screen.lockCursor = false;
    }

    void Update()
    {
        // From game lobby to main menu
        if (!Network.isServer && !Network.isClient && gameLobby.activeSelf)
        {
            players = 0;
            RefreshPlayerLobby();
            Mainmenu.SetActive(true);
            gameLobby.SetActive(false);
            RefreshServerList();
        }

        // From game lobby to ingame
        if (inGame.activeSelf && gameLobby.activeSelf)
        {
            players = 0;
            RefreshPlayerLobby();
            gameLobby.SetActive(false);
            Mainmenu.SetActive(false);
            SpawnPlayer();
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }
    }

    public void StartBTN()
    {
        networkView.RPC("StartGame", RPCMode.AllBuffered);
    }

    private void RefreshPlayerLobby()
    {
        foreach (Button i in playersInLobby)
        {
            i.gameObject.SetActive(true);
            i.GetComponentInChildren<Text>().text = "";
            i.gameObject.SetActive(false);
        }
    }

    private void RefreshButtons()
    {
        for (int i = 0; i < btnPrefabs.Length; i++)
        {
            btnPrefabs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < hostData.Length; i++)
        {
            btnPrefabs[i].gameObject.SetActive(true);
            btnPrefabs[i].GetComponentInChildren<Text>().text = "Host: " + hostData[i].gameName + "         " + hostData[i].connectedPlayers + " / " + hostData[i].playerLimit;
        }
    }

    public void StartServer()
    {
        if (enterName.text != "")
        {
            Network.InitializeServer(7, 25002, !Network.HavePublicAddress());
            MasterServer.RegisterHost(registeredGameName, enterName.text);
        }
    }

    public void JoinServer(int i)
    {
        if (enterName.text != "")
        {
            Network.Connect(hostData[i]);
        }
    }

    public void RefreshServerList()
    {
        StartCoroutine("RefreshHostList");
    }

    public IEnumerator RefreshHostList()
    {
        //Debug.Log("Refreshing...");
        MasterServer.RequestHostList(registeredGameName);
        float TimeEnd = Time.time + refreshRequestLenght;

        while (Time.time < TimeEnd)
        {
            hostData = MasterServer.PollHostList();
            RefreshButtons();
            yield return new WaitForEndOfFrame();
        }
        if (hostData == null || hostData.Length == 0)
        {
            //Debug.Log("No active servers have been found.");
        }
        else
        {
            //Debug.Log(hostData.Length + " has been found.");
        }
    }

    public void OnNameChange()
    {
        InputSettings.name = enterName.text;
        //Debug.Log("Name Changed to: " + enterName.text);
    }

    public void LeaveLobby()
    {
        networkView.RPC("LeaveLobbyRPC", RPCMode.AllBuffered, InputSettings.name);
        Network.Disconnect();
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        //Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    void OnServerInitialized()
    {
        //Debug.Log("Server has been initialized" + Network.connections.Length);
        Mainmenu.SetActive(false);
        gameLobby.SetActive(true);
        networkView.RPC("JoinLobby", RPCMode.AllBuffered, enterName.text);
    }

    void OnConnectedToServer()
    {
        Mainmenu.SetActive(false);
        gameLobby.SetActive(true);
        networkView.RPC("JoinLobby", RPCMode.AllBuffered, enterName.text);
        //Debug.Log("Server joined, on team: " + Network.connections.Length);
    }

    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
        {
            //Debug.Log("Registration succesfull" + Network.connections.Length);
        }
    }

    [RPC]
    private void JoinLobby(string name)
    {
        //Debug.Log("" + players);
        playersInLobby[players].gameObject.SetActive(true);
        playersInLobby[players].GetComponentInChildren<Text>().text = name;
        players++;
    }

    [RPC]
    private void LeaveLobbyRPC(string name)
    {
        for (int i = 0; i < playersInLobby.Length; i++)
        {
            if (playersInLobby[i].GetComponentInChildren<Text>() != null && playersInLobby[i].GetComponentInChildren<Text>().text == name)
            {
                playersInLobby[i].GetComponentInChildren<Text>().text = "";
                playersInLobby[i].gameObject.SetActive(false);
	        }
        }
        players--;
    }

    [RPC]
    private void StartGame()
    {
        inGame.SetActive(true);
    }
    private void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, 0);
    }
}
