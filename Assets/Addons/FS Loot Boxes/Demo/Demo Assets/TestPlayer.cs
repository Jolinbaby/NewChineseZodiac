// © 2019 Flying Saci Game Studio
// written by Sylker Teles

using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    float speed = 3;

    private void Start()
    {
        LootBox box = FindObjectOfType<LootBox>();
        if (box) box.OnBoxOpen += GetChestItems;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * -Input.GetAxis("Vertical") * speed);
        transform.Translate(Vector3.right * Time.deltaTime * -Input.GetAxis("Horizontal") * speed);
    }

    void GetChestItems (GameObject[] prizes)
    {
        foreach (GameObject prize in prizes)
        {
            // do something with the prizes here
            Debug.Log("You got " + prize.name);
        }
    }
}
