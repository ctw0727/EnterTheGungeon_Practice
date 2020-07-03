using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_behavior : MonoBehaviour
{
	private int HP = 6;
	private int DEAD = 0;
	private int timer = 0;
	private int Rest = 0;
	private int Stun = 0;
	private int Shooting = 0;
	private int Shooted = 0;
	private float Speed = 4f;
	public float Level = 0f;
	
	private GameObject Player;
	
	private Vector3 PlayerPos;
	private Player_behavior PlayerSCR;
	private Vector3 SelfPos;
	private Rigidbody2D SelfRigid2D;
	
	private Transform CameraTransform;
	
	private EnemySpawner_behavior SpawnerInfo;
	
	public GameObject ShotPrefab;
	
	void Timing(){
		timer++;
		Invoke("Timing",1);
	}
	
    void Start(){
		
		HP += (int)(Level/2f);
		Speed += (Level/5f);
		
		SelfPos = GetComponent<Transform>().position;
		SelfRigid2D = GetComponent<Rigidbody2D>();
		
		Player = GameObject.Find("Player");
		
		PlayerPos = Player.GetComponent<Transform>().position;
		PlayerSCR = Player.GetComponent<Player_behavior>();
		
		CameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
		
		SpawnerInfo = GameObject.Find("EnemySpawn").GetComponent<EnemySpawner_behavior>();
		Invoke("Timing",1);
    }
	
	float PlayerDegree(){
		PlayerPos = Player.GetComponent<Transform>().position;
		SelfPos = GetComponent<Transform>().position;
		return (Mathf.Atan2((PlayerPos.y - SelfPos.y), (PlayerPos.x - SelfPos.x)) * Mathf.Rad2Deg);
	}
	
	float PlayerDistance(){
		PlayerPos = Player.GetComponent<Transform>().position;
		SelfPos = GetComponent<Transform>().position;
		return (Mathf.Sqrt(Mathf.Pow((PlayerPos.x - SelfPos.x),2) + Mathf.Pow((PlayerPos.y - SelfPos.y),2)));
	}
	
	void Move(float type){
		if (Rest == 0){
			if (((PlayerDistance()>= 10f)&&(type == 0f))||(PlayerDistance() >= 25f)){
				if ((PlayerDegree() >= -22.5f)&&(PlayerDegree() <= 22.5f)) // east
					SelfRigid2D.velocity = new Vector2(Speed*1,0);
			
				if ((PlayerDegree() > 22.5f)&&(PlayerDegree() < 67.5f)) // north-east
					SelfRigid2D.velocity = new Vector2(Speed*1,Speed*1);
			
				if ((PlayerDegree() >= 67.5f)&&(PlayerDegree() <= 112.5f)) // north
					SelfRigid2D.velocity = new Vector2(0,Speed*1);
			
				if ((PlayerDegree() > 112.5f)&&(PlayerDegree() < 157.5f)) // north-west
					SelfRigid2D.velocity = new Vector2(Speed*-1,Speed*1);
			
				if ((PlayerDegree() >= 157.5f)||(PlayerDegree() <= -157.5f)) // west
					SelfRigid2D.velocity = new Vector2(Speed*-1,0);
		
				if ((PlayerDegree() > -157.5f)&&(PlayerDegree() < -112.5f)) // south-west
					SelfRigid2D.velocity = new Vector2(Speed*-1,Speed*-1);
			
				if ((PlayerDegree() >= -112.5f)&&(PlayerDegree() <= -67.5f)) // south
					SelfRigid2D.velocity = new Vector2(0,Speed*-1);
		
				if ((PlayerDegree() > -67.5f)&&(PlayerDegree() < -22.5f)) //south-east
					SelfRigid2D.velocity = new Vector2(Speed*1,Speed*-1);
		}
			else if (type != 0f){
				float Spd = Speed / 1.5f;
			
				if ((PlayerDegree() >= -22.5f)&&(PlayerDegree() <= 22.5f)) // east
					SelfRigid2D.velocity = new Vector2(Spd*-1,type*Spd);
				
				if ((PlayerDegree() > 22.5f)&&(PlayerDegree() < 67.5f)) // north-east
					SelfRigid2D.velocity = new Vector2(Spd*(type-1)/2f,-Spd*(type+1)/2f);
			
				if ((PlayerDegree() >= 67.5f)&&(PlayerDegree() <= 112.5f)) // north
					SelfRigid2D.velocity = new Vector2(type*Spd,Spd*-1);
			
				if ((PlayerDegree() > 112.5f)&&(PlayerDegree() < 157.5f)) // north-west
					SelfRigid2D.velocity = new Vector2(Spd*(type+1)/2f,Spd*(type-1)/2f);
			
				if ((PlayerDegree() >= 157.5f)||(PlayerDegree() <= -157.5f)) // west
					SelfRigid2D.velocity = new Vector2(Spd*1,type*Spd);
		
				if ((PlayerDegree() > -157.5f)&&(PlayerDegree() < -112.5f)) // south-west
					SelfRigid2D.velocity = new Vector2(-Spd*(type-1)/2f,Spd*(type+1)/2f);
			
				if ((PlayerDegree() >= -112.5f)&&(PlayerDegree() <= -67.5f)) // south
					SelfRigid2D.velocity = new Vector2(type*Spd,Spd*1);
		
				if ((PlayerDegree() > -67.5f)&&(PlayerDegree() < -22.5f)) //south-east
					SelfRigid2D.velocity = new Vector2(Spd*(type-1)/2f,Spd*(type+1)/2f);
		}
		}
	}
	
	void ShootCool(){
		Shooting = 0;
	}
	
	void Reload(){
		Shooted = 0;
	}
	
	void ShotPlayer(){
		if ((Shooting == 0)&&(Shooted < 3)){
			PlayerPos = Player.GetComponent<Transform>().position;
			SelfPos = GetComponent<Transform>().position;
			
			Quaternion rotation = Quaternion.AngleAxis(PlayerDegree(), Vector3.forward);
			
			GameObject Bullet;
			
			Bullet = Instantiate(ShotPrefab,SelfPos,rotation);
			
			Bullet_behavior ShotVelocity = Bullet.GetComponent<Bullet_behavior>();
			Damage_Script ShotDamage = Bullet.GetComponent<Damage_Script>();
			
			Vector3 BulletVelocity = (PlayerPos - SelfPos);
			float RangeKey = Mathf.Sqrt(Mathf.Pow(BulletVelocity.x,2)+Mathf.Pow(BulletVelocity.y,2)) / 1;
			BulletVelocity = BulletVelocity/RangeKey*20;
			
			ShotVelocity.Velocity = BulletVelocity;
			ShotDamage.HitDamage = 1;
			
			Shooting = 1;
			Shooted++;
			Invoke("ShootCool",0.8f);
		}
		if (Shooted == 3){
			Invoke("Reload",2f);
			Shooted++;
		}
	}
	
	void ScreenLock(){
		
		Vector3 CameraPos = CameraTransform.position;
		float CamHeight = Camera.main.orthographicSize;
		float CamWidth = CamHeight*Camera.main.aspect;
		
		float XMinCut = CameraPos.x - CamWidth;
		float XMaxCut = CameraPos.x + CamWidth;
		float YMinCut = CameraPos.y - CamHeight;
		float YMaxCut = CameraPos.y + CamHeight;
		
		if (SelfPos.x > XMaxCut)
			SelfPos.x = XMaxCut;
		
		if (SelfPos.y > YMaxCut)
			SelfPos.y = YMaxCut;
		
		if (SelfPos.x < XMinCut)
			SelfPos.x = XMinCut;
		
		if (SelfPos.y < YMinCut)
			SelfPos.y = YMinCut;
	}
	
	void StunCool(){
		Stun = 0;
	}
	
	void GetStuned(){
		SelfRigid2D.velocity = (SelfRigid2D.velocity / 1.1f);
	}
	
	void GetDamaged(Collider2D other){
		int Damage = 1;
		if (other.name != "Environment Damage"){
			Damage_Script Dmg = other.gameObject.GetComponent<Damage_Script>();
			Damage = Dmg.HitDamage;
		}
		
		if (HP != 0){
			if (HP - Damage > 0){
				HP -= Damage;
			}
			else{
				HP = 0;
				DEAD = 1;
			}
		}
		if (other.tag == "Bullet"){
			Vector2 BulletVel = other.gameObject.GetComponent<Rigidbody2D>().velocity;
			Rest = 0;
			Stun = 1;
			SelfRigid2D.velocity = BulletVel;
			Invoke("StunCool",0.3f);
			Destroy(other.gameObject);
		}
	}
	
	void OnTriggerStay2D(Collider2D other){ // Catch Collider on Object!
		if ((other.tag == "Player")||(other.tag == "Bullet")){
			GetDamaged(other);
		}
	}
	
	void Dead(){
		SpawnerInfo.EnemyCount--;
		SpawnerInfo.EnemySlayed++;
	}
	
	void AI(){
		if (Stun != 1){
			if (PlayerDistance() <= 25f)
				ShotPlayer();
		
			float move = 0f;
		
			if (timer >= 5){
			switch(Random.Range(0,4)){
				
				case 0:
				Rest = 1;
				timer = 2;
				break;
				
				case 1:
				Rest = 0;
				move = 0f;
				timer = 0;
				break;
				
				case 2:
				Rest = 0;
				move = -1f;
				timer = 3;
				break;
				
				case 3:
				Rest = 0;
				move = 1f;
				timer = 3;
				break;
			}
		}
		
			Move(move);
			
			if (Rest == 1){
				SelfRigid2D.velocity = new Vector2(0f,0f);
			}
		}
		if (DEAD == 1){
			Dead();
			DEAD++;
			Destroy(gameObject,0.1f);
		}
		if (Stun == 1){
			GetStuned();
		}
	}

    void Update(){
		AI();
		ScreenLock();
    }
}