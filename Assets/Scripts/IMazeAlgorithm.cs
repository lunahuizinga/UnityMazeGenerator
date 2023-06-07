// This interface is meant to be implemented by a class dedicated to a certain maze generation algorithm
public interface IMazeAlgorithm{
    // We take the size of the maze to generate and return an array of MazeCells, representing a maze layout
    public MazeCell[,] Generate(int sizeX, int sizeY);
}