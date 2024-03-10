using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaypointController : MonoBehaviour
{
    public Image waypointImage;
    [HideInInspector] public Transform target;

    private GameManager gameManager;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private Vector2 pos;
    private TextMeshProUGUI distanceText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        target = GameObject.Find("Ravi").transform;

        distanceText = waypointImage.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        minX = waypointImage.GetPixelAdjustedRect().width / 2;
        maxX = Screen.width - minX;

        minY = waypointImage.GetPixelAdjustedRect().height / 2;
        maxY = Screen.width - minY;

        pos = Camera.main.WorldToScreenPoint(target.position);

        if (Vector3.Dot(target.position - transform.position, transform.forward) < 0) {
            // Target is behind player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        waypointImage.transform.position = pos;

        distanceText.text = Vector3.Distance(transform.position, target.transform.position).ToString("0m");
    }
}
