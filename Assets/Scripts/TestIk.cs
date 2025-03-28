using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


/// <summary>
/// 
/// -transform.right is forward
/// transform.forward is right
/// 
/// </summary>


public class TestIk : MonoBehaviour
{
    public GameObject otherLeg;
    //public GameObject pelvisParent;
    public GameObject rotFixParent;
    public GameObject MechMainParent;
    public bool isLeft;
    public float parentBounceMult = 1f;
    public float legLiftTiltMult = 5f;
    public bool legDown;
    public float legHeightAdjust = 1f;
    public float rayLength = 100f;
    public GameObject foot;
    public GameObject ikTip;
    public Vector2 footReach = new Vector2 (3f, 1f);
    public float moveFootSpeed = 1f;
    public float stepHeight = 1f;
    //public AnimationCurve jiggleCurve;

    private TestIk otherLegScript;
    private MechJiggle jiggleScript;
    private Vector3 oldStepLoc;
    private Vector3 newStepLoc;
    private Quaternion oldStepRot;
    private Quaternion newStepRot;
    private int layerToIgnore;
    private int layerMask;
    private Vector3 lastDirLoc;
    private Vector2 footDir;
    private float footReachSmooth = 1f;
    private float moveLerp = 1f;
    private Vector3 constsNewStep;
    private Vector3 constsNewStepNorm;
    private bool stepOnce;
    private Vector3 worldFootReachPos;
    private Vector3 worldFootReachNeg;
    private float previousStepVal;


    void Start()
    {
        otherLegScript = otherLeg.GetComponent<TestIk>();
        jiggleScript = MechMainParent.GetComponent<MechJiggle>();
        lastDirLoc = transform.position;
        layerToIgnore = LayerMask.NameToLayer("Mech");
        layerMask = ~(1 << layerToIgnore);
        StationaryStep();
        ikTip.transform.position = newStepLoc;
        foot.transform.localRotation = newStepRot;
        oldStepLoc = ikTip.transform.position;
        oldStepRot = ikTip.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        worldFootReachPos = transform.position + (-transform.right * footReach.x) + (transform.forward * footReach.y);
        worldFootReachNeg = transform.position + (transform.right * footReach.x) + (-transform.forward * footReach.y);

        ConstStepDetect();

        if (otherLegScript.legDown && !PointInBounds(newStepLoc))
        {
            oldStepLoc = newStepLoc;
            //newStepLoc = constsNewStep;
            oldStepRot = newStepRot;
            moveLerp = 0;
        }
        MoveFoot();

        Debugs();
        //transform.rotation = Quaternion.Euler(parent.transform.rotation.x, parent.transform.rotation.y, parent.transform.rotation.z);
        //print(pelvisParent.transform.localEulerAngles);

        LegPointDown();
    }

    void Debugs()
    {
        //Debug.DrawRay(worldFootReachPos, -transform.up * rayLength, Color.green); //bound corner
        //Debug.DrawRay(worldFootReachNeg, -transform.up * rayLength, Color.green); //bound corner
        Debug.DrawRay(newStepLoc + new Vector3(0, 5, 0), -transform.up * rayLength, Color.magenta); //new foot location
        DebugBoundBox(Color.yellow, Time.deltaTime); //foot reach box
        Debug.DrawRay(transform.position + (-transform.right * footDir.x * footReach.x) + 
            (transform.forward * footDir.y * footReach.y), -transform.up * rayLength, Color.red); //foot step prediction

    }

    void DebugBoundBox(Color color, float duration = 0.1f)
    {
        Vector3 corner1 = worldFootReachPos;
        Vector3 corner2 = worldFootReachNeg;

        Vector3 topRight = corner1;
        Vector3 topLeft = corner1 - 2 * (transform.forward * footReach.y);
        Vector3 bottomRight = corner2 - 2 *(-transform.forward * footReach.y);
        Vector3 bottomLeft = corner2;

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }

    bool PointInBounds(Vector3 p)
    {
        Vector3 corner1 = worldFootReachPos;
        Vector3 corner2 = worldFootReachNeg;

        Vector3 tl = corner1 - 2 * (transform.forward * footReach.y);
        Vector3 tr = corner1;
        Vector3 bl = corner2;
        Vector3 br = corner2 - 2 * (-transform.forward * footReach.y);

        Vector3 u = (tr - tl).normalized;
        Vector3 v = (bl - tl).normalized;

        float maxU = Vector3.Distance(tl, tr);
        float maxV = Vector3.Distance(tl, bl);

        Vector3 relP = p - tl;
        float projU = Vector3.Dot(relP, u);
        float projV = Vector3.Dot(relP, v);

        return (projU >= 0 && projU <= maxU && projV >= 0 && projV <= maxV);
    }

