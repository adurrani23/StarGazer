using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 15.0f;
	public float padding = 1.0f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 250f;
	
	public AudioClip fireSound;
	
	float xMin;
	float xMax;
	
	void Start(){
		//Distance between camera and player object(ship)
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		xMin = leftmost.x + padding;
		xMax = rightmost.x - padding;
	}
	
	void Fire(){
		Vector3 offset = new Vector3(0,1,0);
		GameObject beam = Instantiate(projectile, transform.position+offset, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector3(0,projectileSpeed,0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	
	// Update is called once per frame
	void Update(){
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("Fire");
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {	
			transform.position += Vector3.left*speed*Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.RightArrow)) { 
			transform.position += Vector3.right*speed*Time.deltaTime;
		}
		//Restrict player to the game space
		float newX =  Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
	//Projectile/Enemy ship collision 
	void OnTriggerEnter2D(Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			health -= missile.GetDamage();
			missile.Hit();	//Destroys projectile when it hits something
			if(health <= 0) {
				Die();
			}
		}
	}
	
	void Die () {
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);	// Destroys player ship when health is depleted
	}
}
