using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    QuillAnimationController quillAnim;

    void Start()
    {
        quillAnim = GetComponent<QuillAnimationController>();
    }


    void FixedUpdate()
    {

        if(quillAnim.frameIndex >= quillAnim.animationHolders[quillAnim.animationIndex].frames.Length - 1)
        {
            Destroy(gameObject);
        }
    }
}
