using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leisure.Models
{
    public class Field
    {
        private int size;
        private int bounds;
        private int amount;
        public int[] cells;

        public bool CanPlaceOnCellByEmptiness(int index)
        {
            return this.cells[index] == 0;
        }

        public bool CanPlaceOnCellByNearCells(int kind, int index)
        {
            return
            (
                this.cells[index + 1] == kind // Right cell
                ||
                this.cells[index - 1] == kind // Left cell
                ||
                this.cells[index - this.size] == kind // Top cell
                ||
                this.cells[index + this.size] == kind // Bottom cell
            );
        }

        public bool CanPlacePiece(Piece piece, int index)
        {
            int i = index / this.size;
            int j = index % this.size;

            if (i > bounds || i < 1 || j > bounds || j < 1)
                return false;

            if (i + piece.Height - 1 > bounds || j + piece.Width - 1 > bounds)
                return false;

            bool canPlace = true;

            for (int m = 0; m < piece.Height; m++)
            {
                for (int n = 0; n < piece.Width; n++)
                {
                    canPlace &= CanPlaceOnCellByEmptiness(index + m * this.size + n);
                    if (!canPlace)
                        return false;
                }
            }

            canPlace = false;

            for (int m = 0; m < piece.Height; m++)
            {
                for (int n = 0; n < piece.Width; n++)
                {
                    canPlace |= CanPlaceOnCellByNearCells(piece.Kind, index + m * this.size + n);
                    if (canPlace)
                        break;
                }
                if (canPlace)
                    break;
            }

            if (!canPlace)
                return false;

            return true;
        }

        public bool IsTurnAvailable(Piece piece)
        {
            for (int i = 0; i < this.cells.Length; i++)
            {
                if (this.cells[i] == 0)
                    if (CanPlacePiece(piece, i))
                        return true;
            }
            return false;
        }

        public void ResetCells()
        {
            for (int i = 0; i < this.amount; i++)
                cells[i] = 0;
        }

        public void InitCells()
        {
            int n = this.bounds;

            cells[n + 3] = 1;
            cells[this.amount - n - 4] = 2;

            for (int i = 1; i <= n; i++)
                cells[i] = -1;

            for (int i = 0; i < this.size; i++)
                cells[this.size * i] = -1;

            cells[n + 1] = -1;
            for (int i = 1; i <= this.size; i++)
                cells[this.size * i - 1] = -1;

            for (int i = 0; i < n; i++)
                cells[amount - (n - i) - 1] = -1;
        }

        public Field(int n)
        {
            this.size = n + 2;
            this.bounds = n;
            this.amount = this.size * this.size;
            this.cells = new int[this.amount];

            InitCells();

            //cells[n + 3] = 1;
            //cells[this.amount - n - 4] = 2;

            //for (int i = 1; i <= n; i++)
            //    cells[i] = -1;

            //for (int i = 0; i < this.size; i++)
            //    cells[this.size * i] = -1;

            //cells[n + 1] = -1;
            //for (int i = 1; i <= this.size; i++)
            //    cells[this.size * i - 1] = -1;

            //for (int i = 0; i < n; i++)
            //    cells[amount - (n - i) - 1] = -1;

        }
    }
}
