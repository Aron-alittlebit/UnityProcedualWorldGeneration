using UnityEngine;

public static class MeshGenerator 
{
    public static MeshData GenerateMesh(float[,] noisemap, float heightMultiplier,
        AnimationCurve curve, int levelOfDetail)
    {
        int width = noisemap.GetLength(0);
        int height = noisemap.GetLength(1);
        float topleftx = (width-1) / -2f;
        float topleftz = (height - 1) / 2f;
        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width-1) / meshSimplificationIncrement+1;
        MeshData meshData = new(verticesPerLine, verticesPerLine);
        int i = 0;
        for(int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                meshData.vertices[i] = new Vector3(topleftx + x,
                    curve.Evaluate(noisemap[x, y])*heightMultiplier, topleftz - y);
                meshData.uvs[i] = new Vector2(x/(float)width, y/(float)height);

                if(x < width-1&&y < height - 1)
                {
                    meshData.AddTriangle(i, i + verticesPerLine + 1, i + verticesPerLine);
                    meshData.AddTriangle(i, i + 1, i + verticesPerLine + 1);
                }
                
                i++;
            }
        }

        return meshData;
    }
    
}

public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;
    int triangleIndex = 0;
    public MeshData(int width, int height)
    {
        vertices = new Vector3[(width+1) * (height+1)];
        uvs = new Vector2[(width + 1) * (height + 1)];
        triangles = new int[width*height*6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
