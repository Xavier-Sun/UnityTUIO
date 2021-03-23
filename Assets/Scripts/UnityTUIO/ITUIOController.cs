using TUIO;

namespace UnityTUIO
{
    public interface ITUIOController
    {
        int SymbolID { get; }

        void TUIOObjectAppear(TuioObject tuioObject);
        void TUIOObjectDisappear(TuioObject tuioObject);
        void TUIOObjectUpdate(TuioObject tuioObject);
    }
}
