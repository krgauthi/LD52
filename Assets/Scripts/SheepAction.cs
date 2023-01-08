using Unidux;
using UnityEngine;

public static class SheepAction
{

    //TODO: adjust time increment to keep time down
    private static int sheepScore = 1; 
    private static int timeIncrement = 2; 
    
    public enum ActionType
    {
        Harvest,
        Die
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

        public static Action KillSheep()
        {
            return new Action() { ActionType = ActionType.Die };
        }

        public static Action HarvestSheep()
        {
            return new Action() { ActionType = ActionType.Harvest };
        }
        
        
    }

    public class Reducer : ReducerBase<State, Action>
    {
        public override State Reduce(State state, Action action)
        {
            switch (action.ActionType)
            {
                case ActionType.Harvest:
                    state.score += state.timeLeft * sheepScore;
                    state.sheepCount -= 1;
                    state.timeLeft += timeIncrement;
                    break;
                case ActionType.Die:
                    state.sheepCount -= 1;
                    break;
                
            }

            return state;
        }
    }
}