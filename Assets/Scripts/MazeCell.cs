// The class representing a single cell of the maze
public class MazeCell{
    public MazeCellSide[] CellSides;
    
    // Define the amount of sides we want for our cell
    private const int SideAmount = 4;

    public MazeCell(){
        // Initialise the array of MazeCellSides
        CellSides = new MazeCellSide[SideAmount];
        
        // Initialise every MazeCellSide in the array
        for (int i = 0; i < CellSides.Length; i++){
            CellSides[i] = new MazeCellSide();
        }
    }

    // Overload the constructor to add the option of providing an existing array of MazeCellSides
    public MazeCell(MazeCellSide[] cellSides){
        CellSides = cellSides;
    }
}