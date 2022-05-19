using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    // 记录拼图是否被选中
    public Camera mainCamera;
    private GameObject selectedGameObject;
    // 拼图碎片高度
    private float height = 136f;
    private float upHeight = 0.1f;

    // 拼图碎片
    GameObject[] dragObject;
    // 拼图目标
    GameObject[] dropObject;

    private void Start()
    {
        dragObject = GameObject.FindGameObjectsWithTag("drag");
        dropObject = GameObject.FindGameObjectsWithTag("drop");
    }

    void Update()
    {
        // 如果按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            // 选中的物体为空
            if (selectedGameObject == null)
            {
                // 存储的射线信息
                RaycastHit hit = CastRay();
                // 碰到的物体有碰撞器
                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))
                    {
                        // 若标签不是drag直接返回
                        return;
                    }
                    // 若标签为drag，则为selectedGameObject赋值
                    selectedGameObject = hit.collider.gameObject;
                    // 设置鼠标光标不可见
                    Cursor.visible = false;
                }
            }
            // 选中物体后，再次按下主表左键，则放下物体
            else
            {
                // 记录鼠标点击的点
                Vector3 position = new Vector3(
               Input.mousePosition.x,
               Input.mousePosition.y,
               mainCamera.WorldToScreenPoint(selectedGameObject.transform.position).z);
                // 转为世界坐标
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(position);

                // 吸附
                //遍历每个放置点，找到和鼠标点击最近的点
                Vector3 tempdrop = Vector3.zero;
                float minDistance = 5.0f;
                foreach (var item in dropObject)
                {
                    if (Vector3.Distance(item.transform.position, worldPosition) <= minDistance)
                    {
                        Debug.Log("距离近不");
                        minDistance = Vector3.Distance(item.transform.position, worldPosition);
                        tempdrop = item.transform.position;
                    }
                }
                // 如果最小距离小于---限定值，说明在拼图位置上，就赋值，否则就是鼠标位置
                if (minDistance < 0.5f)
                {
                    Debug.Log("吸附");
                    // 赋值
                    selectedGameObject.transform.position = tempdrop + new Vector3(0, upHeight, 0);
                    Debug.Log("Selected:" + selectedGameObject.transform.position);

                }
                else
                {
                    Debug.Log("没吸附");
                    selectedGameObject.transform.position = worldPosition;
                }


                selectedGameObject = null;
                // 显示光标
                Cursor.visible = true;

                Check();
            }
        }

        // 如果被选择物体不为空，则物体随鼠标位置移动
        if (selectedGameObject != null)
        {
            // 鼠标屏幕坐标
            Vector3 position = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                mainCamera.WorldToScreenPoint(selectedGameObject.transform.position).z);
            // 鼠标位置转换为世界坐标
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(position);
            // 为选中物体最表赋值
            selectedGameObject.transform.position = new Vector3(worldPosition.x, height+upHeight, worldPosition.z);

            // 按下鼠标右键，旋转
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
        // 射线最远点
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.farClipPlane);

        // 射线最近点
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
        // 获取有多少张拼图
        int childCount = dropObject.Length;
        // 遍历每张拼图的位置
        // i从1开始，因为第0个物体为父物体
        for (int i = 1; i < childCount; i++)
        {
            //获取两张拼图的位置
            Vector3 dropPos = dropObject[i].transform.position;
            Vector3 dragPos = dragObject[i].transform.position;

            //只要有一张拼图位置不对就没有完成
            if (dragPos != dropPos+ new Vector3(0, upHeight, 0))
            {
                Debug.Log(dropObject[i].name+" "+dropPos + " " + dropObject[i].name +" "+ dragPos);
                isDone = false;
                break;
            }
        }

        if (isDone)
        {
            Debug.Log("完成");
            var transition = this.GetComponent<Transition>();
            transition?.TransitionToScene(false); // puzzle Scene非主场景，isMain=false，所以销毁自身
        }
        else
        {
            Debug.Log("没有完成");
        }
    }
}
