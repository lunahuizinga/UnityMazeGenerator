// This abstract class is meant to be implemented by a class dedicated to a certain maze generation algorithm
public abstract class MazeAlgorithm{
    
    // We take the size of the maze to generate and return an array of MazeCells, representing a maze layout
    // The method used is up to the individual implementation of this class
    public abstract MazeCell[,] Generate(int sizeX, int sizeY);
}