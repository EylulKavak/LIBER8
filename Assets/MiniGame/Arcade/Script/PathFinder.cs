using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    EnemySpawn enemySpawn;
    WaveConfig waveConfig;
    List<Transform> wayPoints;
    int wayPointIndex = 0;

    private void Awake()
    {
        enemySpawn =FindObjectOfType<EnemySpawn>();
    }
    void Start()
    {
        waveConfig = enemySpawn.GetCurrentWave();
        wayPoints = waveConfig.GetWayPoint();
        transform.position = wayPoints[wayPointIndex].position; 
    }

    // Update is called once per frame
    void Update()
    {
        FollowPath();
    }
    void FollowPath()
    {
        if(wayPointIndex< wayPoints.Count)
        {
            Vector3 TargetPosition = wayPoints[wayPointIndex].position;
            float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position= Vector2.MoveTowards(transform.position, TargetPosition, delta);
            if(transform.position==TargetPosition)
            {
                wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
