using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
	public void OnButtonClick()
	{
		rigidbody.velocity = new Vector3(0, 4, 0);
	}
}
