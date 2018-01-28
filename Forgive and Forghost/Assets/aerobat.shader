// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShadyBusiness/aerobat" {
	Properties {
		_MainTex ("Input", 2D) = "" {}
		_SelfTex ("Self", 2D) = ""{}
		_Noise ("Noise", 2D) = "" {}
		_Dispersion("Dispersion", Float) = 0.001
		_StartAlpha("StartAlpha", Float) = 0.5
		_FadeValue("FadeValue", Float) = 0.01
		_WhiteShiftValue("FadeValue", Float) = 0.01
		_WindX("WindX", Float) = 0.0
		_WindY("WindY", Float) = 0.0
	}
	
	Subshader {
		  ZTest Always Cull Off ZWrite Off
		  Fog { Mode off }  
		  ColorMask RGBA
	    
  		Pass {   			
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
                float4 pos : SV_POSITION;
                fixed3 color : COLOR0;
				fixed4 screenPos : COLOR1;
            };

			sampler2D _MainTex;
			float _StartAlpha;
			uniform float4 _MainTex_TexelSize;

			v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.color = v.normal * 0.5 + 0.5;
				o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

			float4 frag (v2f i) : SV_Target {
				float4 output = tex2D(_MainTex, i.screenPos);
				output.a *= _StartAlpha;
				return output;
			}
			ENDCG
		}
		Pass {			   
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
                float4 pos : SV_POSITION;
                fixed3 color : COLOR0;
				fixed4 screenPos : COLOR1;
            };

			uniform float4 _MainTex_TexelSize;
			sampler2D _MainTex;
			sampler2D _Noise;
			float _Dispersion;
			float _FadeValue;
			float _WhiteShiftValue;
			float _WindX;
			float _WindY;

			v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
				
                o.color = v.normal * 0.5 + 0.5;
				o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

			float4 frag (v2f i) : SV_Target {
				float4 noiseInput = tex2D(_Noise, float2(i.screenPos.x + _Time.x, i.screenPos.y + _Time.x));
				float offsetX = _Dispersion * ((noiseInput.r*2.0)-1.0 + _WindX) / (_ScreenParams.x / _ScreenParams.y);
				float offsetY = _Dispersion * ((noiseInput.g*2.0)-1.0 + _WindY);
				float4 output = tex2D(_MainTex, i.screenPos + float2(offsetX, offsetY));
				output.r += output.a * _WhiteShiftValue * unity_DeltaTime.x;
				output.g += output.a * _WhiteShiftValue * unity_DeltaTime.x;
				output.b += output.a * _WhiteShiftValue * unity_DeltaTime.x;
				output.a -= _FadeValue * unity_DeltaTime.x;
				return output;
			}
			ENDCG
		}
				Pass{
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
#pragma fragmentoption ARB_precision_hint_fastest
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				fixed3 color : COLOR0;
				fixed4 screenPos : COLOR1;
			};

			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			float _StartAlpha;
			float _Antialiasing;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.normal * 0.5 + 0.5;
				o.screenPos = v.texcoord;
				if(_Antialiasing > 0)
					o.screenPos.y = 1 - o.screenPos.y;
				return o;
			}

			float4 frag(v2f i) : SV_Target{
				float4 output = tex2D(_MainTex, i.screenPos);
				output.a *= _StartAlpha;
				return output;
			}
				ENDCG
			}

			Pass{

				Blend One Zero
				CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				fixed3 color : COLOR0;
				fixed4 screenPos : COLOR1;
			};

			sampler2D _MainTex;
			half4 _MainTex_TexelSize;
			float _Antialiasing;
			float _StartAlpha;

			sampler2D _SmokeMaskTex;
 

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.normal * 0.5 + 0.5;
				o.screenPos = v.texcoord;
				if(_Antialiasing > 0)
					o.screenPos.y = 1 - o.screenPos.y;
				return o;
			}

			float4 frag(v2f i) : SV_Target{
				float4 output = tex2D(_MainTex, i.screenPos);
				output.a *= _StartAlpha;
				float4 mask = tex2D(_SmokeMaskTex, i.screenPos);
				if(mask.a >.9 || mask.a < .1){
					return (0,0,0,0);
				}
				return output;
			}
			ENDCG
	}
	}

Fallback off
	
} // shader