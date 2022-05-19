using UnityEngine;

public class MaxMapShow : MonoBehaviour
{
    private Transform child1;
    private Transform child2;

    private void Awake()
    {
        Debug.Log("������awake");
        child1 = this.transform.GetChild(0);
        child2 = this.transform.GetChild(1);
        child1.gameObject.SetActive(false);
        child2.gameObject.SetActive(true);
    }

    void Update()
    {
        Debug.Log("������update");
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("����M��");
            //ShowUI(childs, true);
        }

        //if (Input.GetKeyUp(KeyCode.M))
        //{
          //  Debug.Log("�ɿ�M��");
            //ShowUI(childs, false);
        //}
        
    }

    void ShowUI(Transform[] gameObjects, bool active)
    {
        foreach (var child in gameObjects)
        {
            Debug.Log(gameObject.name);
            child.gameObject.SetActive(active);
        }
    }
}
