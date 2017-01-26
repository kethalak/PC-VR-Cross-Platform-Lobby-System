using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Steamworks;
using TMPro;

public class SteamLobbyPlayer : NetworkLobbyPlayer {

	public Button readyButton;

	// [SyncVar(hook = "OnGetAvatar")]
	// public Sprite avatarSprite;
	// [SyncVar(hook = "OnGetName")]
	// public string playerNameString = "";

	public Image playerAvatar;
	public TextMeshProUGUI playerName;

	int userInt;
	uint width, height;
	Texture2D downloadedAvatar;
	Rect rect = new Rect(0, 0, 64, 64);
	Vector2 pivot = new Vector2(0.5f, 0.5f);

	static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
	static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
	static Color JoinColor = new Color(255.0f/255.0f, 0.0f, 101.0f/255.0f,1.0f);
	
	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();
		StartCoroutine(GetSteamInfo());
		SteamLobbyPlayerList._instance.AddPlayer(this);

		if (isLocalPlayer)
		{
			SetupLocalPlayer();
		}
		else
		{
			SetupOtherPlayer();
		}
	}

	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		SetupLocalPlayer();
	}
	
	void SetupOtherPlayer()
	{
		// ChangeReadyButtonColor(NotReadyColor);

		readyButton.GetComponent<TextMeshProUGUI>().text = "...";
		readyButton.interactable = false;

		OnClientReady(false);
	}

	void SetupLocalPlayer()
	{
		CheckRemoveButton();

		// ChangeReadyButtonColor(JoinColor);

		readyButton.GetComponent<TextMeshProUGUI>().text = "READY";
		readyButton.interactable = true;

		readyButton.onClick.RemoveAllListeners();
		readyButton.onClick.AddListener(OnReadyClicked);
	}

	// void ChangeReadyButtonColor(Color c)
	// {
	// 	ColorBlock b = readyButton.colors;
	// 	b.normalColor = c;
	// 	b.pressedColor = c;
	// 	b.highlightedColor = c;
	// 	b.disabledColor = c;
	// 	readyButton.colors = b;
	// }

	public void CheckRemoveButton()
	{
		if (!isLocalPlayer)
			return;

		int localPlayerCount = 0;
		foreach (PlayerController p in ClientScene.localPlayers)
			localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;
	}

	IEnumerator GetSteamInfo()
	{
		CSteamID id = SteamUser.GetSteamID();
		userInt = SteamFriends.GetMediumFriendAvatar(id);
		// playerName.text = SteamFriends.GetPersonaName();
		CmdNameChanged(SteamFriends.GetPersonaName());

		while(userInt == -1){
			yield return null;
		}
		if(userInt > 0){
			SteamUtils.GetImageSize(userInt, out width, out height);
		}
		if(width > 0 && height > 0){
			byte[] avatarStream = new byte[4 * (int)width * (int)height];
			SteamUtils.GetImageRGBA(userInt, avatarStream, 4 * (int)width * (int)height);
			downloadedAvatar = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
			downloadedAvatar.LoadRawTextureData(avatarStream);
			downloadedAvatar.Apply();

			playerAvatar.sprite = Sprite.Create(downloadedAvatar, rect, pivot);
		}
	}

	[Command]
	public void CmdNameChanged(string name)
	{
		playerName.text = name;
	}

	public void OnReadyClicked()
	{
		ColorBlock b = readyButton.colors;
		b.normalColor = Color.green;
		SendReadyToBeginMessage();
	}

	[ClientRpc]
	public void RpcUpdateCountdown(int countdown)
	{
		SteamLobbyManager._instance.awaitMsg.text = "Match Starting in " + countdown;
	}
}
