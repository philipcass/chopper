using UnityEngine;
using System.Collections.Generic;
using System;

public class Chopper : MonoBehaviour {
    FPNodeLink link;
    public FSprite sprite;
    BoxCollider boxCollider;
    FContainer holder;
    float _leftRight = 0;
    float _upDown = 0;
    GameObject _lastLink;
    int _personCount = 0;

    public static Chopper Create() {
        GameObject chopperGO = new GameObject("Chopper");
        Chopper chopper = chopperGO.AddComponent<Chopper>();
        return chopper;
    }
 
    public void Init(Vector2 startPos, FContainer container) {
     
        gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS, 0);
     
        sprite = new FSprite(Futile.whiteElement);
        sprite.SetPosition(startPos);
     
        container.AddChild(holder = new FContainer());
        holder.AddChild(sprite);
     
        InitPhysics();
     
        holder.ListenForUpdate(HandleUpdate);
     
        _lastLink = this.gameObject;
    }
 
    public void Destroy() {
        UnityEngine.Object.Destroy(gameObject);
    }
 
    void InitPhysics() {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        rb.angularDrag = 5.0f;
        rb.mass = 1f;
        rb.drag = 1f;
        RXWatcher.Watch(rb);
     
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
        //float vaxis = Input.GetAxis("Vertical");
        //float haxis = Input.GetAxis("Horizontal");
        
        if(Input.GetKey(KeyCode.UpArrow)) {
            _upDown += gameObject.GetComponent<Rigidbody>().mass;
        } else if(Input.GetKey(KeyCode.DownArrow)) {
            _upDown -= gameObject.GetComponent<Rigidbody>().mass;
        } else {
            //gameObject.rigidbody.velocity =Vector3.zero;
        }
     
        if(Input.GetKey(KeyCode.LeftArrow)) {
            _leftRight += gameObject.GetComponent<Rigidbody>().mass;
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            _leftRight -= gameObject.GetComponent<Rigidbody>().mass;
        } else {
            //gameObject.rigidbody.velocity =Vector3.zero;
        }
     
        if(Input.GetKey(KeyCode.Space)) {
            this.rigidbody.AddExplosionForce(this._personCount * 25, this.transform.position, 1);
        }
        
        _upDown = Mathf.Clamp(_upDown, -50, 50);
        _leftRight = Mathf.Clamp(_leftRight, -50, 50);
     
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * _leftRight);
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * _upDown);
        sprite.SetPosition(GetPos());
        
        _upDown = _leftRight = 0;
     
        Debug.Log(rigidbody.velocity);     
    }
 
    void HandleFixedUpdate() {

    }
 
    public Vector2 GetPos() {
        return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
    }
    
    public void OnCollisionEnter(Collision coll) {
        Person person = coll.collider.gameObject.GetComponent<Person>();

        if(person != null && person.parent == null) {
            HingeJoint hinge = this._lastLink.AddComponent<HingeJoint>();
            hinge.connectedBody = person.rigidbody;
            hinge.breakForce = 10;
            JointSpring jspring = hinge.spring;

            jspring.spring = 0.1f;   
            hinge.spring = jspring;
            hinge.axis = new Vector3(0.0f, 0.0f, 1.0f);
         
            person.parent = this;
            _lastLink = person.gameObject;
            this._personCount++;
        }

    }


}


