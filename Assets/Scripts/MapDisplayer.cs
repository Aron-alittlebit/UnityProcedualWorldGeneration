using UnityEngine;


public class MapDisplayer : MonoBehaviour
{
    public Renderer textureRenderer;
    public void DrawMap(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    
}
