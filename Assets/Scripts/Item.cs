using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isMetal;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interacted()
    {
        if (isMetal)
        {
            GameManager.Instance.player.GetComponent<PlayerScript>().pdaObj.GetComponent<PdaScreen>().IncreaseMetal();
        }
        else
        {
            GameManager.Instance.player.GetComponent<PlayerScript>().pdaObj.GetComponent<PdaScreen>().IncreasePlastic();
        }
        Destroy(gameObject);
    }
}
