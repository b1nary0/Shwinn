using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GGameplayUI : MonoBehaviour 
{

	public Texture2D[] Healthbars;
	public Texture2D GrenadeTexture;

    private GPersistentData PersistentData;

	private GameObject[] PlayerObjects;
	private GPlayer[] PlayerScripts;
	private GPlayerActions[] PlayerActions;
	private FPlayerInfo[] PlayerInfos;
    public Vector2[] PlayerTextureSizes;

    public Font SmallGUIFont;
    public Font MediumGUIFont;
    public Font LargeGUIFont;

	private int HealthBarWidth;
	private int HealthBarHeight;
	private int HealthBarPadding; 

	private const float HealthBarScale = 0.4f;

    private const int PlayerScoreBoxHeight = 80;
    private const int PlayerScoreBoxWidth = 250;

	// To ensure information has been properly set up for OnGUI
	bool bInitialized;

	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForFixedUpdate();

		// Allocations
		PlayerScripts = new GPlayer[GInputManager.NumConnectedControllers];
		PlayerInfos = new FPlayerInfo[GInputManager.NumConnectedControllers];
		PlayerActions = new GPlayerActions[GInputManager.NumConnectedControllers];
        PlayerTextureSizes = new Vector2[GInputManager.NumConnectedControllers];

		// GameObject and Script setup
		PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
		for (int PlayerObjectIdx = 0; PlayerObjectIdx < PlayerObjects.Length; ++PlayerObjectIdx)
		{
			PlayerScripts[PlayerObjectIdx] = PlayerObjects[PlayerObjectIdx].GetComponent<GPlayer>();
			PlayerActions[PlayerObjectIdx] = PlayerObjects[PlayerObjectIdx].GetComponent<GPlayerActions>();
			PlayerInfos[PlayerObjectIdx] = PlayerScripts[PlayerObjectIdx].GetPlayerInfo();

            SpriteRenderer SprtRenderer = PlayerObjects[PlayerObjectIdx].GetComponent<SpriteRenderer>();
            PlayerTextureSizes[PlayerObjectIdx] = new Vector2(SprtRenderer.bounds.size.x, SprtRenderer.bounds.size.y);

            Debug.Log(PlayerTextureSizes[PlayerObjectIdx]);
		}

        PersistentData = GameObject.Find("PersistentData").GetComponent<GPersistentData>();

		HealthBarWidth = (int)(Healthbars[0].width * HealthBarScale);
		HealthBarHeight = (int)(Healthbars[0].height * HealthBarScale);
		HealthBarPadding = 5;

		bInitialized = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (bInitialized)
		{
			RefreshPlayerInfo();
		}
	}

	void OnGUI()
	{
		if (bInitialized)
		{
		// Draw HealthBars
			for (int PlayerIdx = 0; PlayerIdx < PlayerObjects.Length; ++PlayerIdx)
			{
				Vector3 PlayerPosition = PlayerObjects[PlayerIdx].transform.position;
				Vector3 PlayerScreenCoords = Camera.main.WorldToScreenPoint(PlayerPosition);
				FPlayerInfo PlayerInfo = PlayerInfos[PlayerIdx];

				float HealthPercentage = PlayerInfo.Health / 100.0f;
				if (HealthPercentage < 0)
				{
					HealthPercentage = 0;
				}

                GUI.skin.font = MediumGUIFont;
                GUI.DrawTexture(new Rect(PlayerScreenCoords.x - (HealthBarWidth / 2), (Screen.height - (PlayerScreenCoords.y + 50)), HealthBarWidth * HealthPercentage, HealthBarHeight), Healthbars[PlayerIdx]);
                GUI.Label(new Rect(PlayerScreenCoords.x - (HealthBarWidth / 2) / 2 - 5, Screen.height - (PlayerScreenCoords.y + 58), 50, 25), ((int)PlayerInfo.Health).ToString());

				if (PlayerActions[PlayerIdx].HasGrenade())
				{
					GUI.DrawTexture(new Rect(PlayerScreenCoords.x + (HealthBarWidth / 2) + 5, (Screen.height - PlayerScreenCoords.y) - (GrenadeTexture.width / 2), GrenadeTexture.width * 0.25f, GrenadeTexture.height * 0.25f), GrenadeTexture, ScaleMode.ScaleToFit);
				}

                /*Player Score Area*/
                FCharacterSelectData[] PlayerData = PersistentData.PlayerData;
                Texture2D PlayerTexture = PlayerData[PlayerIdx].SelectedCharacterTexture;

                float PlayerTexturePosX = (PlayerIdx == 0) ? PlayerTexture.width : Screen.width - PlayerTexture.width * 2;
                GUI.DrawTexture(new Rect(PlayerTexturePosX, Screen.height - (PlayerTexture.height + 20), PlayerTexture.width, PlayerTexture.height), PlayerTexture);

                float InfoBoxPosX = (PlayerIdx == 0) ? PlayerTexture.width * 2 : Screen.width - (PlayerTexture.width * 6.5f);
                
                GUI.BeginGroup(new Rect(InfoBoxPosX, Screen.height - (PlayerTexture.height + 10), PlayerScoreBoxWidth, PlayerScoreBoxHeight));
                GUI.Box(new Rect(0, 0, PlayerScoreBoxWidth, PlayerScoreBoxHeight), PlayerData[PlayerIdx].PlayerTitle);

                GUI.Label(new Rect(PlayerScoreBoxWidth / 4, PlayerScoreBoxHeight / 4, PlayerScoreBoxWidth / 2, PlayerScoreBoxHeight / 3), "Score:");
                GUI.Label(new Rect(PlayerScoreBoxWidth / 4 + 20, PlayerScoreBoxHeight / 4 + 20, PlayerScoreBoxWidth / 2, PlayerScoreBoxHeight / 3), PlayerInfo.Score.ToString());

                GUI.EndGroup();
			}
		}
	}

	private void RefreshPlayerInfo()
	{
		for (int PlayerInfoIdx = 0; PlayerInfoIdx < PlayerInfos.Length; ++PlayerInfoIdx)
		{
			PlayerInfos[PlayerInfoIdx] = PlayerScripts[PlayerInfoIdx].GetPlayerInfo();
		}
	}
}
