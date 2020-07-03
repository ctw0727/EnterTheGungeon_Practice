using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_behavior : MonoBehaviour
{
	// Set basics
	public int DEAD = 0;
	public int HP = 6;
	public int Armor = 0;
	private int Weapon = 0;
	private float Speed = 8f;
	
	private static float[] WeaponDelay = new float[5];
	private static int[] WeaponDamage = new int[5];
	private static float[] ReloadTime = new float[5];
	private static int[] MaxAmmo = new int[5];
	private static float[] BulletSpeed = new float[5];
	private int[] Ammo = new int[5];
	private int[] AmmoPool = new int[5];

	
	public int Roll = 0;
	public int Invincible = 0;
	public int Reloading = 0;
	public int Shooting = 0;
	
	private LineRenderer MousePointer;
	private Rigidbody2D PlayerRigid2D;
	private Transform PlayerTransform;
	private Transform CameraTransform;
	public GameObject RoundShotPrefab;
	public GameObject LongShotPrefab;
	private Camera MainCamera;
	private Vector3 MouseStaticPos;
	private Vector3 MousePrivatePos;
	
    void Start(){ // Setting before the First frame!
		
		//Setting
        WeaponDamage[0] = 2;
		WeaponDamage[1] = 3;
		WeaponDamage[2] = 1;
		WeaponDamage[3] = 1;
		WeaponDamage[4] = 2;
		MaxAmmo[0] = 8;
		MaxAmmo[1] = 6;
		MaxAmmo[2] = 15;
		MaxAmmo[3] = 2;
		MaxAmmo[4] = 31;
		Ammo[0] = 8;
		Ammo[1] = 6;
		Ammo[2] = 15;
		Ammo[3] = 2;
		Ammo[4] = 31;
		AmmoPool[0] = -1;
		AmmoPool[1] = -1;
		AmmoPool[2] = -1;
		AmmoPool[3] = -1;
		AmmoPool[4] = -1;
		WeaponDelay[0] = 0.8f;
		WeaponDelay[1] = 1.0f;
		WeaponDelay[2] = 0.1f;
		WeaponDelay[3] = 0.5f;
		WeaponDelay[4] = 0.15f;
		ReloadTime[0] = 1.5f;
		ReloadTime[1] = 2.5f;
		ReloadTime[2] = 2.0f;
		ReloadTime[3] = 2.5f;
		ReloadTime[4] = 3.0f;
		BulletSpeed[0] = 23.0f;
		BulletSpeed[1] = 30.0f;
		BulletSpeed[2] = 27.0f;
		BulletSpeed[3] = 20.0f;
		BulletSpeed[4] = 30.0f;
		
		PlayerRigid2D = GetComponent<Rigidbody2D>();
		MousePointer = GetComponent<LineRenderer>();
		PlayerTransform = GetComponent<Transform>();
		CameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
		MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
	
	void GunReset(){ // Success
		Shooting = 0;
		Reloading = 0;
	}
	
	void GunReload(){ // Success
		if (AmmoPool[Weapon] == -1){
			Ammo[Weapon] = MaxAmmo[Weapon];
		}
		else if ((MaxAmmo[Weapon] - Ammo[Weapon])>= AmmoPool[Weapon]){
			Ammo[Weapon] += AmmoPool[Weapon];
			AmmoPool[Weapon] = 0;
		}
		else{
			AmmoPool[Weapon] -= (MaxAmmo[Weapon] - Ammo[Weapon]);
			Ammo[Weapon] = MaxAmmo[Weapon];
		}
		GunReset();
	}
	
	void GunShot(){ // Success
		
		Vector3 PlayerPos = PlayerTransform.position;
		
		float angle = Mathf.Atan2(MousePrivatePos.y - PlayerPos.y, MousePrivatePos.x - PlayerPos.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		GameObject Bullet;
		
		if ((Weapon != 2)&&(Weapon != 4)) 
			Bullet = Instantiate(RoundShotPrefab,PlayerPos,rotation);
		else
			Bullet = Instantiate(LongShotPrefab,PlayerPos,rotation);
		
		Bullet_behavior ShotVelocity = Bullet.GetComponent<Bullet_behavior>();
		Damage_Script ShotDamage = Bullet.GetComponent<Damage_Script>();
		
		ShotVelocity.Velocity = (MousePrivatePos - PlayerPos) * BulletSpeed[Weapon];
		ShotDamage.HitDamage = WeaponDamage[Weapon];
		
		if (Weapon == 3){
			for (int i = 0;i <= 4;i++){
				GameObject Bullets = Instantiate(RoundShotPrefab,PlayerPos,rotation);
			
				Bullet_behavior MoreShotVelocity = Bullets.GetComponent<Bullet_behavior>();
				Damage_Script MoreShotDamage = Bullets.GetComponent<Damage_Script>();
			
				MoreShotVelocity.Velocity = (MousePrivatePos - PlayerPos + new Vector3(Random.Range(-0.30f,0.31f),Random.Range(-0.30f,0.31f),0)) * (BulletSpeed[Weapon]+Random.Range(-1.0f,1.1f));
				MoreShotDamage.HitDamage = WeaponDamage[Weapon];
			}
		}
	}
	
	void UnInvincible(){ // Success
		if (Roll == 0)
			Invincible = 0;
	}
	
	void UnRoll(){ // Success
		Roll = 0;
		Invincible = 0;
	}
	
	void Rolling(float x,float y){ // Success
		Invincible = 1;
		PlayerRigid2D.velocity = new Vector2(12f*x,12f*y);
		Invoke("UnRoll",0.5f);
	}
	
	void GetDamaged(Collider2D other){ // Success
		int Damage = 1;
		if (other.name != "Environment Damage"){
			Damage_Script Dmg = other.gameObject.GetComponent<Damage_Script>();
			Damage = Dmg.HitDamage;
		}
		
		if (Invincible == 0){
			if (Armor != 0){
				Armor -= 1;
				Invincible = 1;
				//ArmorExplode();
				Invoke("UnInvincible",1f);
			}
			else if (HP != 0){
				if (HP - Damage > 0){
					HP -= Damage;
					Invincible = 1;
					Invoke("UnInvincible",1f);
				}
				else{
					HP = 0;
					DEAD = 1;
				}
			}
			if (other.tag == "Enemy_Bullet"){
				Destroy(other.gameObject);
			}
		}
	}
	
	void InputMove(){ // Success
		
		float XMoveCount = 0;
		float YMoveCount = 0;
		if (Input.GetKey(KeyCode.A))
			XMoveCount--;
		if (Input.GetKey(KeyCode.D))
			XMoveCount++;
		if (Input.GetKey(KeyCode.W))
			YMoveCount++;
		if (Input.GetKey(KeyCode.S))
			YMoveCount--;
		
		if ((Input.GetKey(KeyCode.Space))&&(Roll == 0)&&((XMoveCount!=0)||(YMoveCount!=0))){
			Roll = 1;
			Rolling(XMoveCount,YMoveCount);
		}
		
		if (Roll == 0){
			PlayerRigid2D.velocity = new Vector2(Speed*XMoveCount,Speed*YMoveCount);
		}
	}
	
	void InputWeaponChange(){ // Success
		
		int Key;
		
		if ((Input.GetKeyDown(KeyCode.RightArrow))&&(Shooting == 0)&&(Reloading == 0)){
			for(int i=1;i<4;i++){
				Key = Weapon + i;
				if (Key>4) Key = Key - 5;
				if ((Ammo[Key]!= -1)&&(AmmoPool[Key]!= 0)){
					Weapon = Key;
					break;
				}
			}
		}
		
		if ((Input.GetKeyDown(KeyCode.LeftArrow))&&(Shooting == 0)&&(Reloading == 0)){
			for(int i=1;i<4;i++){
				Key = Weapon - i;
				if (Key<0) Key = 4 + Key + 1;
				if ((Ammo[Key]!= -1)&&(AmmoPool[Key]!= 0)){
					Weapon = Key;
					break;
				}
			}
		}
		
		Key = 0;
		
		if (Input.GetKeyDown(KeyCode.Alpha1))
			Key = 1;
		if (Input.GetKeyDown(KeyCode.Alpha2))
			Key = 2;
		if (Input.GetKeyDown(KeyCode.Alpha3))
			Key = 3;
		if (Input.GetKeyDown(KeyCode.Alpha4))
			Key = 4;
		if (Input.GetKeyDown(KeyCode.Alpha5))
			Key = 5;
		
		if (Key != 0){
			if ((Ammo[Key-1]!= -1)&&(AmmoPool[Key-1]!= 0)){
					Weapon = Key-1;
			}
		}
		
	}
	
	void InputAttackOrReload(){ // Success
		
		if ((Input.GetMouseButton(0))&&(Shooting == 0)&&(Reloading == 0)){
			if (Ammo[Weapon] != 0){
				
				GunShot();
				
				if (--Ammo[Weapon] != 0){
					Shooting = 1;
					Invoke("GunReset",WeaponDelay[Weapon]);
				}
			}
			else if (AmmoPool[Weapon] != 0){
				Reloading = 1;
				Invoke("GunReload",ReloadTime[Weapon]);
			}
			else{
				for(int i=1;i<4;i++){
					int Key = Weapon + i;
					if (Key>4)
						Key = Key - 5;
					if ((Ammo[Key]!= -1)&&(AmmoPool[Key]!= 0)){
						Weapon = Key;
						break;
					}
				}
			}
		}
		if ((Input.GetKey(KeyCode.R))&&(Reloading == 0)&&(Ammo[Weapon]!=MaxAmmo[Weapon])){
			Reloading = 1;
			Invoke("GunReload",ReloadTime[Weapon]);
		}
	}
	
	void InputMouseDirectionTracker(){ // Success
		
		Vector3 PlayerPos = PlayerTransform.position;
		MouseStaticPos = new Vector3(MainCamera.ScreenToWorldPoint(Input.mousePosition).x,MainCamera.ScreenToWorldPoint(Input.mousePosition).y,0);
		MousePrivatePos = MouseStaticPos - PlayerPos;
		
		float RangeKey = Mathf.Sqrt(Mathf.Pow(MousePrivatePos.x,2)+Mathf.Pow(MousePrivatePos.y,2)) / 1;
		
		MousePrivatePos = PlayerPos + (MousePrivatePos/RangeKey);
		
		Vector3[] Positions = new Vector3[2];
		Positions[0] = PlayerPos + new Vector3(0,0,-1);
		Positions[1] = MousePrivatePos + new Vector3(0,0,-1);
		MousePointer.positionCount = Positions.Length;
		MousePointer.SetPositions(Positions);
	}
	
	void OnTriggerStay2D(Collider2D other){ // Catch Collider on Object!
		if ((other.tag == "Enemy")||(other.tag == "Enemy_Bullet")){
			GetDamaged(other);
		}
	}
	
    void Update(){ // Call Functions per frame!
		if (DEAD == 0){
			InputMove();
			InputWeaponChange();
			InputMouseDirectionTracker();
			if (Roll == 0){
				InputAttackOrReload();
			}
		}
		else{
			PlayerRigid2D.velocity = new Vector3(0f,0f,0f);
		}
    }
	
}
