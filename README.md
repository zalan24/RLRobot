# RLRobot
Reinfercement learning test with robots

This project is a test project for learning RL (Reinforcement learning) models.
It is under developement, but a lot of models will be available to test.

This project is made with Unity3D 5. If you don't have this software, you can go to Build/ and execute the application there.

#If you want to create a new model:
#1. Use untiy designer to construct it
#2. Create a script for your model.
  Use the Robot.cs as a parent script and override the functions.
  getStateNum() returns the number of possible states of your model.
  getSatet() returns what state your models is in (integer).
  getActionNum(int state) returns the number of possible actions in a specific state.
  getReward(int state, int action, int state2):
    You need to give your model rewards in this function.
#3. Create a brain
  The script RLBrain.cs containes all the information that the model learns. This way you can create multiple models which help each other learn.
  Put this script on an other GameObject and set the brain parameter of the Robot objects.
  The brain has a filename as an input, this is the information is stored. (Only saves on exit)
