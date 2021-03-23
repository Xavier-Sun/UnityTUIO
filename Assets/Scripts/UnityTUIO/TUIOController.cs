using TUIO;
using UnityEngine;

namespace UnityTUIO
{
    public abstract class TUIOController : MonoBehaviour, ITUIOController
    {
        [SerializeField]
        private int symbolID = -1;

        public int SymbolID
        {
            get => symbolID;
            set
            {
                if (HasRegistered)
                {
                    Unregister();
                    symbolID = value;
                    Register();
                }
                else
                {
                    symbolID = value;
                }
            }
        }

        public bool HasRegistered { get; private set; }

        protected virtual void Start()
        {
            if (symbolID < 0)
            {
                Debug.LogError("Invalid SymbolID!");
            }
        }

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }

        private void Register()
        {
            TUIOManager.Instance.RegisterController(this);
            HasRegistered = true;
        }

        private void Unregister()
        {
            if (TUIOManager.Instance != null)
            {
                TUIOManager.Instance.UnregisterController(this);
                HasRegistered = false;
            }
        }

        public abstract void TUIOObjectAppear(TuioObject tuioObject);

        public abstract void TUIOObjectUpdate(TuioObject tuioObject);

        public abstract void TUIOObjectDisappear(TuioObject tuioObject);
    }
}
