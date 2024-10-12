using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pausemenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool menuOpen = false;
    public GameObject pausemenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuOpen == false)
        {
            pausemenu.SetActive(true);
            menuOpen = true;
            Debug.Log("wow");
        }
        
    }
    public void Resume()
    {
        pausemenu.SetActive(false);
        menuOpen = false;

    }
    public void goToMainmenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
