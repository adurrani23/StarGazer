using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public float health = 150f;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float ShotsPerSecond = 0.5f;
	public int scoreValue = 150;
	
	private ScoreKeeper scoreKeeper;
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	void Start() {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void Update() {
		float probability = Time.deltaTime*ShotsPerSecond;
		if(Random.value < probability) {
			Fire();
		}
	}

	void Fire() {
		GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		missile.rigidbody2D.velocity = new Vector2(0,-projectileSpeed);
		AudioSource.PlayClipAtPoint(fireSound,transform.position);
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
	
	void Die() {
		AudioSource.PlayClipAtPoint(deathSound,transform.position);
		scoreKeeper.Score(scoreValue);
		Destroy(gameObject);	// Destroys enemy ship when health is depleted
	}
}

