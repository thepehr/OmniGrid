using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace LGrid
{
	public class GridManager : SerializedMonoBehaviour
	{
		#region singleton
		private static GridManager _instance;
		public static GridManager Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<GridManager>()); } }
		#endregion

		public Dictionary<Position, DataEntity> dataMap = new Dictionary<Position, DataEntity>();
		public Dictionary<Type, HashSet<Position>> componentsMap = new Dictionary<Type, HashSet<Position>>();
        public Dictionary<Position, HashSet<string>> tags = new Dictionary<Position, HashSet<string>>();
        public Dictionary<string, HashSet<Position>> positions = new Dictionary<string, HashSet<Position>>();

        public HashSet<string> this[Position position]
        {
            get 
            {
                if (tags.ContainsKey(position)){
                    if (tags[position] != null)
                        return tags[position];
                }
                return new HashSet<string>();
            }
        }

        public HashSet<Position> this[string tag]
        {
            get
            {
                if (positions.ContainsKey(tag))
                {
                    return positions[tag];
                }
                return new HashSet<Position>();
            }
        }

        public bool HasTag(Position position, string tag){
            return tags.ContainsKey(position) && tags[position] != null && tags[position].Contains(tag);
        }

        public void AddTag(Position position, string tag){
            if (tags.ContainsKey(position))
            {
                if (tags[position] != null)
                    tags[position].Add(tag);
                else
                    tags[position] = new HashSet<string>(){tag};
            }
            else
            {
                tags.Add(position, new HashSet<string>(){tag});
            }
            if (positions.ContainsKey(tag))
            {
                positions[tag].Add(position);
            }
            else
            {
                positions.Add(tag, new HashSet<Position>() { position });
            }
        }
        public void AddTags(Position position, HashSet<string> tags)
        {
            if (tags == null)
                return;
            foreach (var item in tags)
            {
                AddTag(position, item);
            }
        }

        public void RemoveTag(Position position, string tag){
            if (tags.ContainsKey(position) && tags[position] != null){
                tags[position].Remove(tag);
            }
            if (positions.ContainsKey(tag))
            {
                positions[tag].Remove(position);
            }
        }
        public void RemoveTags(Position position, HashSet<string> tags)
        {
            if (tags == null)
                return;
            foreach (var item in tags)
            {
                RemoveTag(position, item);
            }
        }


        [Button("Clear")]
        public void Clear()
        {
            dataMap.Clear();
            componentsMap.Clear();
        }

        public static T AddComponent<T>(Position position) where T : DataComponent, new(){
            if (Instance.componentsMap.ContainsKey(typeof(T)))
                Instance.componentsMap[typeof(T)].Add(position);
            else{
                Instance.componentsMap.Add(typeof(T), new HashSet<Position>(){position});
            }
            if (Instance.dataMap.ContainsKey(position)){
                Instance.dataMap[position].AddComponent<T>();
            }
            else {
                var d = new DataEntity();
                d.AddComponent<T>();
                Instance.dataMap.Add(position, d);
            }
            return Instance.dataMap[position].GetComponent<T>();
        }

        public static T GetComponent<T>(Position position) where T : DataComponent{
            if (Instance.dataMap.ContainsKey(position) && Instance.dataMap[position].HasComponent<T>()){
                return Instance.dataMap[position].GetComponent<T>();
            }
            return null;
        }

        public static void RemoveComponent<T>(Position position) where T : DataComponent{
            if (Instance.dataMap.ContainsKey(position) && Instance.dataMap[position].HasComponent<T>()){
                Instance.componentsMap[typeof(T)].Remove(position);
                if (GridManager.Instance.componentsMap[typeof(T)].Count == 0)
                    GridManager.Instance.componentsMap.Remove(typeof(T));
                Instance.dataMap[position].RemoveComponent<T>();
            }
        }

        [Button("HasTag")]
        public static bool HasComponent<T>(Position position) where T : DataComponent
        {
            return Instance.dataMap.ContainsKey(position) && Instance.dataMap[position].HasComponent<T>();
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            foreach (var item in tags.Keys)
            {
                var t = "";
                foreach (var tag in tags[item])
                {
                    t += (tag + ", ");
                }
                Handles.Label(item.GetWorldPosition(), t);
            }
        }
#endif
	}

}



