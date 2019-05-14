using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.Threading;
/* Algoritmo para resolução busca em profundidade, basicamente faz as células com 4 paredes e vai sorteando e quebrando as paredes. */
//https://plaxdev.wordpress.com/2017/08/17/maze-generation-depth-first-search-part-1/
namespace Tentativa01LabirintoGrafico
{
    public partial class Form1 : Form
    {
        Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
        Pilha<Cell> stackNovo = new Pilha<Cell>();
        Pilha<ModelCaminho> emPilha = new Pilha<ModelCaminho>();
        int altura;
        int largura;
        int larguraPixels;

        public Form1()
        {
            InitializeComponent();
        }

        private void proDesenharPronto()
        {
            /* Cria o bitMap com o tamanho e largura do labirinto */
            Bitmap Maze = new Bitmap((largura * larguraPixels ) + 5, (altura * larguraPixels) + 5);
            pictureBox1.Height = ((largura * larguraPixels) + 5);
            pictureBox1.Width = (altura * larguraPixels) + 5;

            Pen Psen = Pens.Black;
            int c = 0;
            int r = 0;
            /* desenha as 4 paredes de cada celula do labirinto */
            using (Graphics g = Graphics.FromImage(Maze))
            {
                g.Clear(Color.White);
                if (cells.Count > 0)
                {
                    for (r = 0; r <= altura - 1; r++)
                    {
                        for (c = 0; c <= largura - 1; c++)
                        {
                            Cell cell = cells["c" + c + "r" + r];
                            cell.drawTeste(g);
                        }
                    }
                }
            }
            pictureBox1.Image = Maze;
        }

        private void desenharCaminhoResolvido() {
            /* Desenpilha os caminhos e seta para desenhar o círculo em cada célula */
            while (!emPilha.IsEmpty) {
                cells["c" + emPilha.peek().getX() + "r" + emPilha.peek().getY()].desenhar = true;
                emPilha.RemoveLast();
            }
            proDesenharPronto();
        }

        private Boolean solve(int x, int y) {

            Cell cell = cells["c" + x + "r" + y];
            cell.desenhar = true;
            Thread ThreadDesenho = new Thread(proDesenharPronto);
            ThreadDesenho.Start();
            ThreadDesenho.Join();
            Thread.Sleep(10);
            
            if (x == largura - 1 && y == altura - 1) {
                /* desenpilha e chama a função de desenho. */
                Thread ThreadDesempilha = new Thread(desenharCaminhoResolvido);
                ThreadDesempilha.Start();
                ThreadDesempilha.Join();
                MessageBox.Show("Labirinto resolvido com sucesso.");
                return true;
            }
            /* após desenhado ele seta para não desenhar o círculo na célula novamente */
            cell.desenhar = false;
            if (x < largura && !cell.direitaWall)
            {
                /* ele seta p proximo para que a parede fique livre e retira a parede da direita
                   e o padrão se repete para todos os if  */
                cells["c" + (x + 1) + "r" + y].esquerdaWall = true;
                cell.direitaWall = true;
                emPilha.InsertLast(new ModelCaminho(x + 1, y));
                return solve(x + 1, y);
            }
            else
            {
                if (y < altura && !cell.baixoWall)
                {
                    cell.baixoWall = true;
                    cells["c" + x + "r" + (y + 1)].cimaWall = true;
                    emPilha.InsertLast(new ModelCaminho(x, y + 1));
                    return solve(x, y + 1);
                }
                else
                {
                    if (x > 0 && !cell.esquerdaWall)
                    {
                        cells["c" + (x - 1) + "r" + y].direitaWall = true;
                        cell.esquerdaWall = true;
                        emPilha.InsertLast(new ModelCaminho(x - 1, y));
                        return solve(x - 1, y);
                    }
                    else {
                        if (y > 0 && !cell.cimaWall)
                        {
                            cells["c" + x + "r" + (y - 1)].baixoWall = true;
                            cell.cimaWall = true;
                            emPilha.InsertLast(new ModelCaminho(x, y - 1));
                            return solve(x, y - 1);
                        }
                    }
                }
            }
            /* se caso ainda não retornou significa que ainda não se resolveu e volta uma posição */
            emPilha.RemoveLast();
            return solve(emPilha.peek().getX(), emPilha.peek().getY());
        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            if (cells.Count == 0) {
                return;
            }
            /* Insere o primeiro caminho na pilha */
            emPilha.InsertLast(new ModelCaminho(0, 0));
            solve(0, 0);
        }

        private void buttonGerar_Click_1(object sender, EventArgs e)
        {
            /* Pega os valores do numeric UP DOWN */
            altura = Convert.ToInt32(numAltura.Value);
            largura = Convert.ToInt32(numLargura.Value);
            larguraPixels = Convert.ToInt32(numPixels.Value);

            cells.Clear();

            int Rows = altura ;
            int Columns = largura;
            int c = 0;
            int r = 0;

            /* Adiciona as celular com 4 paredes em cada posição do labiirinto */
            for (int y = 0; y <= (altura * larguraPixels)-5; y += larguraPixels)
            {
                for (int x = 0; x <= (largura * larguraPixels)-5; x += larguraPixels)
                {
                    Cell cell = new Cell(new Point(x, y), new Size(larguraPixels, larguraPixels), ref cells, r, c, (Rows - 1), (Columns - 1));
                    c += 1;
                }
                c = 0;
                r += 1;
            }
            proDesenharPronto();

            string key = "c" + 0 + "r" + 0;
            Cell startCell = cells[key];           
            startCell.Visited = true;
            /* Inicia a geração do labirinto */
            while ((startCell != null))
            {
                startCell = startCell.Dig(ref stackNovo);
                if (startCell != null)
                {
                    /* desenha o labirinto a cada mudança de uma célula */
                    startCell.desenhar = true;
                    startCell.Visited = true;
                    startCell.Pen = Pens.Black;
                    Thread t = new Thread(proDesenharPronto);
                    t.Start();
                    t.Join();
                    Thread.Sleep(1);
                    startCell.desenhar = false;
                }
            }

            string keys = "c" + (largura - 1) + "r" +(altura - 1) ;
            Cell saida = cells[keys];
            saida.EastWall = false;
            saida = cells["c0r0"];
            saida.desenhar = true;
            saida.WestWall = false;
            /* desenha uma abertura no inicio e no final do labirinto */
            proDesenharPronto();
        }
    }
}
