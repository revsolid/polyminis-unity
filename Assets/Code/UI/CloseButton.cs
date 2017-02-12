using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public GameObject ToDestroy;
    public void Close()
    {
        Destroy(ToDestroy);
    }

}
