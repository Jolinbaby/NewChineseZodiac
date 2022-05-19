using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class subTuichu : MonoBehaviour
{
    public string sceneFrom;
    public void OnclickedGetout()
    {
        Debug.Log("cliced out");
        SceneManager.UnloadSceneAsync(sceneFrom); //–∂‘ÿfrom
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
