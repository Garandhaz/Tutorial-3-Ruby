using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
        //created public float variable speed
    public bool vertical;
        //created public bool variable vertical
    public float changeTime = 3.0f;
        //created public float changeTime variable storing the result of 3.0 float
    bool broken = true;

    public ParticleSystem smokeEffect;
    
    Rigidbody2D rigidbody2d;
        //created variable rigidbody2d to store the Rigidbody2D component
    float timer;
        //created float variable timer
    int direction = 1;
        //created interger variable direction that stores the result of 1;    

    Animator animator;
        //Created component Animator and variable animator
    AudioSource audioSource;

    public AudioClip hitPlayer;
    
    public AudioClip cogFix;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
            //rigidbody2d stores the result of unity giving me the Rigidbody2D component that is on the gameobject this script is attached to\
        timer = changeTime;
            //variable timer stores the result of variable changeTime
        animator = GetComponent<Animator>();
            //variable animator stores result of function GetComponent fetching unity Animator component attached to current gameobject
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!broken)
            //if parameters of not and the bariable broken are true
        {
            return;
                //stop here
        }

        timer -= Time.deltaTime;
            //variable timer stores the subtractive result of variable Time containing variable deltaTime

        if(timer < 0)
            //if variable timer is inferior (less than) 0
        {
            direction = -direction;
                //variable direction stores the restult of negative variable direction
            timer = changeTime;
                //timer stores the restult of variable changeTime
        }
    }
    
    // FixedUpdate is called for Physics
    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;
            //declare variable position and store ruby's current position in it. 
            //vector2 variable is a data type that stores two numbers. 
            //"." can be seen to mean contains. transform is a unity variable
            //the 2 numbers the variable position gets from the variable vector 2 stores the result that the variable transform containing position provides
        
        if(vertical)
            //if the bool variable vertical is true
        {
            position.y = position.y + Time.deltaTime * speed * direction;;
                //variable position containing y stores the result position of y + variable Time containing deltaTime * variable speed variable direction
                //variable Time contains deltaTime. deltaTime is a variable that unity fills with the time it takes for a frame to be rendered
                //using Time.deltaTime makes your character run at the same speed regardless of the number of frames rendered. they are now 'frame independent'
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
                //variable animator contains function setfloat calling parameter Move X to 0 and Move Y to variable direction
        }
        else
            //if it is not true
        {
            position.x = position.x + Time.deltaTime * speed * direction;;
                //variable position containing x stores the result position of x + variable Time containing deltaTime * variable speed * variable direction
                //variable Time contains deltaTime. deltaTime is a variable that unity fills with the time it takes for a frame to be rendered
                //using Time.deltaTime makes your character run at the same speed regardless of the number of frames rendered. they are now 'frame independent'            
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
                //variable animator contains function setfloat calling parameter Move Y to 0 and Move X to variable direction
        }

        rigidbody2d.MovePosition(position);
            //rigidbody2d containing the MovePosition function that calls on the parameter of the position variable
    }

    //created OnCollisionEnter2D function calling on the parameter of function Collision2D and variable other when player collider enters the robot's collission
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();
            //Access the RubyController component player variable on the game object of the collider that enters the trigger
            //RubyController component and variable player store the result of variable other containing the function gameObject containing GetComponent fetching RubyController from triggering object

        if(player != null)
            //if player is not null
        {
            player.ChangeHealth(-1);
                //variable player contains function ChangeHealth calling parameter -1
            PlaySound(hitPlayer);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        smokeEffect.Stop();
        
        PlaySound(cogFix);
        
        animator.SetTrigger("Fixed");
    }

        public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
