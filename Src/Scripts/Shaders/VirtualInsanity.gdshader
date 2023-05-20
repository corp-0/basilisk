shader_type canvas_item;

uniform sampler2D mask_texture: hint_default_black;
uniform float intensity: hint_range(0.4, 1.6) = 0.5;
uniform float blur: hint_range(0.0, 0.1) = 0.01;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

void fragment() {
    vec2 uv = SCREEN_UV;
    
    // Shift the UV coordinates so that the center of the texture is at (0,0)
    uv = uv - vec2(0.5, 0.5);

    // Scale the UV coordinates
    uv *= intensity;
    
    // Shift the UV coordinates back so that the center of the texture is at (0.5,0.5)
    uv = uv + vec2(0.5, 0.5);

    // Apply the blur effect
    vec4 mask = vec4(0.0);
    for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
            vec2 offset = vec2(float(x), float(y)) * blur;
            mask += texture(mask_texture, uv + offset) / 9.0;
        }
    }
    
    vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
    
    COLOR = mix(color, vec4(0.0, 0.0, 0.0, color.a), 1.0 - mask.r);
}
