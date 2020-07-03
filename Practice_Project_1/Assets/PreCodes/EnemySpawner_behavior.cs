using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_behavior : MonoBehaviour
{
	public int EnemyCount = 0;
	private int MaxEnemy = 3;
	public int EnemySlayed = 0;
	private int MaxCount = 3;
	private float EnemyLevel = 0f;
	
	public GameObject Enemy1Prefab;
	
	private Camera MainCamera;
	private Transform PlayerTransform;
	private Transform CameraTransform;
	
    void Start(){
		MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
		CameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
	
	bool CheckMaxEnemy(){
		if (EnemyCount < MaxEnemy){
			return true;
		}
		else{
			return false;
		}
	}
	
	bool CheckSlayedEnemy(){
		if (EnemySlayed >= MaxCount){
			return true;
		}
		else{
			return false;
		}
	}
	
	Vector3 RocateEnemy(){
		
		Vector3 Rocate;
		Vector3 CameraPos = CameraTransform.position;
		float CamHeight = Camera.main.orthographicSize;
		float CamWidth = CamHeight*Camera.main.aspect;
		
		float XYSet = (Random.Range(-5.0f,5.1f) / 1f);
		
		float YMinCut = CameraPos.y - CamHeight;
		float YMaxCut = CameraPos.y + CamHeight;
		
		float XMinCut = CameraPos.x - CamWidth;
		float XMaxCut = CameraPos.x + CamWidth;
		
		float Y;
		
		for(Y = 0f;;Y += Random.Range(-2f,2f)){
			if ((Y-5 > YMaxCut)||(Y+5 < YMinCut)){
				if ((Y*XYSet-5 > XMaxCut)||(Y*XYSet+5 < XMinCut)){
					Rocate = new Vector3(Y*XYSet,Y,0);
					break;
				}
			}
		}
		
		return Rocate;
	}
	
	void SpawnEnemy(){
		
		Vector3 PlayerPos = PlayerTransform.position;
		
		GameObject Enemy1;

		Enemy1 = Instantiate(Enemy1Prefab);
		
		Transform EnemyT = Enemy1.GetComponent<Transform>();
		Enemy1_behavior EnemySet = Enemy1.GetComponent<Enemy1_behavior>();
		Damage_Script Damage = Enemy1.GetComponent<Damage_Script>();
		
		EnemyT.position = RocateEnemy();
		
		EnemySet.Level = EnemyLevel;
		Damage.HitDamage = 1;
	}

    void Update(){
        if (CheckMaxEnemy()){
			SpawnEnemy();
			EnemyCount++;
		}
		if (CheckSlayedEnemy()){
			MaxCount += 3;
			EnemyLevel += 1f;
		}
		MaxEnemy = 3 + (int)(EnemyLevel/2f);
	}
}