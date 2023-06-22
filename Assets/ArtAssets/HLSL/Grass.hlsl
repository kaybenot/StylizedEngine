#ifndef CUSTOM_LIGHT_NODES
#define CUSTOM_LIGHT_NODES

struct InstanceData
{
    float3 position;
    float3 normal;
};

StructuredBuffer<InstanceData> _PerInstanceData;

void instancingSetup() {}

void Instancing_float(float3 Position, out float3 Out)
{
    Out = Position;
}

void GetValues_float(float instanceID, out float3 Position, out float3 Normal)
{
    #if !defined(SHADERGRAPH_PREVIEW)
    
    Position = _PerInstanceData[instanceID].position;
    Normal = _PerInstanceData[instanceID].normal;

    #else

    Position = 0;
    Normal = 0;

    #endif
}

#endif