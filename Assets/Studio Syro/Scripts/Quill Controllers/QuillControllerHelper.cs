using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class QuillControllerHelper : MonoBehaviour
{
    QuillAnimationController quillAnimation;

    public void FillEmptys()
    {
        quillAnimation = GetComponent<QuillAnimationController>();

        for (int i = 0; i < quillAnimation.animationHolders.Count; i++)
        {
            for (int j = 0; j < quillAnimation.animationHolders[i].frames.Length; j++)
            {

                if (j > 0)
                {
                    if (!quillAnimation.animationHolders[i].frames[j].GetComponent<MeshRenderer>())
                    {
                        MeshFilter addedFilter = quillAnimation.animationHolders[i].frames[j].gameObject.AddComponent<MeshFilter>();
                        addedFilter.mesh = quillAnimation.animationHolders[i].frames[j - 1].GetComponent<MeshFilter>().mesh;
                        MeshRenderer addedRenderer = quillAnimation.animationHolders[i].frames[j].gameObject.AddComponent<MeshRenderer>();
                        addedRenderer.material = quillAnimation.animationHolders[i].frames[j - 1].GetComponent<MeshRenderer>().material;                        
                    }
                }
            }
        }
    }
}
