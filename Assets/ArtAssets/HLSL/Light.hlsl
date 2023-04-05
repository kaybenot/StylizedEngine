#ifndef CUSTOM_LIGHT_NODES
#define CUSTOM_LIGHT_NODES

// Uncomment for intellisense
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

void GetLightingMain_float(float3 WorldPosition, out float3 Color, out float3 Direction, out float DistanceAttenuation,
    out float ShadowAttenuation)
{
    #if defined(UNIVERSAL_LIGHTING_INCLUDED)

    //float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);

    #if SHADOWS_SCREEN
        float4 clipPos = TransformWorldToHClip(WorldPosition);
        float4 shadowCoord = ComputeScreenPos(clipPos);
    #else
        float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
    #endif
    
    Light mainLight = GetMainLight(shadowCoord, WorldPosition, 1);
    
    Color = mainLight.color;
    Direction = mainLight.direction;
    DistanceAttenuation = mainLight.distanceAttenuation;
    ShadowAttenuation = mainLight.shadowAttenuation;

    #else

    Color = float3(1.0, 1.0, 1.0);
    Direction = float3(0.0, 1.0, 0.0);
    DistanceAttenuation = 1.0;
    ShadowAttenuation = 1.0;

    #endif
}

/**
 * \brief Modifies color with additional lights in lit way
 * \param Color Color to be modified
 * \param Normal Pixel normal
 * \param WorldPosition Pixel world position
 * \param OutColor Modified color
 */
void ApplyAddidionalLighting_float(float4 Color, float3 Normal, float3 WorldPosition, out float4 OutColor)
{
    #if defined(UNIVERSAL_LIGHTING_INCLUDED)
    
    float4 baseColor = Color;
    
    for (int i = 0; i < GetAdditionalLightsCount(); i++)
    {
        Light additionalLight = GetAdditionalLight(i, WorldPosition);

        float diffuse = normalize(dot(Normal, additionalLight.direction));
        baseColor += float4(diffuse * additionalLight.color * additionalLight.distanceAttenuation * additionalLight.shadowAttenuation, 0.0);
    }

    OutColor = normalize(baseColor);

    #else

    OutColor = float4(1.0, 1.0, 1.0, 1.0);

    #endif
}

#endif