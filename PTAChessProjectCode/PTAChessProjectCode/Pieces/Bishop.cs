﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTAChessProjectCode
{
    class Bishop:IChessPiece
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
        public Bishop()
        {
            AllMoveOptionsForThisPiece = new List<MovementOptions>();
            FullName = "Bishop";
            Name = "B";
            Value = 3;
        }
        public void MoveOption(int teamDirection)
        {
            List<MovementOptions> possibleMoves = new List<MovementOptions>();

            possibleMoves.Add(new MovementOptions(-1, -1, 7, true, true));
            possibleMoves.Add(new MovementOptions(1, 1, 7, true, true));
            possibleMoves.Add(new MovementOptions(-1, 1, 7, true, true));
            possibleMoves.Add(new MovementOptions(1, -1, 7, true, true));
            AllMoveOptionsForThisPiece = possibleMoves;
        }
    }
}
