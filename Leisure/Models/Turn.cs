using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Leisure.Models
{
    public class Turn : INotifyPropertyChanged
    {
        #region Private Variables

        private int playerSerial;
        private int pieceWidth;
        private int pieceHeight;
        private int score;
        private int number;
        private bool available;

        #endregion

        #region Public Fields

        public int PlayerSerial
        {
            get
            {
                return this.playerSerial;
            }
            set
            {
                this.playerSerial = value;
                OnPropertyChanged(nameof(PlayerSerial));
            }
        }

        public int PieceWidth
        {
            get
            {
                return this.pieceWidth;
            }
            set
            {
                this.pieceWidth = value;
                OnPropertyChanged(nameof(PieceWidth));
            }
        }

        public int PieceHeight
        {
            get
            {
                return this.pieceHeight;
            }
            set
            {
                this.pieceHeight = value;
                OnPropertyChanged(nameof(PieceHeight));
            }
        }

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public int Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public bool Available
        {
            get
            {
                return this.available;
            }
            set
            {
                this.available = value;
                OnPropertyChanged(nameof(Available));
            }
        }

        #endregion

        public Turn(int number, int playerSerial, int pieceWidth, int pieceHeight, int score, bool available)
        {
            this.Number = number;
            this.PlayerSerial = playerSerial;
            this.PieceWidth = pieceWidth;
            this.PieceHeight = pieceHeight;
            this.Score = score;
            this.Available = available;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
