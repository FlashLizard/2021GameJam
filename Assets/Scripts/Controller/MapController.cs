using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController current;
    [SerializeField]
    public List<GameObject> positions= new List<GameObject>();
    private void Awake()
    {
        current = this;
    }
}
