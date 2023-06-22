using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Sad grass spawner, does not use MaterialPropertyBlock because shader graphs, thus does not render grass properly
/// on curved terrain.
/// TODO: Improve component, badly written
/// </summary>
public class GrassSpawner : MonoBehaviour, IChunkComponent
{
    [field: SerializeField] public Terrain Terrain { get; set; }
    
    [SerializeField] private int density = 1;
    
    [Header("Grass mesh settings")]
    [SerializeField] private float width = 0.6f;
    [SerializeField] private float height = 0.6f;

    private static Material grassMaterial;
    private ComputeBuffer computeBuffer;
    private ComputeBuffer argsBuffer;
    private List<ItemInstanceData> instanceDatas;
    private static Mesh mesh;
    private Bounds chunkBounds;

    private struct ItemInstanceData
    {
        public Vector3 Position;
        public Vector3 Normal;

        public static int Size()
        {
            return sizeof(float) * 3 + sizeof(float) * 3;
        }
    }

    private void OnDestroy()
    {
        computeBuffer.Release();
        argsBuffer.Release();
    }

    private void Update()
    {
        RenderGrass();
    }
    
    public void Initialize(IChunk chunk)
    {
        chunkBounds = chunk.Bounds;
        Spawn();
    }

    public void ChunkLoaded()
    {
    }

    public void ChunkUnloaded()
    {
    }
    
    public void Spawn()
    {
        if (grassMaterial == null)
            grassMaterial = Resources.Load<Material>("Grass Material");
        
        if (mesh == null)
            CreateGrassMesh();
        
        var terrainSize = Terrain.terrainData.size;
        var terrainPos = Terrain.GetPosition();
        
        instanceDatas = new List<ItemInstanceData>();

        var noiseTexture = Resources.Load<Texture2D>("Grass Noise");

        for (var x = -terrainSize.x / 2f; x < terrainSize.x / 2f; x += terrainSize.x / 50f)
        for (var z = -terrainSize.z / 2f; z < terrainSize.z / 2f; z += terrainSize.z / 50f)
        {
            var instance = new ItemInstanceData();
            instance.Normal = Vector3.up;
            instance.Position = new Vector3(x + terrainPos.x, 0f, z + terrainPos.z);
            
            instanceDatas.Add(instance);
        }
        
        InitializeBuffers((uint)instanceDatas.Count);
        
        computeBuffer = new ComputeBuffer(instanceDatas.Count, ItemInstanceData.Size(),
            ComputeBufferType.IndirectArguments);
        computeBuffer.SetData(instanceDatas);
        grassMaterial.SetBuffer("_PerInstanceData", computeBuffer);
    }
    
    private void InitializeBuffers(uint size)
    {
        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        args[0] = mesh.GetIndexCount(0);
        args[1] = size;
        args[2] = mesh.GetIndexStart(0);
        args[3] = mesh.GetBaseVertex(0);
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);
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
    
    private void RenderGrass()
    {
        // for (var i = 0; i < matrices.Count; i++)
        //     Graphics.DrawMeshInstanced(mesh, 0, grassMaterial, matrices[i], propertyBlocks[i], ShadowCastingMode.Off, true);
        
        Graphics.DrawMeshInstancedIndirect(mesh, 0, grassMaterial, chunkBounds, argsBuffer, 0, null, ShadowCastingMode.Off, true);
    }
}
