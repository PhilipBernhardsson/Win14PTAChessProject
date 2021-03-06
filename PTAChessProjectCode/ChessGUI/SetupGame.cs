﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTAChessProjectCode;

namespace ChessGUI
{
    class SetupGame
    {

        PrintGUI Printer;
        GameEngine engine;
        RunGame newGame;

        public void SetupNewGame()
        {
            while (true)
            {
             
            InitiateNewGame();
            PrintGameBoard();
            newGame.StartGame();

            }
        }

        private void InitiateNewGame()
        {
            Logger.CreateCleanLog();
            engine = new GameEngine();
            Printer = new PrintGUI();
            newGame = new RunGame(Printer, engine);
            engine.InitiateGame();
        }

        private void PrintGameBoard()
        {
            Printer.PrintBoard(engine.countMoves);
            Console.ForegroundColor = ConsoleColor.White;
            Printer.PrintPieceOnBoard(engine.AIWhiteComp.PieceList);
            Console.ForegroundColor = ConsoleColor.Black;
            Printer.PrintPieceOnBoard(engine.AIBlackComp.PieceList);
            Console.ResetColor();
        }

        

    }  
}
