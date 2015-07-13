using UnityEngine;
using System.Collections;

public class OpenSceneScript : MonoBehaviour
{
    public void openScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
