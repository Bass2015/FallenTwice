using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	Player playerScript;
	LightWhip whip;
	Animator mAnimator;
	Rigidbody2D mBody;
	Collider2D mCollider;
	SpriteRenderer mRenderer;
	[HideInInspector]
	public int anyLayerMask;
	bool doubleJump;
	public float speed;
	public float impulsoSalto;

	public float impulsoColgado;
	// Use this for initialization
	void Start () {
		playerScript = GetComponent<Player>();
		mAnimator= GetComponent<Animator>();
		mBody = GetComponent<Rigidbody2D>();
		mCollider = GetComponent <Collider2D>();
		mRenderer = GetComponent<SpriteRenderer>();
		whip = GetComponent<LightWhip>();
		anyLayerMask = 1 << 8;
		anyLayerMask = ~anyLayerMask;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Horizontal") != 0){
			Move();
		}else{
			mAnimator.SetFloat("Speed", 0f);
		}
		if (Input.GetButtonDown("Jump") && (CheckOnGround() || !doubleJump)){
			Jump();
		}
	}

	void FixedUpdate(){
		
	}

	#region Movimiento

	void Move(){
		float direction = Input.GetAxis("Horizontal");
		if(playerScript.UpsideDown){
			direction *= -1;
		}
		if (!whip.WhipEnabled() || (whip.WhipEnabled() && CheckOnGround())) {
			mBody.position = new Vector2(mBody.position.x + speed * direction * Time.deltaTime, mBody.position.y);
			LookingTo(direction);
			mAnimator.SetFloat("Speed", 0.5f);
			if(whip.WhipEnabled() && CheckOnGround()){
				mAnimator.SetFloat("Speed", 0.2f);
			}
		}else {
			/*Se añade una fuerza normal(con respecto al mundo, no a sus propias coordenadas)
			Multiplico la componente horizontal con la fuerza de impulso colgado (variable pública para controlarlo)
			y divido esa fuerza por la distancia al ancla, para reflejar que desde más lejos es más difícil moverse.*/
			mBody.AddForce(new Vector2(direction * impulsoColgado  / whip.WhipDistance(), 0), ForceMode2D.Impulse);
		}
	} 

	void LookingTo(float direction)	{
		if(playerScript.UpsideDown){
			direction *= -1;
		}
		if (direction > 0) {
			mRenderer.flipX = false;
		}
		if (direction < 0){
			if (!mRenderer.flipX)
				mRenderer.flipX = true;
		}
	}

	void Jump() {
		
		/*Si no reseteo la velocity, el segundo salto es muy poderoso si se 
		 * apreta muy rápido la tecla de saltar, y muy poco efectivo en caso contrario*/
		mBody.velocity = Vector2.zero;
		float jumpForce = playerScript.UpsideDown ? impulsoSalto * -1 : impulsoSalto;
		mBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
		if (!doubleJump)
			doubleJump = true;
		else
			doubleJump = false;
	}
	public bool CheckOnGround() {
		//tiro un rayo hacia abajo, si toca algo es que está en el suelo
		//Mejor un box, así, aunque no esté completamente sobre la plataforma, detecta que tiene un piesito en ella
		Vector3 colliderSize = mCollider.bounds.size;
		Vector2 direction = Vector2.down;
		float yOrigin = transform.position.y - colliderSize.y / 2;
		if(playerScript.UpsideDown){
			direction *= -1;
			yOrigin += colliderSize.y;
		}
		Vector3 rayOrigin = new Vector3(transform.position.x, yOrigin, 0); 
		Vector3 boxSize = new Vector3(colliderSize.x, 0.1f, 0);
        string[] masks = { "Player", "Lightshot" };
		int mask = LayerMask.GetMask(masks);
		mask = ~mask;
		RaycastHit2D hit = Physics2D.BoxCast(rayOrigin, boxSize, 0f, direction, 0.5f, mask);
		if(hit.collider == null) {
			return false;
		} else {
			doubleJump = true;
			return true;
		}
	}
	#endregion
}
