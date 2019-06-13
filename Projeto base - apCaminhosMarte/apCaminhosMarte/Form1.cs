﻿using System;
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
        object[] vetorCaminhos;
        int lClick = 0;

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

            int qtdCaminhos = 0;
            int atual = lsbOrigem.SelectedIndex;
            bool acabouCaminho = false;
            bool[] visitados = new bool[arvore.QuantosDados];
            bool[] saidas = new bool[arvore.QuantosDados]; //quem leva para o destino


            while (!acabouCaminho)
            {
                for (int c = 0; c < arvore.QuantosDados; c++)
                {
                    if (matriz[atual, c] != default(int) && !visitados[c])
                        aux.Empilhar(new Caminho(atual, c, matriz[atual, c], 0));
                }

                if (aux.EstaVazia())
                    acabouCaminho = true;
                else
                {
                    Caminho um = aux.Desempilhar();

                    if (um.Destino == lsbDestino.SelectedIndex)
                    {
                        caminhos.Empilhar(um);
                        saidas[um.Origem] = true;
                        qtdCaminhos++;
                    }
                    else
                    {
                        possiveis.Empilhar(um);
                        visitados[um.Origem] = true;
                        atual = um.Destino;
                    }
                }
            }

            vetorCaminhos = new object[qtdCaminhos];
            int indice = 0;

            while (!possiveis.EstaVazia())
            {
                Caminho um = possiveis.Desempilhar();
                if (saidas[um.Destino])
                    caminhos.Empilhar(um);
            }

            if (caminhos.EstaVazia())
                MessageBox.Show("Não existe nenhum caminho disponível!");
            else
            {
                PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();
                bool acabou = false;

                int origem = lsbOrigem.SelectedIndex;

                while(!acabou)
                {
                    while (!caminhos.EstaVazia())
                    {

                        if (caminhos.OTopo().Origem == origem)
                        {
                            aux.Empilhar(caminhos.OTopo());
                            origem = aux.OTopo().Destino;

                        }
                        else
                            aux2.Empilhar(caminhos.OTopo());

                        caminhos.Desempilhar();
                    }
                    
                    if (origem == lsbDestino.SelectedIndex)
                    {
                        caminhos = aux2;

                        PilhaLista<Caminho> outra = new PilhaLista<Caminho>();

                        while (!aux.EstaVazia())
                            outra.Empilhar(aux.Desempilhar());

                        aux = outra;

                        if (!aux2.EstaVazia())
                            caminhos.Empilhar(aux.OTopo());

                        string[] nomes = new string[23];
                        int n = 0;
                        
                        while (!aux.EstaVazia())
                        {
                            origem = lsbOrigem.SelectedIndex;
                           

                            Cidade c = arvore.BuscaPorDado(new Cidade(aux.Desempilhar().Origem));
                            nomes[n] = c.Nome;
                            n++;
                        }

                        Cidade cidade = arvore.BuscaPorDado(new Cidade(lsbDestino.SelectedIndex));
                        nomes[n] = cidade.Nome;
                        n++;

                        vetorCaminhos[indice] = new Cidade(default(int), nomes[indice], default(int), default(int));
                        indice++;

                        int index = dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].SetValues(nomes.N);
                    }
                    else
                        acabou = true;

                }
            }
            dataGridView1.Rows.RemoveAt(0);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            LerArquivo(e);

        }

        private void LerArquivo(PaintEventArgs e)
        {
            int n = 0;

            StreamReader arq = new StreamReader("CidadesMarte.txt", Encoding.UTF7);
            StreamReader aux = new StreamReader("CidadesMarteOrdenado.txt", Encoding.UTF7);
            string linha = arq.ReadLine();
            string linha2 = aux.ReadLine();
            Cidade cid = new Cidade(linha);
            Cidade cid2 = new Cidade(linha2);

            lsbDestino.Items.Add(n+"-"+cid2.Nome);
            lsbOrigem.Items.Add(n + "-" + cid2.Nome);

            arvore.Raiz = new NoArvore<Cidade>(cid);
            Graphics grafico = e.Graphics;
            Point p = new Point();
            p.X = cid.X * pbMapa.Width / 4096;
            p.Y = cid.Y * pbMapa.Height / 2048;
            SolidBrush pincel = new SolidBrush(Color.Black);

            grafico.DrawString(cid.Nome, new Font("Arial", 10, FontStyle.Bold), pincel, new Point(p.X, p.Y - 20));
            grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));
            while (!arq.EndOfStream )
            {
                n++;
                linha = arq.ReadLine();
                linha2 = aux.ReadLine();
                cid = new Cidade(linha);
                cid2 = new Cidade(linha2);
                lsbDestino.Items.Add(n + "-" + cid2.Nome);
                lsbOrigem.Items.Add(n + "-" + cid2.Nome);
                
                arvore.Incluir(cid);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lClick = e.RowIndex;
            EscreverLinha();
        }

        private void EscreverLinha()
        {
            Graphics g = pbMapa.CreateGraphics();
            Pen caneta = new Pen(Color.Red);
            caneta.Width = 20;
            string[] str = (string[])vetorCaminhos[lClick];
            Cidade[] cid = new Cidade[str.Length];
            

            for(int i = 0; i < cid.Length; i++)
            {
                int xp = cid[i].X;
                int yp = cid[i].Y;

                int xf = cid[i + 1].X;
                int yf = cid[i + 1].Y;

                g.DrawLine(caneta, xp, yp, xf, yf);
            }
        }
    }
}
