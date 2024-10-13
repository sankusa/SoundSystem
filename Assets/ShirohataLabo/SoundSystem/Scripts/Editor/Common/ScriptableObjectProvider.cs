using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SoundSystem {
    public class ScriptableObjectProvider : AssetPostprocessor {
        private abstract class Cache {
            public abstract Type TargetType { get; }
            public int ReferenceCounter { get; set; }
            public abstract void Reload();
        }

        private class Cache<T> : Cache where T : ScriptableObject {
            public override Type TargetType => typeof(T);
            public IEnumerable<T> Objects { get; private set; }

            public Cache() {
                Objects = EditorUtil.LoadAllAsset<T>();
            }

            public override void Reload() {
                Objects = EditorUtil.LoadAllAsset<T>();
            }
        }

        private static List<Cache> _caches = new();

        public static void StartCache<T>() where T : ScriptableObject {
            Cache<T> cache = _caches.OfType<Cache<T>>().FirstOrDefault();

            if (cache == null) {
                cache = new Cache<T>();
                _caches.Add(cache);
            }

            cache.ReferenceCounter++;
        }

        public static void EndCache<T>() where T : ScriptableObject {
            Cache<T> cache = _caches.OfType<Cache<T>>().First();

            cache.ReferenceCounter--;

            if (cache.ReferenceCounter == 0) {
                _caches.Remove(cache);
            }
        }

        public static IEnumerable<T> GetCache<T>() where T : ScriptableObject {
            return _caches.OfType<Cache<T>>().First().Objects;
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            _caches.ForEach(x => x.Reload());
        }
    }
}