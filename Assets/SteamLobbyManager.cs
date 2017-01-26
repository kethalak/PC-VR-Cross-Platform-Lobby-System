using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks;
using TMPro;

public class SteamLobbyManager : NetworkLobbyManager {

    static public SteamLobbyManager _instance;

    private Callback<LobbyCreated_t> Callback_lobbyCreated;
    private Callback<LobbyMatchList_t> Callback_lobbyList;
    private Callback<LobbyEnter_t> Callback_lobbyEnter;
    private Callback<LobbyDataUpdate_t> Callback_lobbyInfo;
    
    [HideInInspector]
    public ulong current_lobbyID;
    List<CSteamID> lobbyIDS;


    bool isHost = false;

    [Header("UI Reference")]
    public TextMeshProUGUI awaitMsg;
    public Button backButton;

    public RectTransform mainMenuPanel;
    public RectTransform lobbyPanel;
    public RectTransform lobbyListPanel;

    public GameObject lobby;
    public GameObject lobbyJoin;
    List<GameObject> lobbies = new List<GameObject>();

    public float prematchCountdown = 5.0f;

    protected RectTransform currentPanel;
    
	TextMeshProUGUI userText;
	Image userImage;
	int userInt;
	uint width, height;
	Texture2D downloadedAvatar;
	Rect rect = new Rect(0, 0, 184, 184);
	Vector2 pivot = new Vector2(0.5f, 0.5f);

	void Start(){
        _instance = this;
        currentPanel = mainMenuPanel;

		lobbyIDS = new List<CSteamID>();
        backButton.gameObject.SetActive(false);

        Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
        Callback_lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        Callback_lobbyInfo = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyInfo);

