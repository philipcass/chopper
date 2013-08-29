using UnityEngine;
using System.Collections.Generic;
using System;

public class Platform : MonoBehaviour {
    FPNodeLink link;
    public FSprite sprite;
    BoxCollider boxCollider;
	FContainer holder;

    public static Platform Create() {
        GameObject platformGO = new GameObject("Platform");
        Platform platform = platformGO.AddComponent<Platform>();
        return platform;
    }
 
    public void Init(Vector2 startPos, FContainer container) {
     
        gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS, 0);
     
        sprite = new FSprite(Futile.whiteElement);
        sprite.width = 100;
        sprite.height = 1000;
        sprite.SetPosition(startPos);
		
		container.AddChild(holder = new FContainer());
        holder.AddChild(sprite);
     
        InitPhysics();
     
        holder.ListenForUpdate(HandleUpdate);
    }
 
    public void Destroy() {
        UnityEngine.Object.Destroy(gameObject);
    }
 
    void InitPhysics() {
     
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
    }

    void HandleUpdate() {
        sprite.SetPosition(GetPos());
    }
 
    void HandleFixedUpdate() {

    }
 
    public Vector2 GetPos() {
        return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
    }
}


