using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

        /*  
            Autoras: 
            Ariane Paula Barros - 18173
            Ana Clara Sampaio Pires   - 18201

            A classe Caminho tem por objetivo armazenar o código das cidades de origem e de destino e 
            a distância entre elas, alterando e retornando os valores dos atributos, além de comparar 
            as distâncias entre este e outro caminho

        */
class Caminho : IComparable<Caminho>
{
    int destino,                                        //Atributo que indica o código da cidade de destino do caminho
        origem,                                         //Atributo que indica o código da cidade de origem do caminhos
        distancia;                                      //Atributo que indica a distância entre a cidade de origem e a de destino

    public Caminho(int o, int d, int distancia)         //Contrutor comum de uma instância da classe Caminho
    {
        Destino = d;
        Origem = o;
        Distancia = distancia;
    }
    public Caminho()                                    //Contrutor sem parâmetros da classe Caminho
    {
        Destino = 0;
        Origem = 0;
        Distancia = 0;
    }

    public int Destino { get => destino; set => destino = value; }      //Propriedade para alterar e retornar o valor do atributo "destino"
    public int Origem { get => origem; set => origem = value; }         //Propriedade para alterar e retornar o valor do atributo "origem"
    public int Distancia { get => distancia; set => distancia = value; }//Propriedade para alterar e retornar o valor do atributo "distancia"

    public int CompareTo(Caminho outroCaminho)                          //Método que compara com outro caminho de acordo com a distância, retornando a diferença das distâncias
    {
        return  distancia - outroCaminho.Distancia;
    }

}