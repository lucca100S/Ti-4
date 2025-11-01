void MainLight_float (float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    Color = 1.0f;
    Attenuation = 1.0f;
#else
    float shadowCoord = TransformWorldToShadowCoord(WorldPos);

    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation;
#endif
}

void AdditionalLight_float (float3 WorldPos, float lightID, out float3 Direction, out float3 Color, out float Attenuation)
{
    Direction = normalize(float3(0.0f, 0.0f, 0.0f));
    Color = 0.0f;
    Attenuation = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();
    if(lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos);
        Direction = light.direction;
        Color = light.color;
        Attenuation = light.distanceAttenuation;
    }
#endif
}