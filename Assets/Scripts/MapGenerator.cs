using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int Height;
    public int Width;
    public float Scale;
    public int Octaves;
    public float Persistence;
    public float Lacunarity;
    public Vector2 Offset;
    public int Seed;

    public bool AutoUpdate;
    public void GenerateMap()
    {

        float[,] map = Noise.GenerateNoiseMap(Height, Width, Seed, Scale, Octaves,
            Persistence, Lacunarity, Offset);

        MapDisplayer displayer = FindFirstObjectByType<MapDisplayer>();
        displayer.DrawNoiseMap(map);
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
