#pragma instancing_options
#include <HLSLSupport.cginc>
#include "UnityInstancing.cginc"

UNITY_INSTANCING_BUFFER_START(PerInstanceData)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color1)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color2)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color3)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color4)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color5)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color6)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color7)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color8)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color9)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color10)
UNITY_DEFINE_INSTANCED_PROP(float4, _Color11)
UNITY_INSTANCING_BUFFER_END(PerInstanceData)

float4 TextureColor(float uvX)
{
    float count = 11;
    float scaledUvX = uvX * (count - 1);
        
    // Using step() to avoid conditionals
    float w1 = step(0.0, 0.5 - abs(scaledUvX - 0.0));
    float w2 = step(0.0, 0.5 - abs(scaledUvX - 1.0));
    float w3 = step(0.0, 0.5 - abs(scaledUvX - 2.0));
    float w4 = step(0.0, 0.5 - abs(scaledUvX - 3.0));
    float w5 = step(0.0, 0.5 - abs(scaledUvX - 4.0));
    float w6 = step(0.0, 0.5 - abs(scaledUvX - 5.0));
    float w7 = step(0.0, 0.5 - abs(scaledUvX - 6.0));
    float w8 = step(0.0, 0.5 - abs(scaledUvX - 7.0));
    float w9 = step(0.0, 0.5 - abs(scaledUvX - 8.0));
    float w10 = step(0.0, 0.5 - abs(scaledUvX - 9.0));
    float w11 = step(0.0, 0.5 - abs(scaledUvX - 10.0));

    // Compute final color by summing weighted colors
    float4 color = UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color1) * w1 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color2) * w2 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color3) * w3 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color4) * w4 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color5) * w5 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color6) * w6 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color7) * w7 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color8) * w8 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color9) * w9 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color10) * w10 +
               UNITY_ACCESS_INSTANCED_PROP(PerInstanceData, _Color11) * w11;
        
    return color;
}