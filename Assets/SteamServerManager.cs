using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamServerManager : MonoBehaviour {

	static public SteamServerManager _instance;

    const string WIZVR_SERVER_VERSION = "1.0.0.0";
    const ushort WIZVR_AUTHENTICATION_PORT = 8766;
    const ushort WIZVR_SERVER_PORT = 27015;
    const ushort WIZVR_MASTER_SERVER_UPDATER_PORT = 27016;
    
    EServerMode eMode;
    bool gs_Initialized = false;
    public bool gs_ConnectedToSteam = false;

    protected Callback<SteamServersConnected_t> Callback_ServerConnected;
	protected Callback<SteamServersDisconnected_t> Callback_ServerDisconnected;
	protected Callback<SteamServerConnectFailure_t> Callback_ServerConnectFailure;
	protected Callback<GSPolicyResponse_t> m_CallbackPolicyResponse;

	void Start(){
		_instance = this;
        Callback_ServerConnected = Callback<SteamServersConnected_t>.CreateGameServer(OnSteamServerConnected);
        Callback_ServerDisconnected = Callback<SteamServersDisconnected_t>.CreateGameServer(OnSteamServerDisconnected);
		Callback_ServerConnectFailure = Callback<SteamServerConnectFailure_t>.CreateGameServer(OnSteamServersConnectFailure);
	}

    public void CreateServer(){
        gs_Initialized = GameServer.Init(0, WIZVR_AUTHENTICATION_PORT, WIZVR_SERVER_PORT, WIZVR_MASTER_SERVER_UPDATER_PORT, eMode, WIZVR_SERVER_VERSION);
		if (!gs_Initialized) {
			Debug.Log("SteamGameServer_Init call failed");
			return;
		}
		SteamGameServer.SetModDir("wizvr");
		// SteamGameServer.SetProduct("WizVR Session");
		// SteamGameServer.SetGameDescription("WizVR Server");
        SteamGameServer.LogOnAnonymous();
        Debug.Log("Started.");
    }

	void OnSteamServerConnected(SteamServersConnected_t pLogonSuccess) {
		Debug.Log("WizVR connected to Steam successfully");
		gs_ConnectedToSteam = true;
        SteamAPICall_t try_toHost = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
	}

	void OnSteamServerDisconnected(SteamServersDisconnected_t pLoggedOff) {
		gs_ConnectedToSteam = false;
		Debug.Log("WizVR got logged out of Steam");
	}

	void OnSteamServersConnectFailure(SteamServerConnectFailure_t pConnectFailure) {
		gs_ConnectedToSteam = false;
		Debug.Log("WizVR failed to connect to Steam");
	}

    void OnDisable(){
		if(gs_Initialized || gs_ConnectedToSteam){
		SteamGameServer.LogOff();
		GameServer.Shutdown();
		Debug.Log("Shutdown.");
		}
	}

    void Update(){
        if(!gs_Initialized) {
			return;
		}

		GameServer.RunCallbacks();
    }
}
