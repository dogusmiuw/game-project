using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MobSelector : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField]
    Material mobMaterial;
    [SerializeField]
    Material selectedMobMaterial;
    private Dictionary<GameObject, bool> mobSelectionStates = new Dictionary<GameObject, bool>();

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            RaycastHit hit;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("Mob"))
                {
                    GameObject selectedMob = hit.collider.gameObject;
                    SelectMob(selectedMob);
                }
            }
        }
    }

    public void SelectMob(GameObject mob)
    {
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
                bool isSelected;
                if (mobSelectionStates.TryGetValue(mob, out isSelected) && isSelected)
                {
                    playerShirt.material = mobMaterial;
                    mobSelectionStates[mob] = false;
                }
                else
                {
                    playerShirt.material = selectedMobMaterial;
                    mobSelectionStates[mob] = true;
                }
            }
        }
    }
}
