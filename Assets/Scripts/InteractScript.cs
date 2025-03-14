using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public string FuncName = ("Interacted");
    public bool CallParent;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendInteract()
    {
        if (CallParent)
        {
            gameObject.SendMessageUpwards(FuncName);
        }
        else
        {
            gameObject.SendMessage(FuncName);
        }
        
    }
}
