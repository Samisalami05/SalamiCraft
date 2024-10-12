using JetBrains.Annotations;

using UnityEngine;

public class perlin_noise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public int scale = 20;

    public float xoffset = 0f;
    public float yoffset = 0f;

    void Start()
    {
        xoffset = Random.Range(0f, 99999f);
        yoffset = Random.Range(0, 99999f);
    }

    private void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = Generatetexture();
    }

    Texture2D Generatetexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Color color = GetColor(i, j);
                texture.SetPixel(i, j, color);

            }
        }
        texture.Apply();
        return texture;
    }
    Color GetColor(int i, int j)
    {
        float xCoord = (float)i / width * scale + xoffset;
        float yCoord = (float)j / height * scale + yoffset;

        float sample = Mathf.PerlinNoise(xCoord,yCoord);
        
        return new Color(sample,sample,sample);
    }
}
