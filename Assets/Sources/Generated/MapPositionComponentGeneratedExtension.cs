//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {

    public partial class Entity {

        public MapPositionComponent mapPosition { get { return (MapPositionComponent)GetComponent(CoreComponentIds.MapPosition); } }
        public bool hasMapPosition { get { return HasComponent(CoreComponentIds.MapPosition); } }

        public Entity AddMapPosition(UnityEngine.Vector3 newPosition) {
            var component = CreateComponent<MapPositionComponent>(CoreComponentIds.MapPosition);
            component.Position = newPosition;
            return AddComponent(CoreComponentIds.MapPosition, component);
        }

        public Entity ReplaceMapPosition(UnityEngine.Vector3 newPosition) {
            var component = CreateComponent<MapPositionComponent>(CoreComponentIds.MapPosition);
            component.Position = newPosition;
            ReplaceComponent(CoreComponentIds.MapPosition, component);
            return this;
        }

        public Entity RemoveMapPosition() {
            return RemoveComponent(CoreComponentIds.MapPosition);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherMapPosition;

        public static IMatcher MapPosition {
            get {
                if(_matcherMapPosition == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.MapPosition);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherMapPosition = matcher;
                }

                return _matcherMapPosition;
            }
        }
    }

    public partial class EditorMatcher {

        static IMatcher _matcherMapPosition;

        public static IMatcher MapPosition {
            get {
                if(_matcherMapPosition == null) {
                    var matcher = (Matcher)Matcher.AllOf(EditorComponentIds.MapPosition);
                    matcher.componentNames = EditorComponentIds.componentNames;
                    _matcherMapPosition = matcher;
                }

                return _matcherMapPosition;
            }
        }
    }
