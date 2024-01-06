using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using Leisure.Commands;
using Leisure.Models;
using Leisure.Enums;

namespace Leisure.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly Player firstPlayer;
        private readonly Player secondPlayer;
        private Piece piece;
        private Field field;
        private int turnNumber;
        private int size;
        private readonly ObservableCollection<Turn> turns;
        private int gameState = (int)GameStates.PreTurn;

        public int UnavailableTurns = 0;
        public int GameState
        {
            get
            {
                return this.gameState;
            }
            set
            {
                this.gameState = value;
                OnPropertyChanged(nameof(GameState));
            }
        }
        public Field Field
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
                OnPropertyChanged(nameof(Field));
            }
        }
        public Rectangle[]? FieldRects;
        public Piece Piece
        {
            get
            {
                return piece;
            }
            set
            {
                piece = value;
                OnPropertyChanged(nameof(Piece));
            }
        }
        public Rectangle[]? PieceRects;
        public GeneratePieceCommand command
        {
            get; set;
        }
        public Player FirstPlayer
        {
            get
            {
                return this.firstPlayer;
            }
        }
        public Player SecondPlayer
        {
            get
            {
                return this.secondPlayer;
            }
        }
        public int TurnNumber
        {
            get
            {
                return this.turnNumber;
            }
            set
            {
                this.turnNumber = value;
                OnPropertyChanged(nameof(TurnNumber));
            }
        }
        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
        public ObservableCollection<Turn> Turns
        {
            get
            {
                return this.turns;
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            size = 25;

            piece = new Piece() { Width = 0, Height = 0, Kind = 0};

            field = new Field(size);

            size += 2;

            FieldRects = new Rectangle[size * size];
            PieceRects = new Rectangle[6 * 6];

            firstPlayer = new Player("First Player", 1);
            secondPlayer = new Player("Second Player", 2);

            turnNumber = 1;

            turns = new ObservableCollection<Turn>();

            command = new GeneratePieceCommand(this);
        }
    }
}
