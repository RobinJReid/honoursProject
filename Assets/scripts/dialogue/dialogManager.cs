using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class dialogManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI DialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Ink.Runtime.Story currentStory;

    private static dialogManager instance;

    bool _cursorLocked;

    public bool dialogueisPlaying { get; private set; }

    public bool[] cutsceneComplete;
    public bool stage1Delete;
    public bool stage1Done;
    public bool stage2Delete;
    public bool stage2Done;
    public bool stage3Delete;
    public bool stage3Done;
    public bool stage4Delete;
    public bool stage4Done;
    public bool gameDone;
    GameObject[] Mentor;
    GameObject[] needsDestroyed;

    private void Awake()
    {
        Mentor = GameObject.FindGameObjectsWithTag("mentor1");
        stage1Done = false;
        stage2Done = false;
        stage3Done = false; 
        stage4Done = false;
        stage1Delete = false;
        stage2Delete = false;
        stage3Delete = false;
        stage4Delete = false;
        Debug.Log("thing is awake");
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogManager");
        }
        if (cutsceneComplete == null || cutsceneComplete.Length == 0)
        {
            cutsceneComplete = new bool[10];  
        }
        instance = this;
        for (int i = 0; i<cutsceneComplete.Length; i++)
        {
            if (i > 1)
            {
                cutsceneComplete[i] = true;
            }
            else
            {
                cutsceneComplete[i] = false;
            }

            Debug.Log("cutsceneComplete number : " + i + ". Total length : " + cutsceneComplete.Length);
        }
    }

    public static dialogManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cursorLocked = true;
        dialogueisPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueisPlaying) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset InkJson)
    {
        currentStory = new Ink.Runtime.Story(InkJson.text);
        dialogueisPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
        Cursor.lockState = CursorLockMode.None;
        _cursorLocked = false;
    }

    private IEnumerator ExitDialogueMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cursorLocked = true;
        yield return new WaitForSeconds(0.2f);
        dialogueisPlaying=false;
        Debug.Log("S1Done:" + stage1Done + "S1Delete:" + stage1Delete + "S2Done:" + stage2Done + "S2Delete:" + stage2Delete);
        Debug.Log("S3Done:" + stage3Done + "S3Delete:" + stage3Delete + "S4Done:" + stage4Done + "S4Delete:" + stage4Delete);
        if (stage1Delete)
        {
            for (int i = 0; i<Mentor.Length; i++)
            {
                Destroy(Mentor[i]);
            }
            for (int j = 0; j < cutsceneComplete.Length; j++)
            {
                cutsceneComplete[2] = false;
                cutsceneComplete[3] = false;
                cutsceneComplete[4] = false;
                cutsceneComplete[5] = false;

                Debug.Log("cutsceneComplete number : " + j + ". Total length : " + cutsceneComplete.Length);
                stage1Done = true;
            }
            stage1Delete = false;
        }
        if (stage2Delete)
        {
            for (int j = 0; j < cutsceneComplete.Length; j++)
            {
                cutsceneComplete[6] = false;
                Debug.Log("cutsceneComplete number : " + j + ". Total length : " + cutsceneComplete.Length);
            }
            stage2Done = true;
        }
        if (stage3Delete)
        {
            cutsceneComplete[7] = false;
            stage3Done = true;
        }
        if (stage4Delete)
        {
            cutsceneComplete[8] = false;
            cutsceneComplete[9] = false;
            stage4Done = true;
            stage4Delete = false;
        }
        dialoguePanel.SetActive(false);
        DialogueText.text = "";
        needsDestroyed = GameObject.FindGameObjectsWithTag("TempCharacter");
        for (int i = 0; i < needsDestroyed.Length; i++)
        {
            Destroy(needsDestroyed[i]);
        }
        if (gameDone)
        {

           SceneManager.LoadScene("End");
            
        }
        
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            DialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Ink.Runtime.Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length) 
        {
            Debug.LogError("more choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach(Ink.Runtime.Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);

        }
        SelectFirstChoice();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    public void setCutsceneComplete(int index)
    {
        Debug.Log("S1Done:" + stage1Done + "S1Delete:" + stage1Delete + "S2Done:" + stage2Done + "S2Delete:" + stage2Delete);
        Debug.Log("S3Done:" + stage3Done + "S3Delete:" + stage3Delete + "S4Done:" + stage4Done + "S4Delete:" + stage4Delete);
        Debug.Log("cutscene being set complete");
        for (int i = 0; i < cutsceneComplete.Length; i++)
        {
            if (i == index)
            {
                cutsceneComplete[i] = true;
                Debug.Log("Index and array index:" + index + " " + cutsceneComplete[i]);

                
            }
        }
        if (cutsceneComplete[0] && cutsceneComplete[1] && !stage1Done)
        {
            stage1Delete = true;

        }
        if (cutsceneComplete[4] && cutsceneComplete[5] && cutsceneComplete[3] && !stage2Done && stage1Done)
        {
            stage2Delete = true;

        }
        if (cutsceneComplete[6] && !stage3Done && stage2Done)
        {
            stage3Delete = true;
        }
        if (cutsceneComplete[7] && !stage4Done && stage3Done)
        {
            Debug.Log("Stage 4 should be prepared now, aka cutscene 8 should work T-T");
            stage4Delete = true;
        }
        if (cutsceneComplete[9] && stage4Done)
        {
            gameDone = true;
        }
        Debug.Log("S1Done:" + stage1Done + "S1Delete:" + stage1Delete + "S2Done:" + stage2Done + "S2Delete:" + stage2Delete);
        Debug.Log("S3Done:" + stage3Done + "S3Delete:" + stage3Delete + "S4Done:" + stage4Done + "S4Delete:" + stage4Delete);
    }
    public void GameDone()
    {
        gameDone = true;
    }
}
