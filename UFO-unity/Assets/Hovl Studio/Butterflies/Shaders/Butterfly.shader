Shader "Hovl/Particles/Butterfly"
{
	Properties
	{	
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Float) = 1.5
		_Speed("Speed", Float) = 20
		_WingPower("Wing Power", Float) = 0.2
		_Wingmaxdown("Wing max down", Range( 0 , 1)) = 0.5
		_ColumnsXRowsY("Columns X Rows Y", Vector) = (1,1,0,0)
	}

	Category 
	{
		SubShader
		{
		LOD 0
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {		
				CGPROGRAM			
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR
				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float3 ase_normal : NORMAL;
					float4 ase_texcoord1 : TEXCOORD1;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 ase_texcoord3 : TEXCOORD3;
				};

				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform float _Speed;
				uniform float _Wingmaxdown;
				uniform float _WingPower;
				uniform float4 _ColumnsXRowsY;
				uniform float4 _Color;
				uniform float _Emission;
				float3 HSVToRGB( float3 c )
				{
					float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
					float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
					return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
				}
				
				float3 RGBToHSV(float3 c)
				{
					float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
					float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
					float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
					float d = q.x - min( q.w, q.y );
					float e = 1.0e-10;
					return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				}

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					float mulTime5 = _Time.y * _Speed;
					float4 texCoord3 = v.texcoord;
					texCoord3.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float clampResult13 = clamp( ( pow( frac( ( texCoord3.x + -0.2 ) ) , 20.0 ) * 2000.0 ) , 0.0 , 1.0 );
					
					o.ase_texcoord3 = v.ase_texcoord1;

					v.vertex.xyz += ( (-1.0 + (sin( ( mulTime5 + texCoord3.z ) ) - -1.0) * (_Wingmaxdown - -1.0) / (1.0 - -1.0)) * clampResult13 * v.ase_normal * _WingPower );
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					float4 texCoord75 = i.ase_texcoord3;
					texCoord75.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
					// *** BEGIN Flipbook UV Animation vars ***
					// Total tiles of Flipbook Texture
					float fbtotaltiles74 = _ColumnsXRowsY.x * ( texCoord75.w + _ColumnsXRowsY.y );
					// Offsets for cols and rows of Flipbook Texture
					float fbcolsoffset74 = 1.0f / _ColumnsXRowsY.x;
					float fbrowsoffset74 = 1.0f / ( texCoord75.w + _ColumnsXRowsY.y );
					// Speed of animation
					float fbspeed74 = _Time[ 1 ] * 0.0;
					// UV Tiling (col and row offset)
					float2 fbtiling74 = float2(fbcolsoffset74, fbrowsoffset74);
					// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
					// Calculate current tile linear index
					float fbcurrenttileindex74 = round( fmod( fbspeed74 + texCoord75.z, fbtotaltiles74) );
					fbcurrenttileindex74 += ( fbcurrenttileindex74 < 0) ? fbtotaltiles74 : 0;
					// Obtain Offset X coordinate from current tile linear index
					float fblinearindextox74 = round ( fmod ( fbcurrenttileindex74, _ColumnsXRowsY.x ) );
					// Multiply Offset X by coloffset
					float fboffsetx74 = fblinearindextox74 * fbcolsoffset74;
					// Obtain Offset Y coordinate from current tile linear index
					float fblinearindextoy74 = round( fmod( ( fbcurrenttileindex74 - fblinearindextox74 ) / _ColumnsXRowsY.x, ( texCoord75.w + _ColumnsXRowsY.y ) ) );
					// Reverse Y to get tiles from Top to Bottom
					fblinearindextoy74 = (int)(( texCoord75.w + _ColumnsXRowsY.y )-1) - fblinearindextoy74;
					// Multiply Offset Y by rowoffset
					float fboffsety74 = fblinearindextoy74 * fbrowsoffset74;
					// UV Offset
					float2 fboffset74 = float2(fboffsetx74, fboffsety74);
					// Flipbook UV
					half2 fbuv74 = texCoord75.xy * fbtiling74 + fboffset74;
					// *** END Flipbook UV Animation vars ***
					float4 tex2DNode2 = tex2D( _MainTex, fbuv74 );
					float3 hsvTorgb60 = RGBToHSV( tex2DNode2.rgb );
					float4 texCoord3 = i.texcoord;
					texCoord3.xy = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float3 hsvTorgb58 = HSVToRGB( float3(frac( ( hsvTorgb60.x + texCoord3.w ) ),hsvTorgb60.y,hsvTorgb60.z) );
					float4 appendResult57 = (float4((( float4( hsvTorgb58 , 0.0 ) * _Color * i.color * _Emission )).rgb , ( tex2DNode2.a * _Color.a * i.color.a )));
					
					fixed4 col = appendResult57;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
}
/*ASEBEGIN
Version=18900
489;103;721;650;2701.903;547.9384;1.273909;True;False
Node;AmplifyShaderEditor.Vector4Node;76;-2340.207,-69.39697;Inherit;False;Property;_ColumnsXRowsY;Columns X Rows Y;6;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-2373.477,-327.0016;Inherit;True;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-2081.51,-40.92264;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;74;-1911.72,-326.6855;Inherit;True;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;2;-1621.598,-357.7281;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;6e5d1bc20a7a58d4e8454de78b54cc80;6e5d1bc20a7a58d4e8454de78b54cc80;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;60;-1310.42,-348.3932;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1763.956,673.9937;Inherit;True;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-1041.401,-355.4565;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1394.113,434.94;Float;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1474.827,826.22;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1239.217,436.506;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;8;-1296.268,823.9592;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;72;-884.0082,-341.2802;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;-739.812,-64.32137;Float;False;Property;_Color;Color;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-692.8993,100.5044;Float;False;Property;_Emission;Emission;2;0;Create;True;0;0;0;False;0;False;1.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;10;-1090.523,825.7833;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;20;-720.3961,182.2379;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;58;-762.2819,-334.3967;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-1027.294,503.9936;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;4;-833.6704,461.3597;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-237.4168,-46.76024;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-810.4903,837.5204;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;2000;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-848.9823,686.2422;Float;False;Property;_Wingmaxdown;Wing max down;5;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-240.6586,92.84318;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;13;-574.9629,841.1405;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;6;-601.5726,1000.028;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;56;-81.28167,-27.68152;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-573.6266,1170.955;Float;False;Property;_WingPower;Wing Power;4;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;42;-584.1919,495.214;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-338.4967,699.123;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;57;120.6144,2.836279;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;55;352.2021,319.7822;Float;False;True;-1;2;;0;7;Hovl/Particles/Butterfly;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;77;0;75;4
WireConnection;77;1;76;2
WireConnection;74;0;75;0
WireConnection;74;1;76;1
WireConnection;74;2;77;0
WireConnection;74;4;75;3
WireConnection;2;1;74;0
WireConnection;60;0;2;0
WireConnection;70;0;60;1
WireConnection;70;1;3;4
WireConnection;12;0;3;1
WireConnection;5;0;7;0
WireConnection;8;0;12;0
WireConnection;72;0;70;0
WireConnection;10;0;8;0
WireConnection;58;0;72;0
WireConnection;58;1;60;2
WireConnection;58;2;60;3
WireConnection;38;0;5;0
WireConnection;38;1;3;3
WireConnection;4;0;38;0
WireConnection;39;0;58;0
WireConnection;39;1;40;0
WireConnection;39;2;20;0
WireConnection;39;3;41;0
WireConnection;11;0;10;0
WireConnection;21;0;2;4
WireConnection;21;1;40;4
WireConnection;21;2;20;4
WireConnection;13;0;11;0
WireConnection;56;0;39;0
WireConnection;42;0;4;0
WireConnection;42;4;44;0
WireConnection;14;0;42;0
WireConnection;14;1;13;0
WireConnection;14;2;6;0
WireConnection;14;3;15;0
WireConnection;57;0;56;0
WireConnection;57;3;21;0
WireConnection;55;0;57;0
WireConnection;55;1;14;0
ASEEND*/
//CHKSM=F08609B9DCF118190FFAD82E2053BD67F8DDD937