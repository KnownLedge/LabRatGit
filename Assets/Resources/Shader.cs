using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shader : MonoBehaviour
{
    private HashSet<Mesh> rMeshes = new HashSet<Mesh>();

    private readonly List<Mesh> meshes = new List<Mesh>();
    private readonly List<ListVector3> meshList = new List<ListVector3>();

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }

    private Material fillMaterial, maskMaterial;
    private Renderer[] renderers;

    public Color Fill
    {
        get { return FillColor; }
        set
        {
            FillColor = value;
            UpdateShader = true;
        }
    }

    [SerializeField]
    private Color FillColor = Color.white;

    private bool UpdateShader;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        fillMaterial = Instantiate(Resources.Load<Material>(@"Materials/FillMat"));
        maskMaterial = Instantiate(Resources.Load<Material>(@"Materials/MaskMat"));
        fillMaterial.name = "Fill(instance)";
        maskMaterial.name = "Mask(instance)";

        GenSmoothNormals();

        UpdateShader = true;
    }

    void OnValidate()
    {
        UpdateShader = true;
        if (meshes.Count != 0 || meshes.Count != meshList.Count)
        {
            meshes.Clear();
            meshes.Clear();
        }
    }

    private void Update()
    {
        //check if the shader needs updated
        if (UpdateShader)
        {
            UpdateMatPro();
            UpdateShader = false;
        }
    }

    private void GenSmoothNormals()
    {
        foreach (var meshFiller in GetComponentsInChildren<MeshFilter>())
        {
            var currentMesh = meshFiller.sharedMesh;
            if (!rMeshes.Add(currentMesh))
            {
                continue;
            }
            // Retrieve or generate smooth normals
            var index = meshes.IndexOf(meshFiller.sharedMesh);
            var smoothNormals = (index >= 0) ? meshList[index].data : SmoothNormals(meshFiller.sharedMesh);

            // Store smooth normals in UV3
            meshFiller.sharedMesh.SetUVs(3, smoothNormals);
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            // Skip if UV3 has already been reset
            if (!rMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }
            // Clear UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh)
    {
        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }
            smoothNormal.Normalize();
            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }


    //update material properities
    void UpdateMatPro()
    {
        fillMaterial.SetColor("_OutlineColor", FillColor);
        maskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        fillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
    }

    //when the game object is set to be enabled 
    private void OnEnable()
    {
        //loop throught the renders 
        foreach (var render in renderers)
        {
            //create a list of gameobject current materials 
            List<Material> materials = render.sharedMaterials.ToList();

            //add the shaders
            materials.Add(fillMaterial);
            materials.Add(maskMaterial);
            render.materials = materials.ToArray();
        }
    }
    private void OnDisable()
    {
        Debug.Log("Disable");
        foreach (var render in renderers)
        {
            //Remove shader 
            List<Material> materials = render.sharedMaterials.ToList();
            materials.Remove(fillMaterial);
            materials.Remove(maskMaterial);

            render.materials = materials.ToArray();
        }
    }
}
