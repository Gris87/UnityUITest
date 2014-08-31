using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ComboBoxTestScript : MonoBehaviour, IPointerClickHandler
{
	private Transform mItems         = null;
	private Transform mScrollRect    = null;
	private Transform mScrollContent = null;
	private int       mSelectedItem  = -1;

	// Use this for initialization
	void Start()
	{
		if (GetComponent<Button>() != null)
		{
			return;
		}

		mItems = gameObject.transform.FindChild("Items");

		if (mItems == null)
		{
			Debug.LogError("Items doesn't found in ComboBox");
			return;
		}

		ScrollRect scrollRect = FindInChildren<ScrollRect>();
		
		if (scrollRect == null)
		{
			Debug.LogError("ScrollRect doesn't found in ComboBox");
			return;
		}

		mScrollRect = scrollRect.transform;		
		mScrollRect.gameObject.SetActive(false);

		mScrollContent = scrollRect.content.transform;

		for (int i=0; i<mItems.childCount; ++i)
		{
			if (mItems.GetChild(i).gameObject.activeSelf)
			{
				mSelectedItem = i;
				break;
			}
		}
	}

	public void OnPointerClick(PointerEventData data)
	{
		if (!isInitiated())
		{
			transform.parent.GetComponent<ComboBoxTestScript>().OnPointerClick(data);
			return;
		}

		if (mScrollRect.gameObject.activeSelf)
		{
			mScrollRect.gameObject.SetActive(false);
			moveItemsToComboBox();
		}
		else
		{
			mScrollRect.gameObject.SetActive(true);
			moveItemsToScrollRect();
		}
	}

	public void moveItemsToScrollRect()
	{ 
		while (mItems.childCount>0)
		{
			Transform child = mItems.GetChild(0);

			child.gameObject.SetActive(true);
			child.SetParent(mScrollContent);
		}
	}

	public void moveItemsToComboBox()
	{
		int i = 0;

		while (mScrollContent.childCount>0)
		{
			Transform child = mScrollContent.GetChild(0);
			
			child.gameObject.SetActive(mSelectedItem == i);
			child.SetParent(mItems);

			++i;
		}
	}

	public int GetSelectedItem()
	{
		return mSelectedItem;
	}

	public T FindInChildren<T>() where T : Component
	{
		for (int i=0; i<transform.childCount; ++i)
		{
			var comp = transform.GetChild(i).GetComponent<T>();
			
			if (comp != null)
			{
				return comp;
			}
		}
		
		return null;
	}

	public bool isInitiated()
	{
		return (
				mItems         != null
				&&
				mScrollRect    != null
			   );
	}
}
