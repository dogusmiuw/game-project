using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class Base : MonoBehaviourPunCallbacks
{
    [SerializeField] private int level = 1;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject upgradeButton;
    
    private PhotonView pw;
    private int[] upgradeCosts = new int[] { 1,2,3}; // Wood costs for each level
    private const int MAX_LEVEL =3 ;

    void Start()
    {
        pw = GetComponent<PhotonView>();
        if (pw == null)
        {
            Debug.LogError("PhotonView component not found on Base object", this);
            return;
        }
        UpdateUI();
    }

   public void UpdateUI()
    {
        if (pw == null)
        {
            Debug.LogError("PhotonView not initialized", this);
            return;
        }

        if (Inventory.Instance != null)
        {
            Debug.Log($"Wood amount: {(Inventory.Instance.FindItem("Wood")?.Amount ?? 0)}");
        }
        
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
        
        if (upgradeButton != null)
        {
            bool canShowButton = level < MAX_LEVEL && 
                ((pw.IsMine && gameObject.name == "Base1") || 
                (!pw.IsMine && gameObject.name == "Base2"));
                
            upgradeButton.SetActive(canShowButton);
            
            if (Inventory.Instance != null)
            {
                int cost = GetUpgradeCost();
                Item woodItem = Inventory.Instance.FindItem("Wood");
                bool hasEnoughWood = woodItem != null && woodItem.Amount >= cost;
                
                Debug.Log($"Has enough wood: {hasEnoughWood}, Cost: {cost}, Wood amount: {woodItem?.Amount ?? 0}");
                
                UnityEngine.UI.Button buttonComponent = upgradeButton.GetComponent<UnityEngine.UI.Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.interactable = hasEnoughWood && canShowButton;
                    
                    ColorBlock colors = buttonComponent.colors;
                    colors.disabledColor = new Color(0.7f, 0.7f, 0.7f, 0.5f);
                    colors.normalColor = Color.white;
                    buttonComponent.colors = colors;
                    
                    Debug.Log($"Button interactable: {buttonComponent.interactable}");
                }
            }
        }
    }

    [PunRPC]
    void UpdateLevel(int newLevel)
    {
        level = newLevel;
        UpdateUI();
    }

    public void TryUpgrade()
    {
        if (!pw.IsMine || level >= MAX_LEVEL) return;

        int cost = GetUpgradeCost();
        Item woodItem = Inventory.Instance.FindItem("Wood");
        
        if (woodItem != null && woodItem.Amount >= cost)
        {
            woodItem.Amount -= cost;
            level++;
            
            // Sync level across network
            pw.RPC("UpdateLevel", RpcTarget.All, level);
            
            Inventory.Instance.UpdateUI();
            UpdateUI();
        }
    }

    private int GetUpgradeCost()
    {
        return upgradeCosts[Mathf.Min(level - 1, upgradeCosts.Length - 1)];
    }
}
