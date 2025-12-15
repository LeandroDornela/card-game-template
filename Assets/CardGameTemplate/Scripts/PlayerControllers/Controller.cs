using System;
using UnityEngine;

namespace CardGameTemplate
{
    public class Controller : MonoBehaviour
    {
        private Guid _runtimePlayerGuid;

        public Guid RuntimePlayerGuid
        {
            get
            {
                if(_runtimePlayerGuid == default)
                {
                    throw new ArgumentException();
                }
                else
                {
                    return _runtimePlayerGuid;
                }
            }
        }

        public virtual void Initialize(Guid runtimePlayerGuid)
        {
            _runtimePlayerGuid = runtimePlayerGuid;

            Debug.Log(Debug.Category.GameLogic, $"{runtimePlayerGuid} {GetType()} initialized.");
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
