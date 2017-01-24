using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Collections.Generic;

public class CrazyNetworkManager : NetworkManager 
{
    public Button startHostBtn;
    public Button startClientBtn;

    void Start(){
        startHostBtn.onClick.AddListener(OnHostClick);
        startClientBtn.onClick.AddListener(OnClientClick);
    }

    void OnHostClick(){
        StartHost();
        Destroy(startHostBtn.gameObject);
        Destroy(startClientBtn.gameObject);
    }
    
    void OnClientClick(){
        networkAddress = "98.245.85.134";
        StartClient();
        Destroy(startHostBtn.gameObject);
        Destroy(startClientBtn.gameObject);

    }

    //
    // Summary:
    //     Called on the client when connected to a server.
    //
    // Parameters:
    //   conn:
    //     Connection to the server.
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log(string.Format("OnClientConnect : {0}", conn));
    }
    
    //
    // Summary:
    //     Called on clients when disconnected from a server.
    //
    // Parameters:
    //   conn:
    //     Connection to the server.
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log(string.Format("OnClientDisconnect : {0}", conn));
    }
    
    //
    // Summary:
    //     Called on clients when a network error occurs.
    //
    // Parameters:
    //   conn:
    //     Connection to a server.
    //
    //   errorCode:
    //     Error code.
    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        Debug.Log(string.Format("OnClientError : {0} : {1}", conn, errorCode));
    }
    
    //
    // Summary:
    //     Called on clients when a servers tells the client it is no longer ready.
    //
    // Parameters:
    //   conn:
    //     Connection to a server.
    public override void OnClientNotReady(NetworkConnection conn)
    {
        base.OnClientNotReady(conn);
        Debug.Log(string.Format("OnClientNotReady : {0}", conn));
    }
    
    //
    // Summary:
    //     Called on clients when a scene has completed loaded, when the scene load was
    //     initiated by the server.
    //
    // Parameters:
    //   conn:
    //     The network connection that the scene change message arrived on.
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        Debug.Log(string.Format("OnClientSceneChanged : {0}", conn));
    }


    //
    // Summary:
    //     Callback that happens when a NetworkMatch.DestroyMatch request has been processed
    //     on the server.
    //
    // Parameters:
    //   success:
    //     Indicates if the request succeeded.
    //
    //   extendedInfo:
    //     A text description for the error if success is false.
    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        base.OnDestroyMatch(success, extendedInfo);
        Debug.Log(string.Format("OnDestroyMatch : {0} : {1}", success, extendedInfo));
    }
    
    //
    // Summary:
    //     Callback that happens when a NetworkMatch.DropConnection match request has been
    //     processed on the server.
    //
    // Parameters:
    //   success:
    //     Indicates if the request succeeded.
    //
    //   extendedInfo:
    //     A text description for the error if success is false.
    public override void OnDropConnection(bool success, string extendedInfo)
    {
        base.OnDropConnection(success, extendedInfo);
        Debug.Log(string.Format("OnDropConnection : {0} : {1}", success, extendedInfo));
    }
    
    //
    // Summary:
    //     Callback that happens when a NetworkMatch.CreateMatch request has been processed
    //     on the server.
    //
    // Parameters:
    //   success:
    //     Indicates if the request succeeded.
    //
    //   extendedInfo:
    //     A text description for the error if success is false.
    //
    //   matchInfo:
    //     The information about the newly created match.
    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        Debug.Log(string.Format("OnMatchCreate : {0} : {1} : {2}", success, extendedInfo, matchInfo));
    }

    //
    // Summary:
    //     Callback that happens when a NetworkMatch.JoinMatch request has been processed
    //     on the server.
    //
    // Parameters:
    //   success:
    //     Indicates if the request succeeded.
    //
    //   extendedInfo:
    //     A text description for the error if success is false.
    //
    //   matchInfo:
    //     The info for the newly joined match.
    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        Debug.Log(string.Format("OnMatchJoined : {0} : {1} : {2}", success, extendedInfo, matchInfo));
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);
        Debug.Log(string.Format("OnMatchList : {0} : {1} : {2}", success, extendedInfo, matchList));
    }
    
    //
    // Summary:
    //     Called on the server when a client adds a new player with ClientScene.AddPlayer.
    //
    // Parameters:
    //   conn:
    //     Connection from client.
    //
    //   playerControllerId:
    //     Id of the new player.
    //
    //   extraMessageReader:
    //     An extra message object passed for the new player.
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.Log(string.Format("OnServerAddPlayer : {0} : {1}", conn, playerControllerId));
    }
    
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        Debug.Log(string.Format("OnServerAddPlayer : {0} : {1} : {2}", conn, playerControllerId, extraMessageReader));
    }
    
    //
    // Summary:
    //     Called on the server when a new client connects.
    //
    // Parameters:
    //   conn:
    //     Connection from client.
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log(string.Format("OnServerConnect : {0}", conn));
    }
    
    //
    // Summary:
    //     Called on the server when a client disconnects.
    //
    // Parameters:
    //   conn:
    //     Connection from client.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log(string.Format("OnServerDisconnect : {0}", conn));
    }
    
    //
    // Summary:
    //     Called on the server when a network error occurs for a client connection.
    //
    // Parameters:
    //   conn:
    //     Connection from client.
    //
    //   errorCode:
    //     Error code.
    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
        Debug.Log(string.Format("OnServerError : {0} : {1}", conn, errorCode));
    }
    
    //
    // Summary:
    //     Called on the server when a client is ready.
    //
    // Parameters:
    //   conn:
    //     Connection from client.
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        Debug.Log(string.Format("OnServerReady : {0}", conn));
    }

    //
    // Summary:
    //     Called on the server when a client removes a player.
    //
    // Parameters:
    //   conn:
    //     The connection to remove the player from.
    //
    //   player:
    //     The player controller to remove.
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        base.OnServerRemovePlayer(conn, player);
        Debug.Log(string.Format("OnServerRemovePlayer : {0} : {1}", conn, player));
    }

    //
    // Summary:
    //     Called on the server when a scene is completed loaded, when the scene load was
    //     initiated by the server with ServerChangeScene().
    //
    // Parameters:
    //   sceneName:
    //     The name of the new scene.
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log(string.Format("OnServerSceneChanged : {0}", sceneName));
    }
    
    //
    // Summary:
    //     Callback that happens when a NetworkMatch.SetMatchAttributes has been processed
    //     on the server.
    //
    // Parameters:
    //   success:
    //     Indicates if the request succeeded.
    //
    //   extendedInfo:
    //     A text description for the error if success is false.
    public override void OnSetMatchAttributes(bool success, string extendedInfo)
    {
        base.OnSetMatchAttributes(success, extendedInfo);
        Debug.Log(string.Format("OnSetMatchAttributes : {0} : {1}", success, extendedInfo));
    }

    //
    // Summary:
    //     This is a hook that is invoked when the client is started.
    //
    // Parameters:
    //   client:
    //     The NetworkClient object that was started.
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        Debug.Log(string.Format("OnStartClient : {0}", client));
    }

    //
    // Summary:
    //     This hook is invoked when a host is started.
    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log(string.Format("OnStartHost : {0}", "OnStartHost"));
    }
    
    //
    // Summary:
    //     This hook is invoked when a server is started - including when a host is started.
    public override void OnStartServer()
    {
        Debug.Log(string.Format("OnStartServer : {0}", "OnStartServer"));
    }

    //
    // Summary:
    //     This hook is called when a client is stopped.
    public override void OnStopClient()
    {
        Debug.Log(string.Format("OnStopClient : {0}", "OnStopClient"));
    }

    //
    // Summary:
    //     This hook is called when a host is stopped.
    public override void OnStopHost()
    {
        Debug.Log(string.Format("OnStopHost : {0}", "OnStopHost"));
    }
    
    //
    // Summary:
    //     This hook is called when a server is stopped - including when a host is stopped.
    public override void OnStopServer()
    {
        Debug.Log(string.Format("OnStopServer : {0}", "OnStopServer"));
    }
}