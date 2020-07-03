using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector_Script : MonoBehaviour
{
	
	public GameObject EffectPrefab;
	private Transform SelfTransform;
	
    // Start is called before the first frame update
    void Start()
    {
        SelfTransform = GetComponent<Transform>();
    }
	
	void GetHitted(Collider2D other){
		
		Vector3 SelfPos = SelfTransform.position;
		
		Rigidbody2D OtherRigid = other.gameObject.GetComponent<Rigidbody2D>();
		Vector2 OtherVel = OtherRigid.velocity;
		
		for(int i=0;i<=5;i++){
			GameObject Effect = Instantiate(EffectPrefab,SelfTransform);
			Rigidbody2D EffectVel = Effect.GetComponent<Rigidbody2D>();
			Vector2 Vel = new Vector2(OtherVel.x+Random.Range(-5.0f,5.1f), OtherVel.y+Random.Range(-5.0f,5.1f));
			EffectVel.velocity = Vel;
		}
	}
	
	void OnTriggerStay2D(Collider2D other){ // Catch Collider on Object!
		if (((gameObject.tag == "Enemy")&&(other.tag == "Bullet")) || ((gameObject.tag == "Player")&&(other.tag == "Enemy_Bullet"))){
			GetHitted(other);
		}
	}
}
