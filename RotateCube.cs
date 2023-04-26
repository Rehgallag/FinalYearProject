using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 


    Vector2 firstPressPosition; // position that the movement started in
    Vector2 secondPressPosition;// position that the movement ended in
    Vector2 currentSlide; // get direction of current movement

    // vars for visual feedback for cube to move after dragging it
    Vector3 prevMousePos;
    Vector3 mouseDelta;

    public GameObject target; // this is what is going to be rotated

    float speed = 150; // speed at which the cube rotates
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Slide();
        Dragged();
    }
    
    void Dragged()
    {
        if (Input.GetMouseButton(1))
        {
            // for mouse being held down
            mouseDelta = Input.mousePosition - prevMousePos;
            mouseDelta *= 0.5f; // needed to reduce rotate speed by multiplier, was way too fast. 
            // cubes pos is updated based on how much the mouse had moved
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }
        else
        {
            // auto move to target pos if the end position is not in the set pos
            if (transform.rotation != target.transform.rotation)
            {
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }
        prevMousePos = Input.mousePosition;
    }
    // handle the movement
    void Slide()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // 2d pos of first mouse click
            firstPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //print(firstPressPosition);
        }
        if (Input.GetMouseButtonUp(1))
        {
            // 2d pos of second mouse click
            secondPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // vector made from 1st & 2nd positions
            currentSlide = new Vector2(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);
            // normalize (Keep the vector pointing in the same direction)
            currentSlide.Normalize();
            // rotate based on left or right slide
            if (LeftS(currentSlide))
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightS(currentSlide))
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (UpLeftS(currentSlide))
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (UpRightS(currentSlide))
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownLeftS(currentSlide))
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (DownRightS(currentSlide))
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
        }
    }

    // Cube moved left
    bool LeftS(Vector2 s)
    {
        return s.x < 0 && s.y > -0.5f && s.y < 0.5f;
    }

    // Cube moved right
    bool RightS(Vector2 s)
    {
        return s.x > 0 && s.y > -0.5f && s.y < 0.5f;
    }

    // Cube moved upleft
    bool UpLeftS(Vector2 s)
    {
        return s.y > 0 && s.x < 0f;
    }

    // Cube moved upright
    bool UpRightS(Vector2 s)
    {
        return s.y > 0 && s.x > 0f;
    }

    // Cube moved downleft
    bool DownLeftS(Vector2 s)
    {
        return s.y < 0 && s.x < 0f;
    }

    // Cube moved downright
    bool DownRightS(Vector2 s)
    {
        return s.y < 0 && s.x > 0f;
    }
}
