﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTAChessProjectCode
{
    class Rook:IChessPiece
    {
        public string FullName { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public int teamDirection { get; set; }
        public void ClearMovementoptions()
        {
            AllMoveOptionsForThisPiece.Clear();
        }

        public List<MovementOptions> AllMoveOptionsForThisPiece { get; set; }
        public Rook()
        {
            AllMoveOptionsForThisPiece = new List<MovementOptions>();
            FullName = "Rook";
            Name = "R";
            Value = 5;
        }
        public void MoveOption(int teamDirection)
        {
            List<MovementOptions> possibleMoves = new List<MovementOptions>();

            possibleMoves.Add(new MovementOptions(0, -1, 7, true, true));
            possibleMoves.Add(new MovementOptions(0, 1, 7, true, true));
            possibleMoves.Add(new MovementOptions(-1, 0, 7, true, true));
            possibleMoves.Add(new MovementOptions(1, 0, 7, true, true));
            AllMoveOptionsForThisPiece = possibleMoves;
        }
    }
}
