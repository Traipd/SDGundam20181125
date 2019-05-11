Shader "SDGundam/Effect/ExpandVertLight" {
	Properties {
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_AlphaScale ("Alpha Scale", Range(0, 1)) = 1
		_offset("Light Expand Offset",Float)=1
		_selfAlpha("Self Mesh Alpha",Range(-1, 1))=0.25
		_Frequency("Frequency",Float)=1
		_WaveLength("WaveLength",Float)=29
		_Magnityde("Magnityde",Float)=1
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		CGINCLUDE
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"		
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _AlphaScale;
			fixed _offset;
			fixed _selfAlpha;
			float _Frequency;
			float _WaveLength;
			float _Magnityde;
			
			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};
		ENDCG
		
		Pass {
			ZWrite Off Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			v2f vert(a2v v) {
				v2f o;
				float4 laterV=v.vertex+float4(v.normal,0)*_offset*0.15f*_Magnityde*max(0,sin(_Time.y*_Frequency+(v.vertex.x*v.vertex.y*v.vertex.z)*_WaveLength));
				o.pos= UnityObjectToClipPos(laterV);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);		
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, laterV).xyz;		
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, i.uv);			
				fixed3 albedo = texColor.rgb * _Color.rgb;
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 viewDir=normalize(_WorldSpaceCameraPos.xyz-i.worldPos.xyz);
				fixed alpha= saturate(texColor.a* _AlphaScale*max(0,dot(viewDir,worldNormal)));
				return fixed4(albedo, alpha);
			}
			
			ENDCG
		}
		Pass {
			ZWrite Off Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			v2f vert(a2v v) {
				v2f o;
				float4 laterV=v.vertex+float4(v.normal,0)*_offset*0.08f*_Magnityde*max(0,sin(_Time.y*_Frequency+(v.vertex.x*v.vertex.y*v.vertex.z)*_WaveLength));
				o.pos= UnityObjectToClipPos(laterV);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);	
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, laterV).xyz;				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, i.uv);			
				fixed3 albedo = texColor.rgb * _Color.rgb*1.2f;
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 viewDir=normalize(_WorldSpaceCameraPos.xyz-i.worldPos.xyz);
				fixed alpha= saturate(texColor.a* _AlphaScale*1.2f*max(0,dot(viewDir,worldNormal)));
				return fixed4(albedo, alpha);
			}
			
			ENDCG
		}
		Pass {
			ZWrite Off Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			v2f vert(a2v v) {
				v2f o;
				float4 laterV=v.vertex*0.5f+float4(v.normal,0)*0.5f*_Magnityde*sin(_Time.y*_Frequency+(v.vertex.x*v.vertex.y*v.vertex.z)*_WaveLength);//+float4(v.normal,0)*_offset
				o.pos= UnityObjectToClipPos(laterV);//+float4(v.normal,0)*0.05f
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);	
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, laterV).xyz;			
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, i.uv);			
				fixed3 albedo = texColor.rgb * _Color.rgb+fixed3(0.7f,0.7f,0.7f);
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 viewDir=normalize(_WorldSpaceCameraPos.xyz-i.worldPos.xyz);
				fixed alpha= saturate(_selfAlpha+texColor.a* _AlphaScale*max(0,dot(viewDir,worldNormal)));
				return fixed4(albedo, alpha);
			}
			
			ENDCG
		}
	} 
	FallBack "Transparent/VertexLit"
}
