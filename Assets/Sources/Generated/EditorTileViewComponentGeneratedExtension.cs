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

        public EditorTileViewComponent editorTileView { get { return (EditorTileViewComponent)GetComponent(EditorComponentIds.EditorTileView); } }
        public bool hasEditorTileView { get { return HasComponent(EditorComponentIds.EditorTileView); } }

        public Entity AddEditorTileView(EditorTileView newView) {
            var component = CreateComponent<EditorTileViewComponent>(EditorComponentIds.EditorTileView);
            component.View = newView;
            return AddComponent(EditorComponentIds.EditorTileView, component);
        }

        public Entity ReplaceEditorTileView(EditorTileView newView) {
            var component = CreateComponent<EditorTileViewComponent>(EditorComponentIds.EditorTileView);
            component.View = newView;
            ReplaceComponent(EditorComponentIds.EditorTileView, component);
            return this;
        }

        public Entity RemoveEditorTileView() {
            return RemoveComponent(EditorComponentIds.EditorTileView);
        }
    }
}

    public partial class EditorMatcher {

        static IMatcher _matcherEditorTileView;

        public static IMatcher EditorTileView {
            get {
                if(_matcherEditorTileView == null) {
                    var matcher = (Matcher)Matcher.AllOf(EditorComponentIds.EditorTileView);
                    matcher.componentNames = EditorComponentIds.componentNames;
                    _matcherEditorTileView = matcher;
                }

                return _matcherEditorTileView;
            }
        }
    }