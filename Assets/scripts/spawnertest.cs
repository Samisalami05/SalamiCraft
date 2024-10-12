using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class spawnertest : MonoBehaviour
{
    public int width = 50;
    public int height = 50;

    public int scale;

    public float xoffset = 0f;
    public float yoffset = 0f;

    public GameObject grass;

    public List<GameObject> treetypelist;

    public int treespawnchance = 50;


    void Start()
    {
        xoffset = Random.Range(0f, 99999f);
        yoffset = Random.Range(0, 99999f);
        scale = width / 64;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float xCoord = (float)i / width * scale + xoffset;
                float yCoord = (float)j / height * scale + yoffset;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                int heights = (int)(sample*30);

                Instantiate(grass, new Vector3(i, heights, j), Quaternion.Euler(-90, 0, 0));

                int treerand = Random.Range(1, treespawnchance);
                if (treerand == 1)
                {
                    int treetyperand = Random.Range(0, treetypelist.Count - 1);

                    Instantiate(treetypelist[treetyperand], new Vector3(i, heights + 1, j), Quaternion.identity);
                }
            }
        }
    }

    private void Update()
    {
        
    }

    
}
