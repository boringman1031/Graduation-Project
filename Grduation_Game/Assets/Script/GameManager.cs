using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public GameObject playerPrefab;
    public GameObject playerUIPrefab;
    public GameObject playerObject;
    public GameObject playerUIObject;
   
    void Start()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            playerObject = Instantiate(playerPrefab);
            playerUIObject = Instantiate(playerUIPrefab);
            DontDestroyOnLoad(playerObject);
            DontDestroyOnLoad(playerUIObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }
    public void  ChangeScene(string sceneName,int positionIndex)
    {
        SceneManager.LoadScene(sceneName);
        GameObject IndexObject = GameObject.Find(positionIndex.ToString()) as GameObject;
        if (IndexObject != null) { 
        playerObject.transform.position = IndexObject.transform.position;   
        }
    }
}
