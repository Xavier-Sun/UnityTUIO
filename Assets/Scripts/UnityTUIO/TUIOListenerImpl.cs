using TUIO;

namespace UnityTUIO
{
    public class TUIOListenerImpl : TuioListener
    {
        public void addTuioObject(TuioObject tobj)
        {
            TUIOManager.Instance.EnqueueMessage(tobj, TUIOMessageType.Add);
        }

        public void updateTuioObject(TuioObject tobj)
        {
            TUIOManager.Instance.EnqueueMessage(tobj, TUIOMessageType.Update);
        }

        public void removeTuioObject(TuioObject tobj)
        {
            TUIOManager.Instance.EnqueueMessage(tobj, TUIOMessageType.Remove);
        }

        public void addTuioCursor(TuioCursor tcur)
        {
        }

        public void updateTuioCursor(TuioCursor tcur)
        {
        }

        public void removeTuioCursor(TuioCursor tcur)
        {
        }

        public void addTuioBlob(TuioBlob tblb)
        {
        }

        public void updateTuioBlob(TuioBlob tblb)
        {
        }

        public void removeTuioBlob(TuioBlob tblb)
        {
        }

        public void refresh(TuioTime frameTime)
        {
        }
    }
}
