using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DragAndDropTestScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	public ItemType       itemType      = ItemType.NONE;
	public ItemType       allowItemType = ItemType.NONE;

	private Image         mImage;
	private Image         mContainerImage;
	private GameObject    mDraggingIcon;
	private RectTransform mDraggingPlane;
	private Color         mNormalColor;
	private static Color  mHighlightedColor = new Color(1f, 1f, 0f, 0.4f);
	private static Color  mDropColor        = new Color(0f, 1f, 1f, 0.4f);

	// Use this for initialization
	void Start()
	{
		mImage          = GetComponent<Image>();
		mContainerImage = transform.parent.GetComponent<Image>();
		mDraggingIcon   = null;
		mDraggingPlane  = null;
		mNormalColor    = mContainerImage.color;
	}
	
	public void OnPointerEnter(PointerEventData data)
	{
		if (data.pointerDrag != null)
		{
			DragAndDropTestScript sourceObject = data.pointerDrag.GetComponent<DragAndDropTestScript>();

			if (
				sourceObject == null
				||
				sourceObject.itemType == ItemType.NONE
				||
				(
				 allowItemType != ItemType.NONE
				 &&
				 allowItemType != sourceObject.itemType
				)
			   )
			{
				return;
			}

			mContainerImage.color = mDropColor;
		}
		else
		{
			if (itemType == ItemType.NONE)
			{
				return;
			}

			mContainerImage.color = mHighlightedColor;
		}
	}

	public void OnPointerExit(PointerEventData data)
	{
		mContainerImage.color = mNormalColor;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (itemType == ItemType.NONE)
		{
			return;
		}

		Canvas canvas = FindInParents<Canvas>(gameObject);

		if (canvas == null)
		{
			return;
		}

		mContainerImage.color = mDropColor;
		
		// We have clicked something that can be dragged.
		// What we want to do is create an icon for this.
		mDraggingIcon = new GameObject("Dragging icon");
		
		mDraggingIcon.transform.SetParent(canvas.transform, false);
		mDraggingIcon.transform.SetAsLastSibling();
		
		Image image = mDraggingIcon.AddComponent<Image>();
		// The icon will be under the cursor.
		// We want it to be ignored by the event system.
		mDraggingIcon.AddComponent<IgnoreRaycast>();

		image.sprite = mImage.sprite;
		image.rectTransform.sizeDelta = new Vector2(32, 32);
		
		mDraggingPlane = canvas.transform as RectTransform;
		
		SetDraggedPosition(eventData);
	}

	public void OnDrag(PointerEventData data)
	{
		if (mDraggingIcon != null)
		{
			SetDraggedPosition(data);
		}
	}
		
	public void OnEndDrag(PointerEventData eventData)
	{
		if (mDraggingIcon != null)
		{
			Destroy(mDraggingIcon);
			mDraggingIcon = null;
		}
	}

	public void OnDrop(PointerEventData data)
	{
		if (data.pointerDrag != this)
		{
			DragAndDropTestScript sourceObject = data.pointerDrag.GetComponent<DragAndDropTestScript>();
			
			if (
				sourceObject == null
				||
				sourceObject.itemType == ItemType.NONE
				||
				(
				 allowItemType != ItemType.NONE
				 &&
				 allowItemType != sourceObject.itemType
				)
			   )
			{
				return;
			}

			Image sourceImage = data.pointerDrag.GetComponent<Image>();

			Sprite temp        = mImage.sprite;
			mImage.sprite      = sourceImage.sprite;
			sourceImage.sprite = temp;

			ItemType tempType     = sourceObject.itemType;
			sourceObject.itemType = itemType;
			itemType              = tempType;
		}
	}

	private void SetDraggedPosition(PointerEventData data)
	{		
		RectTransform rt = mDraggingIcon.GetComponent<RectTransform>();
		Vector3 globalMousePos;

		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(mDraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
		}
	}

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return null;
		}

		var comp = go.GetComponent<T>();
		
		if (comp != null)
		{
			return comp;
		}
		
		Transform t = go.transform.parent;

		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}

		return comp;
	}
}
