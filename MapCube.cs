using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class MapCube : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 


    // Need to know state of cube so we can reference
    RubikState rubikState;

    // attach the parent transforms of the Cube faces
    public Transform upper;
    public Transform down;
    public Transform front;
    public Transform back;
    public Transform left;
    public Transform right;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This sets the colours of the cube map
    public void SetMap()
    {

        rubikState = FindObjectOfType<RubikState>();

        // use mapUpdate method on each of the transforms, passing in the side of the cube from the RubikState script that needs to be compared
        mapUpdate(rubikState.front, front);
        mapUpdate(rubikState.back, back);
        mapUpdate(rubikState.left, left);
        mapUpdate(rubikState.right, right);
        mapUpdate(rubikState.up, upper);
        mapUpdate(rubikState.down, down);
    }

    // Update the colour of the faces on the map depending on the side of the piece that the raycasts hits
    void mapUpdate(List<GameObject> face, Transform s)
    {
        int inc = 0;
        foreach(Transform map in s)
        {
            // Each of the 6 middle pieces are single Letters whilst everything else is listed as descriptions ie FrontLeftUpper, RightBack
            if (face[inc].name[0] == 'B')
            {
                map.GetComponent<Image>().color = new UnityEngine.Color(0, 0.8157f, 0.9622f, 1);
            }
            if (face[inc].name[0] == 'F')
            {
                map.GetComponent<Image>().color = UnityEngine.Color.green;
            }
            if (face[inc].name[0] == 'R')
            {
                map.GetComponent<Image>().color = UnityEngine.Color.red;
            }
            if (face[inc].name[0] == 'L')
            {
                map.GetComponent<Image>().color = new UnityEngine.Color(1, 0.5f, 0, 1);
            }
            if (face[inc].name[0] == 'U')
            {
                map.GetComponent<Image>().color = UnityEngine.Color.white;
            }
            if (face[inc].name[0] == 'D')
            {
                map.GetComponent<Image>().color = UnityEngine.Color.yellow;
            }
            
            inc++;
        }
    }
}
