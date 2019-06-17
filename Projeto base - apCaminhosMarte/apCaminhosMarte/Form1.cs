using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Ana Clara Sampaio Pires RA: 18201
//Ariane Paula Barros     RA: 18173

namespace apCaminhosMarte
{
    public partial class Form1 : Form
    {
        Arvore<Cidade> arvore;   //Declaração da árvore usada
        int[,] matriz;           // Declaração do grafo percorrido na procura de caminhos
        object[] vetorCaminhos;  //vetor usado para guardar, em cada posição, um caminho encontrado e guardado em um vetor de int
        int[] melhorCaminho;       // Variável que guarda a linha seleciona pelo usuário para que o caminho escolhido seja exibido no mapa

        public Form1()
        {
            InitializeComponent();
            arvore = new Arvore<Cidade>();  //Instanciação da árvore 
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {

        }

        private void BtnBuscar_Click(object sender, EventArgs e) //Método chamado no evento click do btnBuscar
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");

            pbMapa.Refresh();   //Método que "limpa" o mapa, apagando o caminho que possivelmente estava sendo exibido antes

            dataGridView1.Rows.Clear();  //Método que apaga as linhas populadas com o caminho escolhido anteriormente peo usuário
            PilhaLista<Caminho> caminhos = new PilhaLista<Caminho>();   //Declaração e instanciação da pilha de caminhos que guardará todos os caminhos encontrados que levarão ao destino desejado
            PilhaLista<Caminho> possiveis = new PilhaLista<Caminho>();  //Declaração e instanciação da pilha de caminhos que guardará todos os possíveis caminhos, cada rota que possivelmente levará ao destino escolhido
            PilhaLista<Caminho> aux = new PilhaLista<Caminho>();  //Declaração e instanciação da pilha de caminhos que inicialmente guardará todos os caminhos que tem como início a origem determinada

            int qtdCaminhos = 0;   //Variável que guarda o número de caminhos encontrados, usada mais tarde na instanciação do vetor de caminhos
            int atual = lsbOrigem.SelectedIndex;  //Variável que guarda o índice da cidade escolhida pelo usuário como origem
            bool acabouCaminho = false;   //Variável que guarda 'true' se a cidade destino foi alcançada ou 'false' caso não
            bool[] visitados = new bool[arvore.QuantosDados]; //Vetor do tipo boolean que guardará true sempre que a cidade correspondente foi verificada/visitada e false em caso contrário   
            bool[] saidas = new bool[arvore.QuantosDados]; //Vetor do tipo boolean que contém todos as cidades existentes no mapa, cada posição verificando se a cidade atual leva ou não ao destino
            int destino = lsbDestino.SelectedIndex; //Variável que guarda o índice da cidade escolhida pelo usuário como destino

            while (!acabouCaminho)   //Loop definido pela verificação do encontro ou não de um caminho 
            {
                for (int c = 0; c < arvore.QuantosDados; c++)    //Loop que se repete pelo número de cidades existentes
                {
                    if (matriz[atual, c] != default(int) && !visitados[c]) //Verificação: se o valor da posição atual da matriz é diferente de 0, ou seja, se entre os índices(cidades) definidos existe uma rota 
                        aux.Empilhar(new Caminho(atual, c, matriz[atual, c], 0));  //Caso exista uma rota entre essas cidades, essa será empilhada na pilha
                }

                if (aux.EstaVazia())     //Verificação: se a pilha aux não tiver elementos, então não é possível prosseguir a procura por um caminho
                    acabouCaminho = true; //Portanto, para sair do loop, a variável acabouCaminho recebe true
                else  //Caso a pilha aux não esteja vazia, a busca por um caminho deve presseguir
                {
                    Caminho um = aux.Desempilhar();   //Variável local que guarda o caminho analisado atualmente

                    if (um.Destino == lsbDestino.SelectedIndex) //Verificação: se um caminho foi encontrado
                    {
                        caminhos.Empilhar(um);     //Empilhamos o novo caminho encontrado na pilha caminhos
                        saidas[um.Origem] = true;   //Como achamos uma saída (a cidade que leva ao destino), sua posição correspondente receberá 'true'
                        qtdCaminhos++;              //Acrescentamos uma unidade à variável qtdCaminhos
                        visitados[um.Destino] = true;
                    }
                    else
                    {
                        possiveis.Empilhar(um);  //Ao acharmos uma possível rota, ela é guardada na pilha de possíveis rotas
                        atual = um.Destino;      //Mudamos o valor da variável atual, que passará a guardar a origem do caminho que será verificado posteriormente
                    }
                    visitados[um.Origem] = true;
                }
            }

            vetorCaminhos = new object[qtdCaminhos];  //instanciação do vetor de caminhos, com o tamanho referente à quantidade de camihos encontrada
            int indice = 0;  //Declaração da variável responsável por representar a posição do vetor declarado acima em que o próximo caminho deverá ser armazenado

            while (!possiveis.EstaVazia())   //Loop responsável por obter todos os caminhos definitivos que estão na pilha de possíveis caminhos
            {
                Caminho um = possiveis.Desempilhar();    //Variável local que guarda o caminho que será verificado
                if (saidas[um.Destino])     //Verficação: se o caminho guardado leverá ao destino
                {
                    caminhos.Empilhar(um);   //Caso o caminho atual seja uma solução, é empilhado na pilha caminhos
                    saidas[um.Origem] = true;  //A origem do caminho encontrado guarda o destino de outro possível caminho, portanto sua posição correspondente no vetor passa a guardar 'true'
                }
            }

            if (caminhos.EstaVazia())  //Verificação: se a pilha definitiva de caminhos está vazia, alertamos ao usuário que não existe um caminho
                MessageBox.Show("Não existe nenhum caminho disponível!");
            else
            {
                //PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();
                melhorCaminho = new int[1];
                int caminhoAnterior = Int32.MaxValue;

                //aux = aux.Copiar(caminhos);

                int origem = lsbOrigem.SelectedIndex;   //Variável que guarda o índice da cidade escolhida como origem pelo usuário                        

                int qtdCaminhosExibidos = 0;

                PilhaLista<Caminho> todos = new PilhaLista<Caminho>();

                while (qtdCaminhosExibidos != qtdCaminhos)
                {
                    PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();

                    //PilhaLista<Caminho> copia = todos.Clone();
                    //if (!caminhos.EstaVazia())
                    //{
                    //    while (!copia.EstaVazia())
                    //    {
                    //        bool pode = true;
                    //        if (copia.OTopo().Destino == caminhos.OTopo().Origem)
                    //        {
                    //            PilhaLista<Caminho> outra = caminhos.Clone();
                    //            while (!outra.EstaVazia())
                    //            {
                    //                if (caminhos.OTopo().Origem == outra.OTopo().Destino)
                    //                    pode = false;
                    //                outra.Desempilhar();
                    //            }
                    //            if (pode)
                    //            {
                    //                caminhos.Empilhar(copia.OTopo());
                    //            }
                    //        }
                    //        copia.Desempilhar();
                    //    }
                    //}
                    PilhaLista<Caminho>[] vetorCaminhosSeparados = new PilhaLista<Caminho>[qtdCaminhos];
                    int aimeudeus = 0;

                    while (aimeudeus < qtdCaminhos)
                    {
                        bool ok = false;
                        while(!ok) //enquanto um caminho não for encontrado
                        {  
                            while (!caminhos.EstaVazia())
                            {
                                if (caminhos.OTopo().Origem == origem)
                                {
                                    aux.Empilhar(caminhos.OTopo());
                                    origem = aux.OTopo().Destino;
                                }
                                aux2.Empilhar(caminhos.OTopo());
                                caminhos.Desempilhar();
                            }

                            if (aux.OTopo().Destino != lsbDestino.SelectedIndex)
                            {
                                while(aux2.OTopo() != aux.OTopo())
                                {
                                    caminhos.Empilhar(aux2.Desempilhar());
                                }
                                aux = new PilhaLista<Caminho>();
                                aux2.Desempilhar();
                                while(!aux2.EstaVazia())
                                    caminhos.Empilhar(aux2.Desempilhar());
                                origem = lsbOrigem.SelectedIndex;
                            }
                            else
                                ok = true;
                        }


                        vetorCaminhosSeparados[aimeudeus] = aux;
                        aimeudeus++;

                        //aux = aux.Inverter();

                        while (aux2.OTopo().CompareTo(aux.OTopo()) != 0)//aqui
                        {
                            caminhos.Empilhar(aux2.Desempilhar());
                        }

                        aux2.Desempilhar();

                        while (!aux2.EstaVazia())
                            caminhos.Empilhar(aux2.Desempilhar());

                        aux = new PilhaLista<Caminho>();
                        origem = lsbOrigem.SelectedIndex;
                    }

                    string[] nomes = new string[23];
                    int[] cod = new int[23];
                    int n = 0;
                    int m = 0;
                    string[] nomeMelhor = new string[1];
                    int distanciaAtual = 0;

                    for(int a = 0; a < aimeudeus; a++)
                    {
                        nomes = new string[23];
                        n = 0;
                        aux = vetorCaminhosSeparados[a];
                        
                        aux = aux.Inverter();

                        while (!aux.EstaVazia())
                        {
                            Caminho caminho = aux.Desempilhar();
                            todos.Empilhar(caminho);

                            Cidade c = arvore.BuscaPorDado(new Cidade(caminho.Origem));
                            nomes[n] = c.Nome;
                            cod[n] = c.Cod;
                            n++;

                            distanciaAtual += caminho.Distancia;
                        }

                        Cidade cidade = arvore.BuscaPorDado(new Cidade(lsbDestino.SelectedIndex));
                        nomes[n] = cidade.Nome;
                        cod[n] = cidade.Cod;
                        n++;

                        if (caminhoAnterior > distanciaAtual)
                        {
                            m = indice;
                            nomeMelhor = nomes;
                            caminhoAnterior = distanciaAtual;
                        }

                        vetorCaminhos[indice] = cod.Clone();  //Atribuição do vetor de códigos de cidades à posição atual do vetor de caminhos
                        indice++; //Acrescentamos uma unidade à variável que guarda a posição em que o próximo caminho deverá ser guardado

                        int index = dataGridView1.Rows.Add();  //Índice da linha adicionada no data grid view
                        dataGridView1.Rows[index].SetValues(nomes); //Atribuição do caminho atual à linha adicionada 


                        qtdCaminhosExibidos++;
                        cod = new int[23];
                        distanciaAtual = 0;
                    }
                    ExibirMelhorCaminho(nomeMelhor);
                    melhorCaminho = (int[])vetorCaminhos[m];

                    //while (!caminhos.EstaVazia())
                    //{

                    //    if (caminhos.OTopo().Origem == origem)
                    //    {
                    //        aux.Empilhar(caminhos.OTopo());
                    //        origem = aux.OTopo().Destino;                            
                    //    }
                    //    aux2.Empilhar(caminhos.OTopo());
                    //    caminhos.Desempilhar();
                    //}

                    //        aux2 = aux2.Inverter();

                    //        while (!aux2.EstaVazia())
                    //        {
                    //            //if (!aux.Existe(aux2.OTopo()))
                    //            //{
                    //                //if (aux2.OTopo().Destino == origem)
                    //                //{
                    //                    caminhos.Empilhar(aux2.OTopo());
                    //                    //origem = aux2.OTopo().Destino;
                    //                //}                         
                    //            //}

                    //            aux2.Desempilhar();
                    //        }

                    //        aux = aux.Inverter();

                    //        string[] nomes = new string[23];
                    //        int[] cod = new int[23];
                    //        int n = 0;
                    //        int distanciaAtual = 0;

                    //        while (!aux.EstaVazia())
                    //        {
                    //            origem = lsbOrigem.SelectedIndex;

                    //            Caminho caminho = aux.Desempilhar();
                    //            todos.Empilhar(caminho);

                    //            Cidade c = arvore.BuscaPorDado(new Cidade(caminho.Origem));
                    //            nomes[n] = c.Nome;
                    //            cod[n] = c.Cod;
                    //            n++;

                    //            distanciaAtual += caminho.Distancia;
                    //        }

                    //        Cidade cidade = arvore.BuscaPorDado(new Cidade(lsbDestino.SelectedIndex));
                    //        nomes[n] = cidade.Nome;
                    //        cod[n] = cidade.Cod;
                    //        n++;

                    //        if (caminhoAnterior > distanciaAtual)
                    //        {
                    //            melhorCaminho = nomes;
                    //            caminhoAnterior = distanciaAtual;
                    //        }

                    //        vetorCaminhos[indice] = cod;  //Atribuição do vetor de códigos de cidades à posição atual do vetor de caminhos
                    //        indice++; //Acrescentamos uma unidade à variável que guarda a posição em que o próximo caminho deverá ser guardado

                    //        int index = dataGridView1.Rows.Add();  //Índice da linha adicionada no data grid view
                    //        dataGridView1.Rows[index].SetValues(nomes); //Atribuição do caminho atual à linha adicionada 

                    //        qtdCaminhosExibidos++;
                    //    }

                    //    ExibirMelhorCaminho(melhorCaminho);  //Depois de todos os caminhos terem sido analisados, o melhor deles é exibido no dataGridView2
                    //}

                    //int qtdCaminhosExibidos = 0;

                    //if (caminhos.EstaVazia())  //Verificação: se a pilha definitiva de caminhos está vazia, alertamos ao usuário que não existe um caminho
                    //    MessageBox.Show("Não existe nenhum caminho disponível!");
                    //else
                    //{
                    //    PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();
                    //    bool acabou = false;
                    //    melhorCaminho = new string[1];
                    //    int caminhoAnterior = Int32.MaxValue;

                    //    int origem = lsbOrigem.SelectedIndex;   //Variável que guarda o índice da cidade escolhida como origem pelo usuário

                    //    while (!acabou)
                    //    {
                    //        while (!caminhos.EstaVazia())
                    //        {
                    //            if (caminhos.OTopo().Origem == origem)
                    //            {
                    //                aux.Empilhar(caminhos.OTopo());
                    //                origem = aux.OTopo().Destino;
                    //            }
                    //            else
                    //                aux2.Empilhar(caminhos.OTopo());

                    //            caminhos.Desempilhar();
                    //        }

                    //        if (origem == lsbDestino.SelectedIndex)
                    //        {
                    //            caminhos = aux2;

                    //            PilhaLista<Caminho> outra = new PilhaLista<Caminho>();

                    //            while (!aux.EstaVazia())
                    //                outra.Empilhar(aux.Desempilhar());

                    //            aux = outra;

                    //            //if (!aux2.EstaVazia())
                    //            //    caminhos.Empilhar(aux.OTopo());

                    //            string[] nomes = new string[23];
                    //            int[] cod = new int[23];
                    //            int n = 0;
                    //            int distanciaAtual = 0;

                    //            while (!aux.EstaVazia())
                    //            {
                    //                origem = lsbOrigem.SelectedIndex;

                    //                Caminho caminho = aux.Desempilhar();

                    //                Cidade c = arvore.BuscaPorDado(new Cidade(caminho.Origem));
                    //                nomes[n] = c.Nome;
                    //                cod[n] = c.Cod;
                    //                n++;

                    //                distanciaAtual += caminho.Distancia;
                    //            }

                    //            Cidade cidade = arvore.BuscaPorDado(new Cidade(lsbDestino.SelectedIndex));
                    //            nomes[n] = cidade.Nome;
                    //            cod[n] = cidade.Cod;
                    //            n++;

                    //            if (caminhoAnterior > distanciaAtual)
                    //            {
                    //                melhorCaminho = nomes;
                    //                caminhoAnterior = distanciaAtual;
                    //            }

                    //            vetorCaminhos[indice] = cod;  //Atribuição do vetor de códigos de cidades à posição atual do vetor de caminhos
                    //            indice++; //Acrescentamos uma unidade à variável que guarda a posição em que o próximo caminho deverá ser guardado

                    //            int index = dataGridView1.Rows.Add();  //Índice da linha adicionada no data grid view
                    //            dataGridView1.Rows[index].SetValues(nomes); //Atribuição do caminho atual à linha adicionada 
                    //        }
                    //        else
                    //            if(qtdCaminhosExibidos == qtdCaminhos)
                    //             acabou = true; //Se chegamos ao destino, então o caminho acabou, assim atribuímos 'true' para a variável e paramos o loop

                    //        qtdCaminhosExibidos++;
                    //    }
                    //    ExibirMelhorCaminho(melhorCaminho);  //Depois de todos os caminhos terem sido analisados, o melhor deles é exibido no dataGridView2
                    //}


                    //if (caminhos.EstaVazia())  //Verificação: se a pilha definitiva de caminhos está vazia, alertamos ao usuário que não existe um caminho
                    //    MessageBox.Show("Não existe nenhum caminho disponível!");
                    //else
                    //{
                    //    PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();
                    //    bool acabou = false;
                    //    melhorCaminho = new string[1];
                    //    int caminhoAnterior = Int32.MaxValue;

                    //    int origem = lsbOrigem.SelectedIndex;   //Variável que guarda o índice da cidade escolhida como origem pelo usuário

                    //    while (!acabou)
                    //    {
                    //        while (!caminhos.EstaVazia())
                    //        {
                    //            if (caminhos.OTopo().Origem == origem)
                    //            {
                    //                aux.Empilhar(caminhos.OTopo());
                    //                origem = aux.OTopo().Destino;
                    //            }
                    //            else
                    //                aux2.Empilhar(caminhos.OTopo());

                    //            caminhos.Desempilhar();
                    //        }

                    //        if (origem == lsbDestino.SelectedIndex)
                    //        {
                    //            caminhos = aux2;

                    //            PilhaLista<Caminho> outra = new PilhaLista<Caminho>();

                    //            while (!aux.EstaVazia())
                    //                outra.Empilhar(aux.Desempilhar());

                    //            aux = outra;

                    //            if (!aux2.EstaVazia())
                    //                caminhos.Empilhar(aux.OTopo());

                    //            string[] nomes = new string[23];
                    //            int[] cod = new int[23];
                    //            int n = 0;
                    //            int distanciaAtual = 0;

                    //            while (!aux.EstaVazia())
                    //            {
                    //                origem = lsbOrigem.SelectedIndex;

                    //                Caminho caminho = aux.Desempilhar();

                    //                Cidade c = arvore.BuscaPorDado(new Cidade(caminho.Origem));
                    //                nomes[n] = c.Nome;
                    //                cod[n] = c.Cod;
                    //                n++;

                    //                distanciaAtual += caminho.Distancia;
                    //            }

                    //            Cidade cidade = arvore.BuscaPorDado(new Cidade(lsbDestino.SelectedIndex));
                    //            nomes[n] = cidade.Nome;
                    //            cod[n] = cidade.Cod;
                    //            n++;

                    //            if (caminhoAnterior > distanciaAtual)
                    //            {
                    //                melhorCaminho = nomes;
                    //                caminhoAnterior = distanciaAtual;
                    //            }

                    //            vetorCaminhos[indice] = cod;  //Atribuição do vetor de códigos de cidades à posição atual do vetor de caminhos
                    //            indice++; //Acrescentamos uma unidade à variável que guarda a posição em que o próximo caminho deverá ser guardado

                    //            int index = dataGridView1.Rows.Add();  //Índice da linha adicionada no data grid view
                    //            dataGridView1.Rows[index].SetValues(nomes); //Atribuição do caminho atual à linha adicionada 
                    //        }
                    //        else
                    //            acabou = true; //Se chegamos ao destino, então o caminho acabou, assim atribuímos 'true' para a variável e paramos o loop
                    //    }
                    //    ExibirMelhorCaminho(melhorCaminho);  //Depois de todos os caminhos terem sido analisados, o melhor deles é exibido no dataGridView2
                }
            }
        }

