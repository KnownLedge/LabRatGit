using System;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;

public class Shader : MonoBehaviour
{
    //variables
    private HashSet<Mesh> rMeshes = new HashSet<Mesh>();

    private List<Mesh> meshes = new List<Mesh>();
    private List<listVector3> meshList = new List<listVector3>();
    

    [Serializable]
    private class listVector3
    {
        public List<Vector3> data;
    }

    Material fillMaterial, maskMaterial;
    Renderer[] renderers;

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
    private Color FillColor = Color.red;

    private bool UpdateShader;
 
    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        fillMaterial = Instantiate(Resources.Load<Material>(@"Materials/fill"));
        maskMaterial = Instantiate(Resources.Load<Material>(@"Materials/mask"));
        fillMaterial.name = "Fill(instance)";
        maskMaterial.name = "Mask(instance)";

       
        genSmoothNormals();

        //update the shader
        UpdateShader = true;
    }

    //when the game object is set to be enabled add the shader materials
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
   
    //Update game object when not playing
    void OnValidate()
    {

        // Update material properties
        UpdateShader = true;

        // Clear cache when baking is disabled or corrupted
        if (meshes.Count != 0 || meshes.Count != meshList.Count)
        {
            meshes.Clear();
            meshes.Clear();
        }

        // Generate smooth normals when baking is enabled
        if (meshes.Count == 0)
        {
            bake();
        }
    }
    //update every frame 
    private void Update()
    {
        //check if the shader needs updated
        if (UpdateShader)
        {
            UpdateShader = false;
            UpdateMatPro();
        }
    }
    //will bake the new materials 
    void bake()
    {
        var bakedMeshes = new HashSet<Mesh>();
        foreach (var mesh in GetComponentsInChildren<MeshFilter>())
        {
            if (!bakedMeshes.Add(mesh.sharedMesh))
            {
                continue;
            }
            var smoothNormals = SmoothNormals(mesh.sharedMesh);

            rMeshes.Add(mesh.sharedMesh);
            meshList.Add(new listVector3() { data  = smoothNormals });

        }
    }
    //generate the normals if none are found
    void genSmoothNormals()
    {
        foreach (var meshFiller in GetComponentsInChildren<MeshFilter>())
        {
            var currentMesh = meshFiller.sharedMesh;
            if (!rMeshes.Add(currentMesh))
            {
                continue ;
            }
            // Retrieve/generate smooth normals
            var index = meshes.IndexOf(meshFiller.sharedMesh);
            var smoothNormals = (index >= 0) ? meshList[index].data : SmoothNormals(meshFiller.sharedMesh);

            // Store smooth normals in UV3
            meshFiller.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            var renderer = meshFiller.GetComponent<Renderer>();
                
            if (renderer != null)
            {
                CombSubMesh(meshFiller.sharedMesh, renderer.sharedMaterials);
            }
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

            // Combine submeshes
            CombSubMesh(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);

        }
    }
    //list the normals
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

    //combine the sub meshes
    void CombSubMesh(Mesh mesh, Material[] materials)
    {
        if(mesh.subMeshCount == 1)
        {
            return;
        }
        if(mesh.subMeshCount > materials.Length)
        {
            return;
        }

        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }
    //will Update the material properties
    void UpdateMatPro()
    {
        fillMaterial.SetColor("_OutlineColor", FillColor);
        maskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        fillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);

    }

    //will remove materials when disabled
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

    private void OnDestroy()
    {
        Debug.Log($"destroyed {name}");
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
