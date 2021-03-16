#version 330 core
// Source: https://github.com/mattdesl/webgl-wireframes

in vec3 vBarycentric;
out vec4 out_Color;

uniform vec3 stroke;
uniform vec3 fill;

uniform float time;
uniform float thickness;
uniform float secondThickness;

uniform bool dashed;
uniform float dashCount;
uniform float dashLength;
uniform bool dashOverlap;
uniform bool dashAnimate;

uniform bool seeThrough;
uniform bool insideAltColor;
uniform bool dualStroke;

uniform bool squeeze;
uniform float squeezeMin;
uniform float squeezeMax;

const float PI = 3.14159265;

// This is like
float aastep(float threshold, float dist) {
    float afwidth = fwidth(dist) * 0.5;
    return smoothstep(threshold - afwidth, threshold + afwidth, dist);
}

// This function is not currently used, but it can be useful
// to achieve a fixed width wireframe regardless of z-depth
float computeScreenSpaceWireframe(vec3 barycentric, float lineWidth) {
    vec3 dist = fwidth(barycentric);
    vec3 smoothed = smoothstep(dist * ((lineWidth * 0.5) - 0.5), dist * ((lineWidth * 0.5) + 0.5), barycentric);
    return 1.0 - min(min(smoothed.x, smoothed.y), smoothed.z);
}

// This function returns the fragment color for our styled wireframe effect
// based on the barycentric coordinates for this fragment
vec4 getStyledWireframe(vec3 barycentric) {
    // this will be our signed distance for the wireframe edge
    float d = min(min(barycentric.x, barycentric.y), barycentric.z);

    // for dashed rendering, we can use this to get the 0 .. 1 value of the line length
    float positionAlong = max(barycentric.x, barycentric.y);
    if (barycentric.y < barycentric.x && barycentric.y < barycentric.z) {
        positionAlong = 1.0 - positionAlong;
    }

    // the thickness of the stroke
    float computedThickness = thickness;

    // if we want to shrink the thickness toward the center of the line segment
    if (squeeze) {
        computedThickness *= mix(squeezeMin, squeezeMax, (1.0 - sin(positionAlong * PI)));
    }

    // if we should create a dash pattern
    if (dashed) {
        // here we offset the stroke position depending on whether it
        // should overlap or not
        float offset = 1.0 / dashCount * dashLength / 2.0;
        if (!dashOverlap) {
            offset += 1.0 / dashCount / 2.0;
        }

        // if we should animate the dash or not
        if (dashAnimate) {
            offset += time * 0.22;
        }

        // create the repeating dash pattern
        float pattern = fract((positionAlong + offset) * dashCount);
        computedThickness *= 1.0 - aastep(dashLength, pattern);
    }

    // compute the anti-aliased stroke edge
    float edge = 1.0 - aastep(computedThickness, d);

    // now compute the final color of the mesh
    vec4 outColor = vec4(0.0);
    if (seeThrough) {
        outColor = vec4(stroke, edge);
        if (insideAltColor && !gl_FrontFacing) {
            outColor.rgb = fill;
        }
    } else {
        vec3 mainStroke = mix(fill, stroke, edge);
        outColor.a = 1.0;
        if (dualStroke) {
            float inner = 1.0 - aastep(secondThickness, d);
            vec3 wireColor = mix(fill, stroke, abs(inner - edge));
            outColor.rgb = wireColor;
        } else {
            outColor.rgb = mainStroke;
        }
    }

    return outColor;
}

void main() {
    out_Color = getStyledWireframe(vBarycentric);
}
