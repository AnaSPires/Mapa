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
        Arvore<Cidade> arvore;      //Declaração da árvore usada
        int[,] matriz;              // Declaração do grafo percorrido na procura de caminhos
        object[] vetorCaminhos;     //vetor usado para guardar, em cada posição, um caminho encontrado e guardado em um vetor de int
        double?[] melhorCaminho;        // Variável que guarda a linha seleciona pelo usuário para que o caminho escolhido seja exibido no mapa

        public Form1()
        {
            InitializeComponent();
            arvore = new Arvore<Cidade>();  //Instanciação da árvore 
        }
        
        private void BtnBuscar_Click(object sender, EventArgs e)             //Método chamado no evento click do btnBuscar
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");

            pbMapa.Refresh();                                               //Método que "limpa" o mapa, apagando o caminho que possivelmente estava sendo exibido antes

            dataGridView1.Rows.Clear();                                     //Método que apaga as linhas populadas com o caminho escolhido anteriormente peo usuário
            dataGridView2.Rows.Clear();                                     //Método que apaga as linhas populadas com o caminho escolhido anteriormente peo usuário
             
            PilhaLista<Caminho> caminhos = new PilhaLista<Caminho>();       //Declaração e instanciação da pilha de caminhos que guardará todos os caminhos encontrados que levarão ao destino desejado
            PilhaLista<Caminho> possiveis = new PilhaLista<Caminho>();      //Declaração e instanciação da pilha de caminhos que guardará todos os possíveis caminhos, cada rota que possivelmente levará ao destino escolhido
            PilhaLista<Caminho> aux = new PilhaLista<Caminho>();            //Declaração e instanciação da pilha de caminhos que inicialmente guardará todos os caminhos que tem como início a origem determinada

            int qtdCaminhos = 0;                                            //Variável que guarda o número de caminhos encontrados, usada mais tarde na instanciação do vetor de caminhos
            int atual = lsbOrigem.SelectedIndex;                            //Variável que guarda o índice da cidade escolhida pelo usuário como origem
            bool acabouCaminho = false;                                     //Variável que guarda 'true' se a cidade destino foi alcançada ou 'false' caso não
            bool[] visitados = new bool[arvore.QuantosDados];               //Vetor do tipo boolean que guardará true sempre que a cidade correspondente foi verificada/visitada e false em caso contrário   
            /*bool[] saidas = new bool[arvore.QuantosDados];    */              //Vetor do tipo boolean que contém todos as cidades existentes no mapa, cada posição verificando se a cidade atual leva ou não ao destino


            vetorCaminhos = new object[33];                         //instanciação do vetor de caminhos, com o tamanho referente à quantidade de camihos encontrada
            int indice = 0;                                                  //Declaração da variável responsável por representar a posição do vetor declarado acima em que o próximo caminho deverá ser armazenado

            int qtd = arvore.QuantosDados;

            while (!acabouCaminho)                                          //Loop definido pela verificação do encontro ou não de um caminho 
            {
                bool empilhou = false;
                for (int c = 0; c < qtd; c++)               //Loop que se repete pelo número de cidades existentes
                {
                    if (matriz[atual, c] != default(int))         //Verificação: se o valor da posição atual da matriz é diferente de 0, ou seja, se entre os índices(cidades) definidos existe uma rota 
                    {
                        aux.Empilhar(new Caminho(atual, c, matriz[atual, c], 0));  //Caso exista uma rota entre essas cidades, essa será empilhada na pilha
                        empilhou = true;
                    }
                }

                if (!empilhou)
                {
                    while (!aux.EstaVazia() &&  !possiveis.EstaVazia() && aux.OTopo().Origem != possiveis.OTopo().Destino)
                        possiveis.Desempilhar();
                }


                if (aux.EstaVazia())                                        //Verificação: se a pilha aux não tiver elementos, então não é possível prosseguir a procura por um caminho
                    acabouCaminho = true;                                   //Portanto, para sair do loop, a variável acabouCaminho recebe true
                else                                                        //Caso a pilha aux não esteja vazia, a busca por um caminho deve presseguir
                {
                    Caminho um = aux.Desempilhar();                          //Variável local que guarda o caminho analisado atualmente

                    if (um.Destino == lsbDestino.SelectedIndex)              //Verificação: se um caminho foi encontrado
                    {
                        
                          /*  caminhos.Empilhar(um);      */                         //Empilhamos o novo caminho encontrado na pilha caminhos
                           /* saidas[um.Origem] = true;*/                            //Como achamos uma saída (a cidade que leva ao destino), sua posição correspondente receberá 'true'
                        possiveis.Empilhar(um);

                        vetorCaminhos[indice] = possiveis.Clone().Inverter();
                        
                        while (!aux.EstaVazia() && !possiveis.EstaVazia() && aux.OTopo().Origem != possiveis.OTopo().Destino)
                            possiveis.Desempilhar();

                        possiveis.Empilhar(um);
                        atual = um.Destino;
                        caminhos = possiveis.Clone();
                        int i = 0;
                        dataGridView1.RowCount++;
                        dataGridView1.ColumnCount = caminhos.Tamanho() + 1;
                        while(!caminhos.EstaVazia())
                        {
                            Caminho c = caminhos.Desempilhar();
                            if (i == 0)
                                dataGridView1[i, dataGridView1.RowCount - 1].Value = c.Origem;

                            dataGridView1[++i, dataGridView1.RowCount - 1].Value = c.Destino;
                        }
                            



                        qtdCaminhos++;                                       //Acrescentamos uma unidade à variável qtdCaminhos
                        indice++;

                        //PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();
                        //int destino = lsbDestino.SelectedIndex;

                        //while (!possiveis.EstaVazia())                     //Loop: enquanto a pilha não tiver sido completamente percorrida, continuamos no loop
                        //{
                        //    if (possiveis.OTopo().Destino == destino)        //Verificação: se o topo da pilha caminhos tiver como origem a origem procurada, este caminho representa uma rota válida, portanto é armazenado
                        //    {
                        //        caminhos.Empilhar(possiveis.OTopo());           //Armazenamento da continuação do caminho na pilha auxiliar, agora reponsável por guardar um caminho completo apenas(da origem até seu destino)
                        //        destino = caminhos.OTopo().Origem;             //a variável origem deve guardar o destino do caminho encontrado, para que a continuação do caminho seja encontrada
                        //    }
                        //    aux2.Empilhar(possiveis.OTopo());
                        //    possiveis.Desempilhar();
                        //}

                        //vetorCaminhos[indice] = caminhos.Clone().Inverter();
                        //string[] nomes = new string[caminhos.Tamanho()];

                        //while (!possiveis.EstaVazia() && aux.OTopo().Origem != possiveis.OTopo().Destino)
                        //    possiveis.Desempilhar();

                        //for(int i = 0; i < caminhos.Tamanho(); i++)
                        //{
                        //    Caminho caminho = caminhos.Desempilhar();
                        //    Cidade c = arvore.BuscaPorDado(new Cidade(caminho.Origem));
                        //    nomes[i] = c.Nome;
                        //}

                        //int index = dataGridView1.Rows.Add();                //Índice da linha adicionada no data grid view
                        //dataGridView1.Rows[index].SetValues(nomes);          //Atribuição do caminho atual à linha adicionada 

                    }
                    else
                    {
                        possiveis.Empilhar(um);                              //Ao acharmos uma possível rota, ela é guardada na pilha de possíveis rotas
                        atual = um.Destino;                                 //Mudamos o valor da variável atual, que passará a guardar a origem do caminho que será verificado posteriormente

                    }

                }
            }

            //PilhaLista<Caminho> outra = caminhos.Clone();
            //while (!outra.EstaVazia())                                   //Loop responsável por obter todos os caminhos definitivos que estão na pilha de possíveis caminhos
            //{
            //    PilhaLista<Caminho> outra2 = possiveis.Clone();
            //    Caminho um = outra.Desempilhar();                              //Caso o caminho atual seja uma solução, é empilhado na pilha caminhos

            //    saidas[um.Origem] = true;
            //    int origem = um.Origem;
            //    while (origem != lsbOrigem.SelectedIndex)
            //    {
            //        while (!outra2.EstaVazia())                                   //Loop responsável por obter todos os caminhos definitivos que estão na pilha de possíveis caminhos
            //        {
            //            Caminho dois = outra2.Desempilhar();                        //Variável local que guarda o caminho que será verificado
            //            if (saidas[dois.Destino])                                      //Verficação: se o caminho guardado leverá ao destino
            //            {
            //                caminhos.Empilhar(dois);                                   //Caso o caminho atual seja uma solução, é empilhado na pilha caminhos
            //                saidas[dois.Origem] = true;                                //A origem do caminho encontrado guarda o destino de outro possível caminho, portanto sua posição correspondente no vetor passa a guardar 'true'
            //            }
            //            origem = dois.Origem;
            //        }

            //    }
            //}



            //if (au.EstaVazia())                                         //Verificação: se a pilha definitiva de caminhos está vazia, alertamos ao usuário que não existe um caminho
            //    MessageBox.Show("Não existe nenhum caminho disponível!");     //Mensagem exibida em um message box ao usuário caso não exista um caminho entre as cidades selecionadas
            //else
            //{
            //    melhorCaminho = new double?[1];                                   //instanciação do vetor responsável por armazenar o melhor caminho encontrado
            //    int caminhoAnterior = Int32.MaxValue;
                
            //    int origem = lsbOrigem.SelectedIndex;                         //Variável que guarda o índice da cidade escolhida como origem pelo usuário                        

            //    int qtdCaminhosExibidos = 0;                                  //Criação da variável responsável por guardar a quantidade de caminhos que já foram exibidos(percorridos/verificados)

            //    PilhaLista<Caminho> todos = new PilhaLista<Caminho>();    

            //    while (qtdCaminhosExibidos != qtdCaminhos)                     //Loop: enquanto todos os caminhos encotrados não tiverem sido exibidos
            //    {
            //        PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();      //Criação da pilha auxiliar
                    
            //        PilhaLista<Caminho>[] vetorCaminhosSeparados = new PilhaLista<Caminho>[qtdCaminhos];
            //        int aimeudeus = 0;                                        //Variável responsável por armazenar a quantidade de caminhos armazanados no vetor de caminhos separados

            //        while (aimeudeus < qtdCaminhos)                           //Loop: enquanto todos os caminhos não tiverem sido armazenados no vetor de caminhos separados
            //        {
            //            bool ok = false;                                      //Declaração da variável que controlará o loop abaixo, de acordo com a verificação se um caminho foi armazenado com sucesso
            //            while(!ok)                                            //enquanto um caminho não for encontrado
            //            {  
            //                while (!caminhos.EstaVazia())                     //Loop: enquanto a pilha não tiver sido completamente percorrida, continuamos no loop
            //                {
            //                    if (caminhos.OTopo().Origem == origem)        //Verificação: se o topo da pilha caminhos tiver como origem a origem procurada, este caminho representa uma rota válida, portanto é armazenado
            //                    {
            //                        aux.Empilhar(caminhos.OTopo());           //Armazenamento da continuação do caminho na pilha auxiliar, agora reponsável por guardar um caminho completo apenas(da origem até seu destino)
            //                        origem = aux.OTopo().Destino;             //a variável origem deve guardar o destino do caminho encontrado, para que a continuação do caminho seja encontrada
            //                    }
            //                    aux2.Empilhar(caminhos.OTopo());              
            //                    caminhos.Desempilhar();
            //                }

            //                if (aux.OTopo().Destino != lsbDestino.SelectedIndex)
            //                {
            //                    while(aux2.OTopo() != aux.OTopo())
            //                    {
            //                        caminhos.Empilhar(aux2.Desempilhar());
            //                    }
            //                    aux = new PilhaLista<Caminho>();
            //                    aux2.Desempilhar();
            //                    while(!aux2.EstaVazia())
            //                        caminhos.Empilhar(aux2.Desempilhar());
            //                    origem = lsbOrigem.SelectedIndex;
            //                }
            //                else
            //                    ok = true;
            //            }


            //            vetorCaminhosSeparados[aimeudeus] = aux;
            //            aimeudeus++;
                        
            //            while (aux2.OTopo().CompareTo(aux.OTopo()) != 0)
            //            {
            //                caminhos.Empilhar(aux2.Desempilhar());
            //            }

            //            aux2.Desempilhar();

            //            while (!aux2.EstaVazia())
            //                caminhos.Empilhar(aux2.Desempilhar());

            //            aux = new PilhaLista<Caminho>();
            //            origem = lsbOrigem.SelectedIndex;
            //        }

            //        string[] nomes = new string[23];                      //Declaração do vetor que guardará os nomes da cidades que compõem o caminho atual
            //        double?[] cod = new double?[23];                              //Declaração do vetor que guardará os códigos da cidades que compõem cada caminho
            //        int n = 0;
            //        int m = 0;
            //        string[] nomeMelhor = new string[1];                  //Declaração do vetor que guardará os nomes das cidades que compõem o melhor caminho
            //        int distanciaAtual = 0;                               //Variável que guarda a distância da rota entre as duas cidades analisadas atualmente, de acordo com o caminho fornecido

            //        for(int a = 0; a < aimeudeus; a++)
            //        {
            //            nomes = new string[arvore.QuantosDados];                             //Instanciação do vetor que guardará os nomes das cidades do caminho, de acordo com o número de cidades no arquivo
            //            n = 0;
            //            aux = vetorCaminhosSeparados[a];                                     //Atribuição da pilha com o caminho atual à pilha aux
                        
            //            aux = aux.Inverter();                                                //Os dados da pilha aux tem sua ordem invertida para que o caminho seja recuperado na ordem correta

            //            while (!aux.EstaVazia())                                             //Loop: enquanto a pilha aux não estiver vazia, o caminho não foi completamente analisado
            //            {
            //                Caminho caminho = aux.Desempilhar();                             //Atribuição do caminho atual à variável auxiliar
            //                todos.Empilhar(caminho);

            //                Cidade c = arvore.BuscaPorDado(new Cidade(caminho.Origem));       //Busca pela cidade cujo código é a origem do caminho atual, para podermos adicioná-la nos vetores como uma rota do caminho
            //                nomes[n] = c.Nome;                                                //Atribuição do nome da cidade atual à posição atual do vetor de nomes
            //                cod[n] = c.Cod;                                                   //Atribuição do código da cidade atual à posição atual do vetor de códigos
            //                n++;                                                              //Incremento de 1 unidade no valor da variável que guarda quantas cidades do caminho atual já foram registradas 

            //                distanciaAtual += caminho.Distancia;                              //Acrescentamos à variável que guarda a distância total percorrida o valor da distância entre a última cidade e a cidade atual
            //            }

            //            Cidade cidade = arvore.BuscaPorDado(new Cidade(lsbDestino.SelectedIndex));   //Após o término do caminho, a cidade destino é atribuída aos vetores para ser exibida posteriormente
            //            nomes[n] = cidade.Nome;                                                      //Atribuição da cidade destino escolhido pela usuário à posição atual do vetor de nomes
            //            cod[n] = cidade.Cod;                                                         //Atribuição da cidade destino escolhido pela usuário à posição atual do vetor de nomes
            //            n++;                                                                         //Incremento de 1 unidade no valor da variável que guarda quantas cidades do caminho atual já foram registradas 
                        
            //            if (caminhoAnterior > distanciaAtual)                //verifica-se se a distância necessária para percorrer esse trajeto é a menor quando comparada às distâncias já percorridas
            //            {//Caso esse caminho seja a melhor opção, é guardado  para ser posteriormente exibido ao usuário
            //                m = indice;
            //                nomeMelhor = nomes;                               
            //                caminhoAnterior = distanciaAtual;                //Atualiza-se o valor da menor distância encontrada para a mesma verificação no próximo caminho
            //            }

            //            vetorCaminhos[indice] = cod.Clone();                 //Atribuição do vetor de códigos de cidades à posição atual do vetor de caminhos
            //            indice++;                                            //Acrescentamos uma unidade à variável que guarda a posição em que o próximo caminho deverá ser guardado

            //            int index = dataGridView1.Rows.Add();                //Índice da linha adicionada no data grid view
            //            dataGridView1.Rows[index].SetValues(nomes);          //Atribuição do caminho atual à linha adicionada 


            //            qtdCaminhosExibidos++;                                 //Acrescentamos uma unidade à variável que guarda a quantidade de caminhos encontrados que já foram analisados
            //            cod = new double?[23];                                     //Instanciação de um novo vetor de códigos, para que este guarde o próximo trajeto a ser percorrido
            //            distanciaAtual = 0;                                    //Zeramos a variável, para que essa possa guardar somente a distância percorrida no próximo caminho, não sendo afetada pela distância percorrida neste caminho
            //        }
            //        ExibirMelhorCaminho(nomeMelhor);                           //Chamada do método responsável por exibir para o usuário o melhor caminho, dentre todos os já exibidos, classificação baseada na menor distância percorrida
            //        melhorCaminho = (double?[])vetorCaminhos[m];                   

            //    }
            //}
        }

        private void ExibirMelhorCaminho(string[] vet)                        //Método responsável por exibir o melhor caminho dentre todos os achados no dataGridView2
        {
            if (dataGridView2.RowCount == 1)                                  //Verificação: se o dataGridView2 contiver uma linha, então sabemos que um caminho foi exibido nela, e a deletamos
                dataGridView2.Rows.RemoveAt(0);                               //Remoção da linha que antes guardava o melhor caminho anteriormente pesquisado
            int index = dataGridView2.Rows.Add();                             //Índice da linha adicionada no data grid view
            dataGridView2.Rows[index].SetValues(vet);                         //Atribuição do caminho atual à linha adicionada
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            LerArquivo(e);                                                      //Chamada do método responsável por ler, criar a árvore e exibir no mapa a localização e os nomes das cidades no arquivo
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
                for (int i = 0; i < qtdCidades; i++)                        //Loop para adicionarmos colunas de acordo com o número de cidades do arquivo
                    dataGridView2.Columns.Add("x", "Cidade");

                CriarMatriz();
            }
        }

        public void CriarMatriz()                                           //Método para criar o grafo
        {
            matriz = new int[arvore.QuantosDados, arvore.QuantosDados];     //essa matriz será usada para armazenar as distâncias entre cada cidade

            //Leitura do arquivo que contém as cidades e a distância entre elas
            StreamReader arq = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.UTF7);           

            while(!arq.EndOfStream)                                          //Loop que ocorre até que o arquivo seja completamente lido
            {
                string linha = arq.ReadLine();                               //variável que guarda todas as informações lidas na linha atual do arquivo

                int origem = Convert.ToInt32(linha.Substring(0, 3));
                int destino = Convert.ToInt32(linha.Substring(3, 3));

                matriz[origem, destino] = Convert.ToInt32(linha.Substring(6, 5));
            }

            arq.Close();
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DesenharArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 520, g);
        }

        private void DesenharArvore(bool primeiraVez, NoArvore<Cidade> raiz,  int x, int y, double angulo, double incremento,    double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.LightCoral);
                caneta.Width = 5;
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                DesenharArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,  incremento * 0.65, comprimento * 0.71, g);
                DesenharArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,  incremento * 0.65, comprimento *0.71, g);
                SolidBrush preenchimento = new SolidBrush(Color.Black);
                g.FillEllipse(preenchimento, xf - 60, yf - 20, 120, 110);
                g.DrawString(Convert.ToString(raiz.Info.Nome), new Font("Century Gothic", 11), new SolidBrush(Color.White), xf - 57, yf+20);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {//Método disparado quando o usuário clica em alguma célula do dataGridView1
            int lClick = e.RowIndex;                                   //atribuição do índice da linha escolhida pelo usuário à variável global lClick
            EscreverLinha((double?[])vetorCaminhos[lClick]);               //Chamada do método responsável por desenhar a linha na tela
        }
       
        private void EscreverLinha(double?[] vetor)                        //Método responsável por exibir no mapa o caminho selecionado pelo usuário
        {
            pbMapa.Refresh();                                          //Método responsável por apagar tudo antes exibido em cima do mapa
            Graphics g = pbMapa.CreateGraphics();                      //Atribuição do gráfico criado à variável g da classe Graphics
            Pen caneta = new Pen(Color.Black);                         //Código que cria a variável que 'desenha' no mapa

            //Código que personaliza a linha que liga as cidades
            AdjustableArrowCap flecha = new AdjustableArrowCap(3, 3);
            caneta.CustomEndCap = flecha;
            caneta.Width = 3;

            double?[] codCidades = vetor;                                  //Declaração do vetor responsável por guardar o caminho selecionado por meio dos códigos da cidades que aparecem nas rotas                                         //Variável que guarda o número de vezes que o valor 0 foi encontrado no vetor. Seu valor pode ser no máximo 1 já que só é possível passar uma vez pela cidade cujo código é 0
            
            for(int i = 0; i < codCidades.Length-1 ; i++)               //Loop que percorre cada posição do vetor de códigos de cidades do caminho selecionado
            {
                if (codCidades[i] != null && codCidades[i + 1] != null)
                {
                    Cidade cid = arvore.BuscaPorDado(new Cidade(Convert.ToInt32(codCidades[i])));
                    int xp = cid.X * pbMapa.Width / 4096;                   //Variável que guarda a coordenada X da cidade origem
                    int yp = cid.Y * pbMapa.Height / 2048;                 //Variável que guarda a coordenada Y da cidade origem

                    Cidade cid2 = arvore.BuscaPorDado(new Cidade(Convert.ToInt32(codCidades[i + 1])));
                    int xf = cid2.X * pbMapa.Width / 4096;                  //Variável que guarda a coordenada X da cidade destino
                    int yf = cid2.Y * pbMapa.Height / 2048;                 //Variável que guarda a coordenada Y da cidade destino
                                    
                        g.DrawLine(caneta, xp + 4, yp + 2, xf + 4, yf + 2);         //Método responsável por desenha a linha na tela, com os parâmetros da caneta que será usada e as coordenadas x e y dos pontos que serão ligados pelas setas
                }
                }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EscreverLinha(melhorCaminho);                                //Chamada do método responsável por exibir no mapa o melhor caminho encontrado, quando o usuário clicar em sua linha correspondente
        }

        private void pbMapa_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
