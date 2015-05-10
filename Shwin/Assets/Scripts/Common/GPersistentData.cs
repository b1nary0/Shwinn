using UnityEngine;
using System.Collections;

public class GPersistentData : MonoBehaviour 
{

    public float ScreenHalfHeight = Screen.height / 2;
    public float ScreenHalfWidth = Screen.width / 2;

    public bool bDebugEnabled;

    public FCharacterSelectData[] PlayerData;

    public GUISkin UISkin;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
