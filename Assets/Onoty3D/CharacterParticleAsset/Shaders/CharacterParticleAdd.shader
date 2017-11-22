﻿Shader "Onoty3D/CharacterParticle/Additive" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		[Enum(OFF,0,FRONT,1,BACK,2)] _CullMode("Cull Mode", int) = 2 //OFF/FRONT/BACK
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha One
		ColorMask RGB
		Cull [_CullMode]
		Lighting Off 
		ZWrite Off

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 3.0
#pragma multi_compile_particles
#pragma multi_compile_fog

#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 custom1 : TEXCOORD1;
				float4 custom2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 area : TEXCOORD1;
				float4 region : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float4 vertex = v.vertex;
				o.vertex = UnityObjectToClipPos(vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				o.area = v.custom1;
				o.region = v.custom2;
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.texcoord;
				fixed4 col = fixed4(0,0,0,0);

				//メッシュの大きさの中で、実際に文字を描画したい範囲にのみ描画処理
				if (uv.x > i.region.z
					&& uv.x < i.region.x + i.region.z
					&& uv.y > i.region.w 
					&& uv.y < i.region.y + i.region.w
					)
				{
					//実際に文字を表示したい部分のx,yの範囲が0-1になるよう再計算
					uv.x = i.area.x + saturate((uv.x - i.region.z) * (1.0 / i.region.x)) * i.area.z;
					uv.y = i.area.y + saturate((uv.y - i.region.w) * (1.0 / i.region.y)) * i.area.w;
					col = tex2Dlod(_MainTex, float4(uv, 0, 0)) * i.color;
				}
				else
				{
					discard;
				}

				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
				return col;
			}
		ENDCG
		}
	}
}
