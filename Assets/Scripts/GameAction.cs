using Unidux;
using UnityEngine;

public static class GameAction
{
    private static int timeBonusMultiplier = 10;
    
    public enum ActionType
    {
        GameOver,
        DecrementTime
    }

    public class Action
    {
        public ActionType ActionType;

        public Action()
        {
        }
    }

    public static class ActionCreator
    {
        public static Action Create(ActionType type, Vector3 position)
        {
            return new Action() { ActionType = type };
        }

        public static Action GameOver()
        {
            return new Action() { ActionType = ActionType.GameOver };
        }
        
        public static Action DecrementTime()
        {
            return new Action() { ActionType = ActionType.DecrementTime };
        }
    }

    public class Reducer : ReducerBase<State, Action>
    {
        public override State Reduce(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.GameOver:
                    state.gameOver = true;
                    state.score += state.timeLeft * timeBonusMultiplier;
                    break;
                case ActionType.DecrementTime:
                    state.timeLeft -= 1;
                    break;
            }

            return state;
        }
    }
}