using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
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

    public List<GameObject> objectsInView;
    private List<GameObject> tempObj;


    public delegate void OnColliderHit(Collider2D col);
    public event OnColliderHit onColliderHit;

    private LayerMask mask;

    private void Start()
    {
        
    }

    public void InitLight()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        objectsInView = new List<GameObject>();
        tempObj = new List<GameObject>();
        mask = LayerMask.GetMask("Grid");
        UpdateLight();
    }

    private void LateUpdate()
    {
        UpdateLight();
    }

    public void UpdateLight()
    {
        tempObj.Clear();
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
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, fromAngle, viewDistance, ~mask);
            RaycastHit2D[] raycastAllCollidersHit2D = Physics2D.RaycastAll(origin, fromAngle, viewDistance);

            if(raycastAllCollidersHit2D.Length > 0)
            {
                AddToView(raycastAllCollidersHit2D);

            }

            if (raycastHit2D.collider == null)
            {
                vertex = origin + fromAngle * viewDistance;
                
            }
            //else if (raycastHit2D.collider.tag == "Node")
            //{
            //    vertex = origin + fromAngle * viewDistance;
            //    onColliderHit?.Invoke(raycastHit2D.collider);
            //}
            else
            {
                vertex = raycastHit2D.point;
                onColliderHit?.Invoke(raycastHit2D.collider);
                //AddToView(raycastHit2D.collider.gameObject);
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

        objectsInView = new List<GameObject>(tempObj);

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        
    }

    private void AddToView(RaycastHit2D[] colliders)
    {
        foreach (RaycastHit2D raycast in colliders)
        {
            if (!tempObj.Contains(raycast.collider.gameObject))
            {
                tempObj.Add(raycast.collider.gameObject);
            }
        }

       
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
