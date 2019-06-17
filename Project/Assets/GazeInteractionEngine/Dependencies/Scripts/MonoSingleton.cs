using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityHelpers
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T self = null;
        private static bool applicationQuit = false;
        public static T instance
        {
            get
            {
                if (self == null)
                {
                    if (applicationQuit)
                    {
                        return null;
                    }

                    self = GameObject.FindObjectOfType(typeof(T)) as T;

                    if (self == null)
                    {

                        self = new GameObject("Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                        if (self == null)
                            Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                        else
                            self.gameObject.name = typeof(T).Name;
                    }
                }
                return self;
            }
        }

        private void Awake()
        {
            if (self == null)
            {
                self = this as T;
            }
            else
            {
                //Duplicate found
                Destroy(gameObject);
                return;
            }
            self.Init();
        }

        public virtual void Init() { }

        protected virtual void OnApplicationQuit()
        {
            self = null;
            applicationQuit = true;
        }

        public static void DestroyInstance()
        {
            self = null;
        }

        protected void OnDestroy()
        {
            if (self == this)
                Clean();
        }

        protected virtual void Clean()
        {

        }

        public static bool exists { get { return self != null && !applicationQuit; } }
    }
}