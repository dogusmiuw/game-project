using UnityEngine;
using System.Collections;

public class TreeCuttable : MonoBehaviour
{
    [SerializeField] private int woodAmount = 3;
    [SerializeField] private float timeToChop = 2f;
    [SerializeField] private float regrowthTime = 10f;
    [SerializeField] private GameObject woodPrefab;
    
    private bool isChopped = false;
    
    public float GetTimeToChop()
    {
        return timeToChop;
    }
    
    public bool CanBeChopped()
    {
        return !isChopped;
    }
    
    public void ChopTree()
    {
        if (isChopped)
        {
            Debug.Log($"Tree {gameObject.name} is already chopped!");
            return;
        }
        
        Debug.Log($"Chopping tree {gameObject.name}");
        DropWood();
        isChopped = true;
        StartCoroutine(RegrowthTimer());
    }
    
    private IEnumerator RegrowthTimer()
    {
        Debug.Log($"Tree {gameObject.name} will regrow in {regrowthTime} seconds");
        yield return new WaitForSeconds(regrowthTime);
        isChopped = false;
        Debug.Log($"Tree {gameObject.name} has regrown!");
    }
    
    private void DropWood()
    {
        Debug.Log($"Dropping {woodAmount} wood from tree {gameObject.name}");
        for (int i = 0; i < woodAmount; i++)
        {
            Vector3 dropPosition = transform.position + Random.insideUnitSphere;
            Instantiate(woodPrefab, dropPosition, Quaternion.identity);
        }
    }
} 