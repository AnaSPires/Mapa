using System;
using System.Collections;
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
        Arvore<Cidade> arvore;                                                        //Declaração da árvore usada
        int[,] matriz;                                                                // Declaração do grafo percorrido na procura de caminhos
        object[] vetorCaminhos;                                                       //vetor usado para guardar, em cada posição, um caminho encontrado e guardado em um vetor de int
        double?[] melhorCaminho;                                                      // Variável que guarda a linha seleciona pelo usuário para que o caminho escolhido seja exibido no mapa
      
        public Form1()
        {
            InitializeComponent();
            arvore = new Arvore<Cidade>();                                           //Instanciação da árvore 
        }
        
        private void BtnBuscar_Click(object sender, EventArgs e)                     //Método chamado no evento click do btnBuscar
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");           //Mensagem mandada ao usuário ao clique do botão btnBuscar

            pbMapa.Refresh();                                                        //Método que "limpa" o mapa, apagando o caminho que possivelmente estava sendo exibido antes

            dataGridView1.Rows.Clear();                                              //Método que apaga as linhas populadas com o caminho escolhido anteriormente peo usuário
            dataGridView2.Rows.Clear();                                              //Método que apaga as linhas populadas com o caminho escolhido anteriormente peo usuário
            dataGridView2.Columns.Clear();                                           //Método que deleta as colunas antes exibidas no dataGridView
            PilhaLista<Caminho> aux2 = new PilhaLista<Caminho>();                    //Declaração e instanciação da pilha de caminhos que guardará todos os caminhos encontrados que levarão ao destino desejado
            PilhaLista<Caminho> caminhos = new PilhaLista<Caminho>();                //Declaração e instanciação da pilha de caminhos que guardará todos os possíveis caminhos, cada rota que possivelmente levará ao destino escolhido
            PilhaLista<Caminho> aux = new PilhaLista<Caminho>();                     //Declaração e instanciação da pilha de caminhos que inicialmente guardará todos os caminhos que tem como início a origem determinada

            int qtdCaminhos = 0;                                                     //Variável que guarda o número de caminhos encontrados, usada mais tarde na instanciação do vetor de caminhos
            int atual = lsbOrigem.SelectedIndex;                                     //Variável que guarda o índice da cidade escolhida pelo usuário como origem
            bool acabouCaminho = false;                                              //Variável que guarda 'true' se a cidade destino foi alcançada ou 'false' caso não
            bool[] visitados = new bool[arvore.QuantosDados];                        //Vetor do tipo boolean que guardará true sempre que a cidade correspondente foi verificada/visitada e false em caso contrário   
            int nCidades = 0;                                                        //Variável que guarda o número de cidades que o caminho com maior cidades contém
            int menorCaminho = Int32.MaxValue;                                       //Variável que guarda a menor distância encontrada
            int distancia;                                                           //Variável que guarda a distância percorrida no caminho atualmente analisado
            string[] nomeMelhor = new string[arvore.QuantosDados];                   //Vetor de string que guarda os nomes das cidades contidas no melhor caminho encontrado
          
            vetorCaminhos = new object[100];

            int qtd = arvore.QuantosDados;                                           //Variável que guarda a quantidade de cidades contidas na árvore criada  

            while (!acabouCaminho)                                                   //Loop definido pela verificação do encontro ou não de um caminho 
            {
                distancia = 0;
                bool empilhou = false;                                               //Variável boolean que verifica se algum caminho foi empilhado com a origem atual
                for (int c = 0; c < qtd; c++)                                        //Loop que se repete pelo número de cidades existentes
                {
                    if (matriz[atual, c] != default(int))                            //Verificação: se o valor da posição atual da matriz é diferente de 0, ou seja, se entre os índices(cidades) definidos existe uma rota 
                    {
                        aux.Empilhar(new Caminho(atual, c, matriz[atual, c]));       //Caso exista uma rota entre essas cidades, essa será empilhada na pilha
                        empilhou = true;                                             //Atribuição de valor true para a variável 'empilhou', já que um posssível caminho foi encontrado
                    }
                }

                if (!empilhou)                                                       //Verificação: se nenhum possível caminho foi encontrado desta vez
                {
                    if (!aux.EstaVazia())                                            //se a pilha aux não estiver vazia
                    {
                        while (!aux.EstaVazia() && !caminhos.EstaVazia() && aux.OTopo().Origem != caminhos.OTopo().Destino) //se a pilha "aux" e a pilha "caminhos" não estiverem vazia e a origem do topo de "aux" for diferente da origem do topo de "caminhos"
                            caminhos.Desempilhar();                                  //Caso nada tenha sido empilhado, devemos desempilhar da pilha caminhos até que a origem do caminho no topo da pilha aux seja igual ao destino do caminho do topo da pilha caminhos 
                    }
                }

                if (aux.EstaVazia())                                               //Verificação: se a pilha aux não tiver elementos, então não é possível prosseguir a procura por um caminho
                    acabouCaminho = true;                                          //Portanto, para sair do loop, a variável acabouCaminho recebe true
                else                                                               //Caso a pilha aux não esteja vazia, a busca por um caminho deve presseguir
                {
                    Caminho um = aux.Desempilhar();                                //Variável local que guarda o caminho analisado atualmente

                    if (um.Destino == lsbDestino.SelectedIndex)                    //Verificação: se um caminho foi encontrado
                    {
                        caminhos.Empilhar(um);                                      //Empilha na pilha "caminhos"

                        if (caminhos.Tamanho() > nCidades)                          //Se o tamanho da pilha "caminhos" atual, a quantidade de cidades, for maior do que a maior quantidades de cidades em um caminho ("nCidades")
                            nCidades = caminhos.Tamanho();                          //A quantidade máxima de cidades em um caminho recebe a quantidade de cidades no caminho atual
                        
                        aux2 = caminhos.Clone().Inverter();                         //A pilha "aux2" recebe o clone da pilha "caminhos" invertida

                        if (!aux.EstaVazia())                                            //se a pilha aux não estiver vazia
                        {
                            while (!aux.EstaVazia() && !caminhos.EstaVazia() && aux.OTopo().Origem != caminhos.OTopo().Destino) //se a pilha "aux" e a pilha "caminhos" não estiverem vazia e a origem do topo de "aux" for diferente da origem do topo de "caminhos"
                                caminhos.Desempilhar();                                  //Caso nada tenha sido empilhado, devemos desempilhar da pilha caminhos até que a origem do caminho no topo da pilha aux seja igual ao destino do caminho do topo da pilha caminhos 
                        }

                        caminhos.Empilhar(um);                                      
                        atual = um.Destino;                                         //variável atual recebe o destino do topo de "aux"

                        int i = 0;                                                  //Variável que armazena o indíce dos vetores
                        dataGridView1.RowCount++;                                   //Aumenta a quantidade de linhas do DataGridView
                        dataGridView1.ColumnCount = nCidades + 1;                   //A quantidade de colunas do DataGridView recebe o maior número de cidades de todos os caminhos possíveis

                        double?[] cod = new double?[arvore.QuantosDados];           //Representa um vetor que armazena os códigos de todas as cidades do caminho atual em ordem
                        string[] nomes = new string[arvore.QuantosDados];           //Representa um vetor que armazena os nomes de todas as cidades do caminho atual em ordem

                        Caminho c = new Caminho();                                  //Representa a rota atual
                        while (!aux2.EstaVazia())                                   //Enquanto a pilha "aux2" estiver vazia
                        {
                             c = aux2.Desempilhar();                               //A rota atual recebe o topo de "aux2" e desempilha esta
                            Cidade cid = arvore.BuscaPorDado(new Cidade(c.Origem));//Representa a cidade de origem da rota atual
                            nomes[i] = cid.Nome;                                    //Armazena o nome desta cidade na posição atual do vetor de nomes
                            cod[i] = cid.Cod;                                       //Armazena o codigo desta cidade na posição atual do vetor de códigos
                            distancia += c.Distancia;                               //A distância total do caminho soma a distância da rota atual
                            dataGridView1[i++, dataGridView1.RowCount - 1].Value = cid.Nome;//"Escreve" o nome da cidade atual no DataGridView
                        }
                       
                        nomes[i] = (arvore.BuscaPorDado(new Cidade(c.Destino))).Nome;//Guarda o nome da última cidade no vetor de nomes
                        cod[i] = (arvore.BuscaPorDado(new Cidade(c.Destino))).Cod;   //Guarda o codigo da última cidade no vetor de códigos
                        dataGridView1[i, dataGridView1.RowCount - 1].Value = nomes[i];//"Escreve" o nome da última cidade no DataGridView

                        if (distancia < menorCaminho)                                 //Verificação: se a distância percorrida nesse caminho for menor que a menor distância já encontrada
                        {
                            menorCaminho = distancia;                                 //A variável menorCaminho passa a guardar a distância atual 
                            melhorCaminho = cod;                                      //Como a distância atual é a menor encontrada, o melhor caminho passa a ser o atual, portanto o valor do vetor de códigos melhorCaminho recebe o vator de códigos atual
                            nomeMelhor = new string[i];                               //Instanciação do vetor que guarda os nomes das cidades que o caminho atual contém
                            nomeMelhor = nomes;                                       //Atribuição do vetor de nomes das cidades percorridas neste caminho ao vetor que guarda nomes de cidades pelas quais passa o melhor caminho
                        } //Caso não seja menor, nada acontece, para que a variável contenha o menor valor de todos corretamente
                        
                        vetorCaminhos[qtdCaminhos] = cod;                        //Atribuição do vetor de códigos atual à posição atual do vetorCaminhos, para que este guarde os códigos das cidades percorridas nesse caminho

                        qtdCaminhos++;                                           //Acrescentamos uma unidade à variável qtdCaminhos
                    }
                    else
                    {
                        caminhos.Empilhar(um);                                  //Ao acharmos uma possível rota, ela é guardada na pilha de possíveis rotas
                        atual = um.Destino;                                     //Mudamos o valor da variável atual, que passará a guardar a origem do caminho que será verificado posteriormente
                    }  
                }
            }
            if (qtdCaminhos == 0)                                               //Caso a quantidade de caminhos encontrados seja zero, não existe uma rota entre as cidades escolhidas, portanto, alertamos o usuário 
                MessageBox.Show("Não existe caminho!");                         //Mensagem exibida ao usuário para alertá-lo da inexistência de caminhos
            else                                                                //Caso a quantidade de caminhos encontrados não seja zero, achamos pelo menos um caminho
                ExibirMelhorCaminho(nomeMelhor);                                //Chamada de método para exibir o melhor caminho encontrado
        }

        private void ExibirMelhorCaminho(string[] vet)                        //Método responsável por exibir o melhor caminho dentre todos os achados no dataGridView2
        {
            if (dataGridView2.RowCount == 1)                                  //Verificação: se o dataGridView2 contiver uma linha, então sabemos que um caminho foi exibido nela, e a deletamos
                dataGridView2.Rows.RemoveAt(0);                               //Remoção da linha que antes guardava o melhor caminho anteriormente pesquisado
            
            for (int i = 0; i < vet.Length && vet[i] != null; i++)            //Loop para adicionarmos colunas de acordo com o número de cidades do arquivo
                dataGridView2.Columns.Add("x", "Cidade");

            int index = dataGridView2.Rows.Add();                             //Índice da linha adicionada no data grid view
            dataGridView2.Rows[index].SetValues(vet);                         //Atribuição do caminho atual à linha adicionada
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            LerArquivo(e);                                                      //Chamada do método responsável por ler, criar a árvore e exibir no mapa a localização e os nomes das cidades no arquivo
        }

        private void LerArquivo(PaintEventArgs e)                                //Método responsável por ler os arquivos e criar a árvore usada pelo programa, juntamente com matriz
        {
            int n = 0;                                                           //
            int qtdCidades = 1;                                                  //Variável local que guarda o número de cidades a serem lidas do arquivo

            StreamReader arq = new StreamReader("CidadesMarte.txt", Encoding.UTF7);         //Criação do StreamReader responsável por ler o arquivo que contém as cidades, seus códigos, nomes e localização no mapa
            StreamReader aux = new StreamReader("CidadesMarteOrdenado.txt", Encoding.UTF7); //Criação do StreamReader responsável por ler o arquivo que contém os nomes cidades, que são exibidas ordenadamente de acordo com seus respectivos códigos
            string linha = arq.ReadLine();                                                  //Variável tipo string que guarda todas as informações lidas no primeiro arquivo mencionado
            string linha2 = aux.ReadLine();                                                 //Variável tipo string que guarda todas as informações lidas no segundo arquivo mencionado
            Cidade cid = new Cidade(linha);                                                 //Criação da primeira cidade que é lida no arquivo desordenado de acordo com as informações lidas na linha atual e por meio do construtor adaptado à esse tipo de construção, na classe Cidade 
            Cidade cid2 = new Cidade(linha2);                                               //Criação da primeira cidade que aparece no arquivo ordenado, de acordo com as informações lidas na linha atual e por meio do construtor adaptado à esse tipo de construção, na classe Cidade 
            var qtdLinhas = File.ReadLines("CidadesMarteOrdenado.txt").Count();             //Variável que guarda a quantidade de linhas que tem o primeiro arquivo; essa quantidade de linhas corresponde ao número de cidades que serão registradas

            if (lsbOrigem.Items.Count < qtdLinhas)                                          //Verificação: se o número de itens do listbox for menor que o número de cidades que esrão/foram registradas
            {//Caso seja, podemos adicionar nos dois listbox
                lsbDestino.Items.Add(n + "-" + cid2.Nome);            //Adição do código e nome da cidade, respectivamente, no listbox de origens
                lsbOrigem.Items.Add(n + "-" + cid2.Nome);             //Adição do código e nome da cidade, respectivamente, no listbox de destinos

                arvore.Raiz = new NoArvore<Cidade>(cid);              //Como essa é a primeira cidade lida, esta deve ser tida como raiz da 
                                                                      //árvore do programa; Como uma árvore criada a partir de um arquivo ordenado é "tombada" para a direita, por conta da contrução de esquerda(menores valores) e direita(maiores valores), a cidade passada como referência é a cid, adquirida pela leitura do arquivo desordenado
            }//Caso não seja, nada é adicionado ou alterado
             
            Graphics grafico = e.Graphics;                            //Atribuição do gráfico adquirido pelo parâmetro à variável grafico do tipo Graphics, responsável pela alteração do gráfico exibido ao usuário
            Point p = new Point();                                    //Instanciação de um Point, necessário na exibição dos pontos no gráficos, representantes das localizações das cidades
            p.X = cid.X * pbMapa.Width / 4096;                        //Atribuição da localização, no eixo X, do ponto p                
            p.Y = cid.Y * pbMapa.Height / 2048;                       //Atribuição da localização, no eixo Y, do ponto p         
            SolidBrush pincel = new SolidBrush(Color.Black);          //Declaração de um SolidBrush, responsávl por escrever os nomes das cidades

            //Método que desenha no gráfico o nome da cidade criada
            grafico.DrawString(cid.Nome, new Font("Arial", 10, FontStyle.Bold), pincel, new Point(p.X, p.Y - 20));
            //Método que desenha no gráfico o ponto representando a localização da cidade criada
            grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));

            while (!arq.EndOfStream)           //Loop: enquanto o arquivo não for totalmente lido; Como os dois arquivos tem necessariamente o mesmo tamanho, é possível verificar o término de qualquer um dois dois
            {
                n++;
                linha = arq.ReadLine();        //Atribuição à variável linha as informações lidas na linha atual do arquivo
                linha2 = aux.ReadLine();       //Atribuição à variável linha2 as informações lidas na linha atual do segundo arquivo
                cid = new Cidade(linha);       //Criação da  cidade que foi atualmente lida no arquivo desordenado de acordo com as informações lidas na linha atual e por meio do construtor adaptado à esse tipo de construção, na classe Cidade 
                cid2 = new Cidade(linha2);     //Criação da  cidade que foi atualmente no arquivo ordenado, de acordo com as informações lidas na linha atual e por meio do construtor adaptado à esse tipo de construção, na classe Cidade 

                if (lsbOrigem.Items.Count < qtdLinhas)                    //Verificação: se o número de itens do listbox for menor que o número de cidades que esrão/foram registradas
                {//Caso seja, podemos adicionar nos dois listbox
                    lsbDestino.Items.Add(n + "-" + cid2.Nome);            //Adição do código e nome da cidade, respectivamente, no listbox de origens
                    lsbOrigem.Items.Add(n + "-" + cid2.Nome);             //Adição do código e nome da cidade, respectivamente, no listbox de destinos

                    arvore.Incluir(cid);                                  //Inclusão da cidade (do arquivo desordenado, para a construção estrutural correta da árvore) lida à árvore
                }//Caso não seja, nada é adicionado ou alterado

                p = new Point();                                          //Instanciação de um novo Point, que irá representar a localização da cidade lida atualmente
                p.X = cid.X * pbMapa.Width / 4096;                        //Atribuição da localização, no eixo X, do ponto p                
                p.Y = cid.Y * pbMapa.Height / 2048;                       //Atribuição da localização, no eixo Y, do ponto p         
                pincel = new SolidBrush(Color.Black);                     //Declaração de um SolidBrush, responsávl por escrever os nomes das cidades
                //Método que desenha no gráfico o nome da cidade criada
                grafico.DrawString(cid.Nome, new Font("Arial", 10, FontStyle.Bold), pincel, new Point(p.X, p.Y - 20));
                //Método que desenha no gráfico o ponto representando a localização da cidade criada
                grafico.FillEllipse(pincel, new RectangleF(p.X, p.Y, 8, 8));

                qtdCidades++;                                             //Adição de uma unidade à variável que guarda a quantidade de cidades lidas 
            }

            arq.Close();                                                  //Encerramento da leitura do arquivo, fechando-o
            aux.Close();                                                  //Encerramento da leitura do arquivo, fechando-o

            CriarMatriz();                                                //Chamada do método responsável por instanciar e preencher a matriz, que é usada na verificação da existência de uma rota ou não entre duas cidades
        }

        public void CriarMatriz()                                           //Método para criar o grafo
        {
            matriz = new int[arvore.QuantosDados, arvore.QuantosDados];     //essa matriz será usada para armazenar as distâncias entre cada cidade

            //Leitura do arquivo que contém as cidades e a distância entre elas
            StreamReader arq = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.UTF7);    //Criação do StreamReader responsável por ler o arquivo que contém as rotas entre as cidades, de acordo com seus códigos, e sua distância         

            while (!arq.EndOfStream)                                          //Loop que ocorre até que o arquivo seja completamente lido
            {
                string linha = arq.ReadLine();                               //variável que guarda todas as informações lidas na linha atual do arquivo

                int origem = Convert.ToInt32(linha.Substring(0, 3));         //Variável que guarda o index da linha atual de acordo com as informações lidas no arquivo o esse intervalo determinado
                int destino = Convert.ToInt32(linha.Substring(3, 3));        //Variável que guarda o index da coluna atual de acordo com as informações lidas no arquivo o esse intervalo determinado

                matriz[origem, destino] = Convert.ToInt32(linha.Substring(6, 5)); //Atribuição da distância entre duas cidades à respectiva célula
            }

            arq.Close();  //Encerramento da leitura do arquivo, fechando-o
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;         //Atribuição do gráfico adquirido pelo parâmetro à variável grafico do tipo Graphics, responsável pela alteração do gráfico exibido ao usuário
            DesenharArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 520, g); //Chamada do método responsável por desenhar a árvore
        }

        private void DesenharArvore(bool primeiraVez, NoArvore<Cidade> raiz,  int x, int y, double angulo, double incremento,    double comprimento, Graphics g)
        {//Método responsável por desenhar a árvore, usando recursão
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
                g.FillEllipse(preenchimento, xf - 60, yf - 20, 110, 105);
                g.DrawString(Convert.ToString(raiz.Info.Nome), new Font("Century Gothic", 11), new SolidBrush(Color.White), xf - 57, yf+20);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {                                                              //Método disparado quando o usuário clica em alguma célula do dataGridView1
            int lClick = e.RowIndex;                                   //atribuição do índice da linha escolhida pelo usuário à variável global lClick
            EscreverLinha((double?[])vetorCaminhos[lClick]);           //Chamada do método responsável por desenhar a linha na tela
        }
       
        private void EscreverLinha(double?[] vetor)                    //Método responsável por exibir no mapa o caminho selecionado pelo usuário
        {
            pbMapa.Refresh();                                          //Método responsável por apagar tudo antes exibido em cima do mapa
            Graphics g = pbMapa.CreateGraphics();                      //Atribuição do gráfico criado à variável g da classe Graphics
            Pen caneta = new Pen(Color.Black);                         //Código que cria a variável que 'desenha' no mapa

            //Código que personaliza a linha que liga as cidades
            AdjustableArrowCap flecha = new AdjustableArrowCap(4, 4);
            caneta.CustomEndCap = flecha;
            caneta.Width = 3;

            double?[] codCidades = vetor;                              //Declaração do vetor responsável por guardar o caminho selecionado por meio dos códigos da cidades que aparecem nas rotas                                         //Variável que guarda o número de vezes que o valor 0 foi encontrado no vetor. Seu valor pode ser no máximo 1 já que só é possível passar uma vez pela cidade cujo código é 0
            
            for(int i = 0; i < codCidades.Length-1 ; i++)              //Loop que percorre cada posição do vetor de códigos de cidades do caminho selecionado
            {
                if (codCidades[i] != null && codCidades[i + 1] != null)
                {
                    Cidade cid = arvore.BuscaPorDado(new Cidade(Convert.ToInt32(codCidades[i])));
                    int xp = cid.X * pbMapa.Width / 4096;                   //Variável que guarda a coordenada X da cidade origem
                    int yp = cid.Y * pbMapa.Height / 2048;                  //Variável que guarda a coordenada Y da cidade origem

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
    }
}
