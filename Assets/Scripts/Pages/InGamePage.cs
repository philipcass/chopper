using UnityEngine;
using System.Collections;

public class InGamePage : BPage {
    public FPWorld root;
    override public void Start() {
        ListenForUpdate(HandleUpdate);
        root = FPWorld.Create(64.0f);
        Platform p = Platform.Create();
        p.Init(new Vector2(0,-100), this);
		for(int i =0; i < 10; i++){
        	Person b = Person.Create();
	        b.Init(new Vector2(i*50,0), this);
		}
        Chopper c = Chopper.Create();
        c.Init(new Vector2(0,0), this);
		
		Futile.stage.Follow(c.sprite, false, false);
    }

    void HandleUpdate() {
    }
}
