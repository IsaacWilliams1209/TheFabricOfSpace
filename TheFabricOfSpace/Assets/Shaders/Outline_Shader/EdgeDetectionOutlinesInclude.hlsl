#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

//The Sobel Effect runs by sampling the texture around a point to see if there a are any large changes. Each sample is multiplied by a convolution
//matrix weight for the x and y components seperately. Each value is then added together, and the final sobel value is the length of the resulting
//float2. Higher values mean the algortihm detected more of an edge. (float2 is a pair of floats and is GPU code that defines SIMD datatypes.)

//These are points to sample relative to the starting point. These coordinates are the sample offsets of each cell in the matrix relative to the central
//pixel.
static float2 sobelSamplePoints[9] = {
	float2(-0.707,0.707) ,	float2(0,1) ,	float2(0.707, 0.707),
	float2(-1,0)		 ,	float2(0,0) ,	float2(1,0)         ,
	float2(-0.707,-0.707),	float2(0,-1),	float2(0.707,-0.707),
};

//Weights for the x components:
static float sobelXMatrix[9] = {
	1,	0,	-1,
	2,	0,	-2,
	1,	0,	-1
};

//Weights for the y components:
static float sobelYMatrix[9] = {
	1,	2,	1,
	0,	0,	0,
	-1,	-2,	-1
};

//This function runs the sobel algortihm over the depth texture. The float suffix tells the shader graph what types of numbers we will be using.
//function arguments serve the following purpose:
//		float2 UV: The is the UV position to place the convolution matrix over.
//		float Thickness: A thickness value to define the sample distance of the convolution matrix.
//		float Out: This is the final value that we will output after getting the length of the vector from the horizontal and vertical matrix results
//		squared.
void DepthSobel_float(float2 UV, float Thickness, out float depthDiff, out float3 debugValue) {
	float2 sobel = 0;
	float aspectRatio = _ScreenParams.x / _ScreenParams.y;
	debugValue = float3(1, 0, 0);

	//Campus soble coordinates. [NOTE] : See if this can be moved out of the function.
	float depthNorthW = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[0].x / aspectRatio, sobelSamplePoints[0].y) * Thickness);
	float depthNorth  = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[1].x / aspectRatio, sobelSamplePoints[1].y) * Thickness);
	float depthNorthE = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[2].x / aspectRatio, sobelSamplePoints[2].y) * Thickness);
	float depthWest   = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[3].x / aspectRatio, sobelSamplePoints[3].y) * Thickness);
	float depthCenter = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[4].x / aspectRatio, sobelSamplePoints[4].y) * Thickness);
	float depthEast   =	SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[5].x / aspectRatio, sobelSamplePoints[5].y) * Thickness);
	float depthSouthW = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[6].x / aspectRatio, sobelSamplePoints[6].y) * Thickness);
	float depthSouth  =	SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[7].x / aspectRatio, sobelSamplePoints[7].y) * Thickness);
	float depthSouthE = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + float2(sobelSamplePoints[8].x / aspectRatio, sobelSamplePoints[8].y) * Thickness);

	sobel += depthNorthW * float2(sobelXMatrix[0], sobelYMatrix[0]);
	sobel += depthNorth  * float2(sobelXMatrix[1], sobelYMatrix[1]);
	sobel += depthNorthE * float2(sobelXMatrix[2], sobelYMatrix[2]);
	sobel += depthWest   * float2(sobelXMatrix[3], sobelYMatrix[3]);
	sobel += depthCenter * float2(sobelXMatrix[4], sobelYMatrix[4]);
	sobel += depthEast   * float2(sobelXMatrix[5], sobelYMatrix[5]);
	sobel += depthSouthW * float2(sobelXMatrix[6], sobelYMatrix[6]);
	sobel += depthSouth  * float2(sobelXMatrix[7], sobelYMatrix[7]);
	sobel += depthSouthE * float2(sobelXMatrix[8], sobelYMatrix[8]);

	float horizontal = (depthNorth + depthSouth) / 2;
	float vertical = (depthWest + depthEast) / 2;

	if (abs(horizontal - depthCenter) < 0.00001) {
		debugValue = float3(0, sobel.y, 0);
		depthDiff = length(debugValue);
	}
	depthDiff = length(sobel);


	////Horizontal Detection (1, 4, 7):
	//float2 horizontalMeanVec = { (depthNorth.x + centerDepth.x + depthSouth.x) / 3 , (depthNorth.y + centerDepth.y + depthSouth.y) / 3 };

	////Vertical Detection (3, 4, 5):
	//float2 verticalMeanVec = { (depthWest.x + centerDepth.x + depthEast.x) / 3, (depthWest.y + centerDepth.y + depthEast.y) / 3 };

	//We can unroll this loop to make it more efficient. The compiler is also smart enough to remove the 1=4 iteration which is always zero.
	//The loop works as follows:
	//	To read the depth texture shader graph defines the 'SHADERGRAPH_SAMPLE_SCENE_DEPTH' macro for us. We than compute the sample UV by adding by the
	//	matrix offset than multiply the depth value by the horizontal and vertical matrix weights. Make sure that before you return you grab the length
	//	of the sobel vector.
	//[unroll] for (int i = 0; i < 9; i++) {
	//	if (length(verticalMeanVec) < 0.5) {
	//		float2 thisUVs = sobelSamplePoints[i];
	//		thisUVs.x /= aspectRatio;
	//		float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + thisUVs * Thickness);
	//		sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
	//	}

	//}
	//Get the final sobel value
	//depthDiff = length(sobel);
	//corner = verticalMeanVec;
}

//This function runs the sobel algorithm over the opaque texture
void ColorSobel_float(float2 UV, float Thickness, out float Out) {
	//We have to run the sobel algorithm over the RGB channels seperately
	float2 sobelR = 0;
	float2 sobelG = 0;
	float2 sobelB = 0;

	[unroll] for (int i = 0; i < 9; i++) {
		//Sample the scene color texture
		float3 rgb = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV + sobelSamplePoints[i] * Thickness);
		//Create the kernel for the iteration
		float2 kernel = float2(sobelXMatrix[i], sobelYMatrix[i]);
		//Accumalte samples for each color
		sobelR += rgb.r * kernel;
		sobelG += rgb.g * kernel;
		sobelB += rgb.b * kernel;
	}
	//Get the final sobel value
	//Combine the RGB values by taking the one with the largest sobel value
	Out = max(length(sobelR), max(length(sobelG), length(sobelB)));
}

#endif