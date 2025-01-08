using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
/*------------------by 017-----------------------*/

public class GameManager : MonoBehaviour
{

    static GameManager instance;
    [SerializeField]
    public GameObject GammingCanvas;
    [SerializeField]
    public GameObject GameInfoCanvas;
    [SerializeField]
    public GameObject RandomChallengeCanvas;
    [SerializeField]
    public GameObject LottoCanvas;
    [SerializeField]
    public GameObject YuelaoTempleCanvas;

    public PlayerStats DefaultPlayerStats = new PlayerStats
    {
        Health = 100,
        Defence = 100,
        Speed = 10.0f,
        Power = 100,
        Attack = 100
    };

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            //載入場景時不會被刪除
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(GammingCanvas);
            DontDestroyOnLoad(GameInfoCanvas);
            DontDestroyOnLoad(RandomChallengeCanvas);
            DontDestroyOnLoad(LottoCanvas);
            DontDestroyOnLoad(YuelaoTempleCanvas);
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }
    public void  ChangeScene(string sceneName,int positionIndex)
    {
       
    }
}
