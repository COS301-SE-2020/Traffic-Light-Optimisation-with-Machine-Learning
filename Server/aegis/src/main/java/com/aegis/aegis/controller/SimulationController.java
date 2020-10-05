package com.aegis.aegis.controller;
 
import com.aegis.aegis.service.IntersectionService;
import dto.completeDto;
import dto.intersectionDto;
import dto.intersectionsDto;
import dto.statisticDto;
import exception.BadGatewayException;
import java.util.List; 
import machineLearning.ReinforcementLearning;
import machineLearning.NeuralNetworkUtitlities;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@CrossOrigin(origins = "*", allowedHeaders = "*") 
@RestController
@RequestMapping("/simu")
public class SimulationController { 
    private ReinforcementLearning rl = new ReinforcementLearning(new int[]{300,300,300,300});
    
    @Autowired
    private IntersectionService intersectionService;
    
     /**
    * getIntersections() - returns data from the intersection (X direction).
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @GetMapping("/getIntersections")
    public List<intersectionDto> getIntersections(){
        return intersectionService.getIntersections();
    } 
    
    /**
    * getIntersections2() - returns data from the intersection (Z direction).
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @GetMapping("/getIntersections2")
    public intersectionsDto getIntersections2(){
        return intersectionService.getIntersections2();
    } 
    
    /**
    * addStatistic() - adds statistics from the intersection data.
    * @param statistic
    */
    @PostMapping("/addStatistic")
    public List<intersectionDto> addStatistic(@RequestBody statisticDto statistic){
        intersectionService.addStatistic(statistic);
        return this.getIntersections();
    }
    
    /**
    * addStat() - adds statistics from the intersection data.
    * @param statistic
    */
    public void addStat(statisticDto statistic){
        intersectionService.addStatistic(statistic);
    }
    
    /**
    * addStat2() - adds statistics from the intersection data.
    * @param statistic
    */
    public void addStat2(statisticDto statistic){
        intersectionService.addStatistic2(statistic);
    }
    
    /**
    * addstatistics() - adds statistics from the simulation AI.
    * @param complete
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*")
    @PostMapping("/addStatistics")
    public String addstatistics(@RequestBody completeDto complete){
        try{
            System.out.println("adding statistic...");
            double [] state = new double[NeuralNetworkUtitlities.numIntersections*NeuralNetworkUtitlities.numNumbersData];
            statisticDto[] stats = complete.getStatistics();
            for (int i = 0; i < NeuralNetworkUtitlities.numIntersections; i++) {
                addStat(stats[i]);
                //addStat2(stats[i]);
                state[(i*NeuralNetworkUtitlities.numNumbersData)+0] = stats[i].getStationaryX();
                state[(i*NeuralNetworkUtitlities.numNumbersData)+1] = stats[i].getStationaryY();
                state[(i*NeuralNetworkUtitlities.numNumbersData)+2] = stats[i].getMovingX();
                state[(i*NeuralNetworkUtitlities.numNumbersData)+3] = stats[i].getMovingY();
                state[(i*NeuralNetworkUtitlities.numNumbersData)+4] = stats[i].getPhase();
                state[(i*NeuralNetworkUtitlities.numNumbersData)+5] = stats[i].getPeriod();
                
            } 
            int action = rl.getAction(state,complete.getNumStationaryCars());
            if(action == -1){
                intersectionService.addGeneration();
            }
            return action+"";
        }catch(Exception e){
            throw new BadGatewayException(e.getMessage(),"ERROR");
        }
        
    }
    
    /**
    * addstatistics2() - adds statistics from the simulation AI.
    * @param complete
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*")
    @PostMapping("/addStatistics2")
    public void addstatistics2(@RequestBody completeDto complete){
        try{
            System.out.println("adding statistic 2...");
            statisticDto[] stats = complete.getStatistics();
            for (int i = 0; i < NeuralNetworkUtitlities.numIntersections; i++) {
                addStat2(stats[i]);
            } 
        }catch(Exception e){
            throw new BadGatewayException(e.getMessage(),"ERROR");
        }

    }
    
    @PostMapping("/resetModel")
    private void resetModel(){
        NeuralNetworkUtitlities.deleteModel();
        rl = new ReinforcementLearning(new int[]{300,300,300,300});
    }
    
    @PostMapping("/print")
    private void print(){
        rl.prediction.Print();
    }
    
}
