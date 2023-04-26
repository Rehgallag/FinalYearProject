using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadRubik : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 


    // Link up ray tranform obj to script
    public Transform transformUp;
    public Transform transformDown;
    public Transform transformFront;
    public Transform transformBack;
    public Transform transformLeft;
    public Transform transformRight;

    // for faces of cube
    private int layerMask = 1 << 8;

    // reference the Rubik Cube's State
    RubikState state;
    MapCube mapCube;

    public GameObject emptyGameObject;

    // Store ray objs of each face/side, each list is 9 rays which are empty GameObjects with their transforms arranged in a grid pattern
    private List<GameObject> uRays = new List<GameObject>();
    private List<GameObject> dRays = new List<GameObject>();
    private List<GameObject> fRays = new List<GameObject>();
    private List<GameObject> bRays = new List<GameObject>();
    private List<GameObject> lRays = new List<GameObject>();
    private List<GameObject> rRays = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SetTransformRays();

        state = FindObjectOfType<RubikState>();
        mapCube = FindObjectOfType<MapCube>();
        // get initial read of the state of the cube
        ReadCubeState();
        // toggle to confirm that everything is loaded
        RubikState.start = true;

    }

    // Update is called once per frame
    void Update()
    {
        //ReadCubeState();

    }

    // builds the rays thgat 
    List<GameObject> RayBuilder(Transform transFormRay, Vector3 dir)
    {
        // ray count ensures the rays are in the correct order 0-8
        int count = 0;
        List<GameObject> ray = new List<GameObject>();
        /*
         * 9 rays created
         * 
         * |0|1|2|
         * |3|4|5|
         * |6|7|8|
         * 
         * Axis origin @ 4 (0,0)
         * 0,0 -1 x, +1 y
         */
        int x = -1, y = 1;

        //gives position of the inital/top left cube also know as index 0
        // iterates through the loop until x = +1 and y = +1
        // original pos was x= -1, y= -1
        for (y = 1; y > -2; y--)
        {
            for (x = -1; x < 2; x++)
            {
                // use each coordinate to build the start pos of the rays created that uses the transFormRay passed into this method
                Vector3 start = new Vector3(transFormRay.localPosition.x + x,
                    transFormRay.localPosition.y + y,
                    transFormRay.localPosition.z);
                // instantiate empty gameObj @ start pos child it to the new transform
                GameObject startRay = Instantiate(emptyGameObject, start, Quaternion.identity, transFormRay);
                // set name of array to couunt 0-8
                // ensures rotation of arrays lines up with the face it represents
                startRay.name = count.ToString(); 
                // add rays to list
                ray.Add(startRay);
                // increment count
                count++;
            }
        }
        // fix the rotation and return list of rays that have been created
        transFormRay.localRotation = Quaternion.Euler(dir);
        return ray;

    }
    // populate cube state with list of rays by calling ray builder method and passing the ray transforms for each side
    // with the directionn that they need to be rotated by for the rays to hit the cube, ray 0 = top left and ray 8 = bottom right
    void SetTransformRays()
    {
        // populate ray lists with raycasts
        uRays = RayBuilder(transformUp, new Vector3(90, 90, 0));
        dRays = RayBuilder(transformDown, new Vector3(270, 90, 0));
        fRays = RayBuilder(transformFront, new Vector3(0, 90, 0));
        bRays = RayBuilder(transformBack, new Vector3(0, 270, 0));
        rRays = RayBuilder(transformRight, new Vector3(0, 0, 0));
        lRays = RayBuilder(transformLeft, new Vector3(0, 180, 0));
    }

    // Read the side of the cube that is focused on, takes in a list of GameObjects the ray starts and the transform
    public List<GameObject> ReadSide(List<GameObject> startRays, Transform transformRay)
    {
        List<GameObject> faces = new List<GameObject>();
        
        // send out a ray for each ray that is passed into the method, for every face hit, add to the list faces
        // this only deals with a single side of the cube, so the cube map won't be set until all sides have been read
        foreach(GameObject startRay in startRays)
        {
            Vector3 ray = startRay.transform.position;
            RaycastHit hit;
            //intersection with layermask objs
            if (Physics.Raycast(ray, transformRay.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, transformRay.forward * hit.distance, Color.green);
                faces.Add(hit.collider.gameObject);
                // check if the faces is hit correctly
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, transformRay.forward * 1000, Color.green);
            }
        }

        //mapCube.SetMap();
        return faces;
        
    }
    // Read the state of the cube
    public void ReadCubeState()
    {
        // make sure the current instance of the Cube State and Cube map scripts
        state = FindObjectOfType<RubikState>();
        mapCube = FindObjectOfType<MapCube>();

        // set state each pos to know each color pos
        state.up = ReadSide(uRays, transformUp);
        state.down = ReadSide(dRays, transformDown);
        state.front = ReadSide(fRays, transformFront);
        state.back = ReadSide(bRays, transformBack);
        state.right = ReadSide(rRays, transformRight);
        state.left = ReadSide(lRays, transformLeft);

        // update map 54 rays at once
        // Somewhat intensive as 54 rays are cast each frame when testing
        mapCube.SetMap();
    }
}
