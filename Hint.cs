using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;
using System.Diagnostics;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public ReadRubik readRubik;
    public RubikState rubikState;
    private bool once = true;
    private string pythonPath;  // path to the Python executable
    private string scriptPath;  // path to the LSTM.py script
    private string scriptArgs;  // command-line arguments for the script
    public Text hintText;
    public GameObject changingHintText;
    // Start is called before the first frame update
    void Start()
    {
        hintText.text = "No moves to make";
        readRubik = FindObjectOfType<ReadRubik>();
        rubikState = FindObjectOfType<RubikState>();
    
        // Set the paths to the Python executable and the LSTM.py script
        pythonPath = @"C:/Users/John/AppData/Local/Programs/Python/Python37/python.exe"; // needs changing if built
        scriptPath = Application.dataPath + "/Models/LSTM.py";
        string moveString = rubikState.GetStateString();
        scriptArgs = "-s " + moveString;  // pass the Rubik's cube state as an argument to the script
       
    }

    // Update is called once per frame
    void Update()
    {
        if (RubikState.start && once)
        {
            once = false;
            //scriptArgs = "-s " + rubikState.GetStateString();

        }
    }

    public void HintClicked()
    {
        readRubik.ReadCubeState();
        // print("StateString: "+rubikState.GetStateString());
        // Call the LSTM.py script to generate the next move in the Rubik's cube sequence
        string hint = GenerateHint(scriptPath, scriptArgs, pythonPath);

       // print("The next move is: " + hint);
       
        if (hint != null)
        {
            print("the next move is: " + hint);
            hintText.text = hint;
        }
        else
        {
            print("there are no moves to do.");
        }

    }

    private string GenerateHint(string scriptPath, string scriptArgs, string pythonPath)
    {
        // Call the Python script to generate the next move in the sequence
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonPath;
        start.Arguments = scriptPath + " " + scriptArgs;
        start.RedirectStandardOutput = true;
        start.UseShellExecute = false;

        Process process = new Process();
        process.StartInfo = start;
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();


        // Extract the hint from the output
        string[] splitOutput = output.Split('\n');
        string hint = splitOutput[0].Trim();
        //print("test: " + output);

        if (string.IsNullOrEmpty(hint))
        {
            return null;
        }
        else
        {
            return hint;
        }
    }


}
