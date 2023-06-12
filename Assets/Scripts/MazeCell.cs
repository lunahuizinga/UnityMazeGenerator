// The class representing a single cell of the maze
public class MazeCell{
    public MazeCellSide[] CellSides;
    public readonly int X;
    public readonly int Y;
    
    // Define the amount of sides we want for our cell
    private const int SideAmount = 4;

    // In case no cellSides array was provided to the constructor, we call GenerateDefaultCellSides,
    // fill it with new instances, and pass the parameters on to the other constructor
    public MazeCell(int x, int y) : this(x, y, GenerateDefaultCellSides()){}

    // Overload the constructor to add the option of providing an existing array of MazeCellSides
    public MazeCell(int x, int y, MazeCellSide[] cellSides){
        // Copy the provided x and y values into the corresponding variables to be used later
        X = x;
        Y = y;
        
        CellSides = cellSides;
    }
    
    // Create a new MazeCellSide array and fill it with new instances of MazeCellSide
    private static MazeCellSide[] GenerateDefaultCellSides(){
        // Initialise the array of MazeCellSides
        MazeCellSide[] cellSides = new MazeCellSide[SideAmount];
        
        // Initialise every MazeCellSide in the array
        for (int i = 0; i < cellSides.Length; i++){
            cellSides[i] = new MazeCellSide();
        }
        
        // Return the newly initialised array
        return cellSides;
    }
}