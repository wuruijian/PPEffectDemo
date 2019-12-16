Shader "Jim/RayImageEffectShader2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Row("row",float) = 4
		_XIndex("_XIndex",float) = 4
        _yHeight("_yHeight",float) = 0
		_Speed("_Speed",float) = 0
		_CenterPoint("_CenterPoint",Vector) = (0,0,0,0)
		_Radius("_Radius",float) = 1
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

			float _Row;

            float _XIndex;
            float _yHeight;

            uniform float _ListSpeed[25];
			uniform float _listLenList[25];
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

				for (int j=0; j<_SpeedLength; j++){
					float gap = 1.0 / _SpeedLength;
					//分行
					if(i.uv.y < j*gap && i.uv.y > j*gap-0.01){
						//每一行中间插入运动的空格
						if(i.uv.x < _ListSpeed[j] + _listLenList[j] && i.uv.x > _ListSpeed[j]){
							return float4(0,0,0,1);
						}
						return float4(1,1,1,1);
					}
				}
				return float4(0,0,0,0);
			}
			ENDCG
		}
	}
}
