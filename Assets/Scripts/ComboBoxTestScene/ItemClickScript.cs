using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemClickScript : MonoBehaviour, IPointerClickHandler
{
	public ComboBoxTestScript mMainScript;

	public void OnPointerClick(PointerEventData data)
	{
		Transform t = transform.parent;

		for (int i=0; i<t.childCount; ++i)
		{
			if (t.GetChild(i) == transform)
			{
				mMainScript.SetSelectedItem(i);
				mMainScript.moveItemsToComboBox();

				break;
			}
		}
	}
}
