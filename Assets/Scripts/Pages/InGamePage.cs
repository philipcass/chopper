using UnityEngine;
using System.Collections;

public class InGamePage : BPage {
    public FPWorld root;
    override public void Start() {
        ListenForUpdate(HandleUpdate);
        root = FPWorld.Create(64.0f);
		for(int i =0; i < 10; i++){
	        Platform p = Platform.Create();
	        p.Init(new Vector2(100*i,-100*i), this);
        	Person b = Person.Create();
	        b.Init(new Vector2(i*100,10), this);
		}
        Chopper c = Chopper.Create();
        c.Init(new Vector2(0,50), this);
		
		Futile.stage.Follow(c.sprite, false, false);
    }

    void HandleUpdate() {
    }
}
