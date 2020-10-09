using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        SceneManager.LoadScene("Menu");
    }
    IEnumerator RestartGameAnim()
    {
        Camera.main.GetComponent<Animator>().SetTrigger("CompleteGame");
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(GameObject.FindGameObjectWithTag("GameManager"));
        SceneManager.LoadScene("Menu");
    }
}
