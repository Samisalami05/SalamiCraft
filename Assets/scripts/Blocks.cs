using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    // Start is called before the first frame update

    public static int[,] block_to_texture = { 
        {-1,-1,-1,-1,-1,-1},
        {0,0,0,0,0,0},
        {2,2,1,3,2,2},
        {3,3,3,3,3,3},
        {4,4,4,4,4,4},
        {5,5,5,5,5,5},
        {6,6,7,7,6,6},
        {8,8,8,8,8,8},
        {9,9,9,9,9,9},
        {10,10,10,10,10,10},
    };

    public enum BlockType
    {
        Air, 
        Stone,
        Grass,
        Dirt,
        Cobblestone,
        OakPlanks,
        OakLogs,
        CoalOre,
        GlassBlock,
        Leaves
    }
    public static bool[] blockIsTransparent = {true,false,false,false,false,false,false,false,true,true};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
