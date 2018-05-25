// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shader Forge/EnergyShieldCollision" {
    Properties {
        _ActiveShieldTexture ("Active Shield Texture", 2D) = "white" {}
        _ActiveShieldColor ("Active Shield Color", Color) = (1,0.9999999,0,1)
        _HitShieldColor ("Hit Shield Color", Color) = (1,0,0,1)
        _HitShieldTexture ("Hit Shield Texture", 2D) = "white" {}
        _HitShieldAlpha ("Hit Shield Alpha", Range(0, 1)) = 1
        _HitPosition ("Hit Position", Vector) = (1,0,0,0)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "DisableBatching"="True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
           
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2
            #pragma target 3.0
            uniform sampler2D _ActiveShieldTexture; uniform float4 _ActiveShieldTexture_ST;
            uniform float4 _ActiveShieldColor;
            uniform float4 _HitShieldColor;
            uniform sampler2D _HitShieldTexture; uniform float4 _HitShieldTexture_ST;
            uniform float _HitShieldAlpha;
            uniform float4 _HitPosition;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _ActiveShieldTexture_var = tex2D(_ActiveShieldTexture,TRANSFORM_TEX(i.uv0, _ActiveShieldTexture));
                float4 _HitShieldTexture_var = tex2D(_HitShieldTexture,TRANSFORM_TEX(i.uv0, _HitShieldTexture));
                float3 emissive = (lerp((_ActiveShieldTexture_var.rgb*_ActiveShieldColor.rgb),(_HitShieldColor.rgb*_HitShieldTexture_var.rgb),(saturate(((1.0 - distance(_HitPosition.rgb,i.normalDir))*3.0+-1.0))*_HitShieldAlpha))*_ActiveShieldColor.a);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}