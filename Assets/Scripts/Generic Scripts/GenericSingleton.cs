using UnityEngine;


/// <summary>
/// Generic Singleton Class
/// </summary>
/// <typeparam name="T"></typeparam>

namespace LoopEnergyClone
{
    public class GenericSingleton<T> : MonoBehaviour where T : class
    {
        static T m_instance;
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = FindObjectOfType(typeof(T)) as T;

                return m_instance;
            }
        }
    }

}