package machineLearning;
 
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;

public class NeuralNetworkUtitlities {

    //public static int numNumbersData    = 5;
    public static int numNumbersData    = 6;
    public static int numIntersections  = 6;
    public static int numInput;
    public static int numOutput;
    public static int[] hiddenLayers;
    private static long initialPosition;
    private static final String filename = "model.txt";
    private static File file;

    public NeuralNetworkUtitlities(int[] hl) { 
        NeuralNetworkUtitlities.numInput = numIntersections * numNumbersData;
        NeuralNetworkUtitlities.numOutput = (int) Math.pow(2, NeuralNetworkUtitlities.numIntersections) - 1;
        NeuralNetworkUtitlities.hiddenLayers = hl;
    }
 
    /**
     * saveModelState() - Save the model state
     * @param nn - the neural network to save
     */
    public static void saveModelState(NeuralNetwork nn) {
        System.out.print("SAVING MODEL...");
        if (nn != null) {
            try {
                File f = new File(filename);
                f.createNewFile();
                FileWriter writer = new FileWriter(filename);
                writer.write(nn.toString());
                writer.close();
                System.out.print("SUCCESS\n");
            } catch (IOException e) {
                System.out.print("FAIL\n");
            }
        } else {
            System.out.print("NULL!\n");
        }
    }

    /**
     * createNeuralNetwork()
     * @param target - boolean indicating whether or not the neural network is the target neural network
     * @return the new Neural Network
     */
    public static NeuralNetwork createNeuralNetwork() {
        NeuralNetwork nn;
        if (fileExists(filename)) {
            nn = restoreNeuralNetwork();
            saveModelState(nn);
        } else {
            System.out.print("CREATING NEW...");
            nn = new NeuralNetwork(numInput, numOutput, hiddenLayers);
            saveModelState(nn);
            System.out.print("SUCCESS!\n");
            
        }
        return nn;
    }

    /**
     * @param target
     * @return a NeuralNetwork restored from a file
     */
    public static NeuralNetwork restoreNeuralNetwork() {
        System.out.print("RESTORING ");
        NeuralNetwork nn = new NeuralNetwork(numInput, numOutput, hiddenLayers);
        try {
            File file           = new File(filename);
            BufferedReader br   = new BufferedReader(new FileReader(file));
            String line         = "";
            Scanner sc;
            line                = br.readLine();
            int numInput        = Integer.parseInt(line.substring(line.indexOf(":") + 1).trim());
            line                = br.readLine();
            int numOutput       = Integer.parseInt(line.substring(line.indexOf(":") + 1).trim());
            line                = br.readLine();
            double learningRate = Double.parseDouble(line.substring(line.indexOf(":") + 1).trim());
            line                = br.readLine();
            String[] lines      = line.substring(line.indexOf("[") + 1, line.indexOf("]")).split(",");
            int[] hiddenLayers  = new int[lines.length];
            for (int i = 0; i < lines.length; i++) {
                hiddenLayers[i] = Integer.parseInt(lines[i].trim());
            }
            ArrayList<Double>[] weights = new ArrayList[nn.getNumberLayers() - 1];
            ArrayList<Double>[] biases = new ArrayList[nn.getNumberLayers() - 1];
            line = br.readLine();
            lines = line.substring(line.indexOf(":") + 1).split("#");
            String numbers[];
            ArrayList<Double> weightArr;
            for (int i = 0; i < nn.getNumberLayers() - 1; i++) {
                numbers = lines[i].substring(lines[i].indexOf("[") + 1, lines[i].indexOf("]")).split(",");
                weightArr = new ArrayList<>();
                for (String number : numbers) {
                    weightArr.add(Double.parseDouble(number));
                }
                weights[i] = weightArr;
            }
            line = br.readLine();
            lines = line.substring(line.indexOf(":") + 1).split("#");
            ArrayList<Double> biasArr;
            for (int i = 0; i < nn.getNumberLayers() - 1; i++) {
                numbers = lines[i].substring(lines[i].indexOf("[") + 1, lines[i].indexOf("]")).split(",");
                biasArr = new ArrayList<>();
                for (String number : numbers) {
                    biasArr.add(Double.parseDouble(number));
                }
                biases[i] = biasArr;
            }
            nn.setBiases(biases);
            nn.setWeights(weights);
            br.close();
        } catch (IOException | NumberFormatException e) {

        }
        return nn;
    }

    /**
     * fileExists()
     * @param filename - the file to check the existence of.
     * @return a boolean whether the file exists or not
     */
    private static boolean fileExists(String filename) {
        try {
            File f = new File(filename);
            return f.exists();
        } catch (Exception e) {
        }
        return false;
    }

    /**
     * deleteModel() - Delete the model
     */
    public static void deleteModel() {
        System.out.print("DELETING ");
        try {
            File f = new File(filename);
            if (f.exists()) {
                f.delete();
            }
        } catch (Exception e) {
        }
    }
}
