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
#4. Using
  The Update function should be overriden the same way as in the samples. There are some variables of the Robot.cs script that are important: S,S2,A. S is the current state (calculated in the base.Update() function), S2 is the last state. To decide what is the best action, use this: A = _getAction_FunctionName_(); You an use an own function for this, but there is a default one.
  Gamma (Robot.cs/Gamma) is used in the decision process. It can be [0;1]. 1 means that it will decide randomly, 0 means it will always do the best it can (without exploration).
  Alpha and Beta are in the RLBrain.cs:
  Alpha [0;1] is the importance of the old information when getting a new reward. 1 means that only the current reward matters, 0 means that the current rewars doesn't metter, only what it remembers (won't learn). Beta [0;1] is the significance of the future rewards. 1 means that the model will value the expected reward the same way as the current rewards. 0 means the expected rewards don't matter.
