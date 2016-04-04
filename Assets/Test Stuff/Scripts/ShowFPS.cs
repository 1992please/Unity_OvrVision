using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {
    [SerializeField]
    [Range(1,10)]
    int RefreshRate = 2;
    float TimeCounter = 0.0f;
    float FramesCounter = 0;
    float FPS = 0;
    void Update()
    {
        if(TimeCounter < 1.0f/RefreshRate)
        {
            TimeCounter += Time.deltaTime;
            FramesCounter++;
        }
        else
        {
            FPS = FramesCounter * RefreshRate;
            FramesCounter = 0;
            TimeCounter = 0;
        }

    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 50);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        GUI.Label(rect, FPS + "", style);
    }
}
