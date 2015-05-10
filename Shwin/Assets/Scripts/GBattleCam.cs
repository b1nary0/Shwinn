using UnityEngine;
using System.Collections;

/* TODO:
 * => Player objects do not exist when start is called. Is there a post-start type thing?
 */

public class GBattleCam : MonoBehaviour 
{
    public float MinDistance;
    public float MaxDistance;
    public float MinOrthoSize;
    public float MaxOrthoSize;

    private const float CameraOffsetY = 0.025f;    
    private const float CameraLerpSpeed = 0.025f;

    private Vector2 FurthestPlayerDistance;

    private GameObject[] PlayerObjects;
    private Camera MainCamera;

	// Use this for initialization
	void Awake () 
    {
        MainCamera = Camera.main;

        MainCamera.orthographicSize = 4;

        // TODO::Player objects do not exist in scene at this point in execution.
        // TODO::Don't use magic numbers
        PlayerObjects = new GameObject[4];
	}
	
	// Update is called once per frame
	void Update () 
    {
        // TODO::Fix this hack.
        if (PlayerObjects[FPlayerIndex.PlayerOne] == null)
        {
            PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        }

        // TODO::Fix this hack. Part of the playerobjects hack.
        if (PlayerObjects[FPlayerIndex.PlayerOne] != null)
        {
            CameraMovement();
        }
	}

    private void CameraMovement()
    {
        GameObject FurthestPlayer = PlayerObjects[FPlayerIndex.PlayerOne];
        Vector2 Midpoint = new Vector2();

        // For each player in scene
        foreach (GameObject Player in PlayerObjects)
        {
            Vector2 FurthestPlayerLocation = FurthestPlayer.transform.position;
            Vector2 PlayerLocation = Player.transform.position;

            // Find two furthest players
            Vector2 DistanceBetween = PlayerLocation - FurthestPlayerLocation;
            FurthestPlayerDistance = DistanceBetween;
            FurthestPlayer = Player;

            Midpoint.x = (PlayerLocation.x + FurthestPlayerLocation.x) / 2;
            Midpoint.y = (PlayerLocation.y + FurthestPlayerLocation.y) / 2;
        }

        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, Mathf.Abs(FurthestPlayerDistance.sqrMagnitude / MainCamera.orthographicSize), CameraLerpSpeed);
        MainCamera.orthographicSize = Mathf.Clamp(MainCamera.orthographicSize, MinOrthoSize, MaxOrthoSize);

        Vector3 CameraPosition = MainCamera.transform.position;
        CameraPosition.x = Mathf.Lerp(CameraPosition.x, Midpoint.x, CameraLerpSpeed);
        CameraPosition.y = Mathf.Lerp(CameraPosition.y, Midpoint.y, CameraLerpSpeed);
        CameraPosition.y += CameraOffsetY;
        MainCamera.transform.position = CameraPosition;
    }
}
