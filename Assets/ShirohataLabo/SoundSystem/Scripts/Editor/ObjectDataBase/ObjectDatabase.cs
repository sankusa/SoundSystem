using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoundSystem {
    [Serializable]
    public class ObjectDatabaseRecordColumn {
        [SerializeField] string _columnId;
        public string ColumnId => _columnId;
        [SerializeField] string _value;
        public string Value {
            get => _value;
            set => _value = value;
        }

        public ObjectDatabaseRecordColumn(string columnId) {
            _columnId = columnId;
        }
    }

    [Serializable]
    public class ObjectDatabaseRecord {
        [SerializeField] Object _asset;
        public Object Asset => _asset;
        [SerializeField] List<ObjectDatabaseRecordColumn> _columns = new();
        public List<ObjectDatabaseRecordColumn> Columns => _columns;

        public ObjectDatabaseRecord(Object asset) {
            _asset = asset;
        }
    }

    public abstract class ObjectDatabase<T> : ScriptableObject where T : ObjectDatabase<T> {
        static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = EditorUtil.LoadAllAsset<T>().FirstOrDefault();
                }
                return _instance;
            }
        }

        [SerializeField] ObjectDatabaseColumnDefinition _columnDefinition;
        public ObjectDatabaseColumnDefinition ColumnDefinition => _columnDefinition;

        [SerializeField] List<ObjectDatabaseRecord> _records;
        public IReadOnlyList<ObjectDatabaseRecord> Records => _records;

        protected virtual bool CanAccept(Type type) {
            return type == typeof(Object);
        }

        public void UpdateDatabase(string[] folders) {
            Undo.RecordObject(this, $"Update {GetType().Name}");
            // レコードの移行
            List<ObjectDatabaseRecord> newRecords = new();
            foreach (string guid in AssetDatabase.FindAssets("", folders)) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Type type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (CanAccept(type)) {
                    Object asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                    ObjectDatabaseRecord oldRecord = _records.Find(x => x.Asset == asset);
                    if (oldRecord == null) {
                        newRecords.Add(new ObjectDatabaseRecord(asset));
                    }
                    else {
                        _records.Remove(oldRecord);
                        newRecords.Add(oldRecord);
                    }
                }
            }
            newRecords.AddRange(_records);
            
            // カラムの移行
            if (_columnDefinition != null) {
                foreach (ObjectDatabaseRecord record in newRecords) {
                    for (int i = 0; i < _columnDefinition.Columns.Count; i++) {
                        string columnId = _columnDefinition.Columns[i].ColumnId;

                        if (i == record.Columns.Count) {
                            record.Columns.Insert(i, new ObjectDatabaseRecordColumn(columnId));
                            continue;
                        }
                        
                        ObjectDatabaseRecordColumn checkingColumn = record.Columns[i];
                        if (checkingColumn.ColumnId == columnId) continue;

                        ObjectDatabaseRecordColumn targetOldColumn = record.Columns.Find(x => x.ColumnId == columnId);
                        if (targetOldColumn != null) {
                            record.Columns.Remove(targetOldColumn);
                            record.Columns.Insert(i, targetOldColumn);
                            continue;
                        }
                        record.Columns.Insert(i, new ObjectDatabaseRecordColumn(columnId));
                    }
                }
            }

            _records = newRecords;
        }
    }
}