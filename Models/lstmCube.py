import numpy as np
import subprocess
import torch.nn as nn
from mlagents_envs.environment import UnityEnvironment

# Set the path to the Unity environment binary
env = UnityEnvironment(file_name='C:/Users/John/Documents/Unity/RubixCube.exe')


# Set the name of the behavior to use test
behavior_name = "RubiksCubeBehavior"

# Get the behavior spec
behavior_spec = env.behavior_specs[behavior_name]

# Create the agent's neural network
class RubiksCubeAgent(nn.Module):
    def __init__(self):
        super().__init__()
        self.lstm = nn.LSTM(input_size=6, hidden_size=128, num_layers=2, batch_first=True)
        self.fc1 = nn.Linear(128, 64)
        self.fc2 = nn.Linear(64, 18)

    def forward(self, x):
        out, _ = self.lstm(x)
        out = self.fc1(out[:, -1, :])
        out = self.fc2(out)
        return out

# Load the agent's neural network
agent = RubiksCubeAgent()
agent.load_state_dict(nn.load('Assets/RubixCube.mlagents.settings.asset'))

# Reset the environment and get the initial state
env.reset()
decision_steps, terminal_steps = env.get_steps(behavior_name)
state = np.concatenate([decision_steps.obs[0], terminal_steps.obs[0]], axis=0)

# Run the agent in the environment
done = False
while not done:
    # Convert the state to a tensor and pass it through the agent's neural network
    state_tensor = nn.from_numpy(state).float()
    action_tensor = agent(state_tensor)
    action = action_tensor.detach().numpy()

    # Take the action and get the next state and reward
    decision_steps, terminal_steps = env.get_steps(behavior_name)
    env.set_actions(behavior_name, action)
    env.step()
    next_state = np.concatenate([decision_steps.obs[0], terminal_steps.obs[0]], axis=0)
    reward = np.sum(decision_steps.reward)

    # Check if the episode is done
    if len(terminal_steps) > 0:
        done = True

    # Update the state
    state = next_state

    # Get the Rubik's cube state from the Unity environment
    rubik_state = decision_steps.observation[0][behavior_spec.observation_shapes[0][0]:]
    rubik_state = rubik_state.reshape((6, 9))

    # TODO: Use the Rubik's cube state to generate the next move and provide a hint to the user
    def generate_hint(state):
        # Call Kociemba algorithm using subprocess module
        process = subprocess.Popen(['python', '-c', 'import kociemba; print(kociemba.solve("' + state + '"))'], stdout=subprocess.PIPE)
        output, error = process.communicate()

        # Extract sequence of moves from output
        move_sequence = output.decode('utf-8').strip().split()

        # Get next move in sequence and return as hint
        next_move = move_sequence[0]
        print(f"Hint: {next_move}")
        
        return next_move
    
    generate_hint(rubik_state)