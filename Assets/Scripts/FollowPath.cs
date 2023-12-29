using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FollowPath : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed;
    float distanceTravelled;
    public Coroutine follow;

    // Start is called before the first frame update
    void Start()
    {
        //follow = StartCoroutine(Follow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Follow()
    {
        while (distanceTravelled < pathCreator.path.length)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

            yield return null;
        }

        StopCoroutine(follow);
        Destroy(gameObject);

        GameObject.Find("Game Manager").GetComponent<GameManager>().OnQuestObjectiveCompletion("Unlock one of the cell doors");
    }
}