    void StationaryStep()
    {
        Ray ray;
        ray = new Ray(transform.position, -transform.up);
        
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
        {
            newStepLoc = hit.point + new Vector3(0, legHeightAdjust, 0);
            Quaternion targetRotation = Quaternion.FromToRotation(foot.transform.up, hit.normal) * foot.transform.rotation;
            Vector3 euler = targetRotation.eulerAngles;
            euler.y = 0;
            newStepRot = Quaternion.Euler(euler);
        }
    }

    void MoveFoot()
    {
        if (moveLerp < 1)
        {
            //foot moving

            legDown = false;

            newStepLoc = constsNewStep;

            Vector3 footPos = Vector3.Lerp(oldStepLoc, newStepLoc, moveLerp);
            float smoothLerp = Mathf.SmoothStep(0f, 1f, moveLerp);

            float footVal = Mathf.Sin(moveLerp * Mathf.PI) * stepHeight;

            footPos.y += footVal;

            //jiggleScript.height += ((Mathf.Sin(moveLerp * Mathf.PI) - 0.5f) * stepHeight) * jiggleScript.stepHeightStrength;

            float deltaStep = footVal - previousStepVal;
            jiggleScript.height += deltaStep * jiggleScript.stepHeightStrength;
            previousStepVal = footVal;
            //print((Mathf.Sin(moveLerp * Mathf.PI) - 0.5f) * stepHeight);

            //float heightVal = Mathf.Sin(smoothLerp * Mathf.PI) * stepHeight;
            //BodyBounce(Mathf.Sin(smoothLerp * Mathf.PI) * stepHeight);             BODY BOUNCE HERE

            ikTip.transform.position = footPos;
            RotFoot();
            moveLerp += Time.deltaTime * moveFootSpeed;
        }
        else
        {
            //foot stationary
            if (legDown == false) jiggleScript.Jiggle();
            ikTip.transform.position = newStepLoc;
            legDown = true;
            
        }
    }

    void RotFoot()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(foot.transform.up, constsNewStepNorm) * foot.transform.rotation;
        Vector3 euler = targetRotation.eulerAngles - new Vector3(rotFixParent.transform.eulerAngles.x + 90, 0, 0);
        euler.y = 0;
        newStepRot = Quaternion.Euler(euler);
        foot.transform.localRotation = newStepRot;
    }

    void ConstStepDetect()
    {
        Vector3 movement = transform.position - lastDirLoc;
        movement.x = Mathf.Abs(movement.x) < 0.001f ? 0f : movement.x;
        movement.y = Mathf.Abs(movement.y) < 0.001f ? 0f : movement.y;
        movement.z = Mathf.Abs(movement.z) < 0.001f ? 0f : movement.z;
        movement = movement.normalized;
        lastDirLoc = transform.position;
        
        if (movement.sqrMagnitude > 0.1f)
        {
            float forwardDot = Vector3.Dot(movement, -transform.right);
            float rightDot = Vector3.Dot(movement, transform.forward);
            Vector2 targdir = new Vector2(forwardDot, rightDot).normalized;
            footDir = Vector2.Lerp(footDir, targdir, Time.deltaTime * footReachSmooth);
        }

        Ray ray = new Ray(transform.position + (-transform.right * footDir.x * (footReach.x - 0.1f)) + (transform.forward * footDir.y * (footReach.y - 0.1f)), -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
        {
            constsNewStep = hit.point + new Vector3(0, legHeightAdjust, 0);
            constsNewStepNorm = hit.normal;
        }
        
    }

    //void BodyBounce(float value)
    //{
    //    Vector3 pos = pelvisParent.transform.localPosition;
    //    pelvisParent.transform.localPosition = new Vector3(pos.x, value * parentBounceMult, pos.z);
    //    //pelvisParent.transform.RotateAround(otherLeg.transform.position, transform.up, value * parentBounceMult);
    //    //float val1 = isLeft ? value : -value;
    //    float val2 = jiggleCurve.Evaluate(isLeft ? value : -value);
    //    pelvisParent.transform.localRotation = Quaternion.Euler(((isLeft ? value : -value) * legLiftTiltMult) - 90f, 90f, 0f);
    //}

    void LegPointDown()
    {
        Quaternion parentRotation = rotFixParent.transform.rotation;
        Vector3 newRotation = parentRotation.eulerAngles;
        newRotation.x = -90;
        rotFixParent.transform.rotation = Quaternion.Euler(newRotation);
    }

}
