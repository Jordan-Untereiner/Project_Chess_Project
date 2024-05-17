using UnityEngine;

namespace Chess.Game {
	[CreateAssetMenu (menuName = "Theme/Pieces")]
	public class PieceTheme : ScriptableObject {

		public PieceSprites whitePieces;
		public PieceSprites blackPieces;

		public PieceMesh WhitePieces;
		public PieceMesh BlackPieces;

		public Sprite GetPieceSprite (int piece) {
			PieceSprites pieceSprites = Piece.IsColour (piece, Piece.White) ? whitePieces : blackPieces;

			switch (Piece.PieceType (piece)) {
				case Piece.Pawn:
					return pieceSprites.pawn;
				case Piece.Rook:
					return pieceSprites.rook;
				case Piece.Knight:
					return pieceSprites.knight;
				case Piece.Bishop:
					return pieceSprites.bishop;
				case Piece.Queen:
					return pieceSprites.queen;
				case Piece.King:
					return pieceSprites.king;
				default:
					if (piece != 0) {
						Debug.Log (piece);
					}
					return null;
			}
		}

        public Mesh GetPieceMesh(int piece)
        {
            PieceMesh pieceMesh = Piece.IsColour(piece, Piece.White) ? WhitePieces : BlackPieces;

            switch (Piece.PieceType(piece))
            {
                case Piece.Pawn:
                    return pieceMesh.pawn;
                case Piece.Rook:
                    return pieceMesh.rook;
                case Piece.Knight:
                    return pieceMesh.knight;
                case Piece.Bishop:
                    return pieceMesh.bishop;
                case Piece.Queen:
                    return pieceMesh.queen;
                case Piece.King:
                    return pieceMesh.king;
                default:
                    if (piece != 0)
                    {
                        Debug.Log(piece);
                    }
                    return null;
            }
        }

        [System.Serializable]
		public class PieceSprites {
			public Sprite pawn, rook, knight, bishop, queen, king;

			public Sprite this [int i] {
				get {
					return new Sprite[] { pawn, rook, knight, bishop, queen, king }[i];
				}
			}
		}

        [System.Serializable]
        public class PieceMesh
        {
            public Mesh pawn, rook, knight, bishop, queen, king;

            public Mesh this[int i]
            {
                get
                {
                    return new Mesh[] { pawn, rook, knight, bishop, queen, king }[i];
                }
            }
        }
    }
}