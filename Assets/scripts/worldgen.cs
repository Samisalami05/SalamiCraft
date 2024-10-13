using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class worldgen : MonoBehaviour
{
    public Dictionary<Vector2, ChunkGenerator> world = new Dictionary<Vector2, ChunkGenerator>();
    public Transform player;
    
    public Material material;

    public GameObject tree;

    public float xoffset = 0f;
    public float yoffset = 0f;
    public static worldgen Instance { get; private set; }
    void Start()
    {
        xoffset = Random.Range(0f, 99999f);
        yoffset = Random.Range(0f, 99999f);
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        wow();
    }
    bool trygenerate(Vector2 pos)
    {
        if (!world.ContainsKey(pos))
        {
            GameObject gameobject = new GameObject
            {
                name = "Chunk x: " + pos.x + " z: " + pos.y,
            };


            ChunkGenerator chunk = gameobject.AddComponent<ChunkGenerator>();
            chunk.xoffset = xoffset;
            chunk.yoffset = yoffset;

            chunk.pos = pos;

            chunk.generateworld();
            
            chunk.addcomponents();
            world.Add(pos, chunk);
            generatemeshwithsides(pos);

            
            return true;
        }
        return false;
    }

    void wow()
    {
        Vector2 pos = blocktochunk(player.position);

        
        int distance = 0;

        while (distance < 3)
        {
            Vector2 renderpos = new Vector2(distance, distance);

            if (trygenerate(renderpos + pos))
            {
                return;
            }

            while (renderpos.x > -distance)
            {
                renderpos.x--;
                if (trygenerate(renderpos + pos))
                {
                    return;
                }
            }
            while (renderpos.y > -distance)
            {
                renderpos.y--;
                if (trygenerate(renderpos + pos))
                {
                    return;
                }
            }
            while (renderpos.x < distance)
            {
                renderpos.x++;
                if (trygenerate(renderpos + pos))
                {
                    return;
                }
            }
            while (renderpos.y < distance - 1)
            {
                renderpos.y++;
                if (trygenerate(renderpos + pos))
                {
                    return;
                }
            }
            distance++;
        }
        
    }
    Vector2 blocktochunk(Vector3 pos)
    {
        return new Vector2(Mathf.Floor(pos.x / ChunkGenerator.width), Mathf.Floor(pos.z / ChunkGenerator.depth));
    }
    public void SetBlock(Vector3 pos, char blockindex)
    {
        
        Vector2 chunk = blocktochunk(pos);

        Vector3 offset = new Vector3(chunk.x * ChunkGenerator.width,0, chunk.y * ChunkGenerator.depth);
        pos -= offset;

        Vector3Int posInt = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
        
        world[chunk].setblock(posInt,blockindex);

        generatemeshwithsides(chunk);

        if (pos.x == 0)
        {
            Vector2 c = new Vector2(chunk.x - 1, chunk.y);
            if(world.ContainsKey(c))
            {
                generatemeshwithsides(c);
                
            }
            
        }
        if (pos.x == ChunkGenerator.width - 1)
        {
            Vector2 c = new Vector2(chunk.x + 1, chunk.y);
            if (world.ContainsKey(c))
            {
                generatemeshwithsides(c);

            }

        }
        if (pos.y == 0)
        {
            Vector2 c = new Vector2(chunk.x, chunk.y - 1);
            if (world.ContainsKey(c))
            {
                generatemeshwithsides(c);

            }

        }
        if (pos.y == ChunkGenerator.depth)
        {
            Vector2 c = new Vector2(chunk.x, chunk.y + 1);
            if (world.ContainsKey(c))
            {
                generatemeshwithsides(c);

            }

        }

    }
    void generatemeshwithsides(Vector2 pos) {
        ChunkGenerator chunk = world[pos];
        chunk.generatemesh(material);

        Vector2 pos1 = new Vector2(pos.x + 1, pos.y);
        Vector2 pos2 = new Vector2(pos.x - 1, pos.y);
        Vector2 pos3 = new Vector2(pos.x, pos.y + 1);
        Vector2 pos4 = new Vector2(pos.x, pos.y - 1);



        if (world.ContainsKey(pos1))
        {
            ChunkGenerator chunk1 = world[pos1];
            chunk1.generateside(chunk, 1);
            chunk.generateside(chunk1, 0);
        }

        if (world.ContainsKey(pos2))
        {
            ChunkGenerator chunk2 = world[pos2];
            chunk2.generateside(chunk, 0);
            chunk.generateside(chunk2, 1);
        }

        if (world.ContainsKey(pos3))
        {
            ChunkGenerator chunk3 = world[pos3];
            chunk3.generateside(chunk, 3);
            chunk.generateside(chunk3, 2);
        }

        if (world.ContainsKey(pos4))
        {
            ChunkGenerator chunk4 = world[pos4];
            chunk4.generateside(chunk, 2);
            chunk.generateside(chunk4, 3);
        }
    }
}
