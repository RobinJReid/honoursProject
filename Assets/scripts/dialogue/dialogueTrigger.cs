using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class dialogueTriggerr : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Dialogue")]
    [SerializeField] private TextAsset inkJson;

    [Header("CutsceneDailogue")]
    [SerializeField] private TextAsset cutsceneDailogue;

    [Header("Cutscene Trigger?")]
    [SerializeField] private bool IsCutscene;
    [SerializeField] private bool IsFinalCutscene;

    [Header("Item Required for Cutscene?")]
    [SerializeField] private bool ItemRequired;
    [SerializeField] private GameObject Item;

    [Header("NonLinear Modifications")]
    [SerializeField] private bool isNonLinear;
    [SerializeField] private int cutsceneIndex;
    [SerializeField] private bool isAreaBlocker;
    [SerializeField] private SphereCollider AreaBlockCollider;

    [Header("Characters")]
    [SerializeField] private GameObject character1;
    [SerializeField] private Vector3 character1Location;
    [SerializeField] private Vector3 character1Rotation;
    [SerializeField] private GameObject character2;
    [SerializeField] private Vector3 character2Location;
    [SerializeField] private Vector3 character2Rotation;
    [SerializeField] private GameObject character3;
    [SerializeField] private Vector3 character3Location;
    [SerializeField] private Vector3 character3Rotation;

    private GameObject Dialogue;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        Debug.Log("thingy shouldnt be active");
        if (isAreaBlocker )
        {
            AreaBlockCollider.enabled = true;
        }
        else if (AreaBlockCollider != null)
        {
            AreaBlockCollider.enabled = false;
        }

    }
    void OnDisable()
    {
        Debug.Log("dialogueTriggerr DISABLED");
    }
    void OnEnable()
    {
        Debug.Log("dialogueTriggerr ENABLED");
    }

    private void Update()
    {
        Debug.Log(dialogManager.getInstance().dialogueisPlaying);
        if (playerInRange && !dialogManager.getInstance().dialogueisPlaying)
        {
            Debug.Log("Player can interact");
            visualCue.SetActive (true);
            if (Input.GetMouseButtonDown(0)) 
            {
                dialogManager.getInstance().EnterDialogueMode(inkJson);
                Debug.Log("thing");
            }
        }
        else if (playerInRange)
        {
            Debug.Log("Player in range is true");
        }
        else
        {
            visualCue.SetActive (false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Dialogue = GameObject.Find("dialogueManager");
        Debug.Log(Dialogue.name);
        Debug.Log("Something colliding");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            playerInRange=true;
            Debug.Log("Player should be in range");
            Debug.Log("Okateruinrange: " + playerInRange);
            if (IsCutscene)
            {
                // non linear confusion and pain
                if (isNonLinear)
                {
                    Debug.Log("trying to do cutscene" + cutsceneIndex);
                    Debug.Log("trying to access" + Dialogue.name);
                    Debug.Log("cutscene index " + cutsceneIndex);
                    if (!Dialogue.GetComponent<dialogManager>().cutsceneComplete[cutsceneIndex] && !isAreaBlocker)
                    {
                        if (ItemRequired)
                        {
                            if (Item == other.gameObject.GetComponent<pickUp>().HeldItem)
                            {
                                DoCutscene();
                                Dialogue.GetComponent<dialogManager>().setCutsceneComplete(cutsceneIndex);
                                Debug.Log("doing cutscene" + cutsceneIndex);
                            }
                            else
                            {
                                Debug.Log("You need to get the right item for this");
                            }
                        }
                        else if (!ItemRequired)
                        {
                            DoCutscene();
                            Debug.Log("doing cutscene" + cutsceneIndex);
                            Dialogue.GetComponent<dialogManager>().setCutsceneComplete(cutsceneIndex);
                        }
                    }
                    else if (isAreaBlocker)
                    {
                        if (cutsceneIndex == 2 && !Dialogue.GetComponent<dialogManager>().stage2Done)
                        {
                            Debug.Log("Stage 2 isnt complete and this is blocking to prevent going past stage 2 so not letting player past");
                            DoCutscene();
                        }
                        else if (cutsceneIndex == 3 && !Dialogue.GetComponent<dialogManager>().stage3Done)
                        {
                            Debug.Log("Stage 3 isnt complete and this is blocking to prevent going past stage 3 so not letting player past");
                            DoCutscene();
                        }
                        else if (cutsceneIndex == 4 && !Dialogue.GetComponent<dialogManager>().stage4Done)
                        {
                            Debug.Log("Stage 4 isnt complete and this is blocking to prevent going past stage 4 so not letting player past");
                            DoCutscene();
                        }
                        else
                        {
                            Debug.Log("the player is allowed past, so deleting collider");
                            AreaBlockCollider.enabled = false;
                        }
                    }
                    else
                    {
                        
                    }
                    
                }
                // linear stuff
                else
                {
                    if (IsFinalCutscene && ItemRequired)
                    {
                        Dialogue.GetComponent<dialogManager>().GameDone();
                        DoCutscene();
                    }
                    else if (ItemRequired)
                    {
                        if (Item == other.gameObject.GetComponent<pickUp>().HeldItem)
                        {
                            DoCutscene();
                        }
                        else
                        {
                            Debug.Log("You need to get the right item for this");
                        }
                    }
                    else if (!ItemRequired)
                    {
                        DoCutscene();
                    }
                    
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void DoCutscene()
    {
        if (character1 != null)
        {
            Instantiate(character1, character1Location, transform.rotation = Quaternion.Euler(character1Rotation));
        }
        if (character2 != null)
        {
            Instantiate(character2, character2Location, transform.rotation = Quaternion.Euler(character2Rotation));
        }
        if (character3 != null)
        {
            Instantiate(character3, character3Location, transform.rotation = Quaternion.Euler(character3Rotation));
        }
        dialogManager.getInstance().EnterDialogueMode(cutsceneDailogue);
        Debug.Log("doing cutscene");
        if (!isAreaBlocker)
        {
            IsCutscene = false;
        }
    }
}
