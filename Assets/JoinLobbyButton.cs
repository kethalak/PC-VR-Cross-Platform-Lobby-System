using UnityEngine;
using UnityEngine.UI;
using Steamworks;
public class JoinLobbyButton : MonoBehaviour {
	public CSteamID joinID;
	Button joinButton;

	void Awake(){
		joinButton = GetComponent<Button>();
	}
	void Start(){
		joinButton.onClick.AddListener(JoinLobby);
	}
	void JoinLobby(){
		SteamAPICall_t try_joinLobby = SteamMatchmaking.JoinLobby(joinID);
	}
}
