﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTAChessProjectCode
{
    /// <summary>
    /// This class contains the AI logic regarding moves.  
    /// </summary>
    public class AIMoveData
    {
        private PlayerPieces AIToMove { get; set; }
        private PlayerPieces AINotToMove { get; set; }
        public List<ChessPiece> PieceThatCanMove { get; set; }
        public List<ChessPiece> PieceThatCanKill { get; set; }
        public List<ChessPiece> EnemyPiecePositions { get; set; }
        public List<string> AllMoves { get; set; }

        //Constructor generating white and black pieces, also instantiates list of all possible moves, all allowed moves and all pieces that can kill an opponent. 
        public AIMoveData(PlayerPieces MyPieces, PlayerPieces EnemyPieces)
        {
            this.AIToMove = MyPieces;
            this.AINotToMove = EnemyPieces;
            this.EnemyPiecePositions = EnemyPieces.PieceList;
            AllMoves = new List<string>();
            PieceThatCanMove = new List<ChessPiece>();
            PieceThatCanKill = new List<ChessPiece>();
            
        }

        //Sets up the move of the current player. 
        public List<ChessPiece> MakeMove(PlayerPieces playerToMove, List<ChessPiece> enemyList)
        {
            this.AIToMove = playerToMove;
            this.EnemyPiecePositions = enemyList;

            List<ChessPiece> AfterMoveList = AIMakeMove(AIToMove);
            return AfterMoveList;
        }

        //Logic deciding which move to make for current player. 
        public List<ChessPiece> AIMakeMove(PlayerPieces AIToMove)
        {
            List<List<MovementOptions>> AllMovesMyPiecesCanMake = AnalyzeMyPieces(AIToMove.PieceList);

            Logger.AmountOfLegalAnalyzedMoves(AllMovesMyPiecesCanMake.Count);

            if (AllMovesMyPiecesCanMake.Count == 0)
            {
                AIToMove.PieceList.Clear();
                return AIToMove.PieceList;
            }

            List<MovementOptions> PiecesICanKill = FindPiecesICanKill(AllMovesMyPiecesCanMake);

            MovementOptions optimalMovementOption;

            if (PiecesICanKill.Count != 0)
            {
                optimalMovementOption = FindHighestPieceValue(PiecesICanKill);
            }
            else
            {
                int randomNumber = GetRandomNumber(0, AllMovesMyPiecesCanMake.Count);
                int randomPiece = AllMovesMyPiecesCanMake[randomNumber].Count;
                int randomMovementOption = GetRandomNumber(0, randomPiece);

                optimalMovementOption = AllMovesMyPiecesCanMake[randomNumber][randomMovementOption];
            }
            Logger.LogDecidedMove(optimalMovementOption);

            MovePiece(optimalMovementOption, AIToMove.PieceList, EnemyPiecePositions);
            return AIToMove.PieceList;
        }

        //The enemy piece with the highest value can be killed by my piece, which has a lower value than the enemy piece. 
        private MovementOptions FindHighestPieceValue(List<MovementOptions> PiecesICanKill)
        {
            var highestValueFound = 0;
            MovementOptions optimalMovementOption = null;
            foreach (var movementOption in PiecesICanKill)
            {
                if ((highestValueFound < movementOption.EnemyPiece.Value) || ((highestValueFound == movementOption.EnemyPiece.Value) && (optimalMovementOption.MyPiece.Value > movementOption.MyPiece.Value)))
                {
                    highestValueFound = movementOption.EnemyPiece.Value;
                    optimalMovementOption = movementOption;
                }
            }
            return optimalMovementOption;
        }
        //Finds all the possible enemy pieces that the current players piece can kill and adds them to a list. 
        private List<MovementOptions> FindPiecesICanKill(List<List<MovementOptions>> AllMovesMyPiecesCanMake)
        {
            List<MovementOptions> PiecesICanKill = new List<MovementOptions>();

            foreach (var movementList in AllMovesMyPiecesCanMake)
            {
                foreach (var movementOption in movementList)
                {
                    if (movementOption.CheckForEnemyResult == 1)
                    {

                        PiecesICanKill.Add(AddEnemyPieceToMovementOption(movementOption));
                    }
                }
            }
            return PiecesICanKill;
        }
        //Checks where the enemy piece can go. 
        private MovementOptions AddEnemyPieceToMovementOption(MovementOptions movementOption)
        {
            foreach (var piece in EnemyPiecePositions)
            {
                if ((movementOption.PositionX == piece.PositionX) && (movementOption.PositionY == piece.PositionY))
                {
                    movementOption.EnemyPiece = piece;
                    return movementOption;
                }
            }
            return movementOption;
        }

        private void MovePiece(MovementOptions pieceToMove, List<ChessPiece> list, List<ChessPiece> EnemyPiecePositions)
        {
            pieceToMove.MyPiece.PositionX = pieceToMove.PositionX;
            pieceToMove.MyPiece.PositionY = pieceToMove.PositionY;

            if (pieceToMove.CheckForEnemyResult == 1)
            {
                RemoveEnemyPiece(pieceToMove.EnemyPiece);
            }
        }

        private void RemoveEnemyPiece(ChessPiece pieceToRemove)
        {
            EnemyPiecePositions.Remove(pieceToRemove);
        }

        private List<List<MovementOptions>> AnalyzeMyPieces(List<ChessPiece> list)
        {
            var AllMoves = new List<List<MovementOptions>>();
            CheckAllMyPieces(list, AllMoves);
            return AllMoves;
        }

        private void CheckAllMyPieces(List<ChessPiece> list, List<List<MovementOptions>> AllMoves)
        {
            foreach (var Piece in list)
            {
                Piece.ClearMovementoptions();
                Piece.MoveOption(Piece.teamDirection);

                var AllLegalMovesForThisPiece = new List<MovementOptions>();

                CheckAllDirections(list, Piece, AllLegalMovesForThisPiece);

                if (AllLegalMovesForThisPiece.Count != 0)
                {
                    AllMoves.Add(AllLegalMovesForThisPiece);
                }
            }
        }
        //Checks whichs directions the current piece can move in.
        private void CheckAllDirections(List<ChessPiece> list, ChessPiece Piece, List<MovementOptions> AllLegalMovesForThisPiece)
        {
            for (int direction = 0; direction < Piece.AllMoveOptionsForThisPiece.Count; direction++)
            {
                Piece.ClearMovementoptions();
                Piece.MoveOption(Piece.teamDirection);

                CheckLengthInDirection(list, Piece, AllLegalMovesForThisPiece, direction);
            }
        }
        //Chechs how far the current piece is allowed to move in the allowed directions. 
        private void CheckLengthInDirection(List<ChessPiece> list, ChessPiece Piece, List<MovementOptions> AllLegalMovesForThisPiece, int direction)
        {
            for (int walkingLength = 1; walkingLength <= Piece.AllMoveOptionsForThisPiece[direction].WalkingLength; walkingLength++)
            {
                var outOfBounds = false;
                var friendlyAhead = false;
                var enemyAhead = false;
                Piece.ClearMovementoptions();
                Piece.MoveOption(Piece.teamDirection);

                int MovingPositionX = Piece.AllMoveOptionsForThisPiece[direction].PositionX * walkingLength;
                int MovingPositionY = Piece.AllMoveOptionsForThisPiece[direction].PositionY * walkingLength;

                int FuturePositionX = Piece.PositionX + MovingPositionX;
                int FuturePositionY = Piece.PositionY + MovingPositionY;

                outOfBounds = CheckIfOutOfBounds(FuturePositionX, FuturePositionY);
                friendlyAhead = CheckIfFriendlyAhead(FuturePositionX, FuturePositionY, list);
                enemyAhead = CheckIfEnemyAhead(FuturePositionX, FuturePositionY, EnemyPiecePositions);

                walkingLength = GameRules.CheckIfLegalMove(Piece, AllLegalMovesForThisPiece, direction, walkingLength, outOfBounds, friendlyAhead, enemyAhead, FuturePositionX, FuturePositionY);
            }
        }

        public static int CreateMovementOption(ChessPiece Piece, List<MovementOptions> AllLegalMovesForThisPiece, int direction, int walkingLength, bool enemyAhead, int FuturePositionX, int FuturePositionY)
        {
            MovementOptions MoveChoice = Piece.AllMoveOptionsForThisPiece[direction];
            MoveChoice.MyPiece = Piece;
            if (enemyAhead)
            {
                MoveChoice.CheckForEnemyResult = 1;
                walkingLength = 100;
            }
            if (Piece.teamDirection == -1)
            {
                Piece.AllMoveOptionsForThisPiece[direction].MyTeam = "White";
            }
            else
            {
                Piece.AllMoveOptionsForThisPiece[direction].MyTeam = "Black";
            }

            Piece.AllMoveOptionsForThisPiece[direction].PositionX = FuturePositionX;
            Piece.AllMoveOptionsForThisPiece[direction].PositionY = FuturePositionY;
            AllLegalMovesForThisPiece.Add(Piece.AllMoveOptionsForThisPiece[direction]);
            Piece.AllMoveOptionsForThisPiece[direction] = MoveChoice;
            return walkingLength;
        }

        private bool CheckIfEnemyAhead(int FuturePositionX, int FuturePositionY, List<ChessPiece> EnemyPiecePositions)
        {
            foreach (var piece in EnemyPiecePositions)
            {
                if (piece.PositionX == FuturePositionX && piece.PositionY == FuturePositionY)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfOutOfBounds(int futureX, int futureY)
        {
            if (((futureX > -1) && (futureX < 8)) && ((futureY > -1) && (futureY < 8)))
            {
                return false;
            }
            return true;
        }

        public bool CheckIfFriendlyAhead(int futureX, int futureY, List<ChessPiece> friendyPieces)
        {
            foreach (var piece in friendyPieces)
            {
                if ((futureX == piece.PositionX) && (futureY == piece.PositionY))
                {
                    return true;
                }
            }
            return false;
        }

        public int GetRandomNumber(int min, int max)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(min, max);
            return randomNumber;
        }
    }
}
