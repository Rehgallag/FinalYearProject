using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

// handles the information related to which side that can be rotated
// selects all peices in a side of the cube based on the face that is clicked
public class FaceSelection : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 


    // Start is called before the first frame update
    RubikState rubikState;
    ReadRubik readRubik;

    int layer = 1 << 8;
    void Start()
    {
        // reference read cube and state
        readRubik = FindObjectOfType<ReadRubik>();
        rubikState = FindObjectOfType<RubikState>();

    }
    
    // Update is called once per frame
    void Update()
    {
        // mouse1 disabled when rotating, this fixes a huge error that would crash the program where the cube would jumble up and throw errors
        if (Input.GetMouseButtonDown(0) && !RubikState.autoRotate)
        { 
            // read current cube state
            readRubik.ReadCubeState();
            // fire raycast from mouse to cube to see if a face is hit
            RaycastHit hit;
            // the start point of the ray will be the mouse position in world units
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // if a face is hit
            if(Physics.Raycast(ray,out hit,100.0f, layer))
            {
                // save the face that is hit
                GameObject face = hit.collider.gameObject;
                // list of all sides
                List<List<GameObject>> sides = new List<List<GameObject>>()
                {
                    rubikState.up,
                    rubikState.down,
                    rubikState.left,
                    rubikState.right,
                    rubikState.back,
                    rubikState.front
                };
                // iof the face of the game object that is hit exsits within a side, 
                // for each list of gameObj cube side in sides
                foreach(List<GameObject> s in sides)
                {
                    // if cube side contains the face just clicked on
                    if (s.Contains(face))
                    {
                        // make the pieces of the side/face children of the central piece (U,F,B,D,R,L)
                        rubikState.Pick(s);
                        // start side rotation
                        s[4].transform.parent.GetComponent<PivotRot>().Rotate(s);
                    }
                }
            }
        }
    }

}
