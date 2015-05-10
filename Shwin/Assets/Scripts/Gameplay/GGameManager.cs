using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GBattleCam))]

public class GGameManager : MonoBehaviour 
{
    GPersistentData PersistentDataObject;

    GPlayer[] PlayerScripts;
    GameObject[] PlayerPrefabs;

	private float DefaultTimeScale;
	public float GameSlowTimeScale;

	private float WeaponSpawnInterval;
	private float WeaponSpawnTime;

	// Use this for initialization
	void Start () 
	{
		// GameObject finds
		PersistentDataObject = GameObject.Find("PersistentData").GetComponent<GPersistentData>();

        // Late Allocations
        PlayerPrefabs = new GameObject[PersistentDataObject.PlayerData.Length];

        for (int PlayerIdx = 0; PlayerIdx < PersistentDataObject.PlayerData.Length; ++PlayerIdx)
        {
            GameObject CurrentPlayerObject = null;
            Texture2D Texture = PersistentDataObject.PlayerData[PlayerIdx].SelectedCharacterTexture;

            CurrentPlayerObject = GameObject.Instantiate(Resources.Load("Prefabs/Characters/PlayerPrefab")) as GameObject;
            CurrentPlayerObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));

            // DEBUG::For camera testing purposes, otherwise all characters move the same way from the same input
            if (PlayerIdx > 0)
            {
                CurrentPlayerObject.GetComponent<GCharacterController>().enabled = false;
            }

			CurrentPlayerObject.name = PersistentDataObject.PlayerData[PlayerIdx].PlayerTitle;

            PlayerPrefabs[PlayerIdx] = CurrentPlayerObject;
        }

		// Determine which player controller to enable on each player object.
		for (int PlayerIdx = 0; PlayerIdx < PersistentDataObject.PlayerData.Length; ++PlayerIdx)
		{
			FCharacterSelectData PlayerInfo = PersistentDataObject.PlayerData[PlayerIdx];

			if (PlayerInfo.IsXBoxController())
            {
                GameObject.Find(PlayerInfo.PlayerTitle).GetComponent<GCharacterXboxController>().enabled = true;
			}
			else
			{
                GameObject.Find(PlayerInfo.PlayerTitle).GetComponent<GCharacterController>().enabled = true;
			}
		}

        GBattleCam BattleCamScript = Camera.main.GetComponent<GBattleCam>();
        if (BattleCamScript)
        {
            BattleCamScript.enabled = true;
        }


		DefaultTimeScale = UnityEngine.Time.timeScale;
		GameSlowTimeScale = 0.4f;

		WeaponSpawnTime = Random.Range (30.0f, 60.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        WeaponSpawnTime += Time.deltaTime;
        if (WeaponSpawnTime > WeaponSpawnInterval)
        {
            DropItemInWorld();
        }
	}

	public void SlowTime(float Duration)
	{
		// Set unity timestep slower
		if (UnityEngine.Time.timeScale != GameSlowTimeScale)
		{
            UnityEngine.Time.timeScale = GameSlowTimeScale;

            // Set up a coroutine yield to return it back to normal after Duration seconds
            StartCoroutine(ReturnTimeScaleToDefault(Duration));
		}
	}

	private IEnumerator ReturnTimeScaleToDefault(float Duration)
	{
		if (UnityEngine.Time.timeScale != DefaultTimeScale)
        {
            yield return new WaitForSeconds(Duration);

			UnityEngine.Time.timeScale = DefaultTimeScale;
		}
	}

	public void DropItemInWorld()
	{
        GameObject Gun = Instantiate<GameObject>(Resources.Load("Prefabs/Gameplay/Weapons/Gun") as GameObject);

        Gun.transform.position = new Vector3(Random.Range(-10, 10), 25, -1);
        WeaponSpawnTime = 0;
        WeaponSpawnInterval = Random.Range(30.0f, 60.0f);
	}

}
