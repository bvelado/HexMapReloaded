using System;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class GenerateCharactersSystem : IInitializeSystem
{
    Group tileEntities;

    public void Initialize()
    {
        tileEntities = Pools.sharedInstance.core.GetGroup(Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.MapPosition));

        if (!Pools.sharedInstance.core.hasCharacters)
            Pools.sharedInstance.core.CreateEntity()
                .AddCharacters(
                    new IdIndex(Pools.sharedInstance.core, Matcher.AllOf(CoreMatcher.Character, CoreMatcher.Id)),
                    new PositionIndex(Pools.sharedInstance.core, Matcher.AllOf(CoreMatcher.Character, CoreMatcher.MapPosition)));

        Stack<Unit> characters = new Stack<Unit>();
        characters.Push(new Unit("Leo", 100, 3, 60));
        characters.Push(new Unit("Ruth", 80, 3, 70));
        characters.Push(new Unit("Alexander", 90, 3, 68));

        int i = 0;
        while(characters.Count > 0)
        {
            Pools.sharedInstance.core.CreateEntity()
                .AddCharacter(characters.Pop())
                .AddMapPosition(tileEntities.GetEntities()[UnityEngine.Random.Range(0, tileEntities.count)].mapPosition.Position)
                .AddId(i)
                .AddTurnOrder(i)
                .IsControllable(i==0? true : false);

            ++i;
        }
    }
}

public class AddCharacterViewSystem : IReactiveSystem
{
    public static GameObject _charactersContainer = new GameObject("Characters container");

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Character, CoreMatcher.MapPosition).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        if (!Pools.sharedInstance.view.hasCharactersView)
            Pools.sharedInstance.view.CreateEntity()
                .AddCharactersView(
                    new IdIndex(Pools.sharedInstance.view, Matcher.AllOf(ViewMatcher.CharacterView, ViewMatcher.Id)));

        foreach (var entity in entities)
        {
            if(Pools.sharedInstance.view.charactersView.CharacterViewById.FindEntityAtIndex(entity.id.Id) == null)
            {
                var e = Pools.sharedInstance.view.CreateEntity();
                e.AddWorldPosition(MapUtilities.MapToWorldPosition(entity.mapPosition.Position));
                GameObject characterGO = GameObject.Instantiate(Resources.Load("Prefabs/Character"), e.worldPosition.Position + (Vector3.back*0.25f), Quaternion.identity, _charactersContainer.transform) as GameObject;
                characterGO.name = entity.character.Unit.Name;
                var characterView = characterGO.AddComponent<CharacterView>();
                if (characterView)
                {
                    characterView.Initialize(entity.mapPosition.Position, entity.id.Id);
                    e.AddCharacterView(characterView)
                    .AddSelectedListener(characterView)
                    .AddControlledListener(characterView)
                    .AddId(entity.id.Id);
                }
            }
        }
    }
}
