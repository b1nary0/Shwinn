using UnityEngine;
using System.Collections;

public class GPlayerActions : MonoBehaviour 
{
	private const float MaxAttackDistance = 0.2f;

	public float MoveSpeed;
	public float JumpImpulse;
	public float DoubleJumpDelay;
	public float HorizontalJumpImpulse;

	private float GrenadeImpulse;
	private const float GrenadeImpulseStep = 0.5f;
	private const float MaxGrenadeImpulse = 15.0f;

	private int PlayerTextureWidth;
	private int PlayerTextureHeight;

	private float ElapsedDoubleJumpDelay;

	public bool bFacingRight;
	private bool bIsJumping;
	private bool bDidDoubleJump;
	private bool bAllowDoubleJump;
	private bool bHasGrenade;

	private const float GrenadeInterval = 2.0f;

	private Rigidbody2D PhysicsBody;
	private GPlayer PlayerScript;
	
	// Use this for initialization
	void Start () 
	{
		//Component Gets
		PhysicsBody = GetComponent<Rigidbody2D>();
		PlayerScript = GetComponent<GPlayer>();

		SpriteRenderer SptRenderer = GetComponent<SpriteRenderer>();
		PlayerTextureWidth = (int)SptRenderer.bounds.size.x;
		PlayerTextureHeight = (int)SptRenderer.bounds.size.y;

		bFacingRight = true;
		bHasGrenade = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (bIsJumping)
		{
			bIsJumping = !(PhysicsBody.velocity.y == 0);
			// Only set bDidDoubleJump back to false if we're at rest again.
			// Set here to ensure that it's not set to true just because bIsJumping is.
			if (!bIsJumping)
			{
				bDidDoubleJump = false;
				bAllowDoubleJump = false;
				ElapsedDoubleJumpDelay = 0;
			}
			
			TickDoubleJumpDelay();
		}
	}

	public float GetGrenadeImpulse()
	{
		return GrenadeImpulse;
	}

	public void Jump()
	{
		if (!bIsJumping)
		{
			PhysicsBody.AddForce(new Vector2(0.0f, JumpImpulse), ForceMode2D.Impulse);
			bIsJumping = true;
		}

		if (bAllowDoubleJump && bIsJumping)
		{
			PhysicsBody.AddForce(new Vector2(0.0f, JumpImpulse), ForceMode2D.Impulse);
			bDidDoubleJump = true;
			bAllowDoubleJump = false;
			ElapsedDoubleJumpDelay = 0;
		}
	}
	
	public void MoveLeft()
	{
		if (bFacingRight)
		{
			FlipSprite();
		}

		Vector2 Position = transform.position;
		Position.x -= MoveSpeed * Time.deltaTime;
		transform.position = Position;
	}
	
	public void MoveRight()
	{
		if (!bFacingRight)
		{
			FlipSprite();
		}

		Vector2 Position = transform.position;
		Position.x += MoveSpeed * Time.deltaTime;
		transform.position = Position;
	}

	//TODO::Implement
	public void ShootWeapon()
	{
		// Get currently held weapon
        // Call shoot on it.
	}

	public void IncreaseGrenadeTrajectory()
	{
		if (bHasGrenade)
		{
			if (GrenadeImpulse < MaxGrenadeImpulse)
			{
				GrenadeImpulse += GrenadeImpulseStep;

				if (GrenadeImpulse > MaxGrenadeImpulse)
				{
					GrenadeImpulse = MaxGrenadeImpulse;
				}
			}
		}
	}

	public void ReleaseGrenade()
	{
		if (bHasGrenade)
		{
			GameObject Grenade = Instantiate<GameObject>(Resources.Load("Prefabs/Gameplay/Weapons/Grenade") as GameObject);
			Grenade.GetComponent<GGrenade>().SetOwningGameObject(this.gameObject);

			Rigidbody2D GrenadePhysicsBody = Grenade.GetComponent<Rigidbody2D>();

			Vector3 GrenadePosition = gameObject.transform.position;
			Vector2 GrenadePositionOffset = new Vector2((bFacingRight) ? 0.8f : -0.5f, 0.5f);
			float GrenadeLaunchAngle = (bFacingRight) ? 45 * Mathf.Deg2Rad : 135 * Mathf.Deg2Rad;
			Vector3 GrenadeLaunchPosition = new Vector3(GrenadePosition.x + GrenadePositionOffset.x, GrenadePosition.y + GrenadePositionOffset.y);

			Grenade.transform.position = GrenadeLaunchPosition;

			Vector2 GrenadeLaunchDirection = new Vector2(Vector2.right.x * Mathf.Cos(GrenadeLaunchAngle) - Vector2.right.y * Mathf.Sin(GrenadeLaunchAngle),
			                                                  Vector2.right.x * Mathf.Sin(GrenadeLaunchAngle) + Vector2.right.y * Mathf.Cos(GrenadeLaunchAngle));

			//Vector2 GrenadeLaunchDirection = (bFacingRight) ? RightGrenadeLaunchDirection : RightGrenadeLaunchDirection;

			GrenadeLaunchDirection.Normalize();

			GrenadePhysicsBody.AddForce(GrenadeLaunchDirection * GrenadeImpulse, ForceMode2D.Impulse);

			GrenadeImpulse = 0;
			bHasGrenade = false;

			StartCoroutine(GiveGrenade());
		}
	}

	private IEnumerator GiveGrenade()
	{
		yield return new WaitForSeconds(GrenadeInterval);

		bHasGrenade = true;
	}

	public bool HasGrenade()
	{
		return bHasGrenade;
	}

	public void Attack(float DamageAmt)
	{
		// Raycast
		Vector3 RayStartPos = transform.position;
		RayStartPos.x += (bFacingRight) ? (PlayerTextureWidth / 2) + 0.25f : -((PlayerTextureWidth / 2) + 0.25f);

		RaycastHit2D Hit2D = Physics2D.Raycast(RayStartPos, (bFacingRight) ? Vector2.right * 10 : Vector2.right * -10);
		if (Hit2D && Hit2D.collider)
		{
			if (Hit2D.distance <= MaxAttackDistance)
			{
				// If we've found a player, call take damage for the attack amount.
				FPlayerInfo PlayerInfo = PlayerScript.GetPlayerInfo();
				GameObject HitObject = Hit2D.collider.gameObject;

				float TotalDamage = DamageAmt * (1 - (PlayerInfo.Fatigue / 100.0f));

				HitObject.SendMessage("TakeDamage", TotalDamage);
			}
		}
	}

	public void WeaponPickup(GameObject Weapon)
	{
		PlayerScript.GiveWeapon(Weapon);
	}
	
	private void FlipSprite()
	{
		bFacingRight = !bFacingRight;
		
		Vector2 SpriteScale = transform.localScale;
		SpriteScale.x *= -1;
		transform.localScale = SpriteScale;
	}

	private void TickDoubleJumpDelay()
	{
		if (!bDidDoubleJump)
		{
			ElapsedDoubleJumpDelay += Time.deltaTime;

			if (ElapsedDoubleJumpDelay >= DoubleJumpDelay)
			{
				bAllowDoubleJump = true;
			}
		}
	}
}
