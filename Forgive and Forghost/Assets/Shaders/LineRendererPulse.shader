// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-2805-OUT,voffset-6592-OUT;n:type:ShaderForge.SFN_Vector4,id:2315,x:31783,y:31949,varname:node_2315,prsc:2,v1:0,v2:0,v3:0,v4:1;n:type:ShaderForge.SFN_Vector4,id:1530,x:32112,y:32113,varname:node_1530,prsc:2,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Lerp,id:2805,x:32261,y:32623,varname:node_2805,prsc:2|A-2315-OUT,B-452-OUT,T-1104-OUT;n:type:ShaderForge.SFN_TexCoord,id:4666,x:30478,y:32294,varname:node_4666,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:8356,x:30457,y:32663,varname:node_8356,prsc:2;n:type:ShaderForge.SFN_Sin,id:1104,x:31296,y:32470,varname:node_1104,prsc:2|IN-1527-OUT;n:type:ShaderForge.SFN_Add,id:1527,x:30966,y:32508,varname:node_1527,prsc:2|A-7857-OUT,B-5283-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6446,x:30478,y:32599,ptovrint:False,ptlb:ColorPulseFrequancy,ptin:_ColorPulseFrequancy,varname:node_6446,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:9871,x:30501,y:32463,ptovrint:False,ptlb:ColorWidth,ptin:_ColorWidth,varname:node_9871,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Divide,id:7857,x:30712,y:32406,varname:node_7857,prsc:2|A-4666-U,B-9871-OUT;n:type:ShaderForge.SFN_Multiply,id:5283,x:30712,y:32616,varname:node_5283,prsc:2|A-6446-OUT,B-8356-T;n:type:ShaderForge.SFN_TexCoord,id:1852,x:30802,y:32930,varname:node_1852,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Sin,id:1501,x:31516,y:32974,varname:node_1501,prsc:2|IN-9734-OUT;n:type:ShaderForge.SFN_Multiply,id:3149,x:31132,y:32938,varname:node_3149,prsc:2|A-1852-V,B-6617-OUT;n:type:ShaderForge.SFN_Vector1,id:6617,x:30802,y:33107,varname:node_6617,prsc:2,v1:2;n:type:ShaderForge.SFN_Subtract,id:9734,x:31334,y:32958,varname:node_9734,prsc:2|A-3149-OUT,B-3544-OUT;n:type:ShaderForge.SFN_Vector1,id:3544,x:31034,y:33154,varname:node_3544,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5256,x:31880,y:33010,varname:node_5256,prsc:2|A-3635-OUT,B-7091-OUT;n:type:ShaderForge.SFN_Append,id:6592,x:32285,y:33112,varname:node_6592,prsc:2|A-624-OUT,B-1721-OUT;n:type:ShaderForge.SFN_Vector1,id:624,x:32081,y:33010,varname:node_624,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:7091,x:31637,y:33121,varname:node_7091,prsc:2|A-1501-OUT,B-2347-OUT;n:type:ShaderForge.SFN_Vector1,id:2347,x:31282,y:33154,varname:node_2347,prsc:2,v1:-1;n:type:ShaderForge.SFN_Abs,id:460,x:31178,y:32791,varname:node_460,prsc:2|IN-7783-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8709,x:31804,y:33314,ptovrint:False,ptlb:VertexDistortion,ptin:_VertexDistortion,varname:node_8709,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:1721,x:32096,y:33181,varname:node_1721,prsc:2|A-5256-OUT,B-8709-OUT;n:type:ShaderForge.SFN_TexCoord,id:5974,x:30110,y:32716,varname:node_5974,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:6172,x:30089,y:33085,varname:node_6172,prsc:2;n:type:ShaderForge.SFN_Add,id:4823,x:30612,y:32876,varname:node_4823,prsc:2|A-167-OUT,B-4991-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7641,x:30121,y:32979,ptovrint:False,ptlb:VertexPulseFrequancy,ptin:_VertexPulseFrequancy,varname:_ColorPulseFrequancy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:3136,x:30133,y:32885,ptovrint:False,ptlb:VertexWidth,ptin:_VertexWidth,varname:_ColorWidth_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Divide,id:167,x:30344,y:32828,varname:node_167,prsc:2|A-5974-U,B-3136-OUT;n:type:ShaderForge.SFN_Multiply,id:4991,x:30344,y:33038,varname:node_4991,prsc:2|A-7641-OUT,B-6172-T;n:type:ShaderForge.SFN_Sin,id:7783,x:30894,y:32791,varname:node_7783,prsc:2|IN-4823-OUT;n:type:ShaderForge.SFN_ValueProperty,id:211,x:31311,y:32869,ptovrint:False,ptlb:MinValue,ptin:_MinValue,varname:node_211,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Subtract,id:4942,x:31539,y:32758,varname:node_4942,prsc:2|A-460-OUT,B-211-OUT;n:type:ShaderForge.SFN_Divide,id:3635,x:31968,y:32783,varname:node_3635,prsc:2|A-5694-OUT,B-1674-OUT;n:type:ShaderForge.SFN_Vector1,id:3897,x:31634,y:32878,varname:node_3897,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:1674,x:31792,y:32834,varname:node_1674,prsc:2|A-3897-OUT,B-211-OUT;n:type:ShaderForge.SFN_Clamp01,id:5694,x:31757,y:32710,varname:node_5694,prsc:2|IN-4942-OUT;n:type:ShaderForge.SFN_Vector1,id:5723,x:30903,y:32017,varname:node_5723,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:4872,x:30903,y:31937,varname:node_4872,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:8853,x:31238,y:32070,varname:node_8853,prsc:2|A-5723-OUT,B-6416-OUT;n:type:ShaderForge.SFN_Add,id:4415,x:31238,y:31930,varname:node_4415,prsc:2|A-4872-OUT,B-6416-OUT;n:type:ShaderForge.SFN_Sin,id:159,x:31405,y:31914,varname:node_159,prsc:2|IN-4415-OUT;n:type:ShaderForge.SFN_Sin,id:1654,x:31405,y:32070,varname:node_1654,prsc:2|IN-8853-OUT;n:type:ShaderForge.SFN_Append,id:452,x:31671,y:32049,varname:node_452,prsc:2|A-159-OUT,B-1654-OUT,C-4786-OUT;n:type:ShaderForge.SFN_Time,id:3643,x:30346,y:32074,varname:node_3643,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3667,x:30566,y:32046,varname:node_3667,prsc:2|A-2676-OUT,B-3643-T;n:type:ShaderForge.SFN_Add,id:6416,x:30903,y:32191,varname:node_6416,prsc:2|A-3667-OUT,B-7857-OUT;n:type:ShaderForge.SFN_Sin,id:4786,x:31255,y:32215,varname:node_4786,prsc:2|IN-6416-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2676,x:30041,y:31933,ptovrint:False,ptlb:RainbowFreq,ptin:_RainbowFreq,varname:node_2676,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:6446-9871-8709-7641-3136-211-2676;pass:END;sub:END;*/

