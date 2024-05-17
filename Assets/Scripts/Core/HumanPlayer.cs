using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Game {
	public class HumanPlayer : Player {

		public enum InputState {
			None,
			PieceSelected,
			DraggingPiece
		}

		InputState currentState;

		BoardUI boardUI;
		Camera cam;
		Coord selectedPieceSquare;
		Board board;
		public HumanPlayer (Board board) {
			boardUI = GameObject.FindObjectOfType<BoardUI> ();
			cam = Camera.main;
			this.board = board;
		}

		public override void NotifyTurnToMove () {

		}

		public override void Update () {
			HandleInput ();
		}

		void HandleInput () {
			//Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10 /*La profondeur que vous souhaitez dans le monde 3D*/;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

            if (currentState == InputState.None) {
				HandlePieceSelection (worldPos);
			} else if (currentState == InputState.DraggingPiece) {
				HandleDragMovement (worldPos);
			} else if (currentState == InputState.PieceSelected) {
				HandlePointAndClickMovement (worldPos);
			}

			if (Input.GetMouseButtonDown (1)) {
				CancelPieceSelection ();
			}
		}

		void HandlePointAndClickMovement (Vector3 mousePos) {
			if (Input.GetMouseButton (0)) {
				HandlePiecePlacement (mousePos);
			}
		}

		void HandleDragMovement (Vector3 mousePos) {
			boardUI.DragPiece (selectedPieceSquare, mousePos);
			// If mouse is released, then try place the piece
			if (Input.GetMouseButtonUp (0)) {
				HandlePiecePlacement (mousePos);
			}
		}

		void HandlePiecePlacement (Vector3 mousePos) {
			Coord targetSquare;
			if (boardUI.TryGetSquareUnderMouse (mousePos, out targetSquare)) {
				if (targetSquare.Equals (selectedPieceSquare)) {
					boardUI.ResetPiecePosition (selectedPieceSquare);
					if (currentState == InputState.DraggingPiece) {
						currentState = InputState.PieceSelected;
					} else {
						currentState = InputState.None;
						boardUI.DeselectSquare (selectedPieceSquare);
					}
				} else {
					int targetIndex = BoardRepresentation.IndexFromCoord (targetSquare.fileIndex, targetSquare.rankIndex);
					if (Piece.IsColour (board.Square[targetIndex], board.ColourToMove) && board.Square[targetIndex] != 0) {
						CancelPieceSelection ();
						HandlePieceSelection (mousePos);
					} else {
						TryMakeMove (selectedPieceSquare, targetSquare);
					}
				}
			} else {
				CancelPieceSelection ();
			}

		}

		void CancelPieceSelection () {
			if (currentState != InputState.None) {
				currentState = InputState.None;
				boardUI.DeselectSquare (selectedPieceSquare);
				boardUI.ResetPiecePosition (selectedPieceSquare);
			}
		}

		void TryMakeMove (Coord startSquare, Coord targetSquare) {
			int startIndex = BoardRepresentation.IndexFromCoord (startSquare);
			int targetIndex = BoardRepresentation.IndexFromCoord (targetSquare);
			bool moveIsLegal = false;
			Move chosenMove = new Move ();

			MoveGenerator moveGenerator = new MoveGenerator ();
			bool wantsKnightPromotion = Input.GetKey (KeyCode.LeftAlt);

			var legalMoves = moveGenerator.GenerateMoves (board);
			for (int i = 0; i < legalMoves.Count; i++) {
				var legalMove = legalMoves[i];

				if (legalMove.StartSquare == startIndex && legalMove.TargetSquare == targetIndex) {
					if (legalMove.IsPromotion) {
						if (legalMove.MoveFlag == Move.Flag.PromoteToQueen && wantsKnightPromotion) {
							continue;
						}
						if (legalMove.MoveFlag != Move.Flag.PromoteToQueen && !wantsKnightPromotion) {
							continue;
						}
					}
					moveIsLegal = true;
					chosenMove = legalMove;
					//	Debug.Log (legalMove.PromotionPieceType);
					break;
				}
			}

			if (moveIsLegal) {
				ChoseMove (chosenMove);
				currentState = InputState.None;
			} else {
				CancelPieceSelection ();
			}
		}

		void HandlePieceSelection (Vector3 mousePos) {

			if (Input.GetMouseButtonDown (0)) {
			Debug.Log($"MousePos = {mousePos}");
				if (boardUI.TryGetSquareUnderMouse (mousePos, out selectedPieceSquare)) {
					int index = BoardRepresentation.IndexFromCoord (selectedPieceSquare);
			Debug.Log($"Case index = {selectedPieceSquare.fileIndex} / {selectedPieceSquare.rankIndex}");
					// If square contains a piece, select that piece for dragging
					if (Piece.IsColour (board.Square[index], board.ColourToMove)) {
						boardUI.HighlightLegalMoves (board, selectedPieceSquare);
						boardUI.SelectSquare (selectedPieceSquare);
						currentState = InputState.DraggingPiece;
					}
				}
			}
		}
	}
}