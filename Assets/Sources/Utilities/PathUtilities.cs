using Entitas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum NodeState { Untested, Open, Closed }

public class Node
{
    float g;
    public float G { get { return g; } set { if (value <= G) g = value; else Debug.LogWarning("Trying to set G (" + value + ") superior to the current G ("+ g + ")"); } }
    public float H { get; private set; }
    public float F { get { return this.G + this.H; } }
    public Entity Parent { get; set; }

    public Node(Entity entity, Entity startEntity, Entity goalEntity)
    {
        if (entity.hasMapPosition)
        {
            if (startEntity.hasMapPosition)
                g = MapUtilities.GetDistance(startEntity, entity);
            else
                throw (new Exception("startEntity doesn't have a map position component."));

            if (goalEntity.hasMapPosition)
                H = MapUtilities.GetDistance(goalEntity, entity);
            else
                throw (new Exception("goalEntity doesn't have a map position component."));
            
            Parent = null;
        }
    }

    public Node(Vector3 mapPosition, Vector3 startMapPosition, Vector3 goalMapPosition)
    {
        g = MapUtilities.GetDistance(startMapPosition, mapPosition);
        H = MapUtilities.GetDistance(goalMapPosition, mapPosition);
        Parent = null;
    }
}

public static class PathUtilities {

    public static Coroutine PathProcessing;

    public static void SetNode(this Entity e, Entity startEntity, Entity goalEntity)
    {
        if (e.hasMapPosition && e.hasTile)
        {
            if (!e.hasNode)
                e.AddNode(new Node(e, startEntity, goalEntity));
            else
                e.ReplaceNode(new Node(e, startEntity, goalEntity));
        } else
        {
            throw (new Exception(e + " is not a tile!"));
        }
    }

    static Entity GetLowestFNode(List<Entity> entities)
    {
        if (entities.Count > 0)
        {
            Entity currentLowest = entities[0];
            foreach (var entity in entities)
            {
                if (entity.node.Node.F < currentLowest.node.Node.F)
                    currentLowest = entity;
            }
            return currentLowest;
        }
        return null;
    }

    /// <summary>
    /// Do the pathfinding magic
    /// </summary>
    /// <param name="graph">All the entities that must be checked</param>
    /// <param name="startNode">Entity the path will start from</param>
    /// <param name="goalNode">Entity the path try to reach</param>
    /// <returns>Returns the path if found or null if not</returns>
    public static Entity[] FindPath(Entity[] graph, Entity startEntity, Entity goalEntity)
    {
        foreach(var entity in graph)
        {
            entity.SetNode(startEntity, goalEntity);
        }

        var closedList = new List<Entity>();
        var openList = new List<Entity>();

        openList.Add(startEntity);

        Entity currentEntity;
        while(openList.Count > 0)
        {
            currentEntity = GetLowestFNode(openList);
            
            if (currentEntity == goalEntity)
                return ReconstructPath(currentEntity);

            openList.Remove(currentEntity);
            closedList.Add(currentEntity);

            foreach(var neighbor in MapUtilities.GetNeighbors(currentEntity))
            {
                if (closedList.Contains(neighbor))
                    continue;

                float newG = currentEntity.node.Node.G + MapUtilities.GetDistance(currentEntity, neighbor);

                if (!openList.Contains(neighbor))
                    openList.Add(neighbor);
                else if (newG >= neighbor.node.Node.G)
                    continue;

                neighbor.node.Node.Parent = currentEntity;
                neighbor.node.Node.G = newG;
            }
        }

        return null;
    }

    public static Entity[] ReconstructPath(Entity lastNode)
    {
        List<Entity> path = new List<Entity>();
        Entity current = lastNode;

        while(current.node.Node.Parent != null)
        {
            path.Add(current);
            current = current.node.Node.Parent;
        }

        path.Reverse();

        return path.ToArray();
    }

    public static IEnumerator ProcessPathfinding(Entity[] graph, Entity startNode, Entity goalNode, UnityAction<Entity[]> callbackOnPathFound)
    {
        Entity[] path = FindPath(graph, startNode, goalNode);

        if(path != null)
            callbackOnPathFound(path);

        yield return null;
    }
}
