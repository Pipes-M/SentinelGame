using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firePoint;
    private bool invalidBullet;
    private Rigidbody rb;
    public ItemData bulletData;

    public bool playerGun;
    private PlayerScript playerScript;
    private InventoryGrid playerInvScript;
    
    // Start is called before the first frame update
    void Start()
    {
        BulletValidity();
        rb = GetComponent<Rigidbody>();
        playerScript = GameManager.Instance.player.GetComponent<PlayerScript>();
        playerInvScript = playerScript.playerInv.GetComponent<InventoryGrid>();
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
        if (!invalidBullet && !playerGun) //is ai using gun
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.LookRotation(transform.forward));
            //spawnedBullet.GetComponent<Projectile>().owner = transform.parent.gameObject;

        }
        else if (!invalidBullet && playerInvScript.UseItem(bulletData))
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.LookRotation(transform.forward));
            //spawnedBullet.GetComponent<Projectile>().owner = transform.parent.gameObject;
        }
    }

    public void Interacted()
    {

    }
}
