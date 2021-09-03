#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

//The Sobel Effect runs by sampling the texture around a point to see if there a are any large changes. Each sample is multiplied by a convolution
//matrix weight for the x and y components seperately. Each value is then added together, and the final sobel value is the length of the resulting
//float2. Higher values mean the algortihm detected more of an edge. (float2 is a pair of floats and is GPU code that defines SIMD datatypes.)

//These are points to sample relative to the starting point. These coordinates are the sample offsets of each cell in the matrix relative to the central
//pixel.
static float2 sobelSamplePoints[9] = {
	float2(-1,1),	float2(0,1),	float2(1,1),
	float2(-1,0),	float2(0,0),	float2(1,1),
	float2(-1,-1),	float2(0,-1),	float2(1,-1),
};

//Weights for the x components:
static float sobelXMatrix[9] = {
	1,	0,	-1,
	2,	0,	-2,
	1,	0,	-1,
};

//Weights for the y components:
static float sobelYMatrix[9] = {
	1,	2,	1,
	0,	0,	0,
	-1,	-2,	-1,
};

//This function runs the sobel algortihm over the depth texture. The float suffix tells the shader graph what types of numbers we will be using.
//function arguments serve the following purpose:
//		float2 UV: The is the UV position to place the convolution matrix over.
//		float Thickness: A thickness value to define the sample distance of the convolution matrix.
//		float Out: This is the final value that we will output after getting the length of the vector from the horizontal and vertical matrix results
//		squared.
void DepthSobel_float(float2 UV, float Thickness, out float Out) {
	float2 sobel = 0;
	//We can unroll this loop to make it more efficient. The compiler is also smart enough to remove the 1=4 iteration which is always zero.
	//The loop works as follows:
	//	To read the depth texture shader graph defines the 'SHADERGRAPH_SAMPLE_SCENE_DEPTH' macro for us. We than compute the sample UV by adding by the
	//	matrix offset than multiply the depth value by the horizontal and vertical matrix weights. Make sure that before you return you grab the length
	//	of the sobel vector.
	[unroll] for (int i = 0; i < 9; i++) {
		float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
		sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
	}
	//Get the final sobel value
	Out = length(sobel);
}

#endif