using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class FlashLight : MonoBehaviour
{
    private Mesh mesh;
    public float fov = 60f;
    public Vector3 origin;
    public int rayCount = 50;
    public float angle = 0f;
    public float angleIncrease = 0;
    public float viewDistance = 10f;

    [SerializeField]
    private float startingAngle;
    [SerializeField]
    private Vector3 startVec;


    public delegate void OnColliderHit(Collider2D col);
    public event OnColliderHit onColliderHit;

    private void Start()
    {
        
    }

    public void InitLight()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        UpdateLight();
    }

    private IEnumerator StartLight()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateLight();
    }

    private void LateUpdate()
    {
        UpdateLight();
    }

    public void UpdateLight()
    {
        angleIncrease = fov / rayCount;
        if (startVec != null)
        {
            startingAngle = GetAngleFromVector(startVec) + fov / 2f;
        }
        angle = startingAngle;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            Vector3 fromAngle = GetVectorFromAngle(angle);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, fromAngle, viewDistance);
            if (raycastHit2D.collider == null)
            {
                vertex = origin + fromAngle * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
                onColliderHit?.Invoke(raycastHit2D.collider);

            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;

            }
            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 direction)
    {
        startVec = direction.normalized;
        startingAngle = GetAngleFromVector(direction) + fov / 2f;
    }



    #region Helper Functions
    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0)
        {
            n += 360;
        }
        return n;
    }
#endregion

 
}
