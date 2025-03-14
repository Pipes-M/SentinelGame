using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PdaScreen : MonoBehaviour
{
    public GameObject commandPanel;
    public GameObject mechPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCommandPanel()
    {
        commandPanel.SetActive(true);
    }

    public void ShowMechPanel()
    {
        mechPanel.SetActive(true);
    }
}
