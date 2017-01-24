using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class testlobby : MonoBehaviour {

public NetworkLobbyManager lobby;

	void Start () {
		lobby.StartHost();
		// lobby.ServerChangeScene(lobby.playScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
