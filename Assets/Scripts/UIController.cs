using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct SuccessElements
{
    public MaskableGraphic RootElement;
    public Text TimeTextBox;
}

[Serializable]
public struct EndGameElements
{
    public MaskableGraphic RootElement;
    public Text Outcome;
    public Text PeopleIn;
}

public class UIController : MonoBehaviour
{
    public SuccessElements SuccessElements;
    public EndGameElements EndGameElements;
    

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
        SuccessElements.TimeTextBox.text = timeTaken.ToString(".0s");
        StartCoroutine(ShowSuccess_Coroutine());
    }

    private IEnumerator ShowSuccess_Coroutine()
    {
        SuccessElements.RootElement.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        SuccessElements.RootElement.gameObject.SetActive(false);
    }

    public void ShowFailure(int doorsUnlocked, float totalTime)
    {
        ShowEndGame(false, doorsUnlocked, totalTime);
    }

    public void ShowCompletion(int doorsUnlocked, float totalTime)
    {
        ShowEndGame(true, doorsUnlocked, totalTime);
    }

    private void ShowEndGame(bool diDWin, int doorsUnlocked, float totalTime)
    {
        EndGameElements.RootElement.gameObject.SetActive(true);
        EndGameElements.Outcome.text = diDWin ? "You Won" : "You Lost";
        EndGameElements.PeopleIn.text = EndGameElements.PeopleIn.text
            .Replace("{N}", doorsUnlocked.ToString())
            .Replace("{X}", totalTime.ToString(".0s"));
    }

    public void OnPlayAgain_BtnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit_BtnClick()
    {
        // TODO: main menu
        Application.Quit();
    }
}
