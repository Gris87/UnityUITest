using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabWidgetTestScript : MonoBehaviour
{
	public int tabWidth = 100;

	private Transform mTabs         = null;
	private Transform mPages        = null;
	private int       mCurrentIndex = -1;

	public int currentIndex
	{
		get
		{
			return mCurrentIndex;
		}

		set
		{
			mCurrentIndex = value;
			UpdateSelectedIndex();
		}
	}

	// Use this for initialization
	void Start()
	{
		mTabs = gameObject.transform.FindChild("Tabs");
		
		if (mTabs == null)
		{
			Debug.LogError("Tabs doesn't found in TabWidget");
			return;
		}

		mTabs = mTabs.GetComponent<ScrollRect>().content.transform;

		mPages = gameObject.transform.FindChild("Pages");
		
		if (mPages == null)
		{
			Debug.LogError("Pages doesn't found in TabWidget");
			return;
		}

		if (mTabs.childCount != mPages.childCount)
		{
			Debug.LogError("Different amount of pages and tabs in TabWidget");
			return;
		}

		if (mPages.childCount == 0)
		{
			Debug.LogWarning("Please add atleast one page to TabWidget");
		}

		for (int i=0; i<mPages.childCount; ++i)
		{
			if (mCurrentIndex >= 0)
			{
				mPages.GetChild(i).gameObject.SetActive(false);
				mTabs.GetChild(i).GetComponent<Toggle>().isOn = false;
			}
			else
			if (mPages.GetChild(i).gameObject.activeSelf)
			{
				mCurrentIndex = i;
				mTabs.GetChild(i).GetComponent<Toggle>().isOn = true;
			}
		}

		UpdateTabs();
		UpdateSelectedIndex();
	}

	public void UpdateTabs()
	{
		RectTransform tabsRect  = mTabs.GetComponent<RectTransform>();
		float         tabHeight = tabsRect.sizeDelta.y;

		for (int i=0; i<mTabs.childCount; ++i)
		{
			RectTransform tabRect=mTabs.GetChild(i).GetComponent<RectTransform>();

			tabRect.pivot            = new Vector2(0, 1);
			tabRect.anchoredPosition = new Vector2(0, 0);
			tabRect.localPosition    = new Vector3(tabWidth * i, 0, 0);
			tabRect.sizeDelta        = new Vector2(tabWidth, tabHeight);
		}

		Vector3 prevTabPos = new Vector3(tabsRect.localPosition.x, 0, 0);

		tabsRect.pivot            = new Vector2(0, 1);
		tabsRect.anchoredPosition = new Vector2(0, 0);
		tabsRect.localPosition    = prevTabPos;
		tabsRect.sizeDelta        = new Vector2(tabWidth * mTabs.childCount, tabHeight);
	}

	public void UpdateSelectedIndex()
	{
		for (int i=0; i<mPages.childCount; ++i)
		{
			mPages.GetChild(i).gameObject.SetActive(false);
		}
		
		if (mPages.childCount > 0)
		{
			if (mCurrentIndex < 0)
			{
				mCurrentIndex = 0;
			}
			else
			if (mCurrentIndex >= mPages.childCount)
			{
				mCurrentIndex = mPages.childCount - 1;
			}
						
			mPages.GetChild(mCurrentIndex).gameObject.SetActive(true);
		}
		else
		{
			mCurrentIndex = -1;
		}
	}
}
