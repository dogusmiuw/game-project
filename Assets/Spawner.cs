using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Material mobMaterial;
    [SerializeField]
    Vector3 player1BaseLocation;
    [SerializeField]
    Vector3 player2BaseLocation;

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
        SpawnMob();
    }

    public void SpawnMob()
    {
        if (PhotonNetwork.CountOfPlayers > 0)
        {
            GameObject mob = PhotonNetwork.Instantiate("Player", player2BaseLocation, Quaternion.identity);
            Transform topsTransform = null;
            foreach (Transform child in mob.GetComponentsInChildren<Transform>())
            {
                if (child.name == "Tops")
                {
                    topsTransform = child;
                    break;
                }
            }

            if (topsTransform != null)
            {
                SkinnedMeshRenderer playerShirt = topsTransform.GetComponent<SkinnedMeshRenderer>();
                if (playerShirt != null)
                {
                    playerShirt.material = mobMaterial;
                }
            }
        }
        else
        {
            PhotonNetwork.Instantiate("Player", player1BaseLocation, Quaternion.identity);
        }
        // SkinnedMeshRenderer playerShirt = GameObject.Find("Tops").GetComponent<SkinnedMeshRenderer>();
        // playerShirt.material = mobMaterial;
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
