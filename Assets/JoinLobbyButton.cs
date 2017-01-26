using UnityEngine;
using UnityEngine.UI;
using Steamworks;
public class JoinLobbyButton : MonoBehaviour {
	public CSteamID joinID;
	Button joinBtn;

	void Start(){
		joinBtn = this.GetComponent<Button>();
		joinBtn.onClick.AddListener(JoinLobby);
	}
	void JoinLobby(){
		SteamAPICall_t try_joinLobby = SteamMatchmaking.JoinLobby(joinID);
	}
}
