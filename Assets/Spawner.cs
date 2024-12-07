using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Spawner : MonoBehaviourPunCallbacks
{
    public Material player1Material;
    public Material player2Material;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to the lobby");
        PhotonNetwork.JoinOrCreateRoom("AgeOfDousElif", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CountOfPlayers > 0)
        {
            // TODO: change with player 2 base location later
            PhotonNetwork.Instantiate("Player", new Vector3(24.91796f, 0f, 10.7367f), Quaternion.identity);
            SkinnedMeshRenderer playerShirt = GameObject.Find("Tops").GetComponent<SkinnedMeshRenderer>();
            playerShirt.material = player2Material;
        }
        else
        {
            // TODO: change with player 1 base location later
            PhotonNetwork.Instantiate("Player", new Vector3(-26.02451f, 0f, 7.101749f), Quaternion.identity);
            SkinnedMeshRenderer playerShirt = GameObject.Find("Tops").GetComponent<SkinnedMeshRenderer>();
            playerShirt.material = player1Material;
        }
    }

    void Update()
    {

    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the Room");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left the Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join any room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join any random room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not create room");
    }
}
