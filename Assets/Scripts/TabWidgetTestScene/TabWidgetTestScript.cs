using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabWidgetTestScript : MonoBehaviour
{
	private Transform mTabs         = null;
	private Transform mPages        = null;
	private int       mCurrentIndex = -1;

	// Use this for initialization
	void Start()
	{
		mTabs = gameObject.transform.FindChild("Tabs");
		
		if (mTabs == null)
		{
			Debug.LogError("Tabs doesn't found in ComboBox");
			return;
		}

		mTabs = mTabs.GetComponent<ScrollRect>().content.transform;

		mPages = gameObject.transform.FindChild("Pages");
		
		if (mPages == null)
		{
			Debug.LogError("Pages doesn't found in ComboBox");
			return;
		}
	}

	public void UpdateTabs()
	{
	}

	public void UpdateSelectedIndex()
	{
	}

	public void SetCurrentIndex(int index)
	{
		mCurrentIndex = index;
		UpdateSelectedIndex();
	}
	
	public int GetCurrentIndex()
	{
		return mCurrentIndex;
	}
}
