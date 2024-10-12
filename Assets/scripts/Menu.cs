using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource cricketsound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void singleplayer()
    {
        SceneManager.LoadScene("test");
    }
    public void Options()
    {
        cricketsound.Play();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
}
