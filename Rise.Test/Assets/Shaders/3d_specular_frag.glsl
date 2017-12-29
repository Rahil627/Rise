//#include 3d_header_frag.glsl
//#include 3d_uniforms.glsl

layout(location = 0) out vec4 outColor;
layout(location = 1) out vec3 outNormal;
layout(location = 2) out vec3 outPosition;
layout(location = 3) out vec4 outSpecular;
void main(void)
{
    vec4 color = texture(Texture, fragTexture);
    outColor = color * fragColor * MultiplyColor + AddColor * color.a;
    outNormal = normalize(fragNormal);
    outPosition = fragPosition;
    outSpecular = vec4(SpecularColor, SpecularShininess);
}
