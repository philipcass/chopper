using UnityEngine;
using System.Collections.Generic;
using System;

public class Person : MonoBehaviour {
    FPNodeLink link;
    FSprite sprite;
    FContainer holder;
    BoxCollider boxCollider;
	bool landed = true;

    public Chopper parent{ get; set; }

    public static Person Create() {
        GameObject personGO = new GameObject("Person");
        Person person = personGO.AddComponent<Person>();
        return person;
    }
 
    public void Init(Vector2 startPos, FContainer container) {
     
        gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS, 0);
     
        sprite = new FSprite(Futile.whiteElement);
        sprite.SetPosition(startPos);
     
        container.AddChild(holder = new FContainer());
        holder.AddChild(sprite);
     
        InitPhysics();
     
        holder.ListenForUpdate(HandleUpdate);
    }
 
    public void Destroy() {
        holder.RemoveListenForUpdate();
        sprite.RemoveFromContainer();
        UnityEngine.Object.Destroy(gameObject);
    }
 
    void InitPhysics() {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        rb.angularDrag = 5.0f;
        rb.mass = 1.0f;
        rb.drag = 0.8f;
     
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(sprite.width, sprite.height, 100) * FPhysics.POINTS_TO_METERS;
     
        PhysicMaterial mat = new PhysicMaterial();
        mat.bounciness = 0.3f;
        mat.dynamicFriction = 0.5f;
        mat.staticFriction = 0.5f;
        mat.frictionCombine = PhysicMaterialCombine.Maximum;
        collider.material = mat;

        float speed = 30.0f;
        float angle = RXRandom.Range(0, RXMath.DOUBLE_PI);
        Vector2 startVector = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
        rb.velocity = startVector.ToVector3InMeters();
    }

    void HandleUpdate() {
        sprite.SetPosition(GetPos());
    }
 
    void HandleFixedUpdate() {

    }
 
    public Vector2 GetPos() {
        return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
    }
    
    void OnCollisionEnter(Collision coll) {
        Person person = coll.collider.gameObject.GetComponent<Person>();

        if(person != null && parent != null) {
            parent.OnCollisionEnter(coll);
        }

        Platform platform = coll.collider.gameObject.GetComponent<Platform>();

        if(platform != null && parent != null && landed == false) {
            Debug.Log("GAME OVER");
			this.rigidbody.AddExplosionForce(10000, this.transform.position, 10000);
        }else if(platform == null && landed == true){
			landed = false;
		}

    }


}


