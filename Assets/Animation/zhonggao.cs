using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zhonggao : MonoBehaviour
{
    private AnimatorStateInfo animatorInfo;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _animator);
    }

    // Update is called once per frame
    void Update()
    {
        animatorInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("NullClip"))
        {
            Destroy(this.gameObject);
        }
    }
}
