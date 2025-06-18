Shader "Custom/DisalbeZWrite"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        Pass
        {
            ZWrite Off
        }
    }   
}