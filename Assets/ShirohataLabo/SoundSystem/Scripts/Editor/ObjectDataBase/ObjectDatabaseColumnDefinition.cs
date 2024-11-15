using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem {
    [Serializable]
    public class ObjectDatabaseColumnDefinitionElement {
        [SerializeField] string _columnId;
        public string ColumnId => _columnId;
        [SerializeField] string _columnName;
        public string ColumnName => _columnName;
    }

    [CreateAssetMenu(fileName = nameof(ObjectDatabaseColumnDefinition), menuName = nameof(SoundSystem) + "/Develop/" + nameof(ObjectDatabaseColumnDefinition))]
    public class ObjectDatabaseColumnDefinition : ScriptableObject {
        [SerializeField] List<ObjectDatabaseColumnDefinitionElement> _columns;
        public IReadOnlyList<ObjectDatabaseColumnDefinitionElement> Columns => _columns;
    }
}