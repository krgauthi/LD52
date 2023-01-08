using Unidux;
using UnityEngine;

public static class PlayerAction
{
    // specify the possible types of actions
    public enum ActionType
    {
        UpdatePosition
    }

    // actions must have a type and may include a payload
    public class Action
    {
        public ActionType ActionType;
        public Vector3 Position;

        public Action(Vector3 position)
        {
            Position = position;
        }
    }

    // ActionCreators creates actions and deliver payloads
    // in redux, you do not dispatch from the ActionCreator to allow for easy testability
    public static class ActionCreator
    {
        public static Action Create(ActionType type, Vector3 position)
        {
            return new Action(position) { ActionType = type };
        }

        public static Action UpdatePosition(Vector3 position)
        {
            return new Action(position) { ActionType = ActionType.UpdatePosition };
        }

        public static Action Scare(Vector3 position)
        {
            return new Action(position) { ActionType = ActionType.UpdatePosition };
        }
    }

    // reducers handle state changes
    public class Reducer : ReducerBase<State, Action>
    {
        public override State Reduce(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.UpdatePosition:
                    state.playerPosition = action.Position;
                    break;
            }

            return state;
        }
    }
}