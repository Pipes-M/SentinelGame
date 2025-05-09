using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarScan : MonoBehaviour
{
    public int raysPerScan = 1000;
    public float maxDistance = 50f;
    public Material pointMaterial;

    private List<Vector3> allPoints = new List<Vector3>();
    private ComputeBuffer pointBuffer;

    void Update()
    {
        ScanEnvironment();

        if (pointBuffer != null) pointBuffer.Release();
        pointBuffer = new ComputeBuffer(allPoints.Count, sizeof(float) * 3);
        pointBuffer.SetData(allPoints.ToArray());

        RenderPoints();
    }

    void ScanEnvironment()
    {
        for (int i = 0; i < raysPerScan; i++)
        {
            Vector3 direction = Random.onUnitSphere;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, maxDistance))
            {
                allPoints.Add(hit.point);
            }
        }
    }

    void RenderPoints()
    {
        pointMaterial.SetBuffer("_Points", pointBuffer);
        pointMaterial.SetFloat("_PointSize", 20f);
        pointMaterial.SetColor("_Color", Color.white);
        Graphics.DrawProcedural(pointMaterial, new Bounds(transform.position, Vector3.one * maxDistance), MeshTopology.Points, allPoints.Count);
    }

    private void OnDestroy()
    {
        if (pointBuffer != null) pointBuffer.Release();
    }
}
