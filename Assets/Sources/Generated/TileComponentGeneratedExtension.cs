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

        public TileComponent tile { get { return (TileComponent)GetComponent(CoreComponentIds.Tile); } }
        public bool hasTile { get { return HasComponent(CoreComponentIds.Tile); } }

        public Entity AddTile(string newDescription) {
            var component = CreateComponent<TileComponent>(CoreComponentIds.Tile);
            component.Description = newDescription;
            return AddComponent(CoreComponentIds.Tile, component);
        }

        public Entity ReplaceTile(string newDescription) {
            var component = CreateComponent<TileComponent>(CoreComponentIds.Tile);
            component.Description = newDescription;
            ReplaceComponent(CoreComponentIds.Tile, component);
            return this;
        }

        public Entity RemoveTile() {
            return RemoveComponent(CoreComponentIds.Tile);
        }
    }
}

    public partial class CoreMatcher {

        static IMatcher _matcherTile;

        public static IMatcher Tile {
            get {
                if(_matcherTile == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Tile);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherTile = matcher;
                }

                return _matcherTile;
            }
        }
    }

    public partial class EditorMatcher {

        static IMatcher _matcherTile;

        public static IMatcher Tile {
            get {
                if(_matcherTile == null) {
                    var matcher = (Matcher)Matcher.AllOf(EditorComponentIds.Tile);
                    matcher.componentNames = EditorComponentIds.componentNames;
                    _matcherTile = matcher;
                }

                return _matcherTile;
            }
        }
    }
