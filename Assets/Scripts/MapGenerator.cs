using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public DrawMode drawMode;
    public int Height;
    public int Width;
    public float Scale;
    public int Octaves;
    public float Persistence;
    public float Lacunarity;
    public Vector2 Offset;
    public int Seed;
    public RegionType[] Regions;

    public float HeightMultiplier;
    public AnimationCurve Curve;

    public bool AutoUpdate;
    public void GenerateMap()
    {

        float[,] map = Noise.GenerateNoiseMap(Height, Width, Seed, Scale, Octaves,
            Persistence, Lacunarity, Offset);
        Color[] colourMap = new Color[Height*Width];
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                for(int i = 0; i < Regions.Length; i++)
                {
                    if (Regions[i].height >= map[x,y])
                    {
                        colourMap[y*Width+x] = Regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplayer displayer = FindFirstObjectByType<MapDisplayer>();

        if (drawMode == DrawMode.Noise)
            displayer.DrawMap(TextureGenerator.TextureFromHeightMap(map));
        else if (drawMode == DrawMode.Coloured)
            displayer.DrawMap(TextureGenerator.TextureFromColourMap(colourMap,Width,Height));
        else if(drawMode == DrawMode.Mesh)
            displayer.DrawMesh(MeshGenerator.GenerateMesh(map, HeightMultiplier, Curve)
                ,TextureGenerator.TextureFromColourMap(colourMap, Width, Height));

    }

    private void OnValidate()
    {
        if(Width < 1)
            Width = 1;
        if(Height < 1)
            Height = 1;
        if(Lacunarity < 1)
            Lacunarity = 1;
        if(Octaves < 0)
            Octaves = 0;
    }
}

public enum DrawMode
{
    Noise,
    Coloured,
    Mesh
}

[System.Serializable]
public struct RegionType
{
    public string name;
    public Color colour;
    public float height;
}
