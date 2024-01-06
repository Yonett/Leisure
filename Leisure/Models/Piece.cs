using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Leisure.Models
{
    public class Piece : INotifyPropertyChanged
    {
        private int width;
        private int height;
        private int kind;

        public int Width
        {
            get { return width; }
            set 
            {
                this.width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        public int Height
        { 
            get { return height; }
            set
            {
                this.height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        public int Kind
        {
            get { return kind; }
            set
            {
                this.kind = value;
                OnPropertyChanged(nameof(Kind));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Piece()
        {

        }
    }
}
