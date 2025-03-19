/*{
    "CATEGORIES": [
        "Geometry"
    ],
    "CREDIT": "Renaud Rubiano, Benoît Lahoz",
    "DESCRIPTION": "Zoom, Mirror and Translate, mirror zoom a texture according to time.",
    "INPUTS": [
        {
            "LABEL": "Image",
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "DEFAULT": 0,
            "LABEL": "Clock",
            "MIN": [
                0,
                0
            ],
            "NAME": "clock",
            "TYPE": "point2D"
        },
        {
            "DEFAULT": false,
            "LABEL": "Flip Horizontally",
            "NAME": "mirror_x",
            "TYPE": "bool"
        },
        {
            "DEFAULT": false,
            "LABEL": "Flip Vertically",
            "NAME": "mirror_y",
            "TYPE": "bool"
        },
        {
            "DEFAULT": 0,
            "LABEL": "Enable Zoom",
            "NAME": "zoom_mode",
            "TYPE": "bool"
        },
        {
            "DEFAULT": [
                100,
                100
            ],
            "LABEL": "Zoom",
            "MAX": [
                1000,
                1000
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "zoom_value",
            "TYPE": "point2D"
        }
    ],
    "ISFVSN": "2"
}
*/

/**
 * If `zoom_mode` is activated: scales the texture according to zoom value in x and y.
 * NB: to have more accuracy, zoom input is expressed in percents and normalized in the function.
 */
vec2 zoom(vec2 position) {
	if (zoom_mode){
		position *= 1. / (zoom_value / 100.);
	} 
	
	return position;
}

/**
 * Mirrors alternatively one row / column of texture if enabled.
 */
vec2 mirror (vec2 position) {
	if (mirror_x) {
		float pos_x = mod(position.x, 2.);
		if (pos_x >= 1.){
			pos_x = mix(1.0, 0.0, pos_x - 1.);
			position.x = pos_x;
		}
	}
	
	if (mirror_y) {
		
		float pos_y = mod(position.y, 2.);
		if (pos_y > 1.){
			pos_y = mix(1., 0., mod (pos_y, 1.));
			position.y = pos_y;
		}
	}
	
	return position;
}

void main()	{
	// Get initial fragment position.
	vec2 position = isf_FragNormCoord.xy;
	
	// Apply zoom if enabled.
	position = zoom(position);
	
	// Translate texture. 
	position += clock;
	
	// Apply mirror if enabled.
	position = mirror(position);
	
	// Repeat texture on translation.
	position = mod(position, 1.);
	
	

	// Apply `inputImage` color at computed position.
	gl_FragColor = IMG_NORM_PIXEL(inputImage, position);
	
}
