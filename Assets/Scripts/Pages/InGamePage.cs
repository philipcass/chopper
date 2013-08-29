using UnityEngine;
using System.Collections;

public class InGamePage : BPage {
    public FPWorld root;
    public FContainer persons;
    Chopper c;
    override public void Start() {
        Futile.atlasManager.LoadImage("chopper");
        Futile.atlasManager.LoadImage("man");
        
        this.AddChild(persons = new FContainer());
        
        ListenForUpdate(HandleUpdate);
        root = FPWorld.Create(64.0f);
        
        
		for(int i =0; i < 50; i++){
	        Platform p = Platform.Create();
	        p.Init(new Vector2(i*250,RXRandom.Range(-100,100)*i), this);
        	Person b = Person.Create();
	        b.Init(new Vector2(p.sprite.x,p.sprite.y+532), this);
            b.GameOver = HandleGameOver;
		}
        
        
        c = Chopper.Create();
        c.Init(new Vector2(0,128), this);
		
        RXWatcher.Watch(this);
		Futile.stage.Follow(c.sprite, false, false);
    }

    void HandleUpdate() {
        this.scale = Mathf.Pow(0.95f, c.PersonCount);
        Debug.Log(c.sprite.y);
        if(c.sprite.y > 1500)
            HandleWin();
    }

    public void HandleGameOver ()
    {
        FStage UIstage = new FStage("UI");
        Futile.AddStage(UIstage);
        UIstage.AddChild(new FLabel("Abstract", "GAME OVER"));
        RemoveListenForUpdate();
    }
    
    public void HandleWin ()
    {
        FStage UIstage = new FStage("UI");
        Futile.AddStage(UIstage);
        UIstage.AddChild(new FLabel("Abstract", string.Format("WINNER\nSAVED: {0}", c.PersonCount)));
        RemoveListenForUpdate();
    }
}
