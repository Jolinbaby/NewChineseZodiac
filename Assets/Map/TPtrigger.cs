using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class TPtrigger : MonoBehaviour
{
    private List<GameObject> wells;
    private GameObject tpmanager;
    private int well_nums = 0;
    public int if_certain = 0;
    public bool isinwell;

    void Start()
    {
        tpmanager = GameObject.Find("TPmanager");
        isinwell = false;

        if (if_certain == 1)
        {
            wells = tpmanager.GetComponent<TransportControl>().certain_wells1;
            well_nums = wells.Count;
        }
        else if (if_certain == 2)
        {
            wells = tpmanager.GetComponent<TransportControl>().certain_wells2;
            well_nums = wells.Count;
        }
        else
        {
            wells = tpmanager.GetComponent<TransportControl>().wells;
            well_nums = wells.Count;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        isinwell = true;
        print(collider.gameObject.name + ":" + Time.time);
        Transport(collider.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        isinwell = false;
    }

    private void Transport(GameObject player)
    {
        System.Random r = new System.Random();
        int idx = 0;
        while (isinwell)
        {
            idx = r.Next(0, well_nums);
            Debug.Log("the well index=" + idx);
            if (wells[idx] != this.transform.parent.gameObject)
            {
                Debug.Log(wells[idx]);
                Debug.Log(this.transform.parent.gameObject);
                Vector3 position = wells[idx].transform.position;
                position.y += 10;
                position.z += 10;
                position.x += 10;
                Debug.Log(player);
                Debug.Log(position);
                player.GetComponent<CharacterController>().enabled =false;
                player.GetComponent<PlayerControl.PlayerInputs>().enabled = false;
                player.GetComponent<PlayerInput>().enabled = false;
                player.GetComponent< PlayerControl.ThirdPersonController> ().enabled = false;
                player.transform.SetPositionAndRotation(position, player.transform.rotation);
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerControl.PlayerInputs>().enabled = true;
                player.GetComponent<PlayerInput>().enabled = true;
                player.GetComponent<PlayerControl.ThirdPersonController>().enabled = true;
                break;
            }
        }
    }
}
