// This abstract class is meant to be implemented by a class dedicated to a certain maze generation algorithm
public abstract class MazeAlgorithm{
    
    protected MazeCell[,] MazeCells;
    
    // We take the size of the maze to generate and return an array of MazeCells, representing a maze layout
    // The method used is up to the individual implementation of this class
    public abstract MazeCell[,] Generate(int sizeX, int sizeY);

    protected virtual void Initialise(int sizeX, int sizeY){
        // Initialise the two dimensional cell array with the dimensions set by the user
        MazeCells = new MazeCell[sizeX, sizeY];
        
        // Loop through the array and initialise every cell in the array
        for (int x = 0; x < MazeCells.GetLength(0); x++)
        for (int y = 0; y < MazeCells.GetLength(1); y++){
            MazeCells[x, y] = new MazeCell();
        }
    }
}