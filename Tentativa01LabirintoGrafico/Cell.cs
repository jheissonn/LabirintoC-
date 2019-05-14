using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;

namespace Tentativa01LabirintoGrafico
{
    class Cell
    {
        public bool NorthWall = true;
        public bool SouthWall = true;
        public bool WestWall = true;
        public bool EastWall = true;
        public bool cimaWall = true;
        public bool baixoWall = true;
        public bool esquerdaWall = true;
        public bool direitaWall = true;
        public bool desenhar = false;
        public string id;
        public Pen Pen = Pens.Black;
        Pen bluePen = new Pen(Color.FromArgb(255, 0, 255, 0), 5);
        public Rectangle Bounds;
        public Dictionary<string, Cell> Cells;
        public int Column;
        public int Row;
        public string NeighborNorthID;
        public string NeighborSouthID;
        public string NeighborEastID;
        public string NeighborWestID;
        public bool Visited = false;

        public Cell(Point location, Size size, ref Dictionary<string, Cell> cellList, int r, int c, int maxR, int maxC)
        {
            
            this.Bounds = new Rectangle(location, size);
            this.Column = c;
            this.Row = r;
            /* o id é a coluna e linha que foi inserido de acordo com a matriz */
            this.id = "c" + c + "r" + r;
            int rowNort = r - 1;
            int rowSout = r + 1;
            int colEast = c + 1;
            int colWest = c - 1;
            /* seta os visinhos */ 
            NeighborNorthID = "c" + c + "r" + rowNort;
            NeighborSouthID = "c" + c + "r" + rowSout;
            NeighborEastID = "c" + colEast + "r" + r;
            NeighborWestID = "c" + colWest + "r" + r;

            if (rowNort < 0) NeighborNorthID = "none";
            if (rowSout > maxR) NeighborSouthID = "none";
            if (colEast > maxC) NeighborEastID = "none";
            if (colWest < 0) NeighborWestID = "none";
            this.Cells = cellList;
            this.Cells.Add(this.id, this);
        }

        public void drawTeste(Graphics g)
        {
            /* Desenha o círculo nas células que estão marcadas */
            if (desenhar)
                g.DrawEllipse(bluePen, (Bounds.Left + (Bounds.Width / 4)), (Bounds.Top + (Bounds.Width / 4)), (Bounds.Width / 2), (Bounds.Width / 2));
            /* Desenha o quadrado de cada célula */
            if (NorthWall) g.DrawLine(Pen, new Point(Bounds.Left, Bounds.Top), new Point(Bounds.Right, Bounds.Top));
            if (SouthWall) g.DrawLine(Pen, new Point(Bounds.Left, Bounds.Bottom), new Point(Bounds.Right, Bounds.Bottom));
            if (WestWall) g.DrawLine(Pen, new Point(Bounds.Left, Bounds.Top), new Point(Bounds.Left, Bounds.Bottom));
            if (EastWall) g.DrawLine(Pen, new Point(Bounds.Right, Bounds.Top), new Point(Bounds.Right, Bounds.Bottom));
        }

        public Cell getNeighbor()
        {
            List<Cell> c = new List<Cell>();
            /* pega os vizinhos disponíveis  */
            if (!(NeighborNorthID == "none") && Cells[NeighborNorthID].Visited == false) c.Add(Cells[NeighborNorthID]);
            if (!(NeighborSouthID == "none") && Cells[NeighborSouthID].Visited == false) c.Add(Cells[NeighborSouthID]);
            if (!(NeighborEastID == "none") && Cells[NeighborEastID].Visited == false) c.Add(Cells[NeighborEastID]);
            if (!(NeighborWestID == "none") && Cells[NeighborWestID].Visited == false) c.Add(Cells[NeighborWestID]);
            int max = c.Count;
            Cell currentCell = null;
            /* Sorteia um vizinho */
            if (c.Count > 0)
            {
                Microsoft.VisualBasic.VBMath.Randomize();
                int index = Convert.ToInt32(Conversion.Int(c.Count * VBMath.Rnd()));
                currentCell = c[index];
            }
            return currentCell;
        }

        public Cell Dig(ref Pilha<Cell> stack)
        {
            Cell nextCell = getNeighbor();
            /* pega um vizinho aleatório */
            if ((nextCell != null))
            {
                stack.InsertLast(nextCell);
                /* Insere na pilha a célula sorteada e decide quais paredes irá abrir */
                if (nextCell.id == this.NeighborNorthID)
                {
                    this.cimaWall = false;
                    nextCell.baixoWall = false;
                    this.NorthWall = false;
                    nextCell.SouthWall = false;
                }
                else if (nextCell.id == this.NeighborSouthID)
                {
                    this.baixoWall = false;
                    nextCell.cimaWall = false;
                    this.SouthWall = false;
                    nextCell.NorthWall = false;
                }
                else if (nextCell.id == this.NeighborEastID)
                {
                    this.direitaWall = false;
                    nextCell.esquerdaWall = false;
                    this.EastWall = false;
                    nextCell.WestWall = false;
                }
                else if (nextCell.id == this.NeighborWestID)
                {
                    this.esquerdaWall = false;
                    nextCell.direitaWall = false;
                    this.WestWall = false;
                    nextCell.EastWall = false;
                }
            }
            else if (!(stack.qtde == 0))
            {
                /* Em caso de ter terminado e ainda existir ele vai desempilhando */
                stack.RemoveLast();
                nextCell = stack.peek();
            }
            else
                return null;

            return nextCell;
        }

    }
}
