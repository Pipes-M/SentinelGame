using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firePoint;
    private bool invalidBullet;
    private Rigidbody rb;

    public bool playerGun;
    private PlayerScript playerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        BulletValidity();
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.Instance.player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGun)
        {
            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    transform.parent = null;
            //    rb.isKinematic = false;
            //}
            if (Input.GetButtonDown("Fire1")) Fire();
        }
        

        
    }

    void BulletValidity()
    {
        if (bulletPrefab == null)
        {
            invalidBullet = true;
        }
        else if (bulletPrefab.GetComponent<Projectile>() == null)
        {
            invalidBullet = true;
        }
    }

    public void Fire()
    {
        if (!invalidBullet && !playerGun)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.LookRotation(transform.forward));
            //spawnedBullet.GetComponent<Projectile>().owner = transform.parent.gameObject;
            
        }
        else if (playerScript.ammoCount > 0)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.LookRotation(transform.forward));
            //spawnedBullet.GetComponent<Projectile>().owner = transform.parent.gameObject;
            if (playerGun) playerScript.ammoCount--;
        }
    }

    public void Interacted()
    {

    }
}
