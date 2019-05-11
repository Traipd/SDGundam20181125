Shader "SDGundam/Effect/LaserLightIn" {
	Properties {
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_VerticalColor ("Vertical Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_AlphaScale ("Alpha Scale", Range(0, 1)) = 1
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		Pass {
			Tags { "LightMode"="ForwardBase" }

			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			
			fixed4 _Color;
			fixed4 _VerticalColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _AlphaScale;
			
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
			
			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 viewDir=normalize(_WorldSpaceCameraPos.xyz-i.worldPos.xyz);
				
				fixed4 texColor = tex2D(_MainTex, i.uv);
				
				// fixed3 albedo = fixed3(texColor.r,texColor.r,texColor.r) * _Color.rgb;

				fixed3 albedo = lerp(_VerticalColor.rgb,fixed3(texColor.r,texColor.r,texColor.r) * _Color.rgb,pow(max(0,dot(viewDir,worldNormal)),3));
				
				//fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				
				// fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

				fixed alpha= saturate(texColor.r* _AlphaScale+min(1,dot(viewDir,worldNormal)))  ;
				
				return fixed4(albedo, pow(alpha,3));//texColor.a * _AlphaScale
			}
			
			ENDCG
		}
	} 
	FallBack "Transparent/VertexLit"
}
