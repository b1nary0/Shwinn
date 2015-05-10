using UnityEngine;
using System.Collections;

public class GGrenade : MonoBehaviour 
{
	private const float BlastRadiusSquared = 6;
	private const float MaxDamage = 65;
	private const float DetonationTime = 0.75f;
	private const float BlastImpulse = 120;

	private const float MinSlowDistance = 0.1f;
	private const float MaxSlowDistance = 1.5f;

	private GameObject[] PlayerObjects;
	private Rigidbody2D PhysicsBody;

	private GameObject Owner;

	// Use this for initialization
	void Start () 
	{
		PhysicsBody = GetComponent<Rigidbody2D>();

		PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
		StartCoroutine(Detonate());
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 GrenadePosition = transform.position;
		bool bGrenadeInAir = !(PhysicsBody.velocity.y == 0);
		for (int PlayerIdx = 0; PlayerIdx < PlayerObjects.Length; ++PlayerIdx)
		{
			GameObject PlayerObj = PlayerObjects[PlayerIdx];

			if (PlayerObj != Owner)
			{
				Vector3 PlayerPosition = PlayerObj.transform.position;

				float DistanceSquared = (PlayerPosition - GrenadePosition).sqrMagnitude;
				if (bGrenadeInAir)
				{
					if (DistanceSquared < MaxSlowDistance && DistanceSquared > MinSlowDistance)
					{
						GameObject.Find("GameManager").GetComponent<GGameManager>().SlowTime(0.5f);
					}
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D CollisionInfo)
	{
		Debug.Log("Collision with: " + CollisionInfo.collider.gameObject.tag);
	}

	private IEnumerator Detonate()
	{
		yield return new WaitForSeconds(DetonationTime);

		Vector3 GrenadePosition = transform.position;

		for (int PlayerIdx = 0; PlayerIdx < PlayerObjects.Length; ++PlayerIdx)
		{
			GameObject PlayerObj = PlayerObjects[PlayerIdx];
			Vector3 PlayerPosition = PlayerObj.transform.position;

			float DistanceSquared = (PlayerPosition - GrenadePosition).sqrMagnitude;
			if (DistanceSquared < BlastRadiusSquared)
			{
				FDamageInfo DamageInfo = new FDamageInfo();
				DamageInfo.DamageDone = MaxDamage / DistanceSquared;
				DamageInfo.DamageImpulse = BlastImpulse / DistanceSquared;
				DamageInfo.DamageOrigin = gameObject.transform.position;
				PlayerObj.GetComponent<GPlayer>().TakeDamage(DamageInfo);
			}
		}
		
		GameObject GrenadeExplosion = Instantiate<GameObject>(Resources.Load("Prefabs/Gameplay/Weapons/GrenadeExplosion") as GameObject);
		GrenadeExplosion.transform.position = GrenadePosition;
		Destroy (GrenadeExplosion, 1.0f);

		Destroy(this.gameObject);
	}

	public void SetOwningGameObject(GameObject Owner)
	{
		this.Owner = Owner;
	}
}
