using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ComboBoxTestScript : MonoBehaviour, IPointerClickHandler
{
	public  Button    buttonPrefab   = null;
	public  int       buttonOffsetX  = 2;
	public  int       buttonOffsetY  = 2;
	public  int       buttonWidth    = 120;
	public  int       buttonHeight   = 30;
	public  Color     selectedColor  = new Color(0.25f, 0.25f, 1f);

	private Transform mCanvas        = null;
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

		Canvas canvas = FindInParents<Canvas>();

		if (canvas == null)
		{
			Debug.LogError("ComboBox doesn't belongs to Canvas");
			return;
		}

		mCanvas = canvas.transform;

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
			moveItemsToComboBox();
		}
		else
		{
			moveItemsToScrollRect();
		}
	}

	public void moveItemsToScrollRect()
	{ 
		GameObject selectedItem = null;

		int i = 0;

		RectTransform scrollRect = mScrollContent.GetComponent<RectTransform>();
		scrollRect.sizeDelta = new Vector2(scrollRect.sizeDelta.x, buttonOffsetY + buttonHeight * (mItems.childCount + 1));

		while (mItems.childCount > 0)
		{
			GameObject itemButton = Instantiate(buttonPrefab.gameObject) as GameObject;

			itemButton.transform.SetParent(mScrollContent);
			ItemClickScript itemClickScript = itemButton.AddComponent<ItemClickScript>();
			itemClickScript.mMainScript     = this;
			Button buttonScript             = itemButton.GetComponent<Button>();			
			buttonScript.transition         = Selectable.Transition.ColorTint;

			RectTransform buttonRect    = itemButton.GetComponent<RectTransform>();

			buttonRect.pivot            = new Vector2(0, 1);
			buttonRect.anchoredPosition = new Vector2(0, 0);
			buttonRect.localPosition    = new Vector3(buttonOffsetX, -buttonOffsetY - buttonHeight * i, 0);
			buttonRect.sizeDelta        = new Vector2(buttonWidth, buttonHeight);
			buttonRect.localScale       = new Vector3(1, 1, 1);

			// -----------------------------------------

			Transform child = mItems.GetChild(0);

			child.gameObject.SetActive(true);
			child.SetParent(itemButton.transform);

			RectTransform childRect = child.GetComponent<RectTransform>();

			childRect.anchoredPosition = new Vector2(0, 0);
			childRect.sizeDelta        = new Vector2(-10, -10);

			if (i == mSelectedItem)
			{
				selectedItem = child.gameObject;
				ColorBlock colors = buttonScript.colors;
				colors.normalColor = selectedColor;
				buttonScript.colors = colors;
			}

			++i;
		}

		if (selectedItem != null)
		{
			selectedItem = Instantiate(selectedItem) as GameObject;
			selectedItem.transform.SetParent(mItems);

			RectTransform selectedItemRect = selectedItem.GetComponent<RectTransform>();
			
			selectedItemRect.anchoredPosition = new Vector2(0, 0);
			selectedItemRect.localPosition    = new Vector3(0, 0, 0);
			selectedItemRect.sizeDelta        = new Vector2(-10, -10);
			selectedItemRect.localScale       = new Vector3(1, 1, 1);
		}

		mScrollRect.SetParent(mCanvas);
		mScrollRect.SetAsLastSibling();
		mScrollRect.gameObject.SetActive(true);
	}

	public void moveItemsToComboBox()
	{
		int i = 0;

		if (mItems.childCount == 1)
		{
			DestroyObject(mItems.GetChild(0).gameObject);
		}

		while (mScrollContent.childCount > 0)
		{
			Transform child = mScrollContent.GetChild(0);
			
			if (child.childCount == 1)
			{
				Transform realChild = child.GetChild(0);
				
				realChild.gameObject.SetActive(mSelectedItem == i);
				realChild.SetParent(mItems);

				RectTransform childRect = realChild.GetComponent<RectTransform>();
				
				childRect.anchoredPosition = new Vector2(0, 0);
				childRect.sizeDelta        = new Vector2(-10, -10);
			}

			child.SetParent(null);
			
			DestroyObject(child.gameObject);

			++i;
		}

		mScrollRect.gameObject.SetActive(false);
		mScrollRect.SetParent(transform);
		mScrollRect.SetAsLastSibling();
	}

	public void SetSelectedItem(int value)
	{
		mSelectedItem = value;
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

	public T FindInParents<T>() where T : Component
	{
		var comp = gameObject.GetComponent<T>();
		
		if (comp != null)
		{
			return comp;
		}
		
		Transform t = gameObject.transform.parent;
		
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		
		return comp;
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
			    mCanvas     != null
			    &&
				mItems      != null
				&&
				mScrollRect != null
			   );
	}
}
