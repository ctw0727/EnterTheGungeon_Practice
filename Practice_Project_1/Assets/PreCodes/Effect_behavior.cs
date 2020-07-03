using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_behavior : MonoBehaviour
{
	
	private Transform SelfTransform;
	private Vector3 ScaleChange;
	
    // Start is called before the first frame update
    void Start()
    {
		ScaleChange = new Vector3(-0.01f,-0.01f,0);
        SelfTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
		SelfTransform.localScale += ScaleChange;
		
        if (SelfTransform.localScale.x <= 0f){
			Destroy(gameObject);
		}
    }
}
