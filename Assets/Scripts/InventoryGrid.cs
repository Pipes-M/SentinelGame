using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public float pickupItemSpeed = 1f;
    [SerializeField] InvSlot[] slots;
    public ItemData metalData;
    public ItemData resinData;

    private void Start()
    {
        
    }


    private void Awake()
    {
        
    }

    public void AddItem(GameObject newItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                Rigidbody rb = newItem.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.detectCollisions = false;
                newItem.transform.parent = slots[i].transform;
                StartCoroutine(PickupMoveItem(newItem, slots[i]));
                slots[i].item = newItem;
                break;
            }
        }
    }

    IEnumerator PickupMoveItem(GameObject item, InvSlot emptySlot)
    {
        float time = 0;
        Vector3 startPos = item.transform.localPosition;
        Quaternion startRot = item.transform.localRotation;
        print(startRot);
        while (time < 1)
        {
            item.transform.localPosition = Vector3.Slerp(startPos, emptySlot.anchorLoc, time);
            item.transform.localRotation = Quaternion.Slerp(startRot, emptySlot.anchorRot.normalized, time);
            time += Time.deltaTime * pickupItemSpeed;
            yield return null;
        }
        item.transform.localPosition = emptySlot.anchorLoc;
        item.transform.localRotation = emptySlot.anchorRot;
    }

    public bool UseItem(ItemData itemData)
    {
        bool isFound = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item.GetComponent<Item>().data == itemData)
            {
                isFound = true;
                slots[i].item.GetComponent<Item>().itemAmount -= 1;
                if (slots[i].item.GetComponent<Item>().itemAmount <= 0) Destroy(slots[i].item);
                break;
            }
        }
        return isFound;
    }

    public bool CraftAmmo()
    {
        bool a = false;
        bool b = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item.GetComponent<Item>().data == metalData)
            {
                a = true;
                break;
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item.GetComponent<Item>().data == resinData)
            {
                b = true;
                break;
            }
        }
        if (a && b)
        {
            UseItem(metalData);
            UseItem(resinData);
            return true;
        }
        else
        {
            return false;
        }
    }
}
