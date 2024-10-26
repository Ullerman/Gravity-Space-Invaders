#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Define inputs
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 texCoord : TEXCOORD0;
};

// Define global settings
float hardScan = -8.0;
float hardPix = -3.0;
float maskDark = 0.5;
float maskLight = 1.5;
float scaleInLinearGamma = 1.0;
float shadowMask = 3.0;
float brightboost = 1.0;

float2 textureSize;
float2 videoSize;
float2 outputSize;

Texture2D decal; // Define texture
SamplerState DecalSampler; // Define sampler

// Linear color adjustment
float3 ToLinear(float3 c)
{
    if (scaleInLinearGamma == 0) return c;
    return float3(
        c.r <= 0.04045 ? c.r / 12.92 : pow((c.r + 0.055) / 1.055, 2.4),
        c.g <= 0.04045 ? c.g / 12.92 : pow((c.g + 0.055) / 1.055, 2.4),
        c.b <= 0.04045 ? c.b / 12.92 : pow((c.b + 0.055) / 1.055, 2.4)
    );
}

// Apply CRT effect (simplified)
float4 crt_effect(float2 texture_size, float2 texCoord)
{
    // Calculate sample position and color, with a slight warp effect
    float2 pos = texCoord * (texture_size.xy / videoSize.xy) * 2.0 - 1.0;
    pos *= float2(1.0 + pos.y * pos.y * 0.031, 1.0 + pos.x * pos.x * 0.041);
    pos = pos * 0.5 + 0.5;

    // Sample the decal texture and apply brightboost
    float3 color = decal.Sample(DecalSampler, pos).rgb * brightboost;

    // Convert color to linear space
    color = ToLinear(color);

    // Apply shadow mask for CRT feel
    if (shadowMask == 3.0) {
        pos.x += pos.y * 3.0;
        pos.x = frac(pos.x / 6.0);
        color *= (pos.x < 0.333 ? float3(maskLight, maskDark, maskDark) :
                  pos.x < 0.666 ? float3(maskDark, maskLight, maskDark) :
                                  float3(maskDark, maskDark, maskLight));
    }

    return float4(color, 1.0);
}

// Pixel shader entry point
float4 main_fragment(VertexShaderOutput VOUT) : COLOR0
{
    return crt_effect(textureSize, VOUT.texCoord);
}

// Technique definition
technique
{
    pass
    {
        PixelShader = compile PS_SHADERMODEL main_fragment();
    }
}
