using UnityEngine;
using System.Collections;

public static class GUIUtilities{
	
	public static Rect ConvertToScreenCoords(Rect normal)
	{
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		float x = screenWidth * (normal.xMin - normal.width * 0.5f);
		float y = screenHeight * (normal.yMin - normal.height * 0.5f);
		float width = screenWidth * normal.width;
		float height = screenHeight * normal.height;
		return new Rect(x, y, width, height);
	}
}