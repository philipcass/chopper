using UnityEngine;
using System.Collections;

public class InGamePage : BPage {
    public FPWorld root;
    override public void Start() {
        ListenForUpdate(HandleUpdate);
        root = FPWorld.Create(64.0f);
        Platform p = Platform.Create();
        p.Init(new Vector2(0,-100));
        Person b = Person.Create();
        b.Init(new Vector2(50,0));
        Chopper c = Chopper.Create();
        c.Init(new Vector2(0,0));
    }

    void HandleUpdate() {
    }
}
