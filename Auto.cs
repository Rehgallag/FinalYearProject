using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// automatically perform moves from a list of moves
public class Auto : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 

    // moves
    public static List<string> moveList = new List<string>() { };
    // all moves
    private readonly List<string> allMoveSet = new List<string> 
    {
        "U", "D", "L", "R", "F", "B",
        "U2", "D2", "L2", "R2", "F2", "B2",
        "U'", "D'", "L'", "R'", "F'", "B'"
    };
    // needs access to instance of Rubik State
    private RubikState rubikState;
    // to confirm the current state of the cube, if side is changed the contents of the side will be changed
    private ReadRubik readRubik;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Shuffle").transform.localScale = new Vector3(0, 0, 0);
        //GameObject.Find("Solve").transform.localScale = new Vector3(0, 0, 0);
        rubikState = FindObjectOfType<RubikState>();
        readRubik = FindObjectOfType<ReadRubik>();
    }

    // Update is called once per frame
    void Update()
    {
        // if there are still moves to be done && side is not currently moving && start needs to be true before anything is done
        if(moveList.Count > 0 && !RubikState.autoRotate && RubikState.start)
        {
            // do the move @ 1st index
            DoMove(moveList[0]);
            // remove move once complete
            moveList.Remove(moveList[0]);
        }
    }
    // automatically rotate side by angle
    void RotateSide(List<GameObject> side, float angle)
    {
        // need to know if any of the pivots are auto rotating
        // need to know contents of side that is being rotated 
        PivotRot pr = side[4].transform.parent.GetComponent<PivotRot>();
        // call method for specific pivot
        pr.StartAutoRotate(side, angle);
    }   

    // changes the angle based on the move that being done
    void DoMove(string move)
    {
        // get state of cube before each move
        readRubik.ReadCubeState();
        // rotating a side
        RubikState.autoRotate = true;
        // all moves such as Clockwise (90), Anti-Clockwise (-90) and double moves (180)
        if (move == "U")
        {
            RotateSide(rubikState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(rubikState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(rubikState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(rubikState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(rubikState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(rubikState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(rubikState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(rubikState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(rubikState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(rubikState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(rubikState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(rubikState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(rubikState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(rubikState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(rubikState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(rubikState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(rubikState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(rubikState.back, -180);
        }
    }

    // generate random shuffle of cube
    // linked to the shuffle button
    public void Shuffle()
    {
        // builds a list of shuffled moves 
        List<string> moves = new List<string>();
        // number of moves to make, higher the number more random but it takes time to rotate, don't want user waiting too long
        int shuffleLength = Random.Range(10, 26);
        // loop based on length
        for (int i = 0; i < shuffleLength; i++)
        {
            // pick a random move index from allmoves
            int randomMove = Random.Range(0, allMoveSet.Count);
            // add to list
            moves.Add(allMoveSet[randomMove]);
        }
        // set list to new moves
        moveList = moves;
    }
}
