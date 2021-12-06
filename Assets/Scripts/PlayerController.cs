using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour //class means you're defining a new component
{
    public static PlayerController instance { get; private set; }

    public float speed = 3.0f;
    //Created public float variable speed that stores the result of 3.0 float
    public GameObject projectilePrefab;
    //created public Gameobject variable projectilePrefab 
    public ParticleSystem hurtEffect;

    public float timeInvincible = 2.0f;
    //created public float variable timeInvincible that stores the result 2.0 float

    bool isInvincible;
    //created boolean variable isInvincible
    //booleans are for true or false statements
    float invincibleTimer;
    //created float variable invincible timer
    //how much time is left to be invincible until vulnerable again

    Rigidbody2D rigidbody2d;
    //created variable rigidbody2d to store the Rigidbody2D component
    float horizontal;
    float vertical;
    //created two new float variables to store input data

    Animator animator;
    //Created component Animator and variable animator
    Vector2 lookDirection = new Vector2(1, 0);
    //created Vector2 function variable lookDirection that stores the new Vector2 parameters called 1 and 0

    AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip hitSound;

    //Then in your Awake function (remember this is called as soon as the object is created, which is our case is when the game starts), you store in the static instance this
    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            instance = this;
        }
        // which is a special C# keyword that means “the object that currently runs that function”.
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        //rigidbody2d stores the result of unity's GetComponent giving me the Rigidbody2D component that is on the gameobject this script is attached to
        animator = GetComponent<Animator>();
        //variable animator stores the result of unity's GetComponent calling on the animator component on the game object this script is attached to

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //variable horizontal/vertical stores the result that the input of the axis Horizontal/Vertical provides
        //GetAxis is a function, like Start or Update. when written like this, you 'call' it.
        //the words between parenthesis are parameters. this is information you give to the function to specify what it should do
        //you need to place the name between quote marks so the compiler knows its a word and not a special keyword like a variable or a type
        // dot operators work on functions. here input contains getaxis. functions that work on the same feature under the same name are grouped to access them with the dop operator (google into this more)

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        //if variable isInvincible is true
        {
            invincibleTimer -= Time.deltaTime;
            //variable invincibleTimer stores decreaseing result of function Time containing variable deltaTime

            if (invincibleTimer < 0)
            //if invincibleTimer is inferior to or less than 0
            {
                isInvincible = false;
                //variable isInvincible stores the boolean result of false
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.Interact();
                }
            }
        }
    }

    // FixedUpdate is called for Physics
    void FixedUpdate()
    {
        Vector2 position = transform.position;
        //declare variable position and store ruby's current position in it. 
        //Vector2 variable is a data type that stores two numbers. 
        //"." can be seen to mean contains. transform is a unity variable
        //the 2 numbers the variable position gets from the variable vector 2 stores the result that the variable transform containing position provides
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        //variable position containing x/y stores the result position of x/y + float variable speed * float variable horizontal/vertical(which is linked to the input containing getaxis of Horizontal/Vertical)
        //so no input is x/y + speed * 0 which = 0
        //variable Time contains deltaTime. deltaTime is a variable that unity fills with the time it takes for a frame to be rendered
        //using Time.deltaTime makes your character run at the same speed regardless of the number of frames rendered. they are now 'frame independent'
        rigidbody2d.MovePosition(position);
        //rigidbody2d containing the MovePosition function that calls on the parameter of the position variable
        //PlaySound(walking);
    }

    //public ChangeHealth function created calling on the parameter of interger and amount variable
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        //if variable amount is inferior (<) to 0, health being removed
        {
            animator.SetTrigger("Hit");
            // variable animator contains function set trigger calling the "Hit" parameter for the animation

            if (isInvincible)
            //if variable isInvincible is already active
            {
                return;
                //go back
            }
            isInvincible = true;
            //variable isInvincible stores the result of true

            Instantiate(hurtEffect, transform.position, Quaternion.identity);
            //create an instance of the hurt effect on ruby's position with no rotation

            invincibleTimer = timeInvincible;
            //variable invincibleTime stores the result of variable timeInvincible

            PlaySound(hitSound);
        }

        //Singleton
        GameModel model = GameModel.instance;
        model.ChangeLife(amount);
    }

    public void ChangeAmmo(int amount)
    {
        //Singleton
        GameModel model = GameModel.instance;
        model.ChangeAmmo(amount);
    }

    void Launch()
    {
        //Singleton Reference
        GameModel model = GameModel.instance;
        //Check if Player has Ammo.
        if (model.HasAmmo())
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");

            PlaySound(throwSound);
            ChangeAmmo(-1);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    //Ruby's movement 
    public void SetSimulated(bool active)
    {
        rigidbody2d.simulated = active;
    }

}


//notes:
//in ____.____ the "." can be seen as ______"contains/containing"_____
//every line ends with ; which is used by the compiler (translate english code to computer code)
//Debug.log(); line anywhere in your script to output values (like horizontal) to check why something is not working
//in a function, void means nothing. the functions with void thus dont hold value
//when using two {{}} "blocks" you can use the get keyword in the first block to get whatever kind of function is in the second.
//the second block can act like a normal function with variables, computations, or other calling functions

//QualitySettings.vSyncCount and Application.targetFrameRate both go under void Start()
//QualitySettings.vSyncCount = 0;
//QualitySettings contains vSyncCount, stores the result of 0
//Application.targetFrameRate = 10;
//Application contains targetFrameRate, stores the result of 10
//this makes unity render at 10 fps