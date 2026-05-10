using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const int MaxViewDistant = 450;
    int ChunksInView;
    public Transform Viewer;
    public static Vector2 ViewerPosition;
    int chunkSize;
    Dictionary<Vector2, TerrainChunk> Chunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunkVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        chunkSize = MapGenerator.ChunkSize - 1;
        ChunksInView = Mathf.RoundToInt(ChunksInView/MaxViewDistant);
    }

    private void Update()
    {
        ViewerPosition = new Vector2(Viewer.position.x, Viewer.position.z);
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for(int i = 0; i < terrainChunkVisibleLastUpdate.Count; i++)
        {
            terrainChunkVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunkVisibleLastUpdate.Clear();
        int currentChunkCoordX = Mathf.RoundToInt(ViewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(ViewerPosition.y / chunkSize);

        for (int y = -ChunksInView; y <= ChunksInView; y++)
        {
            for(int x = -ChunksInView; x <= ChunksInView; x++)
            {
                Vector2 chunkPos = new Vector2(currentChunkCoordX + x, currentChunkCoordY + y);
                if (Chunks.ContainsKey(chunkPos))
                {
                    Chunks[chunkPos].UpdateChunk();
                    if (Chunks[chunkPos].IsVisible)
                        terrainChunkVisibleLastUpdate.Add(Chunks[chunkPos]);
                }
                else
                {
                    Chunks.Add(chunkPos, new TerrainChunk(chunkPos, chunkSize, transform));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 Position;
        Bounds bounds;
        
        public TerrainChunk(Vector3 coord, int size, Transform parent)
        {
            Position = coord*size;
            Vector3 positonV3 = new Vector3(Position.x,0, Position.y);
            bounds = new Bounds(Position, Vector2.one * size);
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positonV3;
            meshObject.transform.localScale = Vector3.one*size/10f;
            meshObject.transform.parent = parent;
            SetVisible(false);
        }

        public void UpdateChunk()
        {
            float viewerDistance = Mathf.Sqrt(bounds.SqrDistance(ViewerPosition));
            bool visible = viewerDistance <= MaxViewDistant;
            SetVisible(visible);
        }

        public void SetVisible(bool isVisible)
        {
            meshObject.SetActive(isVisible);
        }

        public bool IsVisible => meshObject.activeSelf;
    }
}
