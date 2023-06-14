using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : IWorldManager
{
    public List<WorldData> ListWorlds()
    {
        var worlds = new List<WorldData>();
        worlds.Add(Resources.Load<WorldData>("Worlds/Example World"));

        return worlds;
    }
}
