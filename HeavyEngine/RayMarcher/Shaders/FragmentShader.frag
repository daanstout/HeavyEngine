#version 330 core

out vec4 FragColor;

struct WorldObject {
	vec3 Pos;
	vec4 Color;
	float Radius;
};

struct Camera {
	vec3 Pos;
	vec3 Dir;
	mat4 Transform;
	int FoV;
	vec2 ScreenSize;
};

uniform WorldObject WO;
uniform Camera Cam;
uniform int MaxStepCount;
uniform mat4 InvViewProj;

struct Ray {
	vec3 Pos;
	vec3 Dir;
	int steps;
};

float SphereDistance(vec3 posSphere, vec3 posRay, float rad){
	return length(posSphere - posRay) - rad;
}

vec4 MarchRay(Ray ray){
	while(ray.steps < MaxStepCount){
		float dist = SphereDistance(WO.Pos, ray.Pos, WO.Radius);

		if(abs(dist) < 0.00001) {
			
			return WO.Color * (1.0 - (ray.steps / 25.0));
		}

		ray.Pos += (ray.Dir * dist);
		ray.steps++;
	}

	return vec4(0, 0, 0, 1);
}

vec3 ScreenToWorld(vec4 fragCoord, vec2 screenSize, mat4 inverseViewProj) {
	vec4 ndc = vec4(
		(fragCoord.x / screenSize.x - 0.5) * 2.0,
		(fragCoord.y / screenSize.y - 0.5) * 2.0,
		(fragCoord.z - 0.5) * 2.0,
		1.0
	);

	vec4 clip = inverseViewProj * ndc;
	return (clip / clip.w).xyz;
}

Ray ConstructRay(vec3 raypos, Camera camera) {
	Ray ray;

	// float xAngle, yAngle;
	// xAngle = adjustedDistance.x / camera.ScreenSize.x;
	// yAngle = adjustedDistance.y / camera.ScreenSize.y;
	// xAngle *= camera.FoV;
	// yAngle *= camera.FoV;

	ray.Pos = raypos;
	ray.Dir = vec3(0, 0, 1);
	ray.steps = 0;

	return ray;
}

void main() {
	vec3 rayPos = ScreenToWorld(gl_FragCoord, Cam.ScreenSize, InvViewProj);
	Ray ray = ConstructRay(rayPos, Cam);

	vec4 pixelColor = MarchRay(ray);

	FragColor = pixelColor;
}