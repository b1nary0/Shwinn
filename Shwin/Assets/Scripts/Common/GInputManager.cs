using UnityEngine;
using System.Collections;

public class GInputManager : MonoBehaviour 
{

    public static int NumConnectedControllers;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);

        string[] ControllerNames = Input.GetJoystickNames();
        Debug.Log("Connected Controllers: " + (ControllerNames.Length + 1));

		// Add one to final count of controllers: Keyboard.
        NumConnectedControllers = ControllerNames.Length + 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
