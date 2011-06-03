using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

using Microsoft.Xna.Framework;

using Bejeweled.States;

namespace Bejeweled
{
    class Dispatch
    {
        // Define possible game states and set our current one
        public enum GameState { StartMenu, GameMenu, Playing };
        public GameState CurrentState = GameState.Playing;

        // State handlers
        private Hashtable gameStates = new Hashtable();

        public Dispatch()
        {
            // We'll instantiate a new State object for each state
            this.gameStates.Add(GameState.StartMenu, (IState)new StartMenu());
            this.gameStates.Add(GameState.GameMenu, (IState)new GameMenu());
            this.gameStates.Add(GameState.Playing, (IState)new Playing());
        }

        public void dispatch(String hook, GameTime gameTime)
        {
            foreach (IState state in this.States())
            {
                Type thisType = state.GetType();
                MethodInfo method = thisType.GetMethod(hook);
                object[] args;

                switch (hook)
                {
                    case "Update":
                        {
                            args = new object[1];
                            args[0] = gameTime;
                            break;
                        }
                    default:
                        {
                            args = new object[0];
                            break;
                        }
                }

                method.Invoke(state, args);
            }
        }

        public void dispatch(String hook)
        {
            this.dispatch(hook, new GameTime());
        }

        // Create a state enumerator for us
        public IEnumerable States()
        {
            foreach (GameState key in this.gameStates.Keys)
            {
                yield return this.gameStates[key];
            }
        }

        // Get a state for use
        public IState getState(GameState state)
        {
            return (IState)this.gameStates[state];
        }
    }
}
