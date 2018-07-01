using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordFun
{
    public partial class MenuScreen : Form
    {
        /// <summary>
        /// On form creation this constructor sets the next menu in program to 99, this
        /// menu will end the program.
        /// </summary>
        public MenuScreen()
        {
            Program.NextMenuID = 99;//Sets next menu to 99
            InitializeComponent();//Starts the screen
        }
        /// <summary>
        /// This event function loads the images for the logo on the menu screen and 
        /// configures them, Size and picture box image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuScreen_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox1.ImageLocation = @"Data\W.png";//Sets the image of the Picture Box
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox2.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox2.ImageLocation = @"Data\O.png";//Sets the image of the Picture Box
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox3.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox3.ImageLocation = @"Data\R.png";//Sets the image of the Picture Box
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox4.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox4.ImageLocation = @"Data\D.png";//Sets the image of the Picture Box
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox5.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox5.ImageLocation = @"Data\-.png";//Sets the image of the Picture Box
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox6.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox6.ImageLocation = @"Data\F.png";//Sets the image of the Picture Box
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox7.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox7.ImageLocation = @"Data\U.png";//Sets the image of the Picture Box
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box

            pictureBox8.Size = new System.Drawing.Size(75, 75);//Sets the Picture box Dimensions
            pictureBox8.ImageLocation = @"Data\N.png";//Sets the image of the Picture Box
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;//Changes the image Size mode to Stretch, make sure it resizes to the dimensions of the box
        }
        /// <summary>
        /// This event function sets the next menu to menu 1 and then closes this form, this is so the next form is menu 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Program.NextMenuID = 1;//Sets next menu to 1
            this.Close();//closes this screen
        }
        /// <summary>
        /// This event function sets the next menu to menu 2 and then closes this form, this is so the next form is menu 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Program.NextMenuID = 2;//Sets next menu to 2
            this.Close();//closes this screen
        }
        /// <summary>
        /// This event function sets the next menu to menu 99 and then closes this form, this is so the program will end on 
        /// menu 99.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Program.NextMenuID = 99;//Sets next menu to 3
            this.Close();//closes this screen
        }
    }
}
