using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_behavior : MonoBehaviour
{
	public Vector3 Velocity;
	public Rigidbody2D SelfRigid2D;
	int slow = 0;
	
    // Start is called before the first frame update
	void getSlowed(){
		slow = 1;
	}
	
    void Start()
    {
		SelfRigid2D = GetComponent<Rigidbody2D>();
		Invoke("getSlowed",1f);
    }

    // Update is called once per frame
    void Update()
    {
		SelfRigid2D.velocity = Velocity;
		
		if (slow == 1){
			Velocity = Velocity / (1.01f);
			if ((Velocity.x + Velocity.y >= -2.0f)&&(Velocity.x + Velocity.y <= 2.0f)){
				Destroy(gameObject,0.5f);
			}
		}
    }
}
