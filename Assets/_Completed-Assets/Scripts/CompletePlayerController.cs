using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

public class CompletePlayerController : MonoBehaviour {

	public float speed;				//Floating point variable to store the player's movement speed.
	//public Text countText;			//Store a reference to the UI Text component which will display the number of pickups collected.
	//public Text winText;			//Store a reference to the UI Text component which will display the 'You win' message.

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.
	private int count;				//Integer to store the number of pickups collected so far.
    public GameObject prefab;
    public int xmin = -9, xmax = 9;
    public int ymin = -8, ymax = 8;
    public float timeLeft;
    public float timer;
    public GameObject gameManagerGO;
    public int totalNumberOfFood;
    public int totalNumberOfEnemy;
    public GameObject enemy;
    public int health = 5;
    public Image circle;
    public bl_Joystick Joystick;
    public Slider HealthSlider;
    public SpriteRenderer icon;
    public Sprite sp;
    // Use this for initialization
    void Start()
	{
        HealthSlider.maxValue = health;
        if (GameManager.instance == null)
        {
            Instantiate(gameManagerGO);
        }
        GameManager gm = GameManager.instance;
        //gm.SetLevel(1);
        timeLeft = gm.GetTime();
        //StartCoroutine("LoseTime");
        //Time.timeScale = 1;
        totalNumberOfFood = gm.GetFoodCount();
        for (int i = 0; i < totalNumberOfFood;  i++)
        {
            Instantiate(prefab, new Vector2(Random.Range(xmin, xmax), Random.Range(ymin, ymax)),Quaternion.identity);
        }
        totalNumberOfEnemy = gm.GetLevel();
        for (int j = 0; j < totalNumberOfEnemy; j++)
        {
            Instantiate(enemy, new Vector2(Random.Range(xmin, xmax), Random.Range(ymin, ymax)), Quaternion.identity);
        }
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D> ();
        icon = GetComponent<SpriteRenderer>();
        if (totalNumberOfEnemy ==2) {
            icon.sprite = sp;
        }
		//Initialize count to zero.
		count = 0;
      //  circle.fillAmount = 0;
        //Initialze winText to a blank string since we haven't won yet at beginning.
        //winText.text = "";

        //Call our SetCountText function which will update the text with the current value for count.
        //SetCountText ();
    }

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
        HealthSlider.value = health;
        timer += Time.deltaTime;

        circle.fillAmount = (1 / timeLeft)*timer; 
        if (timer > timeLeft)
        {
            GameManager gm = GameManager.instance;
            gm.SetLevel(1);
            SceneManager.LoadScene(0, LoadSceneMode.Single);

           
        }
        
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Joystick.Horizontal;

		//Store the current vertical input in the float moveVertical.
		float moveVertical = Joystick.Vertical;

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
		rb2d.AddForce (movement * speed);
	}

	//OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
	void OnTriggerEnter2D(Collider2D other) 
	{
		//Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
		if (other.gameObject.CompareTag ("PickUp")) 
		{
			//... then set the other object we just collided with to inactive.
			Destroy(other.gameObject);
			
			//Add one to the current value of our count variable.
			count = count + 1;

            //Update the currently displayed count by calling the SetCountText function.
            //SetCountText ();
            if (count == totalNumberOfFood && timer < timeLeft)
            {
                GoNextlevel();
            }
		}

	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            if (health <= 0)
            {
                health = 5;
                GameManager gm = GameManager.instance;
                int level = gm.GetLevel();
                gm.SetLevel(level);
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            health--;

        }
    }



    /*IEnumerator LoseTime()
    {
        while (true)
        {
            Debug.Log(timeLeft);

            yield return new WaitForSeconds(1);
            timeLeft--;
            if (timeLeft == 0 && count<5)
            {
                Destroy(gameObject);
            }
            if (timeLeft>0 && count==5)
            {
                GoNextlevel();
               
            }
        }
    }*/

    void GoNextlevel()
    {
        
            GameManager gm = GameManager.instance;
            int level = gm.GetLevel();
            level++;
            gm.SetLevel(level);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        
    }

    //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
    /*void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Count: " + count.ToString ();

        //Check if we've collected all 12 pickups. If we have...
        if (count >= 12)
            //... then set the text property of our winText object to "You win!"
            winText.text = "You win!";
    }*/
}
