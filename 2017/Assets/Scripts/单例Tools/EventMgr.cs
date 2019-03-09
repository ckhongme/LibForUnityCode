using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace K
{
    public delegate void EventListener(params object[] args);

    public class EventMgr : MonoBehaviour
    {
        public static EventMgr Instance;
        private Dictionary<string, List<EventListener>> _listeners;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(gameObject);
                }
            }

            _listeners = new Dictionary<string, List<EventListener>>();
        }

        private void OnDestroy()
        {
        }

        public void AddListener(string key, EventListener listener)
        {
            if (!_listeners.ContainsKey(key))
            {
                _listeners.Add(key, new List<EventListener>());
            }
            _listeners[key].Add(listener);
        }

        public void RemoveListener(string key, EventListener listener)
        {
            if (_listeners.ContainsKey(key))
                _listeners[key].Remove(listener);
        }

        public void SentEvent(string key, params object[] param)
        {
            List<EventListener> list = null;
            if (_listeners.TryGetValue(key, out list))
            {
                List<EventListener> tempList = new List<EventListener>(list);
                foreach (EventListener _delegate in tempList)
                {
                    _delegate(param);
                }
            }
        }

        public void SentEventDelay(float time, string key, params object[] param)
        {
            StartCoroutine(Delay(time, key, param));
        }

        IEnumerator Delay(float time, string key, params object[] param)
        {
            yield return new WaitForSeconds(time);
            SentEvent(key, param);
        }
    }
}

