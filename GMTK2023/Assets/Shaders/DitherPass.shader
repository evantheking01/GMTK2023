Shader "Hidden/DitherPass" 
{
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader 
    {
        ZTest Always
        
        Tags
        {
            "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct VertexData
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        TEXTURE2D(_MainTex);
        SamplerState point_clamp_sampler;
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        uniform float _Spread;
        uniform int _RedColorCount;
        uniform int _GreenColorCount;
        uniform int _BlueColorCount;
        uniform int _BayerLevel;
        
        v2f vp(VertexData v)
        {
            v2f o;
            o.vertex = TransformObjectToHClip(v.vertex.xyz);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }
        ENDHLSL

        Pass 
        {
            Name "DitherPass"
            ZTest Always
            
            HLSLPROGRAM
            #pragma vertex vp
            #pragma fragment fp
            
            static const uint bayer2[2 * 2] =
            {
                0, 2,
                3, 1
            };

            static const uint bayer4[4 * 4] =
            {
                0, 8, 2, 10,
                12, 4, 14, 6,
                3, 11, 1, 9,
                15, 7, 13, 5
            };

            static const float bayer8[8 * 8] = {
                0, 32, 8, 40, 2, 34, 10, 42,
                48, 16, 56, 24, 50, 18, 58, 26,  
                12, 44,  4, 36, 14, 46,  6, 38, 
                60, 28, 52, 20, 62, 30, 54, 22,  
                3, 35, 11, 43,  1, 33,  9, 41,  
                51, 19, 59, 27, 49, 17, 57, 25, 
                15, 47,  7, 39, 13, 45,  5, 37, 
                63, 31, 55, 23, 61, 29, 53, 21
            };

            // int mod(int x, int y)
            // {
            //     //return x - y * trunc(x/y);
            //     return y * floor(x / y);
            // }
            float mod(float a,float b)
            {
                float m=a-floor((a+0.5)/b)*b;
                return floor(m+0.5);
            }
            
            float GetBayer2(uint x, uint y)
            {
                return float(bayer2[(mod(x, 2)) + mod(y, 2) * 2]) * (1.0f / 4.0f) - 0.5f;
            }

            float GetBayer4(uint x, uint y)
            {
                return float(bayer4[mod(x, 4) + mod(y, 4) * 4]) * (1.0f / 16.0f) - 0.5f;
            }

            float GetBayer8(uint x, uint y)
            {
                return float(bayer8[mod(x, 8) + mod(y, 8) * 8]) * (1.0f / 64.0f) - 0.5f;
            }
            
            float4 fp(v2f i) : SV_TARGET
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, point_clamp_sampler, i.uv);

                uint x = i.uv.x * _MainTex_TexelSize.z;
                uint y = i.uv.y * _MainTex_TexelSize.w;

                float bayerValues = GetBayer8(x, y);
                // float bayerValues[3] = { 0, 0, 0 };
                // bayerValues[0] = GetBayer2(x, y);
                // bayerValues[1] = GetBayer4(x, y);
                // bayerValues[2] = GetBayer8(x, y);

                float4 output = col + _Spread * bayerValues;

                output.r = floor((_RedColorCount - 1.0f) * output.r + 0.5) / (_RedColorCount - 1.0f);
                output.g = floor((_GreenColorCount - 1.0f) * output.g + 0.5) / (_GreenColorCount - 1.0f);
                output.b = floor((_BlueColorCount - 1.0f) * output.b + 0.5) / (_BlueColorCount - 1.0f);
                
                return output;
            }
            ENDHLSL
        }
    }
}