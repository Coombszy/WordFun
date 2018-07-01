using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFun
{
    class MultiplayerHandler
    {
        public GameEngine Game;
        public ImageHandler GameImageHandler;
        public MultiplayerHandler(MultiplayerGameScreen GameScreen)
        {
            Game = new GameEngine();
            GameImageHandler = new ImageHandler(GameScreen);
            BuildAllTiles(GameScreen, GameImageHandler);
            UpdateBoardTiles(Game, GameImageHandler);
        }
        private void BuildAllTiles(MultiplayerGameScreen GameScreen, ImageHandler GameImageHandler)
        {
            int TileCounter = 0;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    GameImageHandler.BuildNewTile("-");//creates a new tile
                    GameImageHandler.MoveATile(TileCounter, 10 + i * 40, 10 + j * 40);//moves the tile to its position on the board
                    TileCounter++;//increase the tile counter so that the next tile to be created can be referenced and translated
                }//loop for each row
            }//loop for each col
        }//builds all the tiles to create the game board
        public void UpdateBoardTiles(GameEngine Game, ImageHandler GameImageHandler)
        {
            int TileCounter = 0;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (Game.Board[i, j] != "-")
                    {
                        GameImageHandler.ActiveImages[TileCounter].Update(Game.Board[i, j]);
                    }
                    TileCounter++;//So that the next tile can bre referenced correctly
                }//loop for each row
            }//loop for each col
        }
    }
}