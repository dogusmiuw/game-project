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

    [SerializeField]
    private int foodCostPerMob = 5; // Cost to spawn each mob

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
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Current Player Count: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // Spawn main player when joining room
        SpawnMainPlayer();
    }

    // Spawn the main player based on their Photon ActorNumber
    public void SpawnMainPlayer()
    {
        // Check if the current player is Player 1 or Player 2 based on ActorNumber
        Vector3 spawnPosition = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? player1BaseLocation : player2BaseLocation;
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
    }

    // Method to spawn a clone mob when the button is clicked
    public void OnSpawnMobButtonClicked()
    {
        Inventory inventory = Inventory.Instance; // Assuming you have an Inventory class
        Item foodItem = inventory.FindItem("Food");

        if (foodItem != null && foodItem.Amount >= foodCostPerMob)
        {
            // Deduct food cost and spawn the mob
            foodItem.Amount -= foodCostPerMob;
            inventory.UpdateUI();

            // Determine spawn location for the clone based on player ActorNumber
            Vector3 spawnPosition = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? player1BaseLocation : player2BaseLocation;

            // Spawn the mob at the correct position for the player
            GameObject mob = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

            // Customize mob appearance
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
            Debug.Log("Not enough food to spawn a new unit!");
        }
    }

    void Update()
    {
    }

    // Photon callback methods
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
