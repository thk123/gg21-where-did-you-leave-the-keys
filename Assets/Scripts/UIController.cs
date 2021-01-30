using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SuccessElements
{
    public MaskableGraphic RootElement;
    public Text TimeTextBox;
}

public class UIController : MonoBehaviour
{
    public SuccessElements SuccessElements;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(SuccessElements.RootElement != null, "Success root element must be set");
        Debug.Assert(SuccessElements.TimeTextBox != null, "Time box must be set");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSuccess(float timeTaken)
    {
        SuccessElements.TimeTextBox.text = timeTaken.ToString("1.0s");
        StartCoroutine(ShowSuccess_Coroutine());
    }

    private IEnumerator ShowSuccess_Coroutine()
    {
        SuccessElements.RootElement.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        SuccessElements.RootElement.gameObject.SetActive(false);
    }

    public void ShowFailure()
    {

    }
}
