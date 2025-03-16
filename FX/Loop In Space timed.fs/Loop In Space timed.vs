varying vec2 vertex_coord;

void main()
{
	isf_vertShaderInit();
	vertex_coord = isf_FragNormCoord * vec2(mirror_x ? -1. : 1., mirror_y ? -1. : 1.);
}
