using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTool : MonoBehaviour
{
    public Transform reference;
    public Transform target;

    [ContextMenu("CopyTfmInfo")]
    public void CopyTfmInfo()
    {
        var list_Ref = new List<Transform>();
        var list_target = new List<Transform>();

        _AddChildToList(reference, list_Ref);
        _AddChildToList(target, list_target);

        for (int i = 0; i < list_Ref.Count; i++)
        {
            list_target[i].localPosition = list_Ref[i].localPosition;
            list_target[i].localRotation = list_Ref[i].localRotation;
        }
    }

    private void _AddChildToList(Transform tfm, List<Transform> list)
    { 
        for (int i = 0; i < tfm.childCount; i++)
        {
            var child = tfm.GetChild(i);
            if (child != null)
            {
                list.Add(child);
                _AddChildToList(child, list);
            }
        }
    }
         






    


}


