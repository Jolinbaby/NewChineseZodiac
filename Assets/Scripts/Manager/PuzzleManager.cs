using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    // ��¼ƴͼ�Ƿ�ѡ��
    public Camera mainCamera;
    private GameObject selectedGameObject;
    // ƴͼ��Ƭ�߶�
    private float height = 136f;
    private float upHeight = 0.1f;

    // ƴͼ��Ƭ
    GameObject[] dragObject;
    // ƴͼĿ��
    GameObject[] dropObject;

    private void Start()
    {
        dragObject = GameObject.FindGameObjectsWithTag("drag");
        dropObject = GameObject.FindGameObjectsWithTag("drop");
    }

    void Update()
    {
        // �������������
        if (Input.GetMouseButtonDown(0))
        {
            // ѡ�е�����Ϊ��
            if (selectedGameObject == null)
            {
                // �洢��������Ϣ
                RaycastHit hit = CastRay();
                // ��������������ײ��
                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))
                    {
                        // ����ǩ����dragֱ�ӷ���
                        return;
                    }
                    // ����ǩΪdrag����ΪselectedGameObject��ֵ
                    selectedGameObject = hit.collider.gameObject;
                    // ��������겻�ɼ�
                    Cursor.visible = false;
                }
            }
            // ѡ��������ٴΰ���������������������
            else
            {
                // ��¼������ĵ�
                Vector3 position = new Vector3(
               Input.mousePosition.x,
               Input.mousePosition.y,
               mainCamera.WorldToScreenPoint(selectedGameObject.transform.position).z);
                // תΪ��������
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(position);

                // ����
                //����ÿ�����õ㣬�ҵ������������ĵ�
                Vector3 tempdrop = Vector3.zero;
                float minDistance = 5.0f;
                foreach (var item in dropObject)
                {
                    if (Vector3.Distance(item.transform.position, worldPosition) <= minDistance)
                    {
                        Debug.Log("�������");
                        minDistance = Vector3.Distance(item.transform.position, worldPosition);
                        tempdrop = item.transform.position;
                    }
                }
                // �����С����С��---�޶�ֵ��˵����ƴͼλ���ϣ��͸�ֵ������������λ��
                if (minDistance < 0.5f)
                {
                    Debug.Log("����");
                    // ��ֵ
                    selectedGameObject.transform.position = tempdrop + new Vector3(0, upHeight, 0);
                    Debug.Log("Selected:" + selectedGameObject.transform.position);

                }
                else
                {
                    Debug.Log("û����");
                    selectedGameObject.transform.position = worldPosition;
                }


                selectedGameObject = null;
                // ��ʾ���
                Cursor.visible = true;

                Check();
            }
        }

        // �����ѡ�����岻Ϊ�գ������������λ���ƶ�
        if (selectedGameObject != null)
        {
            // �����Ļ����
            Vector3 position = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                mainCamera.WorldToScreenPoint(selectedGameObject.transform.position).z);
            // ���λ��ת��Ϊ��������
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(position);
            // Ϊѡ���������ֵ
            selectedGameObject.transform.position = new Vector3(worldPosition.x, height+upHeight, worldPosition.z);

            // ��������Ҽ�����ת
            if (Input.GetMouseButtonDown(1))
            {
                selectedGameObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedGameObject.transform.rotation.eulerAngles.x,
                    selectedGameObject.transform.rotation.eulerAngles.y + 90f,
                    selectedGameObject.transform.rotation.eulerAngles.z));
            }
        }
    }

    private RaycastHit CastRay()
    {
        // ������Զ��
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.farClipPlane);

        // ���������
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.nearClipPlane);

        Vector3 worldMousePosFar = mainCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = mainCamera.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    public void Check()
    {
        bool isDone = true;
        // ��ȡ�ж�����ƴͼ
        int childCount = dropObject.Length;
        // ����ÿ��ƴͼ��λ��
        // i��1��ʼ����Ϊ��0������Ϊ������
        for (int i = 1; i < childCount; i++)
        {
            //��ȡ����ƴͼ��λ��
            Vector3 dropPos = dropObject[i].transform.position;
            Vector3 dragPos = dragObject[i].transform.position;

            //ֻҪ��һ��ƴͼλ�ò��Ծ�û�����
            if (dragPos != dropPos+ new Vector3(0, upHeight, 0))
            {
                Debug.Log(dropObject[i].name+" "+dropPos + " " + dropObject[i].name +" "+ dragPos);
                isDone = false;
                break;
            }
        }

        if (isDone)
        {
            Debug.Log("���");
            var transition = this.GetComponent<Transition>();
            transition?.TransitionToScene(false); // puzzle Scene����������isMain=false��������������
        }
        else
        {
            Debug.Log("û�����");
        }
    }
}
