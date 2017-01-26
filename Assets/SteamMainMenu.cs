using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

public class SteamMainMenu : MonoBehaviour {

	public SteamLobbyManager steamLobbyManager;

	public RectTransform lobbyPanel;
	public RectTransform lobbyListPanel;

	void OnEnable(){

	}

	public void OnClickCreateLobby(){
		SteamLobbyManager._instance.ChangeTo(lobbyPanel);
		SteamLobbyManager._instance.ToggleAwaitCallbackMsg("creating lobby...");
		SteamServerManager._instance.CreateServer();
	}

	public void OnClickViewLobbies(){
		SteamLobbyManager._instance.ChangeTo(lobbyListPanel);
		SteamLobbyManager._instance.ToggleAwaitCallbackMsg("finding available lobbies...");
		SteamAPICall_t try_getList = SteamMatchmaking.RequestLobbyList();
	}

	public void OnClickQuit(){
		Application.Quit();
	}

	public void OnClickSwitchDevice(){
	SceneManager.LoadScene("StartUp");
	}
}
