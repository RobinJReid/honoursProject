using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pickUp : MonoBehaviour
{
    private bool in_object;
    private bool hasItem;
    public GameObject PossibleItem;
    public GameObject HeldItem;
    public GameObject myHands;
    public GameObject player;

    void Start()
    {
        in_object = false;
        hasItem = false;

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.E) && in_object == true && hasItem == false)
        {
            Debug.Log("Would be picking up item rn");
            PossibleItem.GetComponent<Rigidbody>().isKinematic = true;
            PossibleItem.transform.position = myHands.transform.position; // sets the position of the object to your hand position
            PossibleItem.transform.parent = myHands.transform;
            HeldItem = PossibleItem;
            hasItem = true;
        }
        if (Input.GetKey(KeyCode.Mouse1) && hasItem == true)
        {
            Debug.Log("throwing item");
            HeldItem.GetComponent<Rigidbody>().isKinematic = false;
            HeldItem.transform.parent = HeldItem.transform.parent = null;
            HeldItem.GetComponent<Rigidbody>().AddForce(player.transform.forward.normalized * 50);
            HeldItem = null;
            hasItem = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        in_object = true;
        if (other.gameObject.transform.parent != null)
        {
            PossibleItem = other.gameObject.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        in_object = false;
        PossibleItem = null;
    }
}
