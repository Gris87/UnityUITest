using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorTestScript : MonoBehaviour
{
	private Button    mLeftButton    = null;
	private Button    mRightButton   = null;
	private Transform mItems         = null;
	private int       mSelectedItem  = -1;

	// Use this for initialization
	void Start()
	{
		Transform leftButton = gameObject.transform.FindChild("LeftButton");

		if (leftButton == null)
		{
			Debug.LogError("LeftButton doesn't found in Selector");
			return;
		}

		mLeftButton = leftButton.GetComponent<Button>();

		if (mLeftButton == null)
		{
			Debug.LogError("LeftButton doesn't found in Selector");
			return;
		}

		Transform rightButton = gameObject.transform.FindChild("RightButton");
		
		if (rightButton == null)
		{
			Debug.LogError("RightButton doesn't found in Selector");
			return;
		}
		
		mRightButton = rightButton.GetComponent<Button>();
		
		if (mRightButton == null)
		{
			Debug.LogError("RightButton doesn't found in Selector");
			return;
		}

		mItems = gameObject.transform.FindChild("Items");
		
		if (mItems == null)
		{
			Debug.LogError("Items doesn't found in Selector");
			return;
		}

		for (int i=0; i<mItems.childCount; ++i)
		{
			if (mSelectedItem >= 0)
			{
				mItems.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				if (mItems.GetChild(i).gameObject.activeSelf)
				{
					mSelectedItem = i;
				}
			}
		}

		UpdateSelectedItem();
	}
	
	public void OnLeftButtonClicked()
	{
		SetSelectedItem(mSelectedItem - 1);
	}

	public void OnRightButtonClicked()
	{
		SetSelectedItem(mSelectedItem + 1);
	}

	public void UpdateSelectedItem()
	{
		for (int i=0; i<mItems.childCount; ++i)
		{
			mItems.GetChild(i).gameObject.SetActive(false);
		}

		if (mItems.childCount > 0)
		{
			if (mSelectedItem < 0)
			{
				mSelectedItem = 0;
			}
			else
			if (mSelectedItem >= mItems.childCount)
			{
				mSelectedItem = mItems.childCount - 1;
			}

			mLeftButton.interactable  = (mSelectedItem > 0);
			mRightButton.interactable = (mSelectedItem < mItems.childCount - 1);

			mItems.GetChild(mSelectedItem).gameObject.SetActive(true);
		}
		else
		{
			mSelectedItem              = -1;
			mLeftButton.interactable  = false;
			mRightButton.interactable = false;
		}
	}

	public void SetSelectedItem(int value)
	{
		mSelectedItem = value;
		UpdateSelectedItem();
	}	
	
	public int GetSelectedItem()
	{
		return mSelectedItem;
	}
}
