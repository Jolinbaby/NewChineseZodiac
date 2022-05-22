
// 采用新的Input System之后
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateModel : MonoBehaviour
{
    public Transform modelTransform;
    private bool isRotate;//能否发生旋转

    private Vector3 startPoint;
    private Vector3 startAngle;


    [Range(0.1f, 50f)] //小技巧,这样调整范围的时候会比较方便
    public float rotateScale = 0.5f; //类似于旋转时候的鼠标灵敏度

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isRotate)
        {
            isRotate = true;
            startPoint = Mouse.current.position.ReadValue();
            startAngle = modelTransform.eulerAngles;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isRotate = false;
        }
        if (isRotate)
        {
            var currentPoint = Mouse.current.position.ReadValue();
            var x = startPoint.x - currentPoint.x;

            modelTransform.eulerAngles = startAngle + new Vector3(0, x * rotateScale, 0);
        }
    }
}