using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{
    public GameObject obj;
    public float doubleGrowPercent = 10;
    public float yRandGrow = 1;
    public float xzRandGrow = 1;
    public float delay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
        gameObject.name = "Stem" + transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(delay);
        transform.position += new Vector3(Random.Range(-xzRandGrow, xzRandGrow), Random.Range(0, yRandGrow), Random.Range(-xzRandGrow, xzRandGrow));
        if (Random.Range(0, 100) <= doubleGrowPercent)
        {
            Instantiate(obj);
        }
        Instantiate(gameObject);
    }
}
