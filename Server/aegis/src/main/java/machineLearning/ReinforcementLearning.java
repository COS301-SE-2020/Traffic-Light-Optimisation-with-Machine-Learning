package machineLearning;

import java.util.ArrayList;
import java.util.Random;

public class ReinforcementLearning {

    public NeuralNetwork target, prediction;
    private NeuralNetworkUtitlities utility;
    //private ArrayList<Transition> replayBuffer;                                   //replay buffer
    private int lastAction;
    private int currentAction;
    private int numMemories;
    private double[] lastState = null;                                              //holds the last 
    private double[] currState = null;                                              //
    private int currIteration;
    private final static int minibatchSize = 64;                                    //minibatch size m
    private final static double discount = 0.8;
    private final static int totalIterations = 100;                                 //how many iterations before updating target 
    private final static Random random = new Random(42069);                         //
    private final ReplayBuffer replayBuffer;

    /**
     * 
    */
    public ReinforcementLearning(int[] hl) {
        this.lastAction     = -1;
        this.utility        = new NeuralNetworkUtitlities(hl);
        this.target         = NeuralNetworkUtitlities.createNeuralNetwork();
        this.prediction     = NeuralNetworkUtitlities.createNeuralNetwork();
        this.currIteration  = 0;
        this.replayBuffer   = new ReplayBuffer();
        //this.replayBuffer           = new ArrayList<>();
    }

    /**
     * getAction() - get the action to take given the state
     * @param state - the current state of the intersections
     * @param numStationaryCars - the current global number of moving cars
     * @return the action
     */
    public int getAction(double[] state, int numStationaryCars) {
        this.lastAction = this.currentAction;
        this.lastState = this.currState;
        this.currState = state;
        this.currentAction = prediction.maxA(this.currState); 
        if (lastState != null) {
            //double difference = Math.abs(this.prediction.Q(lastState,lastAction)-this.target.maxQ(currState));
            //double difference = Math.abs(Reward(lastState, lastAction, currState));
            double TDError = Reward(lastState, lastAction, currState) + (discount * this.target.maxQ(currState))-this.prediction.Q(lastState, lastAction);
            storeTransitionAndLearn(new Transition(lastState, lastAction, Reward(lastState, lastAction, currState), currState, TDError));
            //this.replayBuffer.add(new Transition(lastState, lastAction, Reward(lastState,lastAction,currState), currState));
            NeuralNetworkUtitlities.saveModelState(prediction);
            //NeuralNetworkUtitlities.emptyFile();
        }
        if (numStationaryCars < 400) {
            ++currIteration;
            if (currIteration >= totalIterations) {
                currIteration   = 0;
                target          = NeuralNetworkUtitlities.restoreNeuralNetwork();
                prediction.incrementEpoch();
            }
            return this.currentAction;
        } else {
            this.replayBuffer.reset();
            this.prediction.Backpropagate(0, this.lastAction, Reward(this.lastState, this.lastAction, this.currState));
            return -1;
        } 
    }

    /**
     * storeTransition() - stores a transition and samples replay buffer
     * @param t - the transition to store
     */
    public void storeTransitionAndLearn(Transition t) {
        replayBuffer.enqueue(t);
        int i = 0;
        while (i < minibatchSize && i < replayBuffer.occupancy/* && i < numMemories*/) {
            int memoryPosition = this.random.nextInt((i) + 1 - (0)) + 0;
            Transition memory = this.replayBuffer.getTransition(memoryPosition);
            this.prediction.Backpropagate(this.prediction.Q(memory.state_before, memory.action), memory.action, Loss(memory.state_before, memory.action, memory.state_after));
            ++i;
        }
    }

    /**
     * Reward() - calculate the reward for the action
     * @param s_before
     * @param a
     * @param s_after
     * @return a double holding the reward
     */
    private static double Reward(double[] s_before, int a, double[] s_after) {
        double incNumMoving     = 0;
        double incNumStationary = 0;
        for (int i = 0; i < NeuralNetworkUtitlities.numIntersections; i++) {
            double incStationaryX   = s_after[(i * NeuralNetworkUtitlities.numNumbersData) + 0] - s_before[(i * NeuralNetworkUtitlities.numNumbersData) + 0];
            double incStationaryY   = s_after[(i * NeuralNetworkUtitlities.numNumbersData) + 1] - s_before[(i * NeuralNetworkUtitlities.numNumbersData) + 1];
            double incMovingX       = s_after[(i * NeuralNetworkUtitlities.numNumbersData) + 2] - s_before[(i * NeuralNetworkUtitlities.numNumbersData) + 2];
            double incMovingY       = s_after[(i * NeuralNetworkUtitlities.numNumbersData) + 3] - s_before[(i * NeuralNetworkUtitlities.numNumbersData) + 3];
            double period           = s_after[(i * NeuralNetworkUtitlities.numNumbersData) + 5];
            incNumStationary        += 
                    ((period+1)*(incStationaryX+incStationaryY));
            incNumMoving            += 
                    ((16-period+1)*(incMovingX+incMovingY)); 
        } 
        
        return incNumMoving - incNumStationary;
    }

    /**
     * Loss() - Calculate the loss.
     * @return the loss
     */
    private double Loss(double[] s_before, int a, double[] s_after) {
        double temporalDifference = Reward(s_before, a, s_after) + (discount * this.target.maxQ(s_after));
        double TDerror = temporalDifference - this.prediction.Q(s_before, a);
        double behaviourDistribution = 0;
        /*return behaviourDistribution*Math.pow(TDerror,2);*/
        return Math.pow(TDerror, 2);
    }

    /**
     * print() - prints the neural network in it's current state.
     */
    public void print() {
        if (target != null) {
            System.out.println("TARGET");
            System.out.println(target.toString());
        }
        if (prediction != null) {
            System.out.println("PREDICTION");
            System.out.println(prediction.toString());
        }
    }

}
