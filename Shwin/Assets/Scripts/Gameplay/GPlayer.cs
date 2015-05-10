using UnityEngine;
using System.Collections;

public class GPlayer : MonoBehaviour 
{
	private const float HealthRegenAmt = 0.10f;
	private const float FatigueLossAmt = 0.6f;
	private const float HealthRegenDelayInterval = 2.0f;

	private float Fatigue;
	private float AttackFatigueAmt;
	private float AttackDamageAmt;
	private float Health;
	private float ElapsedHealthRegenDelay;

    private int Score;

	private bool bCanBeDamaged;
	private const float ImmunityTime = 2.0f;

	private Vector3 InitialPosition;

	Rigidbody2D PhysicsBody;
	GPlayerActions PlayerActions;

	private GameObject Weapon;

	private GameObject[] PlayerObjects;

	// Use this for initialization
	void Start () 
	{
		// Component Gets
		PhysicsBody = GetComponent<Rigidbody2D>();
		PlayerActions = GetComponent<GPlayerActions>();

        PlayerObjects = GameObject.FindGameObjectsWithTag("Player");

		Weapon = null;

		InitialPosition = gameObject.transform.position;

		bCanBeDamaged = false;

		StartCoroutine(SetAbleToBeDamaged());

		Fatigue = 0;
		Health = 100;

		AttackFatigueAmt = 4.0f;
		AttackDamageAmt = 4.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ElapsedHealthRegenDelay += Time.deltaTime;

		// Process checks for health. When dead, play stun animation indefinately until player resets.
		if (Health <= 0)
		{
			// Player is dead, setup for reset.
			Die();
		}

		bool bCanRegenHealth = (ElapsedHealthRegenDelay >= HealthRegenDelayInterval);
		if (bCanRegenHealth)
		{
			// Health regen
			if (Health < 100)
			{
				Health += HealthRegenAmt;
			}
			else
			{
				ElapsedHealthRegenDelay = 0;
			}
		}

		// Fatigue loss (power regen)
		if (Fatigue > 0)
        {
            Fatigue -= FatigueLossAmt;
			// Prevent cases where fatigue will drop below 0
			if (Fatigue < 0)
			{
				Fatigue = 0;
			}
		}
	}

	public void GiveWeapon(GameObject Weapon)
	{
		if (this.Weapon == null)
		{
			this.Weapon = Weapon;

			Vector3 InitalScale = Weapon.transform.localScale;
			this.Weapon.transform.parent = gameObject.transform;

			Vector3 GunPosition = gameObject.transform.position;
			GunPosition.z = -1;
            GunPosition.y -= 0.2f;

			this.Weapon.transform.position = GunPosition;
			this.Weapon.transform.localScale = InitalScale;
			this.Weapon.transform.rotation = Quaternion.identity;

			this.Weapon.GetComponent<Rigidbody2D>().isKinematic = true;

			Destroy (this.Weapon, 5.0f);
			this.Weapon.GetComponent<GGun>().SetOwner(gameObject);
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), this.Weapon.GetComponent<Collider2D>());
		}
	}

    public void IncreaseScore()
    {
        ++Score;
    }

	private void Die()
	{
		Fatigue = 0;
		Health = 100;

        foreach (GameObject Player in PlayerObjects)
        {
            if (Player.name != this.gameObject.name)
            {
                Debug.Log(Player.name);
                Player.GetComponent<GPlayer>().IncreaseScore();
            }
        }

		gameObject.transform.position = InitialPosition;
		
		bCanBeDamaged = false;
		StartCoroutine(SetAbleToBeDamaged());
	}

	public GameObject GetWeapon()
	{
		return Weapon;
	}

	private IEnumerator SetAbleToBeDamaged()
	{
		yield return new WaitForSeconds(ImmunityTime);
		bCanBeDamaged = true;
	}

	public void TakeDamage(float DamageAmt)
	{
        if (bCanBeDamaged)
        {
            ElapsedHealthRegenDelay = 0;
            Health -= DamageAmt;
        }
	}

	public void TakeDamage(FDamageInfo DamageInfo)
	{
		if (bCanBeDamaged)
		{
            ElapsedHealthRegenDelay = 0;

			Health -= DamageInfo.DamageDone;
			if (DamageInfo.DamageImpulse > 0)
			{
				// Apply an impulse from the direction the damage is coming from.
				Vector3 ImpulseDirection = transform.position - DamageInfo.DamageOrigin;
				ImpulseDirection.Normalize();

				PhysicsBody.AddForce(DamageInfo.DamageImpulse * ImpulseDirection, ForceMode2D.Force);
			}

			float CurrentGrenadeImpulse = PlayerActions.GetGrenadeImpulse();
			if (CurrentGrenadeImpulse > 0)
			{
				PlayerActions.ReleaseGrenade();
			}
		}
	}

	public float GetAttackDamage()
	{
		return AttackDamageAmt;
	}

	public float GetAttackFatigue()
	{
		return AttackFatigueAmt;
	}

	public void ApplyAttackFatigue()
	{
		Fatigue += AttackFatigueAmt;
	}

	public FPlayerInfo GetPlayerInfo()
	{
		FPlayerInfo PlayerInfo;
		PlayerInfo.Health = Health;
		PlayerInfo.Fatigue = Fatigue;
        PlayerInfo.Score = this.Score;

		return PlayerInfo;
	}

	void OnCollisionEnter2D(Collision2D CollisionInfo)
	{
		if (CollisionInfo.collider.tag == "YKill")
		{
			Die();
		}
	}
}
