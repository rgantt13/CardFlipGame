using System;
using System.Collections.Generic;

namespace CardFlipGame
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Card Flip Hell!");
            Console.WriteLine("\r\n");
            Console.WriteLine("Please submit a game string. This must be a set of 0s and 1s.");
            string input = Console.ReadLine();
            char[] board = input.ToCharArray();
            Console.WriteLine("\r\n");
            Console.WriteLine(playGame(board, new List<int>(), 0));

        }


        //Determines if game is won (all .s)
        public static bool isWon(char[] game)
        {
            bool gameWon = true;
            foreach (char node in game)
            {
                if (node != '.')
                {
                    gameWon = false;
                }
            }

            return gameWon;
        }

        // Determines if game is lost (zero island is found)
        public static bool isLost(char[] game)
        {
            bool okIsland = false;

            for (int i = 0; i < game.Length; i++)
            {
                if (game[i] == '.')
                {
                    //Approve sets with . at beginning of board and empty islands
                    if (game[0] == '.')
                    {
                        okIsland = true;
                    }
                    else if (game[i - 1] == '.')
                    {
                        okIsland = true;
                    }

                    //Either lose, or reset okIsland for new subset
                    if (!okIsland)
                    {
                        return true;
                    }
                    else
                    {
                        okIsland = false;
                    }
                }
                else if (game[i] == '1')
                {
                    okIsland = true;
                }
                else if (i == (game.Length - 1) && game[i] == '0')
                {
                    if (!okIsland)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public static char[] removePiece(char[] game, int pieceLocation)
        {
            if (pieceLocation == 0)
            {
                game[pieceLocation] = '.';
                game[pieceLocation + 1] = flip(game[pieceLocation + 1]);
            }
            else if (pieceLocation == game.Length - 1)
            {
                game[pieceLocation] = '.';
                game[pieceLocation - 1] = flip(game[pieceLocation - 1]);

            }
            else
            {
                game[pieceLocation] = '.';
                game[pieceLocation + 1] = flip(game[pieceLocation + 1]);
                game[pieceLocation - 1] = flip(game[pieceLocation - 1]);
            }
            return game;
        }

        public static char[] returnPiece(char[] game, int pieceLocation)
        {
            if (pieceLocation == 0)
            {
                game[pieceLocation] = '1';
                game[pieceLocation + 1] = flip(game[pieceLocation + 1]);
            }
            else if (pieceLocation == game.Length - 1)
            {
                game[pieceLocation] = '1';
                game[pieceLocation - 1] = flip(game[pieceLocation - 1]);

            }
            else
            {
                game[pieceLocation] = '1';
                game[pieceLocation + 1] = flip(game[pieceLocation + 1]);
                game[pieceLocation - 1] = flip(game[pieceLocation - 1]);
            }
            return game;
        }

        public static char flip(char piece)
        {
            if (piece == '0')
            {
                piece = '1';
            }
            else if (piece == '1')
            {
                piece = '0';
            }

            return piece;
        }

        public static string playGame(char[] gameBoard, List<int> movesMade,  int currentIndex)
        {
                //Winning Case
                if (isWon(gameBoard))
                {
                    return string.Join(", ", movesMade);
                }
                //Game is Playable
                else if (!isLost(gameBoard))
                {
                    //Location at a removable piece
                    if (gameBoard[currentIndex] == '1')
                    {
                        //Remove piece and descend
                        movesMade.Add(currentIndex);
                        return playGame(removePiece(gameBoard, currentIndex), movesMade, 0);
                    }
                    //Location at a unremoveable piece
                    else
                    {
                    int futureLocation = currentIndex + 1;
                        //Location at end of board
                        if (futureLocation == gameBoard.Length)
                        {
                            //And at the beginning board
                            if (movesMade.Count == 0)
                            {
                                //YOU LOSE
                                return "No Solution";
                            }
                            //Go up one level in tree
                            else
                            {
                            int wrongMoveLocation = movesMade[movesMade.Count - 1];
                            movesMade.RemoveAt(movesMade.Count - 1);

                            //Location at end of previous board
                            if ((wrongMoveLocation + 1) == gameBoard.Length)
                            {
                                //And at the beginning set
                                if (movesMade.Count == 0)
                                {
                                    //YOU LOSE
                                    return "No Solution";
                                }
                                //Go up two levels
                                else
                                {
                                    int wrongMoveLocation2 = movesMade[movesMade.Count - 1];
                                    movesMade.RemoveAt(movesMade.Count - 1);
                                    return playGame(returnPiece(gameBoard, wrongMoveLocation2), movesMade, wrongMoveLocation2 + 1);
                                }
                            }
                            // Go up one level in tree
                            return playGame(returnPiece(gameBoard, wrongMoveLocation), movesMade, wrongMoveLocation + 1);
                        }
                            
                        }
                        
                        //Look to the next piece
                        return playGame(gameBoard, movesMade, futureLocation);
                    }

                }
                //Game was lost
                else
                {
                    int wrongMoveLocation = movesMade[movesMade.Count - 1];
                    movesMade.RemoveAt(movesMade.Count - 1);

                    //Location at end of previous board
                    if ((wrongMoveLocation + 1) == gameBoard.Length)
                    {
                        //And at the beginning set
                        if (movesMade.Count == 0)
                        {
                            //YOU LOSE
                            return "No Solution";
                        }
                        //Go up two levels
                        else
                        {
                            int wrongMoveLocation2 = movesMade[movesMade.Count - 1];
                            movesMade.RemoveAt(movesMade.Count - 1);
                            return playGame(returnPiece(gameBoard, wrongMoveLocation2), movesMade, wrongMoveLocation2 + 1);
                        }
                    }
                    // Go up one level in tree
                    return playGame(returnPiece(gameBoard, wrongMoveLocation), movesMade, wrongMoveLocation + 1);
                }
            
        }


    }
}
