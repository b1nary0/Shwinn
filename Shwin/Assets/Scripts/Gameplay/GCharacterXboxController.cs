using UnityEngine;
using System.Collections;

public class GCharacterXboxController : MonoBehaviour 
{
	private GPlayerActions PlayerActions;
	private GPlayer PlayerScript;

	public float DeadZone;

    private bool bTappedLeft;

    private const float MaxDoubleTapInterval = 0.25f;
    private float ElapsedDoubleTapTime;

	// Use this for initialization
	void Start () 
	{
		PlayerActions = GetComponent<GPlayerActions>();
		PlayerScript = GetComponent<GPlayer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ProcessInput();

        if (bTappedLeft)
        {
            ElapsedDoubleTapTime += Time.deltaTime;
            if (ElapsedDoubleTapTime >= MaxDoubleTapInterval)
            {
                bTappedLeft = false;
                ElapsedDoubleTapTime = 0;
            }
        }
	}

	private void ProcessInput()
	{
		KeyCode KCode = KeyCode.JoystickButton0;

		if (Input.GetKeyDown(KCode))
		{
			PlayerActions.Jump();
		}

		if (Input.GetKeyDown(KCode + FXboxControllerButton.Button_X))
		{
			GameObject WeaponGO = PlayerScript.GetWeapon();

			if (WeaponGO == null)
			{
				float AttackDamage = PlayerScript.GetAttackDamage();
				PlayerActions.Attack(AttackDamage);
				PlayerScript.ApplyAttackFatigue();
			}
		}

		if (Input.GetKey (KCode + FXboxControllerButton.Button_X))
		{
			GameObject WeaponGO = PlayerScript.GetWeapon();
			if (WeaponGO != null)
			{
				GGun WeaponScript = WeaponGO.GetComponent<GGun>();
				WeaponScript.Fire();
			}
		}

		if (Input.GetKey(KCode + FXboxControllerButton.Button_B))
		{
			PlayerActions.IncreaseGrenadeTrajectory();
		}

		if (Input.GetKeyUp(KCode + FXboxControllerButton.Button_B))
		{
			PlayerActions.ReleaseGrenade();
		}

		float AxisX = Input.GetAxis("XboxP1_DPadHorizontal");
		if (AxisX < -DeadZone)
		{
            if (!bTappedLeft)
            {
                bTappedLeft = true;
            }
            //TODO::The rest of the double tap impl.
            
			PlayerActions.MoveLeft();
		}
		else if (AxisX > DeadZone)
		{
			PlayerActions.MoveRight();
		}
	}
}
