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

        public SelectedListenerComponent selectedListener { get { return (SelectedListenerComponent)GetComponent(ViewComponentIds.SelectedListener); } }
        public bool hasSelectedListener { get { return HasComponent(ViewComponentIds.SelectedListener); } }

        public Entity AddSelectedListener(ISelectedListener newListener) {
            var component = CreateComponent<SelectedListenerComponent>(ViewComponentIds.SelectedListener);
            component.Listener = newListener;
            return AddComponent(ViewComponentIds.SelectedListener, component);
        }

        public Entity ReplaceSelectedListener(ISelectedListener newListener) {
            var component = CreateComponent<SelectedListenerComponent>(ViewComponentIds.SelectedListener);
            component.Listener = newListener;
            ReplaceComponent(ViewComponentIds.SelectedListener, component);
            return this;
        }

        public Entity RemoveSelectedListener() {
            return RemoveComponent(ViewComponentIds.SelectedListener);
        }
    }
}

    public partial class ViewMatcher {

        static IMatcher _matcherSelectedListener;

        public static IMatcher SelectedListener {
            get {
                if(_matcherSelectedListener == null) {
                    var matcher = (Matcher)Matcher.AllOf(ViewComponentIds.SelectedListener);
                    matcher.componentNames = ViewComponentIds.componentNames;
                    _matcherSelectedListener = matcher;
                }

                return _matcherSelectedListener;
            }
        }
    }
