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
        int[,] matriz;
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

            PilhaLista<Caminho> caminhos = new PilhaLista<Caminho>();
            PilhaLista<Caminho> possiveis = new PilhaLista<Caminho>();
            PilhaLista<Caminho> aux = new PilhaLista<Caminho>();

            int atual = lsbOrigem.SelectedIndex;
            bool acabouCaminho = false;
            bool[] visitados = new bool[arvore.QuantosDados];
            bool[] saidas = new bool[arvore.QuantosDados]; //quem leva para o destino

            while(!acabouCaminho)
            {
                for (int c = 0; c < arvore.QuantosDados; c++)
                {
                    if (matriz[atual, c] != default(int) && !visitados[c])
                        aux.Empilhar(new Caminho(atual, c));
                }

                if(aux.EstaVazia())
                    acabouCaminho = true;
                else
                {
                    Caminho um = aux.Desempilhar();

                    if (um.Destino == lsbDestino.SelectedIndex)
                    {
                        caminhos.Empilhar(um);
                        saidas[um.Origem] = true;
                    }
                    else
                    {
                        possiveis.Empilhar(um);
                        visitados[um.Origem] = true;
                        atual = um.Destino;
                    }
                }

            }

            while(!possiveis.EstaVazia())
            {
                Caminho um = possiveis.Desempilhar();
                if (saidas[um.Destino])
                    caminhos.Empilhar(um);
            }

            string[] cidades = new string[caminhos.Tamanho()];
            int cont = 0;

            if (caminhos.EstaVazia())
                MessageBox.Show("Não existe nenhum caminho disponível!");
            else
            {
                while (!caminhos.EstaVazia())
                {
                    Caminho caminho = caminhos.Desempilhar();

                    DataGridViewColumn d = new DataGridViewColumn();
                    d.HeaderText = "Cidade";
                    dataGridView1.Columns.Add(d);

                    Cidade cid = arvore.BuscaPorDado(new Cidade(caminho.Destino));
                    cidades[cont] = cid.Nome;
                    cont++;
                }
                dataGridView1.Rows.Add(cidades);
            }
               

            ////converterrrr
            //PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();
            //while (!caminhos.EstaVazia())
            //{
            //    aux2.Empilhar(caminhos.Desempilhar());
            //}

            ////Exibir

            //int qtdCidades = 0;

            //if (aux2.EstaVazia())
            //    MessageBox.Show("Não existe nenhum caminho disponível!");
            //else
            //    while(qtdCidades < aux2.Tamanho())
            //    {
            //        Caminho caminho = aux2.Desempilhar();

            //        DataGridViewColumn d = new DataGridViewColumn();
            //        d.HeaderText = caminho.Destino.ToString();
            //        dataGridView1.Columns.Add(d);

            //        DataGridViewRow n = new DataGridViewRow();
            //        n.HeaderCell.Value = caminho.Origem.ToString();
            //        dataGridView1.Rows.Add(n);


            //        qtdCidades++;
            //    }

            ////exibir preço da viagem

            //StreamReader arq = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.UTF7);

            //while (!arq.EndOfStream)
            //{
            //    string linha = arq.ReadLine();

            //    int origem = Convert.ToInt32(linha.Substring(0, 3));
            //    int destino = Convert.ToInt32(linha.Substring(3, 3));

            //    matriz[origem, destino] = Convert.ToInt32(linha.Substring(6, 5));
            //    //matriz[l, 0] = Convert.ToInt32(linha.Substring(6, 5));      
            //}

            //arq.Close();


            //while(!caminhos.EstaVazia())
            //{
            //    for(int i = 0; i < caminhos.Tamanho(); i++)
            //    {
            //        DataGridViewColumn d = new DataGridViewColumn();
            //        d.HeaderText = caminhos.;
            //        dataGridView1.Columns.Add(d);

            //        DataGridViewRow n = new DataGridViewRow();
            //        n.HeaderCell.Value = "Cidade";
            //        dataGridView1.Rows.Add();

            //        dataGridView1[]

            //        qtdCidades++;
            //    }
            //    Caminho c = caminhos.Desempilhar();
            //    dataGridView1[1, 1].Value = c.;

            //}


            //DataGridViewColumn dc3 = new DataGridViewColumn();
            //dc3.HeaderText = "Cidade";
            //dataGridView2.Columns.Add(dc3);


            //for(int l = 0; l < arvore.QuantosDados; l++)
            //{
            //    if (matriz[l, 0] == lsbOrigem.SelectedIndex)
            //        for(int c = 0; c < arvore.QuantosDados; c++)
            //            pilha.Empilhar(matriz[l, 0] + "," + matriz[l, c]);
            //        } 
            //}

            //PilhaLista<string> aux = new PilhaLista<string>();

            //while (!pilha.EstaVazia())
            //{
            //    for (int l = 0; l < arvore.QuantosDados; l++)
            //    {
            //        if (matriz[l, 0] == )
            //            for (int c = 0; c < arvore.QuantosDados; c++)
            //            {
            //                aux.Empilhar(matriz[l, 0] + "," + matriz[l, c]);
            //            }
            //    }

            //    pilha.Desempilhar();
            //}

            //StreamReader arq2 = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.UTF7);

            //while (!arq2.EndOfStream)
            //{
            //string linha2 = arq2.ReadLine();

            //int cod1Lido = int.Parse(linha2.Substring(0, 3));
            //int cod2Lido = int.Parse(linha2.Substring(3, 3));

            //int cod1Lsb = lsbOrigem.SelectedIndex;
            //int cod2Lsb = int.Parse(linha2.Substring(3, 3));

            //if (cod1Lido == cod1Lsb)
            //    pilha.Adicionar();

            //Pen blackPen = new Pen(Color.Black, 3);
            //Graphics grafico2 = pbMapa.CreateGraphics();

            //Point p1 = new Point();

            ////Cidade c1 = new Cidade();
            ////c1.cod = cod1;
            ////arvore.Existe(c1);
            //p1.X = arvore.Atual.Info.x * pbMapa.Width / 4096;
            //p1.Y = arvore.Atual.Info.y * pbMapa.Height / 2048;

            //Point p2 = new Point();

            ////Cidade c2 = new Cidade();
            ////c2.cod = cod2;
            ////arvore.Existe(c2);
            //p2.X = arvore.Atual.Info.x * pbMapa.Width / 4096;
            //p2.Y = arvore.Atual.Info.y * pbMapa.Height / 2048;

            ////if(cod1 == )

            //grafico2.DrawLine(blackPen, p1.X + 3, p1.Y + 3, p2.X + 3, p2.Y + 3);
            //}
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

            StreamReader arq = new StreamReader("CidadesMarte.txt", Encoding.UTF7);
            string linha = arq.ReadLine();
            Cidade cid = new Cidade(linha);

            lsbDestino.Items.Add(n+"-"+cid.Nome);
            lsbOrigem.Items.Add(n + "-" + cid.Nome);

            //DataGridViewColumn dc = new DataGridViewColumn();
            //dc.HeaderText = "Cidade";
            //dataGridView1.Columns.Add(dc);

            //DataGridViewColumn dc3 = new DataGridViewColumn();
            //dc3.HeaderText = "Cidade";
            //dataGridView2.Columns.Add(dc3);

            arvore.Raiz = new NoArvore<Cidade>(cid);
            Graphics grafico = e.Graphics;
            Point p = new Point();
            p.X = cid.X * pbMapa.Width / 4096;
            p.Y = cid.Y * pbMapa.Height / 2048;
            SolidBrush pincel = new SolidBrush(Color.Black);

            grafico.DrawString(cid.Nome, new Font("Arial", 10, FontStyle.Bold), pincel, new Point(p.X, p.Y - 20));
            grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            while (!arq.EndOfStream)
            {
                n++;
                linha = arq.ReadLine();
                cid = new Cidade(linha);
                lsbDestino.Items.Add(n + "-" + cid.Nome);
                lsbOrigem.Items.Add(n + "-" + cid.Nome);

                //DataGridViewColumn dc2 = new DataGridViewColumn();
                //dc2.HeaderText = "Cidade";
                //dataGridView1.Columns.Add(dc2);

                //DataGridViewColumn dc4 = new DataGridViewColumn();
                //dc4.HeaderText = "Cidade";
                //dataGridView2.Columns.Add(dc4);

                arvore.Incluir(cid);
                //grafico = e.Graphics;
                p = new Point();
                p.X = cid.X * pbMapa.Width / 4096;
                p.Y = cid.Y * pbMapa.Height / 2048;
                grafico.DrawString(cid.Nome, new Font("Arial", 10, FontStyle.Bold), pincel, new Point(p.X, p.Y - 20));
                pincel = new SolidBrush(Color.Black);
                grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            }

            arq.Close();

            CriarMatriz();
        }

        public void CriarMatriz()
        {
            matriz = new int[arvore.QuantosDados, arvore.QuantosDados];

            StreamReader arq = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.UTF7);           

            while(!arq.EndOfStream)
            {
                string linha = arq.ReadLine();

                int origem = Convert.ToInt32(linha.Substring(0, 3));
                int destino = Convert.ToInt32(linha.Substring(3, 3));

                matriz[origem, destino] = Convert.ToInt32(linha.Substring(6, 5));
                //matriz[l, 0] = Convert.ToInt32(linha.Substring(6, 5));      
            }

            arq.Close();
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DesenharArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, g);
        }

        private void DesenharArvore(bool primeiraVez, NoArvore<Cidade> raiz,  int x, int y, double angulo, double incremento,    double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                // sleep(100);
                DesenharArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,  incremento * 0.50, comprimento * 0.8, g);
                DesenharArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,  incremento * 0.50, comprimento * 0.8, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 35, yf - 15, 90, 90);
                g.DrawString(Convert.ToString(raiz.Info.Nome), new Font("Comic Sans", 12), new SolidBrush(Color.Black), xf - 35, yf + 10);
            }
        }
    }
}
