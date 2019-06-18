using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Caminho : IComparable<Caminho>
{
    int destino, origem, distancia, qtdCaminhos;

    public Caminho(int o, int d, int distancia, int qtd)  //Contrutor comum de uma instância da classe Caminho
    {
        Destino = d;
        Origem = o;
        Distancia = distancia;
        QtdCaminhos = qtd;
    }
    public Caminho()  //Contrutor comum de uma instância da classe Caminho
    {
        Destino = 0;
        Origem = 0;
        Distancia = 0;
        QtdCaminhos = 0;
    }

    public int Destino { get => destino; set => destino = value; }
    public int Origem { get => origem; set => origem = value; }
    public int Distancia { get => distancia; set => distancia = value; }
    public int QtdCaminhos { get => qtdCaminhos; set => qtdCaminhos = value; }

    public int CompareTo(Caminho outroCaminho)
    {
        return  distancia - outroCaminho.Distancia;
    }

}