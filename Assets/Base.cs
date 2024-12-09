using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Base : MonoBehaviourPunCallbacks
{
    [SerializeField] private int level = 1;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject upgradeButton;
    
    private PhotonView pw;
    private int[] upgradeCosts = new int[] { 5, 10, 15, 20, 25 }; // Wood costs for each level
    private const int MAX_LEVEL = 5;

    void Start()
    {
        pw = GetComponent<PhotonView>();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
        
        if (upgradeButton != null)
        {
            // Only show upgrade button if not max level and player owns this base
            bool canShowButton = level < MAX_LEVEL && 
                ((pw.IsMine && gameObject.name == "Base1") || 
                (!pw.IsMine && gameObject.name == "Base2"));
                
            upgradeButton.SetActive(canShowButton);
            
            // Check if player has enough wood
            if (canShowButton)
            {
                Item woodItem = Inventory.Instance.FindItem("Wood");
                bool hasEnoughWood = woodItem != null && woodItem.Amount >= GetUpgradeCost();
                upgradeButton.GetComponent<UnityEngine.UI.Button>().interactable = hasEnoughWood;
            }
        }
    }

    public void TryUpgrade()
    {
        if (!pw.IsMine || level >= MAX_LEVEL) return;

        int cost = GetUpgradeCost();
        Item woodItem = Inventory.Instance.FindItem("Wood");
        
        if (woodItem != null && woodItem.Amount >= cost)
        {
            // Deduct wood and increase level
            woodItem.Amount -= cost;
            level++;
            
            // Update UI
            Inventory.Instance.UpdateUI();
            UpdateUI();
        }
    }

    private int GetUpgradeCost()
    {
        return upgradeCosts[Mathf.Min(level - 1, upgradeCosts.Length - 1)];
    }
}
