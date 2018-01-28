// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32953,y:32670,varname:node_3138,prsc:2|emission-2805-OUT,voffset-1721-OUT,tess-6788-OUT;n:type:ShaderForge.SFN_Vector4,id:2315,x:31783,y:31949,varname:node_2315,prsc:2,v1:0,v2:0,v3:0,v4:1;n:type:ShaderForge.SFN_Vector4,id:1530,x:32112,y:32113,varname:node_1530,prsc:2,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Lerp,id:2805,x:32261,y:32623,varname:node_2805,prsc:2|A-2315-OUT,B-452-OUT,T-1104-OUT;n:type:ShaderForge.SFN_TexCoord,id:4666,x:30478,y:32294,varname:node_4666,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:8356,x:30457,y:32663,varname:node_8356,prsc:2;n:type:ShaderForge.SFN_Sin,id:1104,x:31296,y:32470,varname:node_1104,prsc:2|IN-1527-OUT;n:type:ShaderForge.SFN_Add,id:1527,x:30966,y:32508,varname:node_1527,prsc:2|A-7857-OUT,B-5283-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6446,x:30478,y:32599,ptovrint:False,ptlb:ColorPulseFrequancy,ptin:_ColorPulseFrequancy,varname:node_6446,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:9871,x:30501,y:32463,ptovrint:False,ptlb:ColorWidth,ptin:_ColorWidth,varname:node_9871,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Divide,id:7857,x:30712,y:32406,varname:node_7857,prsc:2|A-4666-V,B-9871-OUT;n:type:ShaderForge.SFN_Multiply,id:5283,x:30712,y:32616,varname:node_5283,prsc:2|A-6446-OUT,B-8356-T;n:type:ShaderForge.SFN_Abs,id:460,x:31178,y:32791,varname:node_460,prsc:2|IN-7783-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8709,x:32074,y:33177,ptovrint:False,ptlb:VertexDistortion,ptin:_VertexDistortion,varname:node_8709,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:1721,x:32374,y:33040,varname:node_1721,prsc:2|A-81-OUT,B-8709-OUT;n:type:ShaderForge.SFN_TexCoord,id:5974,x:30082,y:32692,varname:node_5974,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:6172,x:30089,y:33093,varname:node_6172,prsc:2;n:type:ShaderForge.SFN_Add,id:4823,x:30608,y:32896,varname:node_4823,prsc:2|A-167-OUT,B-4991-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7641,x:30100,y:32967,ptovrint:False,ptlb:VertexPulseFrequancy,ptin:_VertexPulseFrequancy,varname:_ColorPulseFrequancy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:3136,x:30062,y:32860,ptovrint:False,ptlb:VertexWidth,ptin:_VertexWidth,varname:_ColorWidth_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Divide,id:167,x:30323,y:32810,varname:node_167,prsc:2|A-5974-V,B-3136-OUT;n:type:ShaderForge.SFN_Multiply,id:4991,x:30341,y:33046,varname:node_4991,prsc:2|A-7641-OUT,B-6172-T;n:type:ShaderForge.SFN_Sin,id:7783,x:30899,y:32823,varname:node_7783,prsc:2|IN-4823-OUT;n:type:ShaderForge.SFN_ValueProperty,id:211,x:31311,y:32869,ptovrint:False,ptlb:MinValue,ptin:_MinValue,varname:node_211,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Subtract,id:4942,x:31539,y:32758,varname:node_4942,prsc:2|A-460-OUT,B-211-OUT;n:type:ShaderForge.SFN_Divide,id:3635,x:31968,y:32783,varname:node_3635,prsc:2|A-5694-OUT,B-1674-OUT;n:type:ShaderForge.SFN_Vector1,id:3897,x:31634,y:32878,varname:node_3897,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:1674,x:31792,y:32834,varname:node_1674,prsc:2|A-3897-OUT,B-211-OUT;n:type:ShaderForge.SFN_Clamp01,id:5694,x:31757,y:32710,varname:node_5694,prsc:2|IN-4942-OUT;n:type:ShaderForge.SFN_Vector1,id:5723,x:30903,y:32017,varname:node_5723,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:4872,x:30903,y:31937,varname:node_4872,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:8853,x:31238,y:32070,varname:node_8853,prsc:2|A-5723-OUT,B-6416-OUT;n:type:ShaderForge.SFN_Add,id:4415,x:31238,y:31930,varname:node_4415,prsc:2|A-4872-OUT,B-6416-OUT;n:type:ShaderForge.SFN_Sin,id:159,x:31405,y:31914,varname:node_159,prsc:2|IN-4415-OUT;n:type:ShaderForge.SFN_Sin,id:1654,x:31405,y:32070,varname:node_1654,prsc:2|IN-8853-OUT;n:type:ShaderForge.SFN_Append,id:452,x:31671,y:32049,varname:node_452,prsc:2|A-159-OUT,B-1654-OUT,C-4786-OUT;n:type:ShaderForge.SFN_Time,id:3643,x:30346,y:32074,varname:node_3643,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3667,x:30566,y:32046,varname:node_3667,prsc:2|A-2676-OUT,B-3643-T;n:type:ShaderForge.SFN_Add,id:6416,x:30903,y:32191,varname:node_6416,prsc:2|A-3667-OUT,B-7857-OUT;n:type:ShaderForge.SFN_Sin,id:4786,x:31255,y:32215,varname:node_4786,prsc:2|IN-6416-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2676,x:30041,y:31933,ptovrint:False,ptlb:RainbowFreq,ptin:_RainbowFreq,varname:node_2676,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_NormalVector,id:3653,x:31985,y:32943,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:81,x:32189,y:32848,varname:node_81,prsc:2|A-3635-OUT,B-3653-OUT;n:type:ShaderForge.SFN_Vector1,id:6788,x:32682,y:33098,varname:node_6788,prsc:2,v1:100;proporder:6446-9871-8709-7641-3136-211-2676;pass:END;sub:END;*/

Shader "Shader Forge/CylinderPulse" {
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
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform float _ColorPulseFrequancy;
            uniform float _ColorWidth;
            uniform float _VertexDistortion;
            uniform float _VertexPulseFrequancy;
            uniform float _VertexWidth;
            uniform float _MinValue;
            uniform float _RainbowFreq;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_6172 = _Time;
                v.vertex.xyz += (((saturate((abs(sin(((o.uv0.g/_VertexWidth)+(_VertexPulseFrequancy*node_6172.g))))-_MinValue))/(1.0-_MinValue))*v.normal)*_VertexDistortion);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return 100.0;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_3643 = _Time;
                float node_7857 = (i.uv0.g/_ColorWidth);
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
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform float _VertexDistortion;
            uniform float _VertexPulseFrequancy;
            uniform float _VertexWidth;
            uniform float _MinValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_6172 = _Time;
                v.vertex.xyz += (((saturate((abs(sin(((o.uv0.g/_VertexWidth)+(_VertexPulseFrequancy*node_6172.g))))-_MinValue))/(1.0-_MinValue))*v.normal)*_VertexDistortion);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return 100.0;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
