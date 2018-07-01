using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WordFun
{
    class ImageHandler
    {
        private Form MyGameScreen = new Form();//Allows this class store a reference to the Active game screen attached to this game handler
        public Tile[] ActiveImages = new Tile[250];//Stores the Pictures in a list so that they can be referenced
        public static Tile HighlightTile = new Tile("_");//This creates an instance of the tile class with the MyLetter value of "_"
        /// <summary>
        /// This constructor creates the instance of the object and parses a reference to
        /// the gamescreen it will be manipulating
        /// </summary>
        /// <param name="GameScreen"></param>
        public ImageHandler(Form GameScreen)
        {
            MyGameScreen = GameScreen;//Sets internal form variable (MyGameScreen) to the parameter of the constructor 
        }
        /// <summary>
        /// This Creates a new instance of the tile object and stores it in the active images
        /// array at the next available slot, it then adds the Picturebox created in the tile 
        /// object to the MyGameScreen form.
        /// </summary>
        /// <param name="MyLetter"></param>
        public void BuildNewTile(string MyLetter)
        {
            Tile NewTile = new Tile(MyLetter);//Creates a new tile object and parses it a tile to display
            int i = 0;//defines and sets the counter i to 0
            while(ActiveImages[i] != null && (i <= 250))
            {
                i++;//increase i by 1
            }//while elements in the array are not null and i is less thatn 250 it will keep counting to find the next free element
            ActiveImages[i] = NewTile;//stores the new tile object at location i
            MyGameScreen.Controls.Add(NewTile.MyPictureBox);//Adds the picturebox created by the tile object to the form
        }
        /// <summary>
        /// This builds a single highlight tile and adds a reference to it to the 
        /// HighlightTile varaible. It then adds it to the target active form.
        /// </summary>
        public void BuildHighlightTile()
        {
            Tile NewTile = new Tile("_");//creates a new tile object and parses it a tile to display ("_")
            HighlightTile = NewTile;//Stores the tile object in the variable HighlightTile
            MyGameScreen.Controls.Add(HighlightTile.MyPictureBox);//Adds the picturebox created by the tile object to the form
        }
        /// <summary>
        /// This function allows me to move a specific tile to a specific set of coordinates
        /// using the Translate method within the tiles
        /// </summary>
        /// <param name="TargetTile"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveATile(int TargetTile, int x, int y)
        {
            Tile TileToMove = ActiveImages[TargetTile];//gets a reference to the tile to be moved using the parameters
            TileToMove.Translate(x, y);//sets the tile to the coords from the parameters
        }
        /// <summary>
        /// This function converts the target coordinates on the game board into coords
        ///  (from parameters) in the form, this then moves the highlight tile to the coords
        ///  created previously.
        /// </summary>
        /// <param name="TargetX"></param>
        /// <param name="TargetY"></param>
        public static void HighlightATile(int TargetX, int TargetY)
        {
            if (TargetX > 14 || TargetY > 14)
            {
                HighlightTile.Translate(-100,-100 );//translates the tile to (-100,-100)
            }//if the coords are not in the gameboard, it moves the tile into a "hiding" location
            else
            {
                int CoordX = (TargetX * 40) + 17;//Converts the x board coordinates into x coords on the form
                int CoordY = (TargetY * 40) + 17;//Converts the y board coordinates into y coords on the form
                HighlightTile.Translate(CoordX, CoordY);//translates the highlight tile to the coordinates caculated
            }//if the location is on the ame board
        }
    }
}
