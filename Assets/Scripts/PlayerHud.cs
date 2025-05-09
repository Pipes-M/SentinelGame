using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    public GameObject itemInfoPanel;
    public GameObject ItemNameTextObj;
    public GameObject ItemAmountTextObj;
    public LayerMask targetLayer;

    private TextMeshProUGUI ItemNameTextText;
    private TextMeshProUGUI ItemAmountTextText;
    // Start is called before the first frame update
    void Start()
    {
        ItemNameTextText = ItemNameTextObj.GetComponent<TextMeshProUGUI>();
        ItemAmountTextText = ItemAmountTextObj.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, targetLayer))
        {
            itemInfoPanel.SetActive(true);

            Item itemScript = hit.collider.GetComponent<Item>();
            ItemNameTextText.text = itemScript.data.itemName;
            if (itemScript.itemAmount > 1)
            {
                ItemAmountTextText.text = "Amount: " + itemScript.itemAmount;
            }
            else
            {
                ItemAmountTextText.text = null;
            }
        }
        else
        {
            itemInfoPanel.SetActive(false);
        }
    }
}
