using UnityEngine;
using System.Collections;

public class AxisAlignedMovingPlatform : MonoBehaviour 
{

    public bool bLeftRight;
    public bool bUpDown;
    private bool bMovingRight;
    private bool bMovingUp;

    public float PlatformSpeed;

    public float XMin;
    public float XMax;

    public float YMin;
    public float YMax;

	// Use this for initialization
	void Start () 
    {
        if (!bLeftRight && !bUpDown)
        {
            bLeftRight = true;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 Position = transform.position;

        if (bLeftRight)
        {
            if (Position.x >= XMax)
            {
                bMovingRight = false;
            }
            else if (Position.x <= XMin)
            {
                bMovingRight = true;
            }

            if (bMovingRight)
            {
                Position.x += PlatformSpeed * Time.deltaTime;
            }
            else
            {
                Position.x -= PlatformSpeed * Time.deltaTime;
            }
        }
        
        if (bUpDown)
        {
            if (Position.y >= YMax)
            {
                bMovingUp = false;
            }
            else if (Position.y <= YMin)
            {
                bMovingUp = true;
            }

            if (bMovingUp)
            {
                Position.y += PlatformSpeed * Time.deltaTime;
            }
            else
            {
                Position.y -= PlatformSpeed * Time.deltaTime;
            }
        }

        transform.position = Position;
	}
}
