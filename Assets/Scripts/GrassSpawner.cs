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

    private Vector2Int TerrainPosition(Vector3 worldPosition)
    {
        var terrainPosition = worldPosition - terrain.transform.position;
        var mapPosition = new Vector3
        (terrainPosition.x / terrain.terrainData.size.x, 0,
            terrainPosition.z / terrain.terrainData.size.z);
        var x = mapPosition.x * terrain.terrainData.alphamapWidth;
        var z = mapPosition.z * terrain.terrainData.alphamapHeight;

        return new Vector2Int((int)x, (int)z);
    }
    
    private Vector4 CheckTexture(Vector2Int terrainPosition)
    {
        var aMap = terrain.terrainData.GetAlphamaps (terrainPosition.x, terrainPosition.y, 1, 1);
        return new Vector4(aMap[0, 0, 0], aMap[0, 0, 1], aMap[0, 0, 2], aMap[0, 0, 3]);
    }

    private bool IsOnGrassTexture(Vector3 position)
    {
        var alphaMap = CheckTexture(TerrainPosition(position));
        return !(alphaMap[0] <= 0.5f);
    }

    private Vector3 CalculateGrassPosition(Vector3 terrainPos, int x, int z)
    {
        var grassPos = new Vector3(terrainPos.x + x * (4f / (noiseTexture.width * density)), 0f,
            terrainPos.z + z * (4f / (noiseTexture.height * density)));
        grassPos.y = terrain.SampleHeight(grassPos);

        return grassPos;
    }

    private Matrix4x4 CalculateTRS(Vector3 grassPos)
    {
        var rotation = Quaternion.identity;
        var scale = Vector3.one;

        return Matrix4x4.TRS(grassPos, rotation, scale);
    }

    private void CreateNewRegion(List<List<Vector4>> normals)
    {
        matrices.Add(new List<Matrix4x4>());
        grassPropertyBlock.Add(new MaterialPropertyBlock());
        normals.Add(new List<Vector4>());
    }

    private bool IsOnNoiseHighValue(int x, int z)
    {
        if (noiseTexture.GetPixel(x % noiseTexture.width, z % noiseTexture.height) != Color.black)
            return false;
        return true;
    }
    
    private void GenerateGrass()
    {
        var terrainSize = terrain.terrainData.size;
        grassPropertyBlock = new List<MaterialPropertyBlock>();
        matrices = new List<List<Matrix4x4>>();
        var terrainPos = terrain.GetPosition();
        var normals = new List<List<Vector4>>();

        int i = 0, region = 0;
        CreateNewRegion(normals);
        for (var x = 0; x < density * terrainSize.x * noiseTexture.width / 4f; x++)
        for (var z = 0; z < density * terrainSize.z * noiseTexture.height / 4f; z++)
        {
            if (i >= 1023)
            {
                CreateNewRegion(normals);
                region++;
                i = 0;
            }
            
            if(!IsOnNoiseHighValue(x, z))
                continue;

            var grassPos = CalculateGrassPosition(terrainPos, x, z);
            var TRS = CalculateTRS(grassPos);
            
            if (!IsOnGrassTexture(grassPos))
                continue;

            matrices[region].Add(TRS);

            var normal = terrain.terrainData.GetInterpolatedNormal(grassPos.x, grassPos.z);
            normals[region].Add(normal);

            i++;
        }
        
        // Property blocks does not work with instanced drawing using shader graphs :(
        grassPropertyBlock[region].SetVectorArray(Normal, normals[region].ToArray());
    }

    private void RenderGrass()
    {
        for (var i = 0; i < matrices.Count; i++)
            Graphics.DrawMeshInstanced(mesh, 0, grassMaterial, matrices[i], grassPropertyBlock[i], ShadowCastingMode.Off, true);
    }
}
