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

        public InputComponent input { get { return (InputComponent)GetComponent(InputComponentIds.Input); } }
        public bool hasInput { get { return HasComponent(InputComponentIds.Input); } }

        public Entity AddInput(InputIntent newIntent) {
            var component = CreateComponent<InputComponent>(InputComponentIds.Input);
            component.Intent = newIntent;
            return AddComponent(InputComponentIds.Input, component);
        }

        public Entity ReplaceInput(InputIntent newIntent) {
            var component = CreateComponent<InputComponent>(InputComponentIds.Input);
            component.Intent = newIntent;
            ReplaceComponent(InputComponentIds.Input, component);
            return this;
        }

        public Entity RemoveInput() {
            return RemoveComponent(InputComponentIds.Input);
        }
    }
}

    public partial class InputMatcher {

        static IMatcher _matcherInput;

        public static IMatcher Input {
            get {
                if(_matcherInput == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.Input);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherInput = matcher;
                }

                return _matcherInput;
            }
        }
    }
