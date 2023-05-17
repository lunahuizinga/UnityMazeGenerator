// The class representing a single cell of the maze
public class MazeCell{
    public MazeCellSide[] CellSides;
    
    // Define the amount of sides we want to our cell
    private const int SideAmount = 4;

    public MazeCell(){
        // Initialise the array of MazeCellSides
        CellSides = new MazeCellSide[SideAmount];
        
        // Initialise every MazeCellSide in the array
        for (int i = 0; i < CellSides.Length; i++){
            CellSides[i] = new MazeCellSide();
        }
    }

    // Overload the constructor in case we already have established 
    public MazeCell(MazeCellSide[] cellSides){
        CellSides = cellSides;
    }
}