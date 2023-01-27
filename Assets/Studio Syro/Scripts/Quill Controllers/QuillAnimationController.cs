using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[System.Serializable]
public class AnimationHolder
{
    [HideInInspector]
    public string name;
    public bool loop;
    [HideInInspector]
    public Transform myTransform;
    public Transform[] frames = new Transform[0];
}

[RequireComponent(typeof(QuillControllerHelper))]
public class QuillAnimationController : MonoBehaviour
{
    QuillControllerHelper quillHelper;
    public Transform animationParent;
    public Transform animationRoot;
    public int animationIndex;
    public int frameIndex;
    public int animationLength;

    public bool initializeOnStart;
    [Space]
    public bool pause;
    public List<AnimationHolder> animationHolders;

    private void Awake()
    {
        if (initializeOnStart)
        {
            GetAnimations();
        }
    }

    [Button]
    public void GetAnimations()
    {
        quillHelper = GetComponent<QuillControllerHelper>();

        if (!animationRoot)
            animationRoot = animationParent.GetChild(0);

        animationHolders.Clear();

        for (int i = 0; i < GetFirstChildren(animationRoot.GetChild(0)).Length; i++)
        {
            AnimationHolder animHolder = new AnimationHolder();
            animHolder.myTransform = GetFirstChildren(animationRoot.GetChild(0))[i];
            animHolder.name = animHolder.myTransform.name;
            animationHolders.Add(animHolder);
        }

        for (int i = 0; i < animationHolders.Count; i++)
        {
            animationHolders[i].frames = GetFirstChildren(animationHolders[i].myTransform.GetChild(0));

            for (int j = 0; j < animationHolders[i].frames.Length; j++)
            {
                if (animationHolders[i].frames[j].GetComponent<MeshRenderer>())
                {
                    animationHolders[i].frames[j].GetComponent<MeshRenderer>().enabled = true;
                }
                animationHolders[i].frames[j].gameObject.SetActive(false);
            }
            animationHolders[i].frames[0].gameObject.SetActive(true);
        }
        if (animationHolders.Count > 1)
            animationHolders[1].frames[0].gameObject.SetActive(true);
        quillHelper.FillEmptys();
    }

    void Update()
    {
        if (animationHolders.Count != 0 && animationRoot)
        {
            for (int i = 0; i < animationHolders.Count; i++)
            {
                if (i != animationIndex)
                {
                    animationHolders[i].myTransform.gameObject.SetActive(false);
                }

                if (i == animationIndex)
                {
                    animationHolders[i].myTransform.gameObject.SetActive(true);
                }
            }
            animationLength = animationHolders[animationIndex].frames.Length - 1;
        }
    }

    void FixedUpdate()
    {
        if (animationHolders.Count != 0 && animationRoot)
        {
            if (!pause)
            {
                frameIndex++;

                if (frameIndex >= animationHolders[animationIndex].frames.Length)
                {
                    frameIndex = 0;
                }
            }

            for (int j = 0; j < animationHolders[animationIndex].frames.Length; j++)
            {
                if (j != frameIndex)
                {
                    animationHolders[animationIndex].frames[j].gameObject.SetActive(false);
                }
                if (j == frameIndex)
                {
                    animationHolders[animationIndex].frames[j].gameObject.SetActive(true);
                }
            }
        }
    }

    private Transform[] GetFirstChildren(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Transform[] firstChildren = new Transform[parent.childCount];
        int index = 0;
        foreach (Transform child in children)
        {
            if (child.parent == parent)
            {
                firstChildren[index] = child;
                index++;
            }
        }
        return firstChildren;
    }
}

    

