using TUIO;
using UnityEngine;
using UnityTUIO;

public class UnityTUIOExample : TUIOController
{
    public override void TUIOObjectAppear(TuioObject tuioObject)
    {
        Debug.Log($"{tuioObject.SymbolID} Appear");
    }

    public override void TUIOObjectDisappear(TuioObject tuioObject)
    {
        Debug.Log($"{tuioObject.SymbolID} Disappear");
    }

    public override void TUIOObjectUpdate(TuioObject tuioObject)
    {
        Debug.Log($"{tuioObject.SymbolID} Update");
    }
}
