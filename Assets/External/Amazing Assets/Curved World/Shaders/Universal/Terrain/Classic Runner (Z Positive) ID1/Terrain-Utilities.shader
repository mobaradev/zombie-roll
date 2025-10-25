// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/Amazing Assets/Curved World/Terrain/Classic Runner (Z Positive) ID 1/Utilities"
{
    SubShader
    {
        Pass
        {
            Name "Picking"
            Tags { "LightMode" = "Picking" }

            BlendOp Add
            Blend One Zero
            ZWrite On
            Cull Off

            CGPROGRAM
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"


            #pragma target 3.0

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma multi_compile_instancing

            #pragma vertex vertEditorPass
            #pragma fragment fragScenePickingPass


            #define CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
            #define CURVEDWORLD_BEND_ID_1            

            #include "../../../Core/SceneSelection.cginc" 
            ENDCG
        }

        Pass
        {
            Name "SceneSelectionPass"
            Tags { "LightMode" = "SceneSelectionPass" }

            BlendOp Add
            Blend One Zero
            ZWrite On
            Cull Off

            CGPROGRAM
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"


            #pragma target 3.0

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma multi_compile_instancing

            #pragma vertex vertEditorPass
            #pragma fragment fragSceneHighlightPass


            #define CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
            #define CURVEDWORLD_BEND_ID_1

            #include "../../../Core/SceneSelection.cginc" 
            ENDCG
        }
    }
}
