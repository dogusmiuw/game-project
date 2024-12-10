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
    private int[] upgradeCosts = new int[] { 1,2,3}; // Wood costs for each level
    private const int MAX_LEVEL =3 ;

    void Start()
    {
        pw = GetComponent<PhotonView>();
        UpdateUI();
    }

    void UpdateUI()
    {
        Debug.Log($"Wood amount: {(Inventory.Instance.FindItem("Wood")?.Amount ?? 0)}");
        
        if (upgradeButton != null)
        {
            bool canShowButton = level < MAX_LEVEL && 
                ((pw.IsMine && gameObject.name == "Base1") || 
                (!pw.IsMine && gameObject.name == "Base2"));
                
            Debug.Log($"Can Show Button: {canShowButton}");
            Debug.Log($"IsMine: {pw.IsMine}");
            Debug.Log($"GameObject name: {gameObject.name}");
            
            upgradeButton.SetActive(true);
            
            int cost = GetUpgradeCost();
            Item woodItem = Inventory.Instance.FindItem("Wood");
            bool hasEnoughWood = woodItem != null && woodItem.Amount >= cost;
            
            Debug.Log($"Has enough wood: {hasEnoughWood}, Cost: {cost}, Wood amount: {woodItem?.Amount ?? 0}");
            
            UnityEngine.UI.Button buttonComponent = upgradeButton.GetComponent<UnityEngine.UI.Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = hasEnoughWood && canShowButton;
                Debug.Log($"Button interactable: {buttonComponent.interactable}");
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
           
            woodItem.Amount -= cost;
            level++;
            
            
            Inventory.Instance.UpdateUI();
            UpdateUI();
        }
    }

    private int GetUpgradeCost()
    {
        return upgradeCosts[Mathf.Min(level - 1, upgradeCosts.Length - 1)];
    }
}
