using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI FPSText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.FPSText.SetText("FPS = " + ((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }
}
