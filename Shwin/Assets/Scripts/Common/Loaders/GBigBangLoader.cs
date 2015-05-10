using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public class GBigBangLoader : MonoBehaviour 
{

    public bool bDebugEnabled;

    List<GameObject> LoadedObjects;

	// Use this for initialization
	void Start () 
    {
        LoadedObjects = new List<GameObject>();

        /*Input Manager*/
        GameObject InputManager = Instantiate<GameObject>(Resources.Load("Prefabs/Core/InputManager") as GameObject);
        InputManager.name = "InputManager";
        LoadedObjects.Add(InputManager);

        /*Persistent Data*/
        GameObject PersistentData = Instantiate<GameObject>(Resources.Load("Prefabs/Core/PersistentData") as GameObject);
        PersistentData.name = "PersistentData";

        GPersistentData PersistentDataScript = PersistentData.GetComponent<GPersistentData>();
        PersistentDataScript.bDebugEnabled = this.bDebugEnabled;

        LoadedObjects.Add(PersistentData);
        
        Application.LoadLevel("Scene_StartMenu");
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

}