        if (SteamAPI.Init())
            Debug.Log("Steam API init -- SUCCESS!");
        else
            Debug.Log("Steam API init -- failure ...");
	}
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            ChangeTo(lobbyPanel);
            if (conn.playerControllers[0].unetView.isClient)
                backDelegate = StopHostClbk;
            else
                backDelegate = StopClientClbk;   
        }
        else
        {
            ChangeTo(null);
        }
    }

    public void ChangeTo(RectTransform newPanel)
    {
        if (currentPanel != null)
            currentPanel.gameObject.SetActive(false);

        if (newPanel != null)
            newPanel.gameObject.SetActive(true);
        else{
            lobbyPanel.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            ToggleAwaitCallbackMsg();
            return;
        }
        currentPanel = newPanel;

        if (currentPanel != mainMenuPanel)
            backButton.gameObject.SetActive(true);

        else
            backButton.gameObject.SetActive(false);

    }

        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        public void GoBackButton()
        {
            backDelegate();
        }

        public void SimpleBackClbk()
        {
            ChangeTo(mainMenuPanel);
        }

        public void StopViewLobbiesClbk()
        {
            foreach(GameObject lobby in lobbies)
            Destroy(lobby);
            lobbies.Clear();
            ChangeTo(mainMenuPanel);
        }
                 
        public void StopHostClbk()
        {
            isHost = false;
            SteamMatchmaking.LeaveLobby((CSteamID)current_lobbyID);
            StopHost();
            ChangeTo(mainMenuPanel);
            SteamGameServer.LogOff();
            GameServer.Shutdown();
            Debug.Log("Shutdown.");
        }

        public void StopClientClbk()
        {
            SteamMatchmaking.LeaveLobby((CSteamID)current_lobbyID);
            StopClient();
            ChangeTo(mainMenuPanel);
        }

    void RefreshLobbies(){
        for(int i = 0; i < lobbyIDS.Count; i++){
            int newPos = 0;
            lobbies.Add(Instantiate(lobby, Vector3.zero, lobby.transform.rotation));
            TextMeshProUGUI[] texts = lobbies[i].GetComponentsInChildren<TextMeshProUGUI>();
                
            foreach(TextMeshProUGUI textMesh in texts)
            {
                if(textMesh.gameObject.transform.parent != null)
                    textMesh.text = SteamMatchmaking.GetNumLobbyMembers(lobbyIDS[i]).ToString() + "/4";
                else
                    textMesh.text = SteamMatchmaking.GetLobbyData(lobbyIDS[i], "name");
            }

            GameObject joinButton = Instantiate(lobbyJoin, Vector3.zero, lobbyJoin.transform.rotation);
            lobbies[i].transform.SetParent(this.transform);
            joinButton.transform.SetParent(lobbies[i].transform);
            lobbies[i].transform.localPosition = new Vector3(-70,90+newPos,0);
            joinButton.transform.localPosition = new Vector3(470,0,0);
            joinButton.GetComponent<JoinLobbyButton>().joinID = lobbyIDS[i];
        }
    }

    public void ToggleAwaitCallbackMsg(string msg = ""){
        awaitMsg.text = msg;
        awaitMsg.gameObject.SetActive(!awaitMsg.gameObject.activeSelf);
    }

    void StartGame(){
        ServerChangeScene(playScene);
    }

	void Update()
	{
        SteamAPI.RunCallbacks();
        // Command - List lobby members
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID);

            Debug.Log("\t Number of players currently in lobby : " + numPlayers);
            for (int i = 0; i < numPlayers; i++)
            {
                Debug.Log("\t Player(" + i + ") == " + SteamFriends.GetFriendPersonaName(SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i)));
                Debug.Log("\t Player(" + i + ") == " + SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i));
            }
        }
    }

    void OnLobbyCreated(LobbyCreated_t result) 
    {
        isHost = true;
        StartHost();
        backDelegate = StopHostClbk;
        ToggleAwaitCallbackMsg();
        if (result.m_eResult == EResult.k_EResultOK)
            Debug.Log("Lobby created -- SUCCESS!");
        else{
            Debug.Log("Lobby created -- failure ...");
            return;
        }

        string gameName = SteamFriends.GetPersonaName() + "'s game";

        uint serverIp = SteamGameServer.GetPublicIP();
        int ipaddr = System.Net.IPAddress.HostToNetworkOrder((int)serverIp);
        string ip = new System.Net.IPAddress(BitConverter.GetBytes(ipaddr)).ToString();
        Debug.Log(ip);
        SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "ServerIP", ip);
        SteamMatchmaking.SetLobbyData((CSteamID)result.m_ulSteamIDLobby, "name", gameName);
    }

    void OnGetLobbiesList(LobbyMatchList_t result)
    {
        backDelegate = StopViewLobbiesClbk;
        ToggleAwaitCallbackMsg();
        lobbyIDS.Clear();
        for(int i=0; i< result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDS.Add(lobbyID);
        }
            RefreshLobbies();
    }

    void OnGetLobbyInfo(LobbyDataUpdate_t result)
    {
       Debug.Log("Something Happened");
    }

    void OnLobbyEntered(LobbyEnter_t result)
    {
        if(!isHost){
            backDelegate = StopClientClbk;
            networkAddress = SteamMatchmaking.GetLobbyData((CSteamID)result.m_ulSteamIDLobby, "ServerIP");
            Debug.Log(networkAddress);
            StartClient();
        }
        foreach(GameObject lobby in lobbies)
        Destroy(lobby);
        lobbies.Clear();
        current_lobbyID = result.m_ulSteamIDLobby;

        // lobbyHeader.text = SteamMatchmaking.GetLobbyData((CSteamID)current_lobbyID, "name");

        // if (result.m_EChatRoomEnterResponse == 1){
        //     int numPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID);
        //     for (int i = 0; i < numPlayers; i++)
        //     {   
        //         StartCoroutine(FetchAvatar(SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)current_lobbyID, i)));
        //     }
        // }
        // else
        //     Debug.Log("Failed to join lobby.");
    }

    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    public override void OnLobbyServerPlayersReady()
    {
        StartCoroutine(ServerCountdownCoroutine());
    }
       public IEnumerator ServerCountdownCoroutine()
        {
            ToggleAwaitCallbackMsg();
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (lobbySlots[i] as SteamLobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as SteamLobbyPlayer).RpcUpdateCountdown(0);
                }
            }
            
            ServerChangeScene(playScene);
        }

}
