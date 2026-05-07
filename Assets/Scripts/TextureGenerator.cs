using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        
        Color[] map = new Color[width * height];
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                map[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x,y]);
            }
        }

        return TextureFromColourMap(map, width, height);
    }

    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D tex = new(width, height);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.SetPixels(colourMap);
        tex.Apply();
        return tex;
    } 
   
}
