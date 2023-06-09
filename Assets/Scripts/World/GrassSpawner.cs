using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Sad grass spawner, does not use MaterialPropertyBlock because shader graphs, thus does not render grass properly
/// on curved terrain
/// </summary>
public class GrassSpawner : MonoBehaviour, ISpawner
{
    [Header("Grass generation settings")]
    [SerializeField] private Material grassMaterial;
    [SerializeField] private Texture2D noiseTexture;
    [SerializeField] private int density = 1;
    
    [Header("Grass mesh settings")]
    [SerializeField] private float width = 0.6f;
    [SerializeField] private float height = 0.6f;

    private Terrain terrain;
    private List<List<Matrix4x4>> matrices;
    private Mesh mesh;

    private void Awake()
    {
        terrain = Terrain.activeTerrain;
    }

    private void Start()
    {
        CreateGrassMesh();
        Spawn();
    }

    private void Update()
    {
        RenderGrass();
    }
    
    public void Spawn()
    {
        var terrainSize = terrain.terrainData.size;
        var terrainPos = terrain.GetPosition();
        
        matrices = new List<List<Matrix4x4>>();

        // Unity supports only 1023 meshes drawn in single instanced call
        var objectNumber = 0;
        var instanceGroup = 0;
        matrices.Add(new List<Matrix4x4>());

        var samplesPerUnitX = noiseTexture.width * density / 4f;
        var samplesPerUnitZ = noiseTexture.height * density / 4f;
        for (var x = 0; x < samplesPerUnitX * terrainSize.x; x++)
        for (var z = 0; z < samplesPerUnitZ * terrainSize.z; z++)
        {
            // Create new instance group whenever hit unity support amount
            if (objectNumber >= 1023)
            {
                matrices.Add(new List<Matrix4x4>());
                instanceGroup++;
                objectNumber = 0;
            }
            
            if(!IsOnNoiseHighValue(x, z))
                continue;

            var grassPos = CalculateGrassPosition(terrainPos, x, z);
            var TRS = CalculateTRS(grassPos);
            
            if (!IsOnGrassTexture(grassPos))
                continue;

            matrices[instanceGroup].Add(TRS);

            objectNumber++;
        }
    }

    private void CreateGrassMesh()
    {
        mesh = new Mesh();
        
        var vertices = new []
        {
            new Vector3(-width / 2f, 0, 0),
            new Vector3(width / 2f, 0, 0),
            new Vector3(-width / 2f, height, 0),
            new Vector3(width / 2f, height, 0)
        };
        mesh.vertices = vertices;
        
        var tris = new []
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;
        
        var normals = new []
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;
        
        var uv = new Vector2[]
        {
            new (0, 0),
            new (1, 0),
            new (0, 1),
            new (1, 1)
        };
        mesh.uv = uv;
    }

    private Vector2Int TerrainPosition(Vector3 worldPosition)
    {
        var terrainPosition = worldPosition - terrain.transform.position;
        var mapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0,
            terrainPosition.z / terrain.terrainData.size.z);
        var x = mapPosition.x * terrain.terrainData.alphamapWidth;
        var z = mapPosition.z * terrain.terrainData.alphamapHeight;

        return new Vector2Int((int)x, (int)z);
    }
    
    private Vector4 SampleTerrainAlphamap(Vector2Int terrainPosition)
    {
        var aMap = terrain.terrainData.GetAlphamaps (terrainPosition.x, terrainPosition.y, 1, 1);
        return new Vector4(aMap[0, 0, 0], aMap[0, 0, 1], aMap[0, 0, 2], aMap[0, 0, 3]);
    }

    private bool IsOnGrassTexture(Vector3 position)
    {
        var alphaMap = SampleTerrainAlphamap(TerrainPosition(position));
        return !(alphaMap[0] <= 0.5f);
    }

    private Vector3 CalculateGrassPosition(Vector3 terrainPos, int x, int z)
    {
        var grassPos = new Vector3(terrainPos.x + x * (4f / (noiseTexture.width * density)), 0f,
            terrainPos.z + z * (4f / (noiseTexture.height * density)));
        grassPos.y = terrain.SampleHeight(grassPos);

        return grassPos;
    }

    private static Matrix4x4 CalculateTRS(Vector3 grassPos)
    {
        var rotation = Quaternion.identity;
        var scale = Vector3.one;

        return Matrix4x4.TRS(grassPos, rotation, scale);
    }

    private bool IsOnNoiseHighValue(int pixelX, int pixelY)
    {
        return noiseTexture.GetPixel(pixelX % noiseTexture.width, pixelY % noiseTexture.height) == Color.black;
    }

    private void RenderGrass()
    {
        foreach (var matrix in matrices)
            Graphics.DrawMeshInstanced(mesh, 0, grassMaterial, matrix, null, ShadowCastingMode.Off, true);
    }
}
