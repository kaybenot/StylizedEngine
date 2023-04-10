using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Renderer))]
public class GrassSpawner : MonoBehaviour
{
    [SerializeField] private Material grassMaterial;
    [SerializeField] private Texture2D noiseTexture;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private int density = 1;
    
    private static readonly int Normal = Shader.PropertyToID("_Normal");

    private Terrain terrain;
    private Renderer grassRenderer;
    private List<List<Matrix4x4>> matrices;
    private List<MaterialPropertyBlock> grassPropertyBlock;
    private Mesh mesh;

    private void Awake()
    {
        terrain = Terrain.activeTerrain;
        grassRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        CreateMesh();
        GenerateGrass();
    }

    private void Update()
    {
        RenderGrass();
    }

    private void CreateMesh()
    {
        //mesh = GameObject.CreatePrimitive(PrimitiveType.Quad).GetComponent<MeshFilter>().mesh;
        
        //return;
        const float width = 0.6f;
        const float height = 0.6f;
        
        mesh = new Mesh();
        
        var vertices = new Vector3[4]
        {
            new Vector3(-width / 2f, 0, 0),
            new Vector3(width / 2f, 0, 0),
            new Vector3(-width / 2f, height, 0),
            new Vector3(width / 2f, height, 0)
        };
        mesh.vertices = vertices;
        
        var tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;
        
        var normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;
        
        var uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
    }

    private void GenerateGrass()
    {
        var terrainSize = terrain.terrainData.size;
        grassPropertyBlock = new List<MaterialPropertyBlock>();
        matrices = new List<List<Matrix4x4>>();
        //var terrainPos = terrain.GetPosition();
        var normals = new List<List<Vector4>>();

        int i = 0, region = 0;
        matrices.Add(new List<Matrix4x4>());
        grassPropertyBlock.Add(new MaterialPropertyBlock());
        normals.Add(new List<Vector4>());
        for (var x = 0; x < density * terrainSize.x * noiseTexture.width / 4f; x++)
        for (var z = 0; z < density * terrainSize.z * noiseTexture.height / 4f; z++)
        {
            if (i >= 1023)
            {
                matrices.Add(new List<Matrix4x4>());
                grassPropertyBlock.Add(new MaterialPropertyBlock());
                normals.Add(new List<Vector4>());
                region++;
                i = 0;
            }
            
            if (noiseTexture.GetPixel(x % noiseTexture.width, z % noiseTexture.height) != Color.black)
                continue;

            var grassPos = new Vector3(x * (4f / (noiseTexture.width * density)), 0f,
                z * (4f / (noiseTexture.height * density)));
            grassPos.y = terrain.SampleHeight(grassPos);
            var rotation = Quaternion.identity;
            var scale = Vector3.one;
            
            matrices[region].Add(Matrix4x4.TRS(grassPos, rotation, scale));

            var normal = terrain.terrainData.GetInterpolatedNormal(grassPos.x, grassPos.z);
            normals[region].Add(normal);

            i++;
        }
        
        grassPropertyBlock[region].SetVectorArray(Normal, normals[region].ToArray());
    }

    private void RenderGrass()
    {
        for (int i = 0; i < matrices.Count; i++)
            Graphics.DrawMeshInstanced(mesh, 0, grassMaterial, matrices[i], grassPropertyBlock[i], ShadowCastingMode.Off, true);
    }
}
