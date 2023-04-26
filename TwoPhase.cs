using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba; // import kociemba package can be found from https://github.com/Megalomatt/Kociemba/tree/Unity

public class TwoPhase : MonoBehaviour
{
    /// <summary>
    /// Updated the original code which is from https://www.megalomobile.com/lets-make-and-solve-a-rubiks-cube-in-unity/ 
    /// And the code has been updated due to the conflicting Unity versions/outdated code
    /// </summary>
    /// 


    public ReadRubik readRubik;
    public RubikState rubikState;
    // needed to make sure that the cube doesnt bug
    private bool once = true;
    // Start is called before the first frame update
    void Start()
    {
        readRubik = FindObjectOfType<ReadRubik>();
        rubikState = FindObjectOfType<RubikState>();
    }

    // Update is called once per frame
    void Update()
    {
        // if cube has started and once is true call the Solver() method
        if(RubikState.start && once)
        {
            once = false;
            Solver();
        }
    }
    // solve the cube
    public void Solver()
    {
        readRubik.ReadCubeState();
        // get state as string
        string moveString = rubikState.GetStateString();
        print(moveString);
        //solve cube
        string info = "";
        // build table
        // this is commented out as this is only to build the tables on the first run (tables are 4.2 mb in size)
        // string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        // this is the method for every other time
        string solution = Search.solution(moveString, out info);

        //convert moves from string to list
        List<string> solutionList = StringtoList(solution);
        
        // automate the list
        Auto.moveList = solutionList;
        
        print(info);
    }

    // convert the string into a list
    List<string> StringtoList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] {" "}, System.StringSplitOptions.RemoveEmptyEntries)); // removes any empty entries
        return solutionList;
    }
}
