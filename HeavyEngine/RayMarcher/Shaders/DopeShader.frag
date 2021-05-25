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
//			return vec4(ray.steps / 5, ray.steps / 10, ray.steps / 15, ray.steps / 20);
			return WO.Color * (1.0 - (ray.steps / 25.0));
		}

		ray.Pos += (ray.Dir * dist);
		ray.steps++;
	}

	return vec4(0, 0, 0, 1);
}

Ray ConstructRay(vec2 adjustedDistance, Camera camera) {
	Ray ray;

	float xAngle, yAngle;
	xAngle = adjustedDistance.x / camera.ScreenSize.x;
	yAngle = adjustedDistance.y / camera.ScreenSize.y;
	xAngle *= camera.FoV;
	yAngle *= camera.FoV;

	vec4 pos = vec4(adjustedDistance, 0.0, 1.0) * camera.Transform;
	ray.Pos = pos.xyz;
	ray.Dir = vec3(sin(xAngle), sin(yAngle), 1);
	ray.steps = 0;

	return ray;
}

void main() {
	vec2 halfScreenSize = Cam.ScreenSize / 2;
	vec2 adjustedDistance = gl_FragCoord.xy - halfScreenSize;
	
	Ray ray = ConstructRay(adjustedDistance, Cam);

	vec4 pixelColor = MarchRay(ray);

	FragColor = pixelColor;
}