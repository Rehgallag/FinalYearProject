using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RubikState : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 

    // all possible sides of the Rubik's cube
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();

    // auto.cs
    public static bool autoRotate = false;
    // to make sure that the moves are loaded before loading to prevent ArgumentOutOfRange Exception
    public static bool start = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 
    public void Pick(List<GameObject> side)
    {
        // foreach  gObj face in sides, atttach the parent of each face, the piece to the parent of the 4th index (Middle piece)
        // unless it is alreayd the 4th index, can't parent something to itself
        foreach (GameObject f in side)
        {
            if (f != side[4]) // 4 is middle piece
            {
                f.transform.parent.transform.parent = side[4].transform.parent;
            }
        }
    }

    // put down pieces picked up ie faces, to pick up another side
    // should work without this but it makes the project tidier
    public void PutDown(List<GameObject> littleCube, Transform pivot)
    {
        foreach(GameObject lc in littleCube)
        {
            if(lc != littleCube[4])// middle
            {
                lc.transform.parent.transform.parent = pivot; // could've been named better but it works
            }
        }
    }

    // this returns the state of a single side as a string
    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        // loop through each face in the side
        foreach (GameObject f in side)
        {
            sideString += f.name[0].ToString();
        }
        return sideString;
    }

    // need to make sure that the string is in the correct order
    // U1 U2 U3 U4 U5 U6 U7 U8 U9,  R1 R2 R3 R4 R5 R6 R7 R8 R9, F1 F2 F3 F4 F5 F6 F7 F7 F9,
    // D1 D2 D3 D4 D5 D6 D7 D8 D9, L1 L2 L3 L4 L5 L6 L7 L8 L9, B1 B2 B3 B4 B5 B6 B7 B8 B9,
    public string GetStateString()
    {
        string state = "";
        // get side of each in correct order and concat to state
        state += GetSideString(up);
        state += GetSideString(right);
        state += GetSideString(front);
        state += GetSideString(down);
        state += GetSideString(left);
        state += GetSideString(back);
        print(state);
        return state;
    }
}
