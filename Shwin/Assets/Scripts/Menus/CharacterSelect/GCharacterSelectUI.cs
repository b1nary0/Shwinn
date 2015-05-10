using UnityEngine;
using System.Collections;

/* TODO:
 * => Check how to make an editable text box and extract the string data from it.
 * .  Put that entered name into persistent data for player names during gameplay.
 * 
 * => Button press for player entering game. Very least, player one always in game.
 * .  Figure out a way to structure this in code. Need a data structure to hold entered players?
 */

public class GCharacterSelectUI : MonoBehaviour 
{
    private const int INFO_BOX_PADDING = 10;

    /* The width and height of the GUI group containing the character information boxes */
    private const int GROUP_WIDTH = (INFO_BOX_WIDTH + INFO_BOX_PADDING) * MAX_PLAYERS;
    private const int GROUP_HEIGHT = 150;

    /* The individual character info box details */
    private const int INFO_BOX_WIDTH = 150;
    private const int INFO_BOX_HEIGHT = 150;

    private const int MAX_PLAYERS = 2;

    private GPersistentData PersistentData;
    private GInputManager InputManager;
    private FCharacterSelectData[] PlayerData;

	private bool bAllowDPadInput;

	// Use this for initialization
	void Start () 
    {
        PersistentData = GameObject.Find("PersistentData").GetComponent<GPersistentData>();
        InitializePlayerData();

        InputManager = GameObject.Find("InputManager").GetComponent<GInputManager>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        ProcessInput();

		// Determine whether all players are ready to start the game.
		int NumReadyPlayers = 0;
		for (int PlayerIdx = 0; PlayerIdx < PlayerData.Length; ++PlayerIdx)
		{
			FCharacterSelectData PlayerInfo = PlayerData[PlayerIdx];
			if (PlayerInfo.IsReady())
			{
				++NumReadyPlayers;
			}
		}

		if (NumReadyPlayers == GInputManager.NumConnectedControllers)
		{
			PreGameSetup();
			Application.LoadLevel("Level_Recourse");
		}
	}

    void OnGUI()
    {
        GUI.skin = PersistentData.UISkin;

        Vector2 GroupPos = new Vector2(PersistentData.ScreenHalfWidth - GROUP_WIDTH / 2, PersistentData.ScreenHalfHeight - GROUP_HEIGHT / 2);

        GUI.BeginGroup(new Rect(GroupPos.x, GroupPos.y, GROUP_WIDTH, GROUP_HEIGHT));

        for (int BoxIdx = 0; BoxIdx < MAX_PLAYERS; ++BoxIdx)
        {
            GUI.Box(new Rect(BoxIdx * (INFO_BOX_WIDTH + INFO_BOX_PADDING), 0, INFO_BOX_WIDTH, INFO_BOX_HEIGHT), PlayerData[BoxIdx].PlayerTitle);

            Texture2D CharacterTexture = PlayerData[BoxIdx].SelectedCharacterTexture;
            Vector2 ImagePosition = new Vector2(INFO_BOX_WIDTH / 2 - CharacterTexture.width / 2, INFO_BOX_HEIGHT / 2 - CharacterTexture.height / 2);

            GUI.DrawTexture(new Rect(ImagePosition.x + BoxIdx * (INFO_BOX_WIDTH + INFO_BOX_PADDING), ImagePosition.y, CharacterTexture.width, CharacterTexture.height), CharacterTexture);

			FCharacterSelectData PlayerInfo = PlayerData[BoxIdx];
            if (!PlayerInfo.IsEnabled())
            {
                GUI.Label(new Rect(BoxIdx * INFO_BOX_WIDTH + INFO_BOX_WIDTH + (INFO_BOX_PADDING / 2) - 100 , INFO_BOX_HEIGHT - 25, 100, 40), "Press To Start");
            }
            else
            {
				if (!PlayerInfo.IsReady())
				{
               		GUI.Label(new Rect(BoxIdx * INFO_BOX_WIDTH + INFO_BOX_WIDTH + (INFO_BOX_PADDING / 2) - 100, INFO_BOX_HEIGHT - 25, 100, 40), "Select Character");
				}
				else
				{
					GUI.Label(new Rect(BoxIdx * INFO_BOX_WIDTH + INFO_BOX_WIDTH + (INFO_BOX_PADDING / 2) - 100, INFO_BOX_HEIGHT - 25, 100, 40), "Player Ready");
				}
            }
        }

        GUI.EndGroup();
    }

    private void ProcessInput()
    {
        ProcessKeyboardInput();

		ProcessControllerInput();
    }

    private void ProcessControllerInput()
    {
        // Joystick enumeration index. This is a base value and should be added to (KCode + ButtonOffset) only.
        const KeyCode KCode = KeyCode.JoystickButton0;
		FCharacterSelectData PlayerInfo = PlayerData[FControllerIndex.XBoxController];

        // Xbox Controller: A
        if (Input.GetKeyDown(KCode))
        {
            if (!PlayerInfo.IsEnabled())
            {
                PlayerInfo.Enable();
				PlayerInfo.IsKeyboard(false);
            }
		}

		// Actions only available once the player is ready to select.
		if (PlayerInfo.IsEnabled() && !PlayerInfo.IsReady())
		{
			float AxisX = Input.GetAxis("XboxP1_DPadHorizontal");
			if (bAllowDPadInput)
			{
				if (AxisX < 0)
				{
					PlayerInfo.PrevTexture();
					bAllowDPadInput = false;
				}
				else if (AxisX > 0)
				{
					PlayerInfo.NextTexture();
					bAllowDPadInput = false;
				}
			}

			if (Input.GetKeyDown(KCode + FXboxControllerButton.Button_Start))
			{
				PlayerInfo.IsReady(true);
			}

			if (!bAllowDPadInput)
			{
				bAllowDPadInput = (AxisX == 0);
			}
		}

		if (PlayerInfo.IsReady())
		{
			if (Input.GetKeyDown(KCode + FXboxControllerButton.Button_B))
			{
				PlayerInfo.IsReady(false);
			}
		}
    }

    private void ProcessKeyboardInput()
    {
		FCharacterSelectData PlayerInfo = PlayerData[FControllerIndex.Keyboard];

		if (!PlayerInfo.IsEnabled())
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				PlayerInfo.Enable();
				PlayerInfo.IsKeyboard(true);
			}
		}
		else
		{
			if (!PlayerInfo.IsReady())
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					PlayerInfo.IsReady(true);
				}

				if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					PlayerInfo.PrevTexture();
				}
				else if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					PlayerInfo.NextTexture();
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					PlayerInfo.IsReady(false);
				}
			}
		}
    }

    private void InitializePlayerData()
    {
        PlayerData = new FCharacterSelectData[MAX_PLAYERS];
        for (int PlayerIdx = 0; PlayerIdx < PlayerData.Length; ++PlayerIdx)
        {
            PlayerData[PlayerIdx] = new FCharacterSelectData();
        }
    }

    private void PreGameSetup()
    {
        GameObject PersistentDataObject = GameObject.Find("PersistentData");
        PersistentDataObject.GetComponent<GPersistentData>().PlayerData = this.PlayerData;
    }
}
