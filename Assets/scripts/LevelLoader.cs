using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void loadLevel2()
    {
        SceneManager.LoadScene("nonLinear");
    }
    public void loadMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
