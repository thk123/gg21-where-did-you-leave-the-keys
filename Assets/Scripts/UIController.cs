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

[Serializable]
public struct TutorialElements
{
    public MaskableGraphic TextRoot;
    public MaskableGraphic InstructionsRoot;
}

public class UIController : MonoBehaviour
{
    public SuccessElements SuccessElements;
    public EndGameElements EndGameElements;
    public TutorialElements TutorialElements;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(SuccessElements.RootElement != null, "Success root element must be set");
        Debug.Assert(SuccessElements.TimeTextBox != null, "Time box must be set");
        Debug.Assert(TutorialElements.InstructionsRoot != null);
        Debug.Assert(TutorialElements.TextRoot != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowSuccess(float timeTaken)
    {
        SuccessElements.TimeTextBox.text = timeTaken.ToString(".0s");
        return ShowSuccess_Coroutine();
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
    public IEnumerator ShowTutorial_Iter()
    {
        TutorialElements.TextRoot.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        TutorialElements.TextRoot.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        TutorialElements.InstructionsRoot.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        TutorialElements.InstructionsRoot.gameObject.SetActive(false);
    }

    public void ShowEndGame(bool diDWin, int doorsUnlocked, float totalTime)
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
        SceneManager.LoadScene("Menu");
    }
}
