using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Leisure.Models
{
    public class Player : INotifyPropertyChanged
    {
        #region Private Variables

        private string? name;
        private int score = 1;
        private int serial = 0;

        #endregion

        #region Public Fields

        public string Name
        {
            get
            { 
                if (this.name == null)
                    return "NO NAME";
                return this.name.ToUpper();
            }

            set
            {
                this.name = value;
                OnPropertyChanged(nameof(Name));
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
        public int Serial
        {
            get
            {
                return this.serial;
            }

            set
            {
                this.serial = value;
                OnPropertyChanged(nameof(Serial));
            }
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Player(string name, int serial)
        {
            this.Name = name;
            this.Serial = serial;
        }
    }
}
