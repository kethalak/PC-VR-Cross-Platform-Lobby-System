using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamLobbyPlayerList : MonoBehaviour {

	public static SteamLobbyPlayerList _instance = null;
	public RectTransform playerListContentTransform;

	protected VerticalLayoutGroup _layout;
	public List<SteamLobbyPlayer> _players = new List<SteamLobbyPlayer>();


	public void OnEnable()
	{
		_instance = this;
		_layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
	}

	void Update () {
		if(_layout)
			_layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
	}

	public void AddPlayer(SteamLobbyPlayer player)
	{
		_players.Add(player);
		player.transform.SetParent(playerListContentTransform, false);

		// PlayerListModified();
	}

	public void RemovePlayer(SteamLobbyPlayer player)
	{
		_players.Remove(player);
		// PlayerListModified();
	}

	// public void PlayerListModified()
	// {
	// 	int i = 0;
	// 	foreach (LobbyPlayer p in _players)
	// 	{
	// 		p.OnPlayerListChanged(i);
	// 		++i;
	// 	}
	// }
}
