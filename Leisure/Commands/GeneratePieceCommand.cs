using System;
using System.Windows.Input;
using System.Security.Cryptography;
using Leisure.Enums;
using Leisure.ViewModels;
using Leisure.Views;
using Leisure.Models;
using System.Windows;

namespace Leisure.Commands
{
    public class GeneratePieceCommand : ICommand
    {
        private readonly MainWindowViewModel viewModel;
        public bool CanExecute(object? parameter)
        {
            if (this.viewModel.GameState == (int)GameStates.PreTurn)
                return true;
            return false;
        }
        public void Execute(object? parameter)
        {
            int width = RandomNumberGenerator.GetInt32(1, 7);
            int height = RandomNumberGenerator.GetInt32(1, 7);
            int score;

            viewModel.Piece.Width = width;
            viewModel.Piece.Height = height;

            if (this.viewModel.TurnNumber % 2 == 0)
            {
                viewModel.Piece.Kind = 2;
                score = viewModel.SecondPlayer.Score;
            }
            else
            {
                viewModel.Piece.Kind = 1;
                score = viewModel.FirstPlayer.Score;
            }

            if (viewModel.Field.IsTurnAvailable(viewModel.Piece) == true)
            {
                this.viewModel.UnavailableTurns = 0;
                this.viewModel.GameState = (int)GameStates.Turn;
            }
            else
            {
                Console.WriteLine("Player {0} W - {1} H - {2} turn is unavailable", viewModel.Piece.Kind, width, height);
                LeisureMessageBox box = new LeisureMessageBox("TURN IS IMPOSSIBLE", "CHANGE PLAYER");
                //MessageBox.Show("Turn is unavailable!");

                viewModel.Turns.Add(new Turn(
                                viewModel.TurnNumber,
                                viewModel.Piece.Kind,
                                viewModel.Piece.Width,
                                viewModel.Piece.Height,
                                score,
                                false)
                                );

                this.viewModel.UnavailableTurns++;
                if (viewModel.UnavailableTurns > 1)
                {
                    Console.WriteLine("Too much unavailabel turns");
                    string winner = String.Format("{0} PLAYER WINS", viewModel.FirstPlayer.Score > viewModel.SecondPlayer.Score ? "FIRST" : "SECOND");
                    if (viewModel.FirstPlayer.Score == viewModel.SecondPlayer.Score)
                        winner = "DRAW";
                    //MessageBox.Show(winner);
                    LeisureMessageBox winner_box = new LeisureMessageBox(winner, "RESTART GAME");
                    this.viewModel.GameState = (int)GameStates.PostGame;
                    return;
                }
                this.viewModel.TurnNumber++;
            }
        }

        public event EventHandler? CanExecuteChanged;

        public GeneratePieceCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
    }
}