        private void ExibirMelhorCaminho(string[] vet) //Método responsável por exibir o melhor caminho dentre todos os achados no dataGridView2
        {
            if (dataGridView2.RowCount == 1)
                dataGridView2.Rows.RemoveAt(0);
            int index = dataGridView2.Rows.Add();  //Índice da linha adicionada no data grid view
            dataGridView2.Rows[index].SetValues(vet);  //Atribuição do caminho atual à linha adicionada
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
            int qtdCidades = 1;

            StreamReader arq = new StreamReader("CidadesMarte.txt", Encoding.UTF7);
            StreamReader aux = new StreamReader("CidadesMarteOrdenado.txt", Encoding.UTF7);
            string linha = arq.ReadLine();
            string linha2 = aux.ReadLine();
            Cidade cid = new Cidade(linha);
            Cidade cid2 = new Cidade(linha2);

            if (lsbOrigem.Items.Count < 23)
            {
                lsbDestino.Items.Add(n + "-" + cid2.Nome);
                lsbOrigem.Items.Add(n + "-" + cid2.Nome);

                arvore.Raiz = new NoArvore<Cidade>(cid);
            }
           
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
                linha2 = aux.ReadLine();
                cid = new Cidade(linha);
                cid2 = new Cidade(linha2);

                if (lsbOrigem.Items.Count < 23)
                {
                    lsbDestino.Items.Add(n + "-" + cid2.Nome);
                    lsbOrigem.Items.Add(n + "-" + cid2.Nome);

                    arvore.Incluir(cid);
                }
                
                p = new Point();
                p.X = cid.X * pbMapa.Width / 4096;
                p.Y = cid.Y * pbMapa.Height / 2048;
                grafico.DrawString(cid.Nome, new Font("Arial", 10, FontStyle.Bold), pincel, new Point(p.X, p.Y - 20));
                pincel = new SolidBrush(Color.Black);
                grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));

