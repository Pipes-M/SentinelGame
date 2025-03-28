using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Cable : MonoBehaviour
{
    public Transform parentObject;
    public float cableStretchSpeed = 1.0f;

    private float maxAngle = 45f;
    private float distanceThreshold = 0.5f;
    private float dangleAngle = 0.5f;
    private int layerToIgnore;
    private int layerMask;
    //private Vector3 resetNorm;
    private Quaternion initialLocalRotation;
    private Vector3 currentTargetPoint;
    public bool test;
    private float lerp;
    private float desiredStretchScale;

    // Start is called before the first frame update
    void Start()
    {
        layerToIgnore = LayerMask.NameToLayer("Drone");
        layerMask = ~(1 << layerToIgnore);

        //parentObject = transform.parent;

        //resetNorm = DirCableNormalise(transform.localPosition);
        initialLocalRotation = Quaternion.LookRotation(DirCableNormalise(transform.localPosition));
        //FireRay(resetNorm);
        FireRayFromInitialRotation();

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion localRotation = Quaternion.Inverse(parentObject.rotation) * transform.rotation;
        float angleDifference = Quaternion.Angle(initialLocalRotation, localRotation);
        if (angleDifference > maxAngle || IsBlocked())
        {
            ResetRay();
        }
        else
        {
            AimAtCurrentTarget();
        }


        Vector3 directionToTarget = (currentTargetPoint - transform.position).normalized;
        Debug.DrawRay(transform.position, directionToTarget * 100, Color.red);
    }

    //void FireRay(Vector3 targDir)
    //{
    //    Ray ray = new Ray(transform.position, targDir);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 10000, layerMask))
    //    {
    //        FireCable(hit.point);
    //        mainDest = hit.point;
    //        Debug.DrawRay(transform.position, targDir * 100, Color.red);
    //    }
    //}

    void FireCable(Vector3 impact)
    {
        transform.localScale = new Vector3(transform.localScale.x,
            transform.localScale.y, Vector3.Distance(transform.position, impact));
    }


    Vector3 DirCableNormalise(Vector3 pos)
    {
        return new Vector3(pos.x > 0 ? dangleAngle : -dangleAngle,
            pos.y > 0 ? dangleAngle : -dangleAngle, pos.z > 0 ? dangleAngle : -dangleAngle);
    }

    void AimAtCurrentTarget()
    {
        Vector3 direction = (currentTargetPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction, parentObject.up);
        FireCable(currentTargetPoint);
    }

    void ResetRay()
    {
        transform.localRotation = initialLocalRotation;

        FireRayFromInitialRotation();
    }

    void FireRayFromInitialRotation()
    {
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, 10000, layerMask))
        {
            currentTargetPoint = hit.point;
        }
        else
        {
            currentTargetPoint = transform.position + rayDirection * 10000;
        }
    }

    bool IsBlocked()
    {
        float expectedDistance = Vector3.Distance(transform.position, currentTargetPoint);

        Vector3 directionToTarget = (currentTargetPoint - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, 10000, layerMask))
        {
            float actualDistance = hit.distance;

            if (Mathf.Abs(expectedDistance - actualDistance) > distanceThreshold)
            {
                return true;
            }
        }

        return false;
    }

    void CableStretch(bool extend)
    {
        if (extend)
        {
            lerp += Time.deltaTime * cableStretchSpeed;
        }
        else
        {
            lerp -= Time.deltaTime * cableStretchSpeed;
        }
        lerp = Mathf.Clamp(lerp, 0, 1);
        float value = Mathf.Lerp(0, Vector3.Distance(transform.position, currentTargetPoint), lerp);
        transform.localScale = new Vector3(transform.localScale.x,
            transform.localScale.y, value);
    }
}
