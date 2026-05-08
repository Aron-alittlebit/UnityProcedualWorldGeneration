using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public DrawMode drawMode;
    public const int ChunkSize = 241;

    [Range(0,6)]
    public int levelOfDetail;
    
    public float Scale;
    public int Octaves;
    [Range(0,1)]
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

        float[,] map = Noise.GenerateNoiseMap(ChunkSize, ChunkSize, Seed, Scale, Octaves,
            Persistence, Lacunarity, Offset);
        Color[] colourMap = new Color[ChunkSize*ChunkSize];
        for(int y = 0; y < ChunkSize; y++)
        {
            for(int x = 0; x < ChunkSize; x++)
            {
                for(int i = 0; i < Regions.Length; i++)
                {
                    if (Regions[i].height >= map[x,y])
                    {
                        colourMap[y*ChunkSize+x] = Regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplayer displayer = FindFirstObjectByType<MapDisplayer>();

        if (drawMode == DrawMode.Noise)
            displayer.DrawMap(TextureGenerator.TextureFromHeightMap(map));
        else if (drawMode == DrawMode.Coloured)
            displayer.DrawMap(TextureGenerator.TextureFromColourMap(colourMap,ChunkSize,ChunkSize));
        else if(drawMode == DrawMode.Mesh)
            displayer.DrawMesh(MeshGenerator.GenerateMesh(map, HeightMultiplier, Curve, levelOfDetail)
                ,TextureGenerator.TextureFromColourMap(colourMap, ChunkSize, ChunkSize));

    }

    private void OnValidate()
    {
       
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
