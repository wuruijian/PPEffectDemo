Shader "Jim/RectImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Row("row",float) = 4
		_Col("col",float) = 4

		_MouseUV_X("_MouseUV_X",float) = 0
		_MouseUV_Y("_MouseUV_Y",float) = 0

		_Speed("_Speed",float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _Col;
			float _Row;
			float _MouseUV_X;
			float _MouseUV_Y;

			float _Speed;

			uniform float4 _ListPoint[100];
			uniform float _Length;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float4 result;

				float xGap = 1 / _Row;
				float yGap = 1 / _Col;

				float xIndex = int(i.uv.x / xGap);
				float yIndex = int(i.uv.y / yGap);

				float2 currentMouseIndex  = float2(0,0);
				currentMouseIndex.x = int(_MouseUV_X / xGap);
				currentMouseIndex.y = int(_MouseUV_Y / yGap);

				for (int j=0; j<_Length; j++)
				{
					float4 p4 = _ListPoint[j]; // 索引取值
					if(p4.x == xIndex && p4.y == yIndex && p4.z == 1){
						return float4(1,1,1,1)*p4.w;
					}
				}

				return float4(0,0,0,0);
			}
			ENDCG
		}
	}
}
