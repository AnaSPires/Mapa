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

            StreamReader arq2 = new StreamReader(@"c:\Users\u18201\Documents\GitHub\Mapa\CaminhosEntreCidadesMarte.txt");
            while (!arq2.EndOfStream)
            {
                string linha2 = arq2.ReadLine();
                int cod1 = int.Parse(linha2.Substring(0, 3));
                int cod2 = int.Parse(linha2.Substring(3, 3));
                Pen blackPen = new Pen(Color.Black, 3);
                Graphics grafico2 = pbMapa.CreateGraphics();

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
                grafico2.DrawLine(blackPen, p1.X + 3, p1.Y + 3, p2.X + 3, p2.Y + 3);
            }
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
            int n = 0;

            StreamReader arq = new StreamReader(@"c:\Users\u18201\Documents\GitHub\Mapa\CidadesMarteOrdenado.txt");
            string linha = arq.ReadLine();
            Cidade cid = new Cidade(linha);

            lsbDestino.Items.Add(n+"-"+cid.nome);
            lsbOrigem.Items.Add(n + "-" + cid.nome);

            DataGridViewColumn dc = new DataGridViewColumn();
            dc.HeaderText = "Cidade";
            dataGridView1.Columns.Add(dc);

            DataGridViewColumn dc3 = new DataGridViewColumn();
            dc3.HeaderText = "Cidade";
            dataGridView2.Columns.Add(dc3);

            Label label = new Label();
            label.Text = cid.nome;
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.BackColor = Color.Transparent;
            label.br


            arvore.Raiz = new NoArvore<Cidade>(cid);
            Graphics grafico = e.Graphics;
            Point p = new Point();
            p.X = cid.x * pbMapa.Width / 4096;
            p.Y = cid.y * pbMapa.Height / 2048;
            label.Location = new Point(p.X, p.Y-20);
            pbMapa.Controls.Add(label);
            SolidBrush pincel = new SolidBrush(Color.Black);
            grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            while (!arq.EndOfStream)
            {
                n++;
                linha = arq.ReadLine();
                cid = new Cidade(linha);
                lsbDestino.Items.Add(n + "-" + cid.nome);
                lsbOrigem.Items.Add(n + "-" + cid.nome);

                DataGridViewColumn dc2 = new DataGridViewColumn();
                dc2.HeaderText = "Cidade";
                dataGridView1.Columns.Add(dc2);

                DataGridViewColumn dc4 = new DataGridViewColumn();
                dc4.HeaderText = "Cidade";
                dataGridView2.Columns.Add(dc4);

                Label label2 = new Label();
                label2.Text = cid.nome;
                label2.Font = new Font("Arial", 10, FontStyle.Bold);
                label2.BackColor = Color.Transparent;

                arvore.Incluir(cid);
                grafico = e.Graphics;
                p = new Point();
                p.X = cid.x * pbMapa.Width / 4096;
                p.Y = cid.y * pbMapa.Height / 2048;

                label2.Location = new Point(p.X, p.Y - 20);
                pbMapa.Controls.Add(label2);

                pincel = new SolidBrush(Color.Black);
                grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            }

        }
    }
}
