using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PdaScreen : MonoBehaviour
{
    public GameObject commandPanel;
    public GameObject mechPanel;
    public GameObject invPanel;
    public GameObject helpPanel;

    public GameObject metalText;
    public GameObject plasticText;
    public GameObject ammoText;

    private GameObject playerObj;
    private PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameManager.Instance.player;
        playerScript = playerObj.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCommandPanel()
    {
        commandPanel.SetActive(true);
        helpPanel.SetActive(false);
        invPanel.SetActive(false);
    }

    public void ShowMechPanel()
    {
        mechPanel.SetActive(true);
    }

    public void ShowInvPanel()
    {
        invPanel.SetActive(true);
        helpPanel.SetActive(false);
        commandPanel.SetActive(false);
    }

    public void ShowHelpPanel()
    {
        helpPanel.SetActive(true);
        invPanel.SetActive(false);
        commandPanel.SetActive(false);
    }

    public void IncreaseMetal()
    {
        playerScript.metalCount++;
        UpdateTexts();
    }

    public void IncreasePlastic()
    {
        playerScript.plasticCount++;
        UpdateTexts();
    }

    public void IncreaseAmmo()
    {
        if (playerScript.metalCount >= 2 && playerScript.plasticCount >= 1)
        {
            playerScript.metalCount -= 2;
            playerScript.plasticCount -= 1;
            playerScript.ammoCount += 50;
        }
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        metalText.GetComponent<TextMeshProUGUI>().text = playerScript.metalCount.ToString();
        plasticText.GetComponent<TextMeshProUGUI>().text = playerScript.plasticCount.ToString();
        ammoText.GetComponent<TextMeshProUGUI>().text = playerScript.ammoCount.ToString();
    }
}
