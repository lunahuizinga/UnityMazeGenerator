// This interface is meant to be implemented by a class dedicated to a certain maze generation algorithm
public abstract class MazeAlgorithm{
    
    // We take the size of the maze to generate and return an array of MazeCells, representing a maze layout
    public abstract MazeCell[,] Generate(int sizeX, int sizeY);
}