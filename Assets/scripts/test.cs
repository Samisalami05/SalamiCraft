using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject dirt;
    public GameObject grass;
    public GameObject tree;
    public GameObject tree2;

    public int chunksize = 3;
    public int minheight = 3;
    public int maxheight = 5;
    public int random;
    public int mapsize = 10;
    public int treechance = 50;

    void Start()
    {
        
        
        for (int i = 0; i < mapsize; i++)
        {
            for (int j = 0; j < mapsize; j++)
            {
                random = Random.Range(minheight, maxheight);
                spawnchunk(minheight, maxheight, i * chunksize, j * chunksize, random);
            }
        }
        
    }

    void spawnchunk(int min, int max, int iof, int jof, int random)
    {
        bool hastree = false;
        for (int i = 0; i < chunksize; i++)
        {

            for (int j = 0; j < chunksize; j++)
            {

                int rand = Random.Range(random - 2, random);
                int treerand = Random.Range(1,treechance);

                

                for (int k = 0; k <= rand; k++)
                {
                    if (i == 1 && j == 1)
                    {
                        rand = random - 1;
                    }
                    if (k == rand)
                    {
                        Instantiate(grass, new Vector3(i + iof, k, j + jof), Quaternion.Euler(-90, 0, 0));
                    }

                    else if (k >= minheight - 1)
                    {
                        Instantiate(dirt, new Vector3(i + iof, k, j + jof), Quaternion.identity);
                    }
                }
                if (treerand == 1 && hastree == false)
                {
                    int treetyperand = Random.Range(1, 3);
                    if (treetyperand == 1 && hastree == false)
                    {

                        Instantiate(tree2, new Vector3(i + iof, rand + 1, j + jof), Quaternion.identity);
                        hastree = true;
                    }
                    else
                    {
                        Instantiate(tree, new Vector3(i + iof, rand + 1, j + jof), Quaternion.identity);
                        hastree = true;
                    }
                }


            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

