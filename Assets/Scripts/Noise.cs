using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int height, int width, int seed ,float scale, int octaves
        , float persistence, float lacunarity, Vector2 offset)
    {
        float[,] map = new float[width, height];
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;
        float halfWidth = width / 2;
        float halfHeight = height / 2;  

        System.Random rand = new System.Random(seed);

        if (scale <= 0)
            scale = 0.00001f;
        Vector2[] offsets = new Vector2[octaves];

        for(int i = 0; i < octaves; i++)
        {
            float OffsetX = rand.Next(-100000,100000)+ offset.x;
            float OffsetY = rand.Next(-100000, 100000)+offset.y;
            offsets[i] = new Vector2(OffsetX,OffsetY);
        }

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfWidth) / scale *frequency + offsets[i].x;
                    float sampleY = (y - halfHeight) / scale *frequency + offsets[i].y;
                    float PerlinValue = Mathf.PerlinNoise(sampleX, sampleY) *2 -1;
                    noiseHeight += PerlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    
                }

                if(noiseHeight > maxHeight)
                    maxHeight = noiseHeight;
                else if(noiseHeight < minHeight)
                    minHeight = noiseHeight;

                map[x, y] = noiseHeight;
            }
        }

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);
            }
        }
        return map;
    }
}
