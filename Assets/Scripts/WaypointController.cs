using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaypointController : MonoBehaviour
{
    public Image waypointImage;
    public GameObject[] targets;
    public int targetIndex;

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

        distanceText = waypointImage.GetComponentInChildren<TextMeshProUGUI>();

        targetIndex = gameManager.totalQuestObjectiveCount;
    }

    // Update is called once per frame
    void Update()
    {
        minX = waypointImage.GetPixelAdjustedRect().width / 2;
        maxX = Screen.width - minX;

        minY = waypointImage.GetPixelAdjustedRect().height / 2;
        maxY = Screen.width - minY;

        pos = Camera.main.WorldToScreenPoint(new Vector3(targets[targetIndex].transform.position.x + targets[targetIndex].GetComponent<Target>().xOffset,
            targets[targetIndex].transform.position.y + targets[targetIndex].GetComponent<Target>().yOffset + 2.5f, targets[targetIndex].transform.position.z));

        if (Vector3.Dot(targets[targetIndex].transform.position - transform.position, transform.forward) < 0) {
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

        distanceText.text = Vector3.Distance(transform.position, targets[targetIndex].transform.position).ToString("0m");
    }
}
