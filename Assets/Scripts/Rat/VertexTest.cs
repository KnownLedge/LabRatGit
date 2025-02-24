using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexTest : MonoBehaviour
{
    public Mesh mesh;
    public Vector3[] vertices;
    public bool[] targetedVertice;
    public Transform targetTransform;
    public Transform linkTarget;
    public Vector3 linkDist;

    public float grabRange;
    public float grabScaler;

    private Vector3[] drawVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        targetedVertice = new bool[vertices.Length];
        drawVertices = new Vector3[vertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            drawVertices[i] = vertices[i];
            if(vertices[i].z < -grabRange){
            targetedVertice[i] = true;
            }else{
            targetedVertice[i] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        linkDist = targetTransform.position - linkTarget.position;
        Debug.Log("TargetTransform: " + targetTransform.position + " - LinkTarget " + linkTarget.position + " = " + linkDist);

         for (var i = 0; i < vertices.Length; i++)
        {
            drawVertices[i] = vertices[i];
            if(targetedVertice[i]){
            drawVertices[i] -= linkDist * grabScaler;
            }
        }

        // assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = drawVertices;
        mesh.RecalculateBounds();
    }
}
