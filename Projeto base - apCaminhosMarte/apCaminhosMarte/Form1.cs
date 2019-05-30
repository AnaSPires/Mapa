using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    public partial class Form1 : Form
    {
        Arvore<Cidade> arvore;
        public Form1()
        {
            InitializeComponent();
            arvore = new Arvore<Cidade>();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {

        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            LerArquivo(e);
        }

        private void LerArquivo(PaintEventArgs e)
        {
            StreamReader arq = new StreamReader(@"c:\Users\u18201\Documents\GitHub\Mapa\CidadesMarte.txt");
            string linha = arq.ReadLine();
            Cidade cid = new Cidade(linha);
            arvore.Raiz = new NoArvore<Cidade>(cid);
            Graphics grafico = e.Graphics;
            Point p = new Point();
            p.X = cid.x * pbMapa.Width / 4096;
            p.Y = cid.y * pbMapa.Height / 2048;
            SolidBrush pincel = new SolidBrush(Color.Black);
            grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            while (!arq.EndOfStream)
            {
                linha = arq.ReadLine();
                cid = new Cidade(linha);
                arvore.Incluir(cid);
                grafico = e.Graphics;
                p = new Point();
                p.X = cid.x * pbMapa.Width / 4096;
                p.Y = cid.y * pbMapa.Height / 2048;
                pincel = new SolidBrush(Color.Black);
                grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            }

            StreamReader arq2 = new StreamReader(@"c:\Users\u18201\Documents\GitHub\Mapa\CaminhosEntreCidadesMarte.txt");
            while(!arq2.EndOfStream)
            {
                string linha2 = arq2.ReadLine();
                int cod1 = int.Parse(linha2.Substring(0, 3));
                int cod2 = int.Parse(linha2.Substring(3, 3));
                Pen blackPen = new Pen(Color.Blue, 3);

                Graphics grafico2 = e.Graphics;

                Point p1 = new Point();
                Cidade c1 = new Cidade();
                c1.cod = cod1;
                arvore.Existe(c1);
                p1.X = arvore.Atual.Info.x * pbMapa.Width / 4096;
                p1.Y = arvore.Atual.Info.y * pbMapa.Height / 2048; 

                Point p2 = new Point();
                Cidade c2 = new Cidade();
                c2.cod = cod2;
                arvore.Existe(c2);
                p2.X = arvore.Atual.Info.x * pbMapa.Width / 4096;
                p2.Y = arvore.Atual.Info.y * pbMapa.Height / 2048;
                grafico2.DrawLine(blackPen, p1.X + 3,p1.Y + 3, p2.X + 3,p2.Y + 3);
            }
        }
    }
}
