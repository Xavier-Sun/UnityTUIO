using TUIO;

namespace UnityTUIO
{
    public class TUIOObjectMessage
    {
        public TuioObject tuioObject;

        public TUIOMessageType messageType;

        public TUIOObjectMessage(TuioObject tuioObject, TUIOMessageType messageType)
        {
            this.tuioObject = tuioObject;
            this.messageType = messageType;
        }
    }
}
