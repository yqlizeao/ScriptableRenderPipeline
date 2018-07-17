Shader "BaseLight/BaseLightShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags {"Queue"="Geometry" "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags{"LightMode" = "BaseLit"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal :NORMAL;
				float4 color :COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR1;
				float4 worldPos :TEXCOORD2;
				float3 worldNormal :TEXCOORD3;


			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float4 _LightDir; //directionalLight LightDirection
			uniform float4 _LightColor;//directionalLight Color
			uniform float4 _CameraPos;//
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				o.worldPos = mul(unity_ObjectToWorld,v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col += saturate(dot(normalize(i.worldNormal),_LightDir))*_LightColor;
				float3 viewDir = normalize(_CameraPos - i.worldPos);
				float3 h = normalize(_LightDir+viewDir);
				col += pow(saturate(dot(h,_LightDir)),5)*_LightColor;
				return col;
			}
			ENDCG
		}
	}
}
