using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject robotPrefab;
    public CircleCollider2D detectionRadius;
    public float spawn_Time = 2f;
    public int robots = 10;
    float spawn_timer;

    AudioSource audioSource;
    public AudioClip spawnBot;


    // Start is called before the first frame update
    void Start()
    {
        spawn_timer = -1;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn_timer >= 0 && robots > 0)
        {
            spawn_timer -= Time.deltaTime;
            if (spawn_timer < 0)
            {
                audioSource.PlayOneShot(spawnBot);

                GameObject newrobot = Instantiate(robotPrefab);
                
                Vector3 newposition = transform.position;
                //Fix Z axis to be the same as player.
                newposition.z = PlayerController.instance.transform.position.z;

                newrobot.transform.position = newposition;

                EnemyController robotcontroller = newrobot.GetComponent<EnemyController>();
                robotcontroller.chaser = true;

                if (robots == 1)
                {
                    robotcontroller.boss = true;
                }

                robots--;//take away one robot
                
                if (robots > 0)
                {
                    spawn_timer = spawn_Time + spawn_timer;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Singleton Reference
        PlayerController controller = PlayerController.instance;

        if (other.gameObject == controller.gameObject)
        {
            detectionRadius.enabled = false;
            spawn_timer = 0;
        }
    }
}