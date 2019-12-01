Shader "Jim/ColImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Row("row",float) = 4
		_XIndex("_XIndex",float) = 4

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

			// float _Col;
			float _Row;
			// float _MouseUV_X;
			// float _MouseUV_Y;

            float _XIndex;

			// float _Speed;

			// uniform float _ListPoint[100];
			// uniform float _Length;

            uniform float _ListSpeed[100];
			uniform float _SpeedLength;

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

				float currentXIndex = int(i.uv.x / xGap);

                for (int j=0; j<_SpeedLength; j++)
				{
                    float alpha = _ListSpeed[currentXIndex];
					return float4(1,1,1,1)*alpha;
				}
				return float4(0,0,0,0);
			}
			ENDCG
		}
	}
}
