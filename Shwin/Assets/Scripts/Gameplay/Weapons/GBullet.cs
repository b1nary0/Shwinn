using UnityEngine;
using System.Collections;

public class GBullet : MonoBehaviour 
{
	private const float BulletSpeed = 25.0f;
	private const float Damage = 5.0f;

	private Vector2 Direction;

	// Use this for initialization
	void Start () 
	{
		transform.Rotate (new Vector3(0, 0, Direction.x * 90));
		Destroy(this.gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log ("bullet updating");
		Vector3 Position = transform.position;
		Position.x += Direction.x * BulletSpeed * Time.deltaTime;
		transform.position = Position;
	}

	public void SetDirection(Vector2 Direction)
	{
		this.Direction = Direction;
	}

	void OnCollisionEnter2D(Collision2D CollisionInfo)
	{
		GameObject CollisionObject = CollisionInfo.collider.gameObject;
        if (CollisionObject.tag == "Environment")
        {
            Destroy(this.gameObject);
        }
		else if (CollisionObject.tag == "Player")
		{
			// Process collision
			CollisionObject.GetComponent<GPlayer>().TakeDamage(Damage);
		}
	}
}
