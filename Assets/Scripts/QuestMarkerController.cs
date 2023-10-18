using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, Vector3.zero);
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
}
