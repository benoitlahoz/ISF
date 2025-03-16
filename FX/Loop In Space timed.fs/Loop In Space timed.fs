/*{
    "CATEGORIES": [
        "Geometry"
    ],
    "CREDIT": "Renaud Rubiano, Benoît Lahoz",
    "DESCRIPTION": "Zoom, Mirror and Translate a texture according to time.",
    "INPUTS": [
        {
            "LABEL": "Image",
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "DEFAULT": false,
            "LABEL": "Millumin",
            "NAME": "millumin_context",
            "TYPE": "bool"
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
            "DEFAULT": true,
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
        },
        {
            "DEFAULT": [
                0,
                0
            ],
            "LABEL": "Speed",
            "MAX": [
                1,
                1
            ],
            "MIN": [
                -1,
                -1
            ],
            "NAME": "speed_value",
            "TYPE": "point2D"
        }
    ],
    "ISFVSN": "2"
}
*/

/**
 * Input from the vertex shader, which handles the x / y flip.
 */
varying vec2 vertex_coord;

/**
 * If `zomm_mode` is activated: scales the texture according to zoom value in x and y.
 * NB: to have more accuracy, zoom input is expressed in percents and normalized in the function.
 */
vec2 zoom(vec2 _position) {
	if (zoom_mode){
		_position *= 1. / (zoom_value / 100.);
	} 
	
	return _position;
}

vec2 speed () {
	// Vertical coordinates are not handled the same way in ISF editor and in Millumin.
	float vertical_diraction = (millumin_context ? 1. : -1.);
	// Transform speed input according to texture mirroring and vertical direction.
	vec2 speed = vec2(mirror_x ? speed_value.x : -speed_value.x, (mirror_y ? speed_value.y : -speed_value.y) * vertical_diraction);

	return speed;
}

void main()	{
	
	
	// TODO: we need an accumulator of translated pixels. 
	// (ping-pong FBO-like, or external uniform)
	// 
	// ISF has two accumulators: TIME and FRAMEINDEX. 
	// How can we use that, eventually with TIMEDELTA?
	// @see: https://gamedev.stackexchange.com/a/54802/26413
	//
	// Or: multiple render passes (first stores the displacement, second applies it to current texture)...
	// But: ISF gives us the first render pass texture, then unusable as final display. 
	// Have a second image input? Usable in e.g. Millumin? 
	
	
	
	// ----- hic sunt leones -----
	vec2 accumulator = vec2(TIME) * speed();
	// ----- hic sunt leones -----
	
	

	// Get position from vertex shader.
	vec2 position = vertex_coord;

	// Apply zoom if enabled.
	position = zoom(position);
	
	// Translate texture. 
	position += accumulator;
	
	// Repeat texture on translation.
	position = mod(position, 1.);
	
	// Apply `inputImage` color at computed position.
	gl_FragColor = IMG_NORM_PIXEL(inputImage, position);;
	
}
