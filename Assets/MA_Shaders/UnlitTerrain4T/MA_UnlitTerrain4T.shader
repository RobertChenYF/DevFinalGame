// Shader by Maxartz15/Max Kruf
// Made with Shader Sandwich

Shader "MA/UnlitTerrain4T" 
{
    Properties 
    {
        [HideInInspector]Texcoord ("Generic UV Coords (You shouldn't be seeing this aaaaah!)", 2D) = "white" {}
        [HideInInspector]_Splat0 ("_Splat0", 2D) = "white" {}
        [HideInInspector]_Splat1 ("_Splat1", 2D) = "white" {}
        [HideInInspector]_Splat2 ("_Splat2", 2D) = "white" {}
        [HideInInspector]_Splat3 ("_Splat3", 2D) = "white" {}
        [HideInInspector]_Control ("_Control", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "Queue"="Geometry" 
        }
        LOD 200
        AlphaToMask Off
        Pass 
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            ZTest LEqual
            Blend Off
            Cull Back
        
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment FragmentShader
            #pragma target 3.0
            #pragma multi_compile_fog
            #pragma multi_compile __ UNITY_COLORSPACE_GAMMA
            #pragma multi_compile_fwdbase noshadow nolightmap nodynlightmap novertexlight
            #include "HLSLSupport.cginc"
            #include "UnityShaderVariables.cginc"
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"

            #define INTERNAL_DATA
            #define WorldReflectionVector(data,normal) data.worldRefl
            #define WorldNormalVector(data,normal) normal
            #pragma glsl
            #ifndef LIGHTMAP_OFF
            #define LIGHTMAP_OFF
            #endif

            sampler2D _Splat0;
            float4 _Splat0_ST;
            sampler2D _Splat1;
            float4 _Splat1_ST;
            sampler2D _Splat2;
            float4 _Splat2_ST;
            sampler2D _Splat3;
            float4 _Splat3_ST;
            sampler2D _Control;
            float4 _Control_ST;

            struct UsefulData
            {
                float3 Albedo;
                float3 Specular;
                float3 Normal;
                float Alpha;
                float Occlusion;
                float Height;
                float4 Emission;
                float Smoothness;
                float LightSmoothness;
                float Atten;
                float3 worldPos;
                fixed4 color;
                float ShellDepth;
                float Mask0;
                float Mask1;
                float Mask2;
                float Mask3;
                float2 uv_Splat0;
                float2 uv_Splat1;
                float2 uv_Splat2;
                float2 uv_Splat3;
                float2 uv_Control;
                float2 uvTexcoord;
            };

            #ifdef LIGHTMAP_OFF
            struct v2f_surf 
            {
                float4 pos : SV_POSITION;
                float3 worldPos: TEXCOORD0;
                float4 dataToPack0 : TEXCOORD1;
                float4 dataToPack2 : TEXCOORD2;
                float4 dataToPack4 : TEXCOORD3;
                fixed4 color : COLOR0;
                #if UNITY_SHOULD_SAMPLE_SH
                    half3 sh : TEXCOORD4;
                #endif
                UNITY_FOG_COORDS(5)
            };
            #endif

            struct appdata_min 
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                fixed4 color : COLOR;
            };

            float4 GammaToLinear(float4 col)
            {
                #if defined(UNITY_COLORSPACE_GAMMA)
                    //Best programming evar XD
                #else
                    col = pow(col,2.2);
                #endif
                return col;
            }

            float4 GammaToLinearForce(float4 col)
            {
                #if defined(UNITY_COLORSPACE_GAMMA)
                    //Best programming evar XD
                #else
                    col = pow(col,2.2);
                #endif
                return col;
            }

            float4 LinearToGamma(float4 col)
            {
                return col;
            }

            float4 LinearToGammaForWeirdSituations(float4 col)
            {
                #if defined(UNITY_COLORSPACE_GAMMA)
                    col = pow(col,2.2);
                #endif
                return col;
            }

            float3 GammaToLinear(float3 col)
            {
                #if defined(UNITY_COLORSPACE_GAMMA)
                    //Best programming evar XD
                #else
                    col = pow(col,2.2);
                #endif
                return col;
            }

            float3 GammaToLinearForce(float3 col)
            {
                #if defined(UNITY_COLORSPACE_GAMMA)
                    //Best programming evar XD
                #else
                    col = pow(col,2.2);
                #endif
                return col;
            }

            float3 LinearToGamma(float3 col)
            {
                return col;
            }

            v2f_surf VertShader (appdata_min v) 
            {
                v2f_surf o;
                UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
                float4 Vertex = v.vertex;
                o.pos = UnityObjectToClipPos (Vertex);
                o.dataToPack0.xy = TRANSFORM_TEX(v.texcoord, _Splat0);
                o.dataToPack0.zw = TRANSFORM_TEX(v.texcoord, _Splat1);
                o.dataToPack2.xy = TRANSFORM_TEX(v.texcoord, _Splat2);
                o.dataToPack2.zw = TRANSFORM_TEX(v.texcoord, _Splat3);
                o.dataToPack4.xy = TRANSFORM_TEX(v.texcoord, _Control);
                o.dataToPack4.zw = v.texcoord;
                o.color = v.color;
                float3 worldPos = mul(unity_ObjectToWorld, Vertex).xyz;
                o.worldPos = worldPos;
                float ShellDepth = 0;
                float Mask0 = 1;
                half4 RMask0_Sample1 = LinearToGamma(tex2Dlod(_Control,float4((((v.texcoord.xyz.xy))),0,0)));
                Mask0 = RMask0_Sample1.r;
                float Mask1 = 1;
                half4 GMask1_Sample1 = LinearToGamma(tex2Dlod(_Control,float4((((v.texcoord.xyz.xy))),0,0)));
                Mask1 = GMask1_Sample1.g;
                float Mask2 = 1;
                half4 BMask2_Sample1 = LinearToGamma(tex2Dlod(_Control,float4((((v.texcoord.xyz.xy))),0,0)));
                Mask2 = BMask2_Sample1.b;
                float Mask3 = 1;
                half4 AMask3_Sample1 = LinearToGamma(tex2Dlod(_Control,float4((((v.texcoord.xyz.xy))),0,0)));
                Mask3 = AMask3_Sample1.a;

                Vertex.w = v.vertex.w;
                v.vertex.xyz = Vertex.xyz;
                o.pos = UnityObjectToClipPos (Vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            void FragmentShader (v2f_surf IN, out fixed4 outputColor : SV_Target) 
            {
                UsefulData d;
                UNITY_INITIALIZE_OUTPUT(UsefulData,d);
                d.Albedo = float3(0.8,0.8,0.8);
                d.Specular = float3(0.3,0.3,0.3);
                d.Occlusion = 1;
                d.Emission = float4(0,0,0,0);
                d.Smoothness = 0;
                d.LightSmoothness = 0;
                d.Alpha = 1;
                d.worldPos = IN.worldPos;
                d.color = IN.color;
                d.ShellDepth = 1-0;
                float ShellDepth = d.ShellDepth;
                d.uv_Splat0 = IN.dataToPack0.xy;
                d.uv_Splat1 = IN.dataToPack0.zw;
                d.uv_Splat2 = IN.dataToPack2.xy;
                d.uv_Splat3 = IN.dataToPack2.zw;
                d.uv_Control = IN.dataToPack4.xy;
                d.uvTexcoord = IN.dataToPack4.zw;
                fixed4 c = 0;
                UnityGI gi;
                UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
                #ifndef USING_DIRECTIONAL_LIGHT
                    fixed3 lightDir = normalize(UnityWorldSpaceLightDir(d.worldPos));
                #else
                    fixed3 lightDir = _WorldSpaceLightPos0.xyz;
                #endif

                float Mask0 = 1;
                half4 RMask0_Sample1 = LinearToGamma(tex2D(_Control,(((d.uv_Control.xy)))));
                Mask0 = RMask0_Sample1.r;
                d.Mask0 = Mask0;
                float Mask1 = 1;
                half4 GMask1_Sample1 = LinearToGamma(tex2D(_Control,(((d.uv_Control.xy)))));
                Mask1 = GMask1_Sample1.g;
                d.Mask1 = Mask1;
                float Mask2 = 1;
                half4 BMask2_Sample1 = LinearToGamma(tex2D(_Control,(((d.uv_Control.xy)))));
                Mask2 = BMask2_Sample1.b;
                d.Mask2 = Mask2;
                float Mask3 = 1;
                half4 AMask3_Sample1 = LinearToGamma(tex2D(_Control,(((d.uv_Control.xy)))));
                Mask3 = AMask3_Sample1.a;
                d.Mask3 = Mask3;
                d.Smoothness = 0;
                d.LightSmoothness = 0;
                half4 splatTexture0Albedo_Sample1 = LinearToGamma(tex2D(_Splat0,(((d.uv_Splat0.xy)))));
                d.Albedo = lerp(d.Albedo,splatTexture0Albedo_Sample1.rgb,Mask0);
                half4 splatTexture1Albedo_Sample1 = LinearToGamma(tex2D(_Splat1,(((d.uv_Splat1.xy)))));
                d.Albedo = lerp(d.Albedo,splatTexture1Albedo_Sample1.rgb,Mask1);
                half4 splatTexture2Albedo_Sample1 = LinearToGamma(tex2D(_Splat2,(((d.uv_Splat2.xy)))));
                d.Albedo = lerp(d.Albedo,splatTexture2Albedo_Sample1.rgb,Mask2);
                half4 splatTexture3Albedo_Sample1 = LinearToGamma(tex2D(_Splat3,(((d.uv_Splat3.xy)))));
                d.Albedo = lerp(d.Albedo,splatTexture3Albedo_Sample1.rgb,Mask3);
                c = float4(d.Albedo+d.Emission.rgb,d.Alpha+d.Emission.a);
                UNITY_APPLY_FOG(IN.fogCoord, c);
                c.a = 1;
                outputColor = c;
            }
            ENDCG
        }
    }
    Fallback "Legacy Shaders/VertexLit"
}