using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;


public class ChunkGenerator : MonoBehaviour
{
    public static int width = 16;
    public static int height = 64;
    public static int depth = 16;

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> tris = new List<int>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uv = new List<Vector2>();

    public MeshFilter meshfilter;
    public MeshRenderer meshrenderer;
    

    public MeshCollider meshcollider;
    

    public Color grasscolor = Color.green;

    //public int Width { get { return width; } }

    List<char> blocklist = new List<char>();

    Vector2Int texturequantity = new Vector2Int(4, 4);

    public float noisescale = 400f;

    public Vector2 pos;

    private GameObject EmptyObj;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }

    public void addcomponents()
    {
        meshfilter = gameObject.AddComponent<MeshFilter>();
        meshrenderer = gameObject.AddComponent<MeshRenderer>();
        meshcollider = gameObject.AddComponent<MeshCollider>();

        EmptyObj = new GameObject("transparent");
        EmptyObj.transform.parent = this.gameObject.transform;

        
    }
    

    public void generatemesh(Material material)
    {
        vertices.Clear();
        tris.Clear();
        uv.Clear();
        normals.Clear();

       


        meshrenderer.material = material;
        
        

        var mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    int blockindex = blocklist[getindex(x, y, z)];
                    if (blockindex != 0)
                    {
                        test(x, y, z, blockindex);
                    }
                }
            }
        }


        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();
        meshfilter.sharedMesh = mesh;

        

        meshcollider.sharedMesh = mesh;
        

        transform.position = new Vector3(pos.x * width, 0, pos.y * depth);
    }
    public void generateworld(GameObject tree)
    {


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    blocklist.Add((char)0);

                }
            }

        }

        

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    
                    float heightblock = 40 + 20 * Perlin.Fbm((x + pos.x * width) * Mathf.PI / noisescale, (z + pos.y * depth) * Mathf.PI / noisescale,5);

                    float treespawn = Perlin.Noise((x + pos.x * width) * Mathf.PI, (z + pos.y * depth) * Mathf.PI);

                    float min = heightblock - 4;
                    float max = heightblock;

                    float smooth = Mathf.SmoothStep(0, 1, (y - min) / (max - min));

                    float value = Perlin.Noise((x + pos.x * width) * Mathf.PI / 20, y * Mathf.PI / 20, (z + pos.y * depth) * Mathf.PI / 20);
                    value /= smooth * 2 + 1;

                    
                    int blockindex;
                    
                    if (value > 0.2 || value < -0.2 || y > heightblock)
                    {
                        blockindex = 0;
                    }
                    else if(y > heightblock - 1)
                    {
                        blockindex = 2;

                        if (treespawn > 0.7)
                        {
                            //Instantiate(tree, new Vector3(x + pos.x * width + 0.5f, Mathf.Ceil(heightblock) + 0.5f, z + pos.y * depth + 0.5f), Quaternion.identity);
                            
                        }
                    }
                    else if(y < heightblock - 6)
                    {
                        blockindex = 1;
                    }
                    else
                    {
                        blockindex = 3;
                    }

                   

                    blocklist[getindex(x, y, z)] = (char)blockindex;
                }
            }
        }
        
    }
    int getindex(int x, int y, int z)
    {
        return x * height * depth + y * depth + z;
    }

    public void generateside(ChunkGenerator chunk, int direction)
    {
        int x, y, z;
        
        switch (direction)
        {
            case 0:
                x = width - 1;
                for (y = 0; y < height; y++)
                {
                    for(z = 0;z < depth; z++)
                    {
                        int blockindex = blocklist[getindex(x, y, z)];
                        int otherblock = chunk.blocklist[getindex(0, y, z)];
                        if (blockindex != 0 && Blocks.blockIsTransparent[otherblock])
                        {
                            generateface(x, y, z, 0, blockindex);
                        }
                        
                    }
                }
                break;
            case 1:
                x = 0;
                for (y = 0; y < height; y++)
                {
                    for (z = 0; z < depth; z++)
                    {
                        int blockindex = blocklist[getindex(x, y, z)];
                        int otherblock = chunk.blocklist[getindex(width - 1, y, z)];
                        if (blockindex != 0 && Blocks.blockIsTransparent[otherblock])
                        {
                            generateface(x, y, z, 1, blockindex);
                        }

                    }
                }
                break;
            case 2:
                z = depth - 1;
                for (x = 0; x < width; x++)
                {
                    for (y = 0; y < height; y++)
                    {
                        int blockindex = blocklist[getindex(x, y, z)];
                        int otherblock = chunk.blocklist[getindex(x, y, 0)];
                        if (blockindex != 0 && Blocks.blockIsTransparent[otherblock])
                        {
                            generateface(x, y, z, 4, blockindex);
                        }

                    }
                }
                break;
            case 3:
                z = 0;
                for (x = 0; x < width; x++)
                {
                    for (y = 0; y < height; y++)
                    {
                        int blockindex = blocklist[getindex(x, y, z)];
                        int otherblock = chunk.blocklist[getindex(x, y, depth - 1)];
                        if (blockindex != 0 && Blocks.blockIsTransparent[otherblock])
                        {
                            generateface(x, y, z, 5, blockindex);
                        }

                    }
                }
                break;
        }

        var mesh = new Mesh
        {
            name = "Procedural Mesh"
        };
        

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();


        meshfilter.sharedMesh = mesh;
        
        meshcollider.sharedMesh = mesh;
       
    }

    void test(int x, int y, int z, int blockindex)
    {

        if (y == height - 1)
        {
            
            generateface(x, y, z, 2, blockindex);
        }
        if (y == 0)
        {
            
            generateface(x, y, z, 3, blockindex);
        }

        if (x < width - 1)
        {
            int otherblock = blocklist[getindex(x + 1, y, z)];
            if (Blocks.blockIsTransparent[otherblock])
            {
                generateface(x, y, z, 0, blockindex);
            }
        }
        if (x > 0)
        {
            int otherblock = blocklist[getindex(x - 1, y, z)];
            if (Blocks.blockIsTransparent[otherblock])
            {
                generateface(x, y, z, 1, blockindex);
            }
        }
        if (y < height - 1)
        {
            int otherblock = blocklist[getindex(x, y + 1, z)];
            if (Blocks.blockIsTransparent[otherblock])
            {
                //facing up
                generateface(x, y, z, 2, blockindex);
            }
        }

        if (y > 0)
        {
            int otherblock = blocklist[getindex(x, y - 1, z)];
            if (Blocks.blockIsTransparent[otherblock])
            {
                generateface(x, y, z, 3, blockindex);
            }
        }
        if (z < depth - 1)
        {
            int otherblock = blocklist[getindex(x, y, z + 1)];
            if (Blocks.blockIsTransparent[otherblock])
            {
                generateface(x, y, z, 4, blockindex);
            }
        }

        if (z > 0)
        {
            int otherblock = blocklist[getindex(x, y, z - 1)];
            if (Blocks.blockIsTransparent[otherblock])
            {
                generateface(x, y, z, 5, blockindex);
            }
        }



    }

    

    void generateface(int x, int y, int z, int direction, int blockindex)
    {
        
        var xoffset = direction == 0 ? 1 : 0;
        Vector3Int[,] faces =
        {
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(1, 0, 1),
                new Vector3Int(1, 1, 0),
                new Vector3Int(1, 1, 1),
            },
            {
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, 0),
                new Vector3Int(0, 1, 1),
                new Vector3Int(0, 1, 0),
            },
            {
                new Vector3Int(0, 1, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(0, 1, 1),
                new Vector3Int(1, 1, 1),
            },
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 0, 0),
                new Vector3Int(1, 0, 1),
                new Vector3Int(0, 0, 1),
            },
            {
                new Vector3Int(1, 0, 1),
                new Vector3Int(0, 0, 1),
                new Vector3Int(1, 1, 1),
                new Vector3Int(0, 1, 1),
            },
            {
                new Vector3Int(0, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(1, 1, 0),
            }
        };
        Vector3Int[] normaldirections =
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.forward,
            Vector3Int.back
        };

        int i = vertices.Count; vertices.Add(new Vector3(x, y, z) + faces[direction, 0]);

        vertices.Add(new Vector3(x, y, z) + faces[direction, 1]);
        vertices.Add(new Vector3(x, y, z) + faces[direction, 2]);
        vertices.Add(new Vector3(x, y, z) + faces[direction, 3]);
        for (int j = 0; j < 4; j++)
        {
            normals.Add(normaldirections[direction]);
        }

        tris.Add(i);
        tris.Add(i + 2);
        tris.Add(i + 1);
        tris.Add(i + 2);
        tris.Add(i + 3);
        tris.Add(i + 1);

        int textureindex = Blocks.block_to_texture[blockindex, direction];


        int texX = (textureindex) % texturequantity.x;
        int texY = texturequantity.y - 1 - (textureindex) / texturequantity.y;

        uv.Add(new Vector2(texX, texY) / texturequantity);
        uv.Add(new Vector2(texX + 1.0f, texY) / texturequantity);
        uv.Add(new Vector2(texX, texY + 1.0f) / texturequantity);
        uv.Add(new Vector2(texX + 1.0f, texY + 1.0f) / texturequantity);

    }
    public void setblock(Vector3Int pos, char blockindex)
    {
        blocklist[getindex(pos.x, pos.y, pos.z)] = blockindex;
    }
    
}
