using UnityEngine;
using System.Collections;

public class LightingTestScript : MonoBehaviour
{
	public  float speed = 60f;

	private bool  forward = true;
		
	// Update is called once per frame
	void Update()
	{
		float rot_x = transform.eulerAngles.x;
		float rot_y = transform.eulerAngles.y;
		float rot_z = transform.eulerAngles.z;

		if (forward)
		{
			rot_y += Time.deltaTime * speed;

			if (rot_y >= 60 && rot_y < 300)
			{
				rot_y = 60;
				forward = false;
			}
		}
		else
		{
			rot_y -= Time.deltaTime * speed;

			if (rot_y > 60 && rot_y <= 300)
			{
				rot_y   = 300;
				forward = true;
			}
		}

		transform.eulerAngles = new Vector3(rot_x, rot_y, rot_z);
	}
}
