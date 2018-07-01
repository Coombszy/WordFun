using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WordFun
{
    class Tile
    {
        public PictureBox MyPictureBox = new PictureBox();//Creates the Picture box of this class
        public string MyLetter;
        /// <summary>
        /// This constructor creates the picture box for this tile apon initiation, it also
        /// configures the picture box settings, like Size, image, Sizemode an location.
        /// The Image used is dependant on the MyLetter variable being parsed to the internal
        /// variable (from parameter Letter). It also adds a Click event to the tile so that
        /// its coordinates can be parsed to the game handler as a target location.
        /// </summary>
        /// <param name="Letter"></param>
        public Tile(string Letter)
        {
            MyLetter = Letter;//Sets value of this Tiles (class) Letter
            MyPictureBox.Location = new Point(-50, -50);//Sets the picture box location, Of screen on creation
            MyPictureBox.Size = new System.Drawing.Size(35, 35);//Sets the Picture box Dimensions
            MyPictureBox.ImageLocation = @"Data\"+MyLetter+".png";//Sets the image of the Picture Box
            MyPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box
            //MyPictureBox.Refresh();//Refreshes the config --Disabled to Improve Performance
            MyPictureBox.Click += new EventHandler(Tile_Click);
            if (MyLetter == "_")
            {
                MyPictureBox.Size = new System.Drawing.Size(42, 42);//Set the size of the tile to slightly larger than normal tiles
            }//if the tiles MyLetter = "_"
        }
        /// <summary>
        /// This function is used to move the position of the picture box for this tile, it 
        /// does this using the Location method (for PicureBox class) to set it to a new
        /// point being made with the parameters x and y. It then refreshes the picturebox
        /// to make sure the config is updated.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Translate(int x, int y)
        {
            MyPictureBox.Location = new Point(x,y);//Sets the new location
            MyPictureBox.Refresh();//Refreshes the PictureBox
        }
        /// <summary>
        /// This function is used to update a Pictureboxes image and change its MyLetter
        /// value to the Parameter being parsed. It also checks if the tile is being changed
        /// to a Highlight tile, and increases the picture boxes size. It then refreshes
        /// the picturebox to make sure the config is updated.
        /// </summary>
        /// <param name="Letter"></param>
        public void Update(string Letter)
        {
            MyLetter = Letter;//Sets value of this Tiles (class) Letter
            MyPictureBox.ImageLocation = @"Data\" + MyLetter + ".png";//Sets the image of the Picture Box
            if (MyLetter == "_")
            {
                MyPictureBox.Size = new System.Drawing.Size(42, 42);//changes the pictureboxes dimensions to 42 by 42
            }
            MyPictureBox.Refresh();//Refreshes the config
        }//Allows the Tile to be rebuilt with a new Letter
        /// <summary>
        /// this function is used to destory the picture box created on the form, and 
        /// destroys the instance of this class. it destroys the picturebox using the
        /// method .Dispose(). 
        /// </summary>
        public void Destroy()
        {
            MyPictureBox.Dispose();//removes and deletes the picturebox from the from
            this.Destroy();
        }
        /// <summary>
        /// This function is used for the click event, it executes this function within this
        /// class and parses the coordinates of the Pictureboxs' location to the
        /// BreakCoordinatesAndParse function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_Click (object sender, EventArgs e)
        {
            BreakCoordinatesAndParse(MyPictureBox.Location);//parses the Pictureboxs' location (Coordinates) to the BreakCoordinatesAndParse function
        }
        /// <summary>
        /// This function converts the form coordinates of the picturebox location into
        /// board coordinate locations, this is so the target location could be parsed
        /// to the game engine later on for making moves.
        /// </summary>
        /// <param name="TargetLocation"></param>
        public void BreakCoordinatesAndParse(Point TargetLocation)
        {
            int CoordX = TargetLocation.X;//stores the target location.X as int CoordX
            int CoordY = TargetLocation.Y;//stores the target location.Y as int CoordY
            GameScreen.UpdateCoords((CoordX - 20) / 40, (CoordY - 20) / 40);//Parses the edited coordinates to the GameScreen
        }
    }
}
