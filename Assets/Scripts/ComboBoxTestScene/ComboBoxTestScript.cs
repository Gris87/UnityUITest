using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ComboBoxTestScript : MonoBehaviour, IPointerClickHandler
{
	public  Button    buttonPrefab   = null;

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

		if (buttonPrefab == null)
		{
			Button selectButton = FindInChildren<Button>();

			if (selectButton == null)
			{
				Debug.LogError("Select button doesn't found in ComboBox");
				return;
			}

			buttonPrefab = selectButton.GetComponent<ComboBoxTestScript>().buttonPrefab;
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
		int i = 0;

		while (mItems.childCount > 0)
		{
			GameObject itemButton = Instantiate(buttonPrefab.gameObject) as GameObject;
			itemButton.transform.SetParent(mScrollContent);

			RectTransform buttonRect    = itemButton.GetComponent<RectTransform>();

			buttonRect.pivot            = new Vector2(0, 1);
			buttonRect.anchoredPosition = new Vector2(0, 0);
			buttonRect.localScale       = new Vector3(1, 1, 1);
			buttonRect.sizeDelta        = new Vector2(120, 30);
			buttonRect.localPosition    = new Vector3(0, -30 * i, 0);

			// -----------------------------------------

			Transform child = mItems.GetChild(0);

			child.gameObject.SetActive(true);
			child.SetParent(itemButton.transform);

			++i;
		}
	}

	public void moveItemsToComboBox()
	{
		int i = 0;

		while (mScrollContent.childCount > 0)
		{
			Transform child = mScrollContent.GetChild(0);
			
			if (child.childCount == 1)
			{
				Transform realChild = child.GetChild(0);
				
				realChild.gameObject.SetActive(mSelectedItem == i);
				realChild.SetParent(mItems);
			}

			child.SetParent(null);
			
			DestroyObject(child.gameObject);

			++i;
		}
	}

	public int GetSelectedItem()
	{
		if (isInitiated())
		{
			return mSelectedItem;
		}
		else
		{
			return transform.parent.GetComponent<ComboBoxTestScript>().mSelectedItem;
		}
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
				mItems      != null
				&&
				mScrollRect != null
			   );
	}
}
