using UnityEngine;
using System.Collections;

public class GGun : MonoBehaviour 
{
	private const float FireRate = 0.1f;
	private float FireInterval;

	private GameObject Owner;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void Fire()
	{
		Debug.Log ("Prefire");
		FireInterval += Time.deltaTime;

		Debug.Log (FireInterval);

		if (FireInterval >= FireRate)
		{
			Debug.Log ("Firing");
			GameObject Bullet = Instantiate<GameObject>(Resources.Load("Prefabs/Gameplay/Weapons/Bullet") as GameObject);
			GBullet BulletScript = Bullet.GetComponent<GBullet>();

			Bullet.transform.position = transform.position;

			GPlayerActions PlayerAction = Owner.GetComponent<GPlayerActions>();
			BulletScript.SetDirection((PlayerAction.bFacingRight) ? new Vector2(1, 0) : new Vector2(-1, 0));

			Physics2D.IgnoreCollision(Bullet.GetComponent<Collider2D>(), Owner.GetComponent<Collider2D>());

			FireInterval = 0;
		}
	}

	public void SetOwner(GameObject Owner)
	{
		this.Owner = Owner;
	}

	public void ResetFireRate()
	{
		FireInterval = 0;
	}

	void OnCollisionEnter2D(Collision2D CollisionInfo)
	{
		GameObject CollisionObject = CollisionInfo.collider.gameObject;

		if (CollisionObject.tag == "Player" && Owner == null)
		{
			CollisionObject.GetComponent<GPlayerActions>().WeaponPickup(this.gameObject);
		}
	}
}
