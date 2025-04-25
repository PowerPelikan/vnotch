void blurUV_float(float4 sourceUV, float angle, float min, float max, float2 blur, out float4 randomizedUV)
{
	float pi = 3.141592;
	float random = frac(sin(dot(sourceUV, float2(12.9898, 78.233))) * 43758.5453);
	float noise = lerp(min, max, random);

	float4 offset = float4(sin(angle + noise)* blur.x, cos(angle + noise) * blur.y, 0, 0);

	randomizedUV = sourceUV + offset;
}