using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechInterior : MonoBehaviour
{
    public bool ControllingMech;
    public GameObject telePoint;
    public GameObject leavTelePoint;
    public GameObject chair;
    public GameObject mechRoom;
    public float seatHeight;
    private GameObject player;
    private PlayerScript playerScript;
    private bool shouldRot;
    private Quaternion lastRotation;
    private float yRotationDelta;
    private bool dontLeaveSeat;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPairRot();
        if (ControllingMech)
        {
            InSeat();
            if (Input.GetButtonDown("Interact"))
            {
                if (dontLeaveSeat)
                {
                    ChairOut();
                }
                StartCoroutine(EnableExitAfterDelay(true));
            }
            
        }
        

    }

    public void Interacted()
    {
        player.transform.position = telePoint.transform.position;
        player.transform.parent = gameObject.transform;
        shouldRot = true;
    }

    void PlayerPairRot()
    {
        if (shouldRot)
        {
            //playerScript.horizontalLookRotation = gameObject.transform.rotation.y;

            yRotationDelta = Mathf.DeltaAngle(lastRotation.eulerAngles.y, transform.rotation.eulerAngles.y);

            lastRotation = transform.rotation;

            playerScript.horizontalLookRotation += yRotationDelta;
        }
    }

    public void ChairInteract()
    {
        ControllingMech = true;
        playerScript.InputsEnabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<MechAi>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<MechMovement>().enabled = true;
    }
    void ChairOut()
    {
        ControllingMech = false;
        playerScript.InputsEnabled = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<MechAi>().enabled = true;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<MechMovement>().enabled = false;
        //playerScript.StandFromCrouch();
        StartCoroutine(EnableExitAfterDelay(false));
    }

    void InSeat()
    {
        playerScript.Crouch();
        playerScript.camAnch.transform.rotation = transform.rotation;
        player.transform.position = chair.transform.position + new Vector3(0, seatHeight, 0);
    }

    public void LeaveMech()
    {
        player.transform.position = leavTelePoint.transform.position;
        player.transform.parent = null;
        shouldRot = false;
    }

    IEnumerator EnableExitAfterDelay(bool isExit)
    {
        if (isExit)
        {
            yield return new WaitForSeconds(0.1f);
            dontLeaveSeat = true;
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            dontLeaveSeat = false;
        }
        
    }
}
