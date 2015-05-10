using UnityEngine;
using System.Collections;

public class GStartMenu : MonoBehaviour 
{
    private float ScreenHalfWidth;
    private float ScreenHalfHeight;

    private GPersistentData PersistentData;

	// Use this for initialization
	void Start () 
    {
        PersistentData = GameObject.Find("PersistentData").GetComponent<GPersistentData>();

        ScreenHalfWidth = Screen.width / 2;
        ScreenHalfHeight = Screen.height / 2;
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnGUI()
    {
        GUI.skin = PersistentData.UISkin;

        if (GUI.Button(new Rect(ScreenHalfWidth - 50, ScreenHalfHeight - 25, 100, 50), "Play"))
        {
            // Play game
            Application.LoadLevel("Scene_CharacterSelect");
        }

        if (GUI.Button(new Rect(ScreenHalfWidth - 50, (ScreenHalfHeight - 25) + 50, 100, 50), "Exit"))
        {
            // Exit
            Application.Quit();
        }
    }
}
