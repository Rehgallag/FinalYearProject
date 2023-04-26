using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PivotRot : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 


    // needs to know which side is rotating
    private List<GameObject> activeS;
    // need to know what axis to rotate around
    private Vector3 localF;
    // amount side rotates is controlled by mouse position
    private Vector3 mouse;
    private char keyRead;
    // need to know if mouse is currently dragging the side
    private bool drag = false;

    // references
    private RubikState rubikState;
    private ReadRubik readRubik;

    // how much the sides should rotate based on the mouse movement
    private float sens = 0.4f;
    // looks after the rotation itself
    private Vector3 rotation;

    // only true when set, this is for the cube side to be placed in the correct position when dragging is finished
    private bool autoRotate = false;
    // for the target angle that we want to auto move to
    private Quaternion targetQ;
    // this moves on its own for auto rotate
    private float speed = 300f;

    // Start is called before the first frame update
    void Start()
    {
        // find objects
        readRubik = FindObjectOfType<ReadRubik>();
        rubikState = FindObjectOfType<RubikState>();
    }

    // Late Update is called once per frame at the end
    void LateUpdate()
    {
        // call spin method every frame
        if (drag && !autoRotate)
        {
            // spin the active side
            Spin(activeS);
            // if mouse1 is released 
            if (Input.GetMouseButtonUp(0))
            {
                // stops calling spin method
                drag = false;
                // figure out the angle to rotate to
                RotateToRight();
            }
        }
        // snaps to nearest position and cube map updated
        if (autoRotate)
        {
            AutoRotate();
        }
    }
    // Called once at the start to send the variables
    // actual rotate will be in update method
    // take in side that is being rotated
    public void Rotate(List<GameObject> side)
    {
        // whatever side is passed becomes the active side
        activeS = side;
        // keep track of start position of mouse to know how much to rotate the side by 
        // as the mouse moves away from the start pos
        mouse = Input.mousePosition;
        // need to know that the side is currently being dragged by mouse
        drag = true;

        // create a vector to rotate around, based on the local position of the piece
        // that is being rotated and the centre of the cube
        localF = Vector3.zero - side[4].transform.parent.transform.localPosition;
    }

    // Called on every frame that is dragging the side
    // takes in a list of gameObj of the side that we want to spin
    private void Spin(List<GameObject> side)
    {
        // clear/reset rotation
        rotation = Vector3.zero;

        // Get current mouse pos - last mouse pos
        // this is to know how much to rotate the side
        Vector3 mOffset = (Input.mousePosition - mouse);

        // if list of gameObj is equal to side in the RubikState list
        if(side == rubikState.front)
        {
            // rotate around x axis
            rotation.x = (mOffset.x + mOffset.y) * sens * -1;
        }
        if (side == rubikState.back)
        {
            rotation.x = (mOffset.x + mOffset.y) * sens * 1;
        }
        if (side == rubikState.up)
        {
            rotation.y = (mOffset.x + mOffset.y) * sens * 1;
        }
        if (side == rubikState.down)
        {
            rotation.y = (mOffset.x + mOffset.y) * sens * -1;
        }
        if (side == rubikState.left)
        {
            rotation.z = (mOffset.x + mOffset.y) * sens * 1;
        }
        if (side == rubikState.right)
        {
            rotation.z = (mOffset.x + mOffset.y) * sens * -1;
        }

        // update the local transforma that the script is attached to 
        // rotate
        transform.Rotate(rotation, Space.Self);

        // store mouse position for the next time that the method is called
        mouse = Input.mousePosition;
    }

    // handles the automatic rotation
    // this is called once to set variables and the rotation is done in the update() method
    // this gets the angle to rotate to when the mouse is let go
    // rotate the current rotation to the nearest 90 degrees and then set that as the target
    public void RotateToRight()
    {
        Vector3 vec = transform.localEulerAngles;
        // rounds the vector to the nearest 90 degrees
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        // set the target quarternion based on the new vector
        targetQ.eulerAngles = vec;
        // toggle autorotate on
        autoRotate = true;
    }

    
    private void AutoRotate()
    {
        // releasing the m1 button turns dragging off, sometimes may want to call the method so drag is set to false if autorotate is toggled on
        drag = false;
        // amount that is rotated is based on the speed that is set above and the delta time at which the game is running at
        var step = speed * Time.deltaTime;
        // use rotatetowards to adjust local rotation of pivot by the step amount over time
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQ, step);

        // if close to the target, end rotation, line up everything and put down piece that was picked up
        // basically if within 1 degree, set angle to target angle and end rotation
        if(Quaternion.Angle(transform.localRotation,targetQ) <= 1)
        {
            transform.localRotation = targetQ;
            // unparent/put down little cubes/pieces
            rubikState.PutDown(activeS, transform.parent); 
            readRubik.ReadCubeState();
            // once the rotation is complete turn off auto rotate 
            RubikState.autoRotate = false;
            autoRotate = false;
            drag = false; // might not be needed
        }
    }

    // need a way to set the variables for the auto rotate passed to individual pivot
    // takes in side that is passes and angle it is being rotated by
    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        // need to pick up pieces
        rubikState.Pick(side);
        // figure out axis to rotate around vector
        Vector3 localF = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQ = Quaternion.AngleAxis(angle, localF) * transform.localRotation;
        // set the active side to side 
        activeS = side;
        autoRotate = true;
    }
}
