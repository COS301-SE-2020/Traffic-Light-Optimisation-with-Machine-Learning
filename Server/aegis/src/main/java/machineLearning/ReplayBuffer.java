 
package machineLearning; 
public class ReplayBuffer {
    private long oldest = 0;
    private final static int maxSize = 500;
    public int occupancy;
    private final Transition head;
    private final Transition[] buffer;
    /**
    * replayBuffer() - instantiates the replay buffer.
    */
    public ReplayBuffer() {
        occupancy   = 0;
        head        = null;
        buffer      = new Transition[maxSize];
    } 
    
    /**
    * getTransition() - gets a transition tuple.
    * @return buffer
    */
    public Transition getTransition(int index){
        /*if(occupancy == 0 || index < 0){
           index = 0;
        }else if(index > occupancy){
            index = occupancy -1;
        }*/
        return this.buffer[index];
    }
    
    /**
    * reset - resets the replays buffer's matrix values to 0.
    */
    public void reset(){
        occupancy = 0;
        for (int i = 0; i < maxSize; i++) {
            buffer[i] = null;
        }
    }
    
    /**
    * enqueue() - adds a transition tuple to the buffer.
    * @param Transition
    */
    public void enqueue(Transition t) {
        if (occupancy == 0) {
            oldest = t.id; 
            buffer[0] = t;
            ++occupancy;
        } else {
            while (occupancy >= maxSize) {
                dequeue();
            }
            int position = 0; 
            while (position < occupancy && buffer[position].difference > t.difference) { 
                ++position;
            } 
            for (int i = maxSize - 1; i > position; i--) {
                buffer[i] = buffer[i - 1];
            }
            buffer[position] = t;  
            ++occupancy;
        }
    }

    /**
    * dequeue() - removes a transition tuple from the buffer.
    */
    public void dequeue() {
        if (occupancy > 0) {
            int position = 0;
            Transition curr = head;
            Transition prev = null;
            while (position < occupancy  && buffer[position].id != oldest) {
                ++position;
            }
            if (position < occupancy) {
                /*is the head*/
                for (int i = position; i < maxSize - 1; i++) {
                    buffer[i] = buffer[i + 1];
                }
                buffer[maxSize-1] = null;
                --occupancy;
                ++oldest;
            } 
        }
    }

}