                qtdCidades++;
            }

            arq.Close();
            aux.Close();

            if (lsbOrigem.Items.Count == 23)
            {
                for (int i = 0; i < qtdCidades; i++) //Loop para adicionarmos colunas de acordo com o número de cidades do arquivo
                    dataGridView2.Columns.Add("x", "Cidade");

                CriarMatriz();
            }
        }

        public void CriarMatriz() //Método para criar o grafo
        {
            matriz = new int[arvore.QuantosDados, arvore.QuantosDados]; //essa matriz será usada para armazenar as distâncias entre cada cidade

            //Leitura do arquivo que contém as cidades e a distância entre elas
            StreamReader arq = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.UTF7);           

            while(!arq.EndOfStream) ///Loop que ocorre até que o arquivo seja completamente lido
            {
                string linha = arq.ReadLine();   //variável que guarda todas as informações lidas na linha atual do arquivo

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
                DesenharArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,  incremento * 0.50, comprimento * 0.8, g);
                DesenharArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,  incremento * 0.50, comprimento * 0.8, g);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 35, yf - 15, 90, 90);
                g.DrawString(Convert.ToString(raiz.Info.Nome), new Font("Comic Sans", 12), new SolidBrush(Color.Black), xf - 35, yf + 10);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {//Método disparado quando o usuário clica em alguma célula do dataGridView1
            int lClick = e.RowIndex;  //atribuição do índice da linha escolhida pelo usuário à variável global lClick
            EscreverLinha((int[])vetorCaminhos[lClick]);      //Chamada do método responsável por desenhar a linha na tela
        }
       
        private void EscreverLinha(int[] vetor) //Método responsável por exibir no mapa o caminho selecionado pelo usuário
        {
            pbMapa.Refresh();  //Método responsável por apagar tudo antes exibido em cima do mapa
            Graphics g = pbMapa.CreateGraphics();  //Atribuição do gráfico criado à variável g da classe Graphics
            Pen caneta = new Pen(Color.Black);  //Código que cria a variável que 'desenha' no mapa

            //Código que personaliza a linha que liga as cidades
            AdjustableArrowCap flecha = new AdjustableArrowCap(3, 3);
            caneta.CustomEndCap = flecha;
            caneta.Width = 3;

            int[] codCidades = vetor; //Declaração do vetor responsável por guardar o caminho selecionado por meio dos códigos da cidades que aparecem nas rotas
            int qtdZero = 0;  //Variável que guarda o número de vezes que o valor 0 foi encontrado no vetor. Seu valor pode ser no máximo 1 já que só é possível passar uma vez pela cidade cujo código é 0
            
            for(int i = 0; i < codCidades.Length-1; i++)  //Loop que percorre cada posição do vetor de códigos de cidades do caminho selecionado
            {
                Cidade cid = arvore.BuscaPorDado(new Cidade(codCidades[i]));
                int xp = cid.X* pbMapa.Width / 4096;  //Variável que guarda a coordenada X da cidade origem
                int yp = cid.Y * pbMapa.Height / 2048;  //Variável que guarda a coordenada Y da cidade origem

                Cidade cid2 = arvore.BuscaPorDado(new Cidade(codCidades[i + 1]));
                int xf = cid2.X * pbMapa.Width / 4096;  //Variável que guarda a coordenada X da cidade destino
                int yf = cid2.Y * pbMapa.Height / 2048;  //Variável que guarda a coordenada Y da cidade destino

                if (cid.Cod == 0 || cid2.Cod == 0) //Verifica se o código da cidade origem ou destino é igual a 0
                    qtdZero++;  //Caso o código da cidade atual seja 0, acrescentamos uma unidade à variável qtdZero

                if (qtdZero < 2) //Como o vetor usado é do tipo int, nenhuma de suas posições pode ser nula. Portanto, as não usadas são preenchidas atomaticamente com 0 e, para evitar que o caminho seja alterado, verificamos quantas vezes o valor 0 foi econtrado
                    g.DrawLine(caneta, xp+4, yp+2, xf+4, yf+2);  //Método responsável por desenha a linha na tela, com os parâmetros da caneta que será usada e as coordenadas x e y dos pontos que serão ligados pelas setas
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EscreverLinha(melhorCaminho);
        }

        private void pbMapa_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
