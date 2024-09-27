using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Situations and actions
        string[] states = new string[]
        {
            "state00", "state01", "state02", "state03", "state04",
            "state10", "state11", "state12", "state13", "state14",
            "state20", "state21", "state22", "state23", "state24",
            "state30", "state31", "state32", "state33", "state34",
            "state40", "state41", "state42", "state43", "state44"
        };
        string[] actions = new string[] { "up", "down", "left", "right" };
        // Q-table
        Dictionary<string, Dictionary<string, double>> QTable = new Dictionary<string, Dictionary<string, double>>();

        // Initialize the Q-table
        foreach (string state in states)
        {
            QTable[state] = new Dictionary<string, double>();
            foreach (string action in actions)
            {
                QTable[state][action] = 0;
            }
        }

        // Reinforcement learning loop
        for (int episode = 0; episode < 10; episode++)
        {
            // Select the initial state
            string state = "state00";
            int stepCount = 0;
            while (true)
            {
                stepCount++;
                // Select the action according to the ε-greedy policy
                string action;
                if (new Random().NextDouble() < 0.1)  // Select a random action with a probability of 10%
                {
                    action = actions[new Random().Next(actions.Length)];
                }
                else  // Select the action with the highest Q-value with a probability of 90%
                {
                    action = MaxQAction(QTable[state]);
                }

                Console.WriteLine("Step " + stepCount + ": " + state + ", " + action);

                // Perform the action and receive the reward
                string nextState = PerformAction(state, action);
                double reward = Reward(nextState);

                // Update the Q-value
                double oldQ = QTable[state][action];
                double maxNextQ = MaxQValue(QTable[nextState]);
                double newQ = oldQ + 0.5 * (reward + 0.9 * maxNextQ - oldQ);
                QTable[state][action] = newQ;

                // Transition to the new state
                state = nextState;

                // If the game is over, break the loop
                if (state == "state44")
                {
                    Console.WriteLine("The goal was reached in " + stepCount + " steps.");
                    break;
                }
            }
        }
    }

    static string PerformAction(string state, string action)
    {
        // Convert the state and action to grid coordinates
        int x = int.Parse(state.Substring(5, 1));
        int y = int.Parse(state.Substring(6, 1));

        switch (action)
        {
            case "up":
                x = Math.Max(0, x - 1);
                break;
            case "down":
                x = Math.Min(4, x + 1);
                break;
            case left":
                y = Math.Max(0, y - 1);
                break;
            case "right":
                y = Math.Min(4, y + 1);
                break;
        }
        // Determine the new state
        string newState = "state" + x.ToString() + y.ToString();

        // If the agent has fallen into a pit or reached the goal, restart the game
        if (newState == "state04" || newState == "state23" || newState == "state21" || newState == "state22" || newState == "state40")
        {
            newState = "state00";  // Return to the initial state
        }

        return newState;
    }

    static double Reward(string state)
    {
        // This function calculates the reward for a given state
        // If the agent has reached the goal state, it receives a large reward
        if (state == "state44")
        {
            return 10;
        }
        // If the agent has fallen into a pit, it receives a large penalty.
        else if (state == "state04" || state == "state21" || state == "state22" || state == "state23" || state == "state40")
        {
            return -1;
        }
        // A small penalty for other states (to minimize the number of moves)
        else
        {
            return -0.1;
        }
    }

    static string MaxQAction(Dictionary<string, double> actionValues)
    {
        // This function returns the action with the highest Q-value for a given state
        string maxQAction = null;
        double maxQValue = double.NegativeInfinity;
        foreach (KeyValuePair<string, double> entry in actionValues)
        {
            if (entry.Value > maxQValue)
            {
                maxQValue = entry.Value;
                maxQAction = entry.Key;
            }
        }

        return maxQAction;
    }

    static double MaxQValue(Dictionary<string, double> actionValues)
    {
        // This function returns the highest Q-value for a given state
        double maxQValue = double.NegativeInfinity;
        foreach (KeyValuePair<string, double> entry in actionValues)
        {
            if (entry.Value > maxQValue)
            {
                maxQValue = entry.Value;
            }
        }

        return maxQValue;
    }
}
