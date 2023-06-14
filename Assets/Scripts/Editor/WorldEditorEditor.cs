using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldEditor), true)]
public class WorldEditorEditor : Editor
{
    private string worldName = "New World";
    private float chunkWidth = 20f;
    private float chunkHeight = 50f;
    private int chunkNumX = 10;
    private int chunkNumZ = 10;
    private int worldChosenIndex = 0;

    private WorldEditor worldEditor;
    
    public override void OnInspectorGUI()
    {
        worldEditor = (WorldEditor)target;
        
        var headerStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal =
            {
                textColor = Color.white
            },
            fontSize = 40
        };
        
        EditorGUILayout.Space();
        GUILayout.Label("World Editor", headerStyle);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Clear current world"))
            ClearCurrentWorld();
        EditorGUILayout.Space();

        var worlds = GetWorlds();
        if (worlds.Count > 0)
        {
            worldChosenIndex = EditorGUILayout.Popup("World Actions", worldChosenIndex, worlds.ToArray());
            if (GUILayout.Button("Load World"))
                LoadWorld(worlds[worldChosenIndex]);
            if (GUILayout.Button("Delete World"))
                DeleteWorld(worlds[worldChosenIndex]);
            EditorGUILayout.Space();
        }
        
        GUILayout.Label("Creating new world", EditorStyles.boldLabel);
        worldName = EditorGUILayout.TextField("World Name", worldName);
        chunkWidth = EditorGUILayout.FloatField("Chunks Width", chunkWidth);
        chunkHeight = EditorGUILayout.FloatField("Chunk Height", chunkHeight);
        chunkNumX = EditorGUILayout.IntField("Chunk Count X", chunkNumX);
        chunkNumZ = EditorGUILayout.IntField("Chunk Count Z", chunkNumZ);
        
        if (GUILayout.Button("Create the world"))
            CreateWorld();
    }

    private List<string> GetWorlds()
    {
        var worlds = new List<string>();
        foreach (var worldPath in AssetDatabase.GetSubFolders("Assets/Resources/Worlds"))
        {
            var split = worldPath.Split('/');
            worlds.Add(split[^1]);
        }
        return worlds;
    }

    private void LoadWorld(string worldToLoad)
    {
        ClearCurrentWorld();
        
        var worldGO = Resources.Load<GameObject>($"Worlds/{worldToLoad}/{worldToLoad}");
        PrefabUtility.InstantiatePrefab(worldGO, worldEditor.transform);
    }

    private void DeleteWorld(string worldNameToDelete)
    {
        var worldContainer = GetOrCreateWorldContainer();
        foreach (var worldData in worldContainer.Worlds.Where(w => w.Name == worldNameToDelete))
            worldContainer.Worlds.Remove(worldData);

        AssetDatabase.DeleteAsset($"Assets/Resources/Worlds/{worldNameToDelete}");
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private void ClearCurrentWorld()
    {
        // Free world editor from holding a world
        if (worldEditor.transform.childCount > 0)
            for (var i = worldEditor.transform.childCount; i > 0 ; i--)
                DestroyImmediate(worldEditor.transform.GetChild(i - 1).gameObject);
    }

    private WorldContainer GetOrCreateWorldContainer()
    {
        var worldContainer = Resources.Load<WorldContainer>("Worlds/World Container");
        if (worldContainer == null)
        {
            worldContainer = CreateInstance<WorldContainer>();
            AssetDatabase.CreateAsset(worldContainer, "Assets/Resources/Worlds/World Container.asset");
        }

        return worldContainer;
    }

    private void CreateWorld()
    {
        if (worldEditor == null)
            Debug.LogError("Somehow WorldEditor script is null");
        
        ClearCurrentWorld();
        
        // Create world object
        var worldGO = new GameObject(worldName);
        worldGO.transform.parent = worldEditor.transform;
        var worldScript = worldGO.AddComponent<World>();
        
        // Create world scriptable asset
        var worldScriptable = CreateInstance<WorldData>();
        worldScript.Data = worldScriptable;
        worldScriptable.Name = worldName;
        worldScriptable.ChunkWidth = chunkWidth;
        worldScriptable.ChunkHeight = chunkHeight;
        worldScriptable.ChunkCountX = chunkNumX;
        worldScriptable.ChunkCountZ = chunkNumZ;

        // Create terrain container
        var terrainContainer = new GameObject("Terrain Container");
        terrainContainer.transform.parent = worldGO.transform;
        
        // Create static object container
        var staticObjectContainer = new GameObject("Static Objects");
        staticObjectContainer.transform.parent = worldGO.transform;
        
        // Create session object container
        var sessionObjectContainer = new GameObject("Session Objects");
        sessionObjectContainer.transform.parent = worldGO.transform;
        
        // Create terrain resource folders
        AssetDatabase.CreateFolder("Assets/Resources/Worlds", worldName);
        AssetDatabase.CreateFolder($"Assets/Resources/Worlds/{worldName}", "TerrainDatas");

        // Create terrains
        for (var x = 0; x < chunkNumX; x++)
        for (var z = 0; z < chunkNumZ; z++)
        {
            var td = new TerrainData
            {
                name = $"Terrain_({x}, {z})",
                size = new Vector3(chunkWidth, chunkHeight, chunkWidth)
            };
            
            var terrainGO = Terrain.CreateTerrainGameObject(td);
            var chunkScript = terrainGO.AddComponent<Chunk>();
            terrainGO.transform.parent = terrainContainer.transform;
            terrainGO.transform.position = new Vector3(x * chunkWidth, 0f, z * chunkWidth);
            terrainGO.name = td.name;
            
            AssetDatabase.CreateAsset(td, $"Assets/Resources/Worlds/{worldName}/TerrainDatas/{td.name}.asset");
        }

        worldScriptable.Prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(worldGO,
            $"Assets/Resources/Worlds/{worldName}/{worldName}.prefab", InteractionMode.UserAction);
        AssetDatabase.CreateAsset(worldScriptable, $"Assets/Resources/Worlds/{worldName}/{worldName} Data.asset");
        worldScriptable.Prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(worldGO,
            $"Assets/Resources/Worlds/{worldName}/{worldName}.prefab", InteractionMode.UserAction);
        
        var worldContainer = GetOrCreateWorldContainer();
        worldContainer.Worlds.Add(worldScriptable);
        EditorUtility.SetDirty(worldContainer);
        
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
