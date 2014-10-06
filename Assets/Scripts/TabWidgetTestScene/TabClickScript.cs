using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TabClickScript : MonoBehaviour
{
	public void OnTabClicked(bool isOn)
	{
		if (isOn)
		{
			Transform t = transform.parent;

			for (int i=0; i<t.childCount; ++i)
			{
				if (t.GetChild(i) == transform)
				{
					TabWidgetTestScript script = FindInParents<TabWidgetTestScript>();
					script.currentIndex = i;

					break;
				}
			}
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
}
