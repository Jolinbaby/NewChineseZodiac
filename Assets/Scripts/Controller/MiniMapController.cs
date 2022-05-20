using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset;

    void Update()
    {
        if (Player != null)
        {
            transform.position = Player.position + offset;

            // rotation
            Vector3 rot = new Vector3(90, Player.eulerAngles.y, 0);
            transform.rotation = Quaternion.Euler(rot);
        }
    }
}
