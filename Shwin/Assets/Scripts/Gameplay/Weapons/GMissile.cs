using UnityEngine;
using System.Collections;

public class GMissile : MonoBehaviour 
{
	public GameObject Owner;

	private Rigidbody2D PhysicsBody;

	private float WeakShotImpulse;
	private float StrongShotImpulse;

	//TODO::Implement dud behaviour
	public bool bIsDud;

	// Use this for initialization
	void Awake () 
	{
		// Component gets
		PhysicsBody = gameObject.GetComponent<Rigidbody2D>();

		WeakShotImpulse = 5.0f;
		StrongShotImpulse = 10.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.LookAt(new Vector2(transform.position.x, transform.position.y) + PhysicsBody.velocity);
	}

	void OnCollisionEnter2D(Collision2D CollisionInfo)
	{
		GameObject OtherObj = CollisionInfo.collider.gameObject;
		if (OtherObj != Owner)
		{
			if (OtherObj.tag == "Player")
			{
				GPlayer PlayerScript = OtherObj.GetComponent<GPlayer>();
				FPlayerInfo PlayerInfo = PlayerScript.GetPlayerInfo();

				float TotalDamage = PlayerInfo.Health * 0.65f;

				FDamageInfo DamageInfo = new FDamageInfo();
				DamageInfo.DamageDone = TotalDamage;

				PlayerScript.SendMessage("TakeDamage", DamageInfo);

				Destroy(this.gameObject);
			}
			else if (OtherObj.tag == "Environment")
			{
				//TODO::Splash damage
				Destroy(this.gameObject);
			}
		}
	}

	public void ShootWeak(GameObject Owner, Vector2 ShootPosition, Vector2 ShootDirection)
	{
		this.Owner = Owner;
		gameObject.transform.position = ShootPosition;

		PhysicsBody.AddForce(WeakShotImpulse * ShootDirection, ForceMode2D.Impulse);
		Owner.GetComponent<Rigidbody2D>().AddForce(WeakShotImpulse * -ShootDirection * 5, ForceMode2D.Impulse);

		PhysicsBody.AddTorque(15.0f * (2 * -ShootDirection.x));
	}

	public void ShootStrong(GameObject Owner, Vector2 ShootPosition, Vector2 ShootDirection)
	{
		this.Owner = Owner;
		gameObject.transform.position = ShootPosition;

		PhysicsBody.AddForce(StrongShotImpulse * ShootDirection, ForceMode2D.Impulse);
		PhysicsBody.AddTorque(15.0f * (2 *-ShootDirection.x));
	}
}
