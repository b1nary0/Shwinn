using UnityEngine;
using System.Collections;

public class GCharacterController : MonoBehaviour 
{
	private GPlayerActions PlayerActions;
	private GPlayer PlayerScript;

	// Use this for initialization
	void Start () 
    {
		// Script Gets
		PlayerActions = GetComponent<GPlayerActions>();
		PlayerScript = GetComponent<GPlayer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        ProcessInput();
	}

    private void ProcessInput()
    {
        ProcessKeyboardInput();
    }

    private void ProcessKeyboardInput()
    {
        /*Move Left*/
        if (Input.GetKey(KeyCode.A))
        {
            PlayerActions.MoveLeft();
        }
        /*Move right*/
        else if (Input.GetKey(KeyCode.D))
        {
			PlayerActions.MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
			PlayerActions.Jump();
        }

		if (Input.GetKeyDown(KeyCode.E))
		{
			float AttackDamage = PlayerScript.GetAttackDamage();
			PlayerActions.Attack(AttackDamage);
			PlayerScript.ApplyAttackFatigue();
		}

		if (Input.GetKey(KeyCode.F))
		{
			GameObject WeaponGO = PlayerScript.GetWeapon();

			if (WeaponGO == null)
			{
				PlayerActions.IncreaseGrenadeTrajectory();
			}
		}

		if (Input.GetKeyUp(KeyCode.F))
		{
			GameObject WeaponGO = PlayerScript.GetWeapon();

			if (WeaponGO == null)
			{
				PlayerActions.ReleaseGrenade();
			}
		}

		if (Input.GetKey(KeyCode.F))
		{
			GameObject WeaponGO = PlayerScript.GetWeapon();

			if (WeaponGO != null)
			{
				GGun WeaponScript = WeaponGO.GetComponent<GGun>();
				WeaponScript.Fire();
			}
		}
    }

    
}
