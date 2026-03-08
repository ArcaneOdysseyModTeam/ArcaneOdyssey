sampler uImage0 : register(s0);
sampler uImage1 : register(s1)
{
	MagFilter = Linear;
	MinFilter = Linear;
	Mipfilter = Linear;
	AddressU = Clamp; // Stops horizontal repeat
	AddressV = Clamp; // Stops vertical repeat
};
float3 uColor; // Colour
float3 uSecondaryColor; // Rot speed red
float uOpacity;
float uSaturation; // Intensity
float uRotation; 
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1; // Size of magic circle

float4 getCol(float2 inUV)
{
	float2 uv = inUV;

	float2 uvrot = uv;
	uv.y -= .5;
	uv.x -= .5;
    
	float theta = uSecondaryColor.r * uTime;
    
	uv = uv * 2.;
	uv.x = uv.x * 4.;
    
	uvrot.x = uv.x * cos(theta) - uv.y * sin(theta);
	uvrot.y = uv.x * sin(theta) + uv.y * cos(theta);
    
	uvrot.y += .5;
	uvrot.x += .5;
    
	float4 col = tex2D(uImage1, uvrot);
    
	float3 colTint = uColor;
    
	float2 colUV = inUV;
	colUV.x = 1. - colUV.x;
    
	colUV = colUV * 4. - 2.;
    
	colUV.x = colUV.x + 0.25;
    
	colUV = clamp(colUV, float2(0., 0.), float2(1., 1.));
    
	float intensity = uSaturation * 4.;
    
    
	col.rgb = colTint * col.a * (colUV.x * intensity);
    
	float4 fragColor = float4(0., 0., 0., 0.);
    
	fragColor = float4(colUV, 0., col.a);
	fragColor = col;
    
	uv = inUV * 8. - 4.;
	uv.x = uv.x * 4.;
	float dist = clamp(distance(float2(0., 0.), uv), 0., 1.);
	dist = 1. - dist;
	float m = dist * uSaturation;
	float4 bloom = float4(m, m, m, m);
	bloom.rgb = bloom.rgb * uColor;

	if (bloom.a > fragColor.a)
	{
		fragColor.rgb = bloom.rgb;
	}
	else
	{
		fragColor.a = (fragColor.r + fragColor.g + fragColor.b) / 3.;
	}
    
	return fragColor;
}

float4 magicCircleShaderBase(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	return getCol(coords);
}

technique Technique1
{
	pass MagicCircleShaderBase
	{
		PixelShader = compile ps_2_0 magicCircleShaderBase();
	}
}