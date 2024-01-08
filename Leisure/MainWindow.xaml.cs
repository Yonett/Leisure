using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Leisure.Enums;
using Leisure.Models;
using Leisure.ViewModels;

namespace Leisure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Brush firstPlayerColor;
        private readonly Brush secondPlayerColor;
        private readonly Brush additionalColor;
        private readonly Brush freeCellWhiteColor;

        private double gameFieldHeight = 0;
        private double gameFieldWidth = 0;
        private double rectSize = 0;
        private double rectSizeWithIndent = 0;
        private double indent = 0;
        private double xOffset = 0;
        public MainWindow()
        {
            InitializeComponent();

            firstPlayerColor   = (Brush)FindResource("firstPlayerColor");
            secondPlayerColor  = (Brush)FindResource("secondPlayerColor");
            additionalColor    = (Brush)FindResource("additional");
            freeCellWhiteColor = (Brush)FindResource("freeCellWhite");

            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Piece.PropertyChanged += ChangePieceColor;
                viewModel.PropertyChanged += RestartGame;
                GameField.Loaded += GameField_Loaded;

                BrushField(true);
                BrushPieceRects(2); // start a game
                viewModel.GameState = (int)GameStates.PreTurn;
            }
        }

        private void GameField_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.FieldRects != null)
                {
                    int size = viewModel.Size;
                    gameFieldHeight = GameField.ActualHeight;
                    gameFieldWidth = GameField.ActualWidth;
                    rectSize = gameFieldHeight * 0.75 / size;
                    indent = gameFieldHeight * 0.25 / (size - 1);
                    rectSizeWithIndent = rectSize + indent;
                    xOffset = (gameFieldWidth - gameFieldHeight) / 2;
                    int index;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            index = i * size + j;

                            viewModel.FieldRects[index].Width = rectSize;
                            viewModel.FieldRects[index].Height = rectSize;

                            Canvas.SetLeft(viewModel.FieldRects[index], j * rectSizeWithIndent + xOffset);
                            Canvas.SetTop(viewModel.FieldRects[index], i * rectSizeWithIndent);
                        }
                    }
                }

                if (viewModel.PieceRects != null)
                {
                    int size = 6;
                    int index;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            index = i * size + j;
                            viewModel.PieceRects[index].Width = rectSize;
                            viewModel.PieceRects[index].Height = rectSize;
                            viewModel.PieceRects[index].StrokeThickness = rectSize / 4;
                        }
                    }
                }
            }
        }

        private void MainWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.GameState == (int)GameStates.Turn)
                {
                    if (viewModel.PieceRects == null || viewModel.FieldRects == null)
                        return;

                    int index = Array.IndexOf(viewModel.FieldRects, sender);

                    if (viewModel.Field.CanPlacePiece(viewModel.Piece, index))
                    {
                        int score = viewModel.Piece.Height * viewModel.Piece.Width;
                        
                        for (int i = 0; i < viewModel.Piece.Height; i++)
                            for (int j = 0; j < viewModel.Piece.Width; j++)
                            { 
                                if(viewModel.Piece.Kind == 1)
                                    viewModel.FieldRects[index + i * viewModel.Size + j].Fill = firstPlayerColor;
                                else
                                    viewModel.FieldRects[index + i * viewModel.Size + j].Fill = secondPlayerColor;
                                viewModel.Field.cells[index + i * viewModel.Size + j] = viewModel.Piece.Kind;
                                viewModel.PieceRects[i * viewModel.Piece.Width + j].Visibility = Visibility.Hidden;
                            }

                        if (viewModel.Piece.Kind == 1)
                        {
                            viewModel.Turns.Add(new Turn(
                                viewModel.TurnNumber,
                                viewModel.Piece.Kind,
                                viewModel.Piece.Width,
                                viewModel.Piece.Height,
                                viewModel.FirstPlayer.Score,
                                true)
                                );
                            viewModel.FirstPlayer.Score += score;
                        }
                        else
                        {
                            viewModel.Turns.Add(new Turn(
                                viewModel.TurnNumber,
                                viewModel.Piece.Kind,
                                viewModel.Piece.Width,
                                viewModel.Piece.Height,
                                viewModel.SecondPlayer.Score,
                                true)
                                );
                            viewModel.SecondPlayer.Score += score;
                        }

                        viewModel.TurnNumber++;

                        viewModel.GameState = (int)GameStates.PreTurn;
                    }
                }
            }
        }

        private void GameField_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.GameState != (int)GameStates.Turn)
                    return;

                if (viewModel.PieceRects == null)
                    return;

                BrushPieceRects(1); // move cursor to field

            }
        }

        private void GameField_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.GameState != (int)GameStates.Turn)
                    return;

                if (viewModel.PieceRects == null)
                    return;
                
                Point pt = e.GetPosition(GameField);
                int index;
                double centerPadding = rectSizeWithIndent / 2;
                for (int i = 0; i < viewModel.Piece.Height; i++)
                    for (int j = 0; j < viewModel.Piece.Width; j++)
                    {
                        index = i * viewModel.Piece.Width + j;

                        Canvas.SetLeft(viewModel.PieceRects[index], pt.X + j * rectSizeWithIndent - centerPadding);
                        Canvas.SetTop(viewModel.PieceRects[index], pt.Y + i * rectSizeWithIndent - centerPadding);
                    }
            }
        }

        private void GameField_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.PieceRects == null)
                    return;

                if (viewModel.GameState != (int)GameStates.Turn)
                    return;
                BrushPieceRects(0); // move cursor from field
            }
        }

        private void ChangePieceColor(object sender, PropertyChangedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.PieceRects == null)
                    return;

                if (e.PropertyName == nameof(viewModel.Piece.Kind))
                {
                    BrushPieceRects(4); // change the color
                    DataScroll.ScrollToBottom();
                }
            }
        }

        private void RestartGame(object sender, PropertyChangedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (e.PropertyName == nameof(viewModel.GameState))
                {
                    if (viewModel.GameState == (int)GameStates.PostGame)
                    {
                        viewModel.FirstPlayer.Score = 1;
                        viewModel.SecondPlayer.Score = 1;
                        viewModel.Piece.Width = 0;
                        viewModel.Piece.Height = 0;
                        viewModel.TurnNumber = 1;
                        viewModel.UnavailableTurns = 0;

                        viewModel.Turns.Clear();

                        viewModel.Field.ResetCells();
                        viewModel.Field.InitCells();

                        BrushField(false);
                        BrushPieceRects(3); // restart
                        viewModel.GameState = (int)GameStates.PreTurn;
                    }
                }
            }
        }



        private void BrushPieceRects(int state)
        {
            if (DataContext is MainWindowViewModel viewModel)
            { 
                if (viewModel.PieceRects == null)
                        return;
                switch (state)
                {
                    case 0: // 0 - move cursor from field
                        {
                            int size = 6;
                            for (int i = 0; i < size; i++)
                                for (int j = 0; j < size; j++)
                                    viewModel.PieceRects[i * viewModel.Piece.Width + j].Visibility = Visibility.Hidden;
                            break;
                        }
                    case 1: // 1 - move cursor to field
                        {
                            for (int i = 0; i < viewModel.Piece.Height; i++)
                                for (int j = 0; j < viewModel.Piece.Width; j++)
                                    viewModel.PieceRects[i * viewModel.Piece.Width + j].Visibility = Visibility.Visible;
                            break;
                        }
                    case 2: // 2 - start a game
                        {
                            int size = 6;
                            int index;
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    index = i * size + j;

                                    viewModel.PieceRects[index] = new Rectangle();
                                    viewModel.PieceRects[index].Stroke = firstPlayerColor;
                                    viewModel.PieceRects[index].Visibility = Visibility.Hidden;

                                    GameField.Children.Add(viewModel.PieceRects[index]);
                                }
                            }
                            break;
                        }
                    case 3: // 3 - restart
                        {
                            int size = 6;
                            int index;
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < size; j++)
                                {
                                    index = i * size + j;

                                    viewModel.PieceRects[index].Stroke = firstPlayerColor;
                                    viewModel.PieceRects[index].Visibility = Visibility.Hidden;
                                }
                            }
                            break;
                        }
                    case 4: // 4 - change a color
                        {
                            int size = 6;
                            Brush brush;
                            if (viewModel.Piece.Kind == 1)
                                brush = firstPlayerColor;
                            else
                                brush = secondPlayerColor;
                            for (int i = 0; i < size; i++)
                                for (int j = 0; j < size; j++)
                                    viewModel.PieceRects[i * size + j].Stroke = brush;
                            break;
                        }

                    default:
                        break;
                }
            }
        }
        
        private void BrushField(bool Initialize) {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (viewModel.FieldRects != null)
                {
                    int size = viewModel.Size;
                    int index;

                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            index = i * size + j;

                            if (Initialize == true) // не будет при Restart
                                viewModel.FieldRects[index] = new Rectangle();

                            switch (viewModel.Field.cells[index])
                            {
                                case 1:
                                    viewModel.FieldRects[index].Fill = firstPlayerColor;
                                    break;
                                case 2:
                                    viewModel.FieldRects[index].Fill = secondPlayerColor;
                                    break;
                                case 0:
                                    viewModel.FieldRects[index].Fill = freeCellWhiteColor;
                                    if (Initialize == true) // не будет при Restart
                                        viewModel.FieldRects[index].MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
                                    break;
                                case -1:
                                    viewModel.FieldRects[index].Fill = additionalColor;
                                    break;
                                default:
                                    break;
                            }
                            if (Initialize == true) // не будет при Restart
                                GameField.Children.Add(viewModel.FieldRects[index]);
                        }
                    }
                }
            }

        }

    }
}