Shader "Shader Forge/LineRendererPulse" {
    Properties {
        _ColorPulseFrequancy ("ColorPulseFrequancy", Float ) = 2
        _ColorWidth ("ColorWidth", Float ) = 0.1
        _VertexDistortion ("VertexDistortion", Float ) = 0.5
        _VertexPulseFrequancy ("VertexPulseFrequancy", Float ) = 2
        _VertexWidth ("VertexWidth", Float ) = 0.1
        _MinValue ("MinValue", Float ) = 0.5
        _RainbowFreq ("RainbowFreq", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _ColorPulseFrequancy;
            uniform float _ColorWidth;
            uniform float _VertexDistortion;
            uniform float _VertexPulseFrequancy;
            uniform float _VertexWidth;
            uniform float _MinValue;
            uniform float _RainbowFreq;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_6172 = _Time;
                v.vertex.xyz += float3(float2(0.0,(((saturate((abs(sin(((o.uv0.r/_VertexWidth)+(_VertexPulseFrequancy*node_6172.g))))-_MinValue))/(1.0-_MinValue))*(sin(((o.uv0.g*2.0)-1.0))*(-1.0)))*_VertexDistortion)),0.0);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_3643 = _Time;
                float node_7857 = (i.uv0.r/_ColorWidth);
                float node_6416 = ((_RainbowFreq*node_3643.g)+node_7857);
                float4 node_8356 = _Time;
                float3 emissive = lerp(float4(0,0,0,1),float4(float3(sin((1.0+node_6416)),sin((2.0+node_6416)),sin(node_6416)),0.0),sin((node_7857+(_ColorPulseFrequancy*node_8356.g)))).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _VertexDistortion;
            uniform float _VertexPulseFrequancy;
            uniform float _VertexWidth;
            uniform float _MinValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_6172 = _Time;
                v.vertex.xyz += float3(float2(0.0,(((saturate((abs(sin(((o.uv0.r/_VertexWidth)+(_VertexPulseFrequancy*node_6172.g))))-_MinValue))/(1.0-_MinValue))*(sin(((o.uv0.g*2.0)-1.0))*(-1.0)))*_VertexDistortion)),0.0);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
