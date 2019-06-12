using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Caminho : IComparable<Caminho>
{
    int destino, origem, distancia, qtdCaminhos;

    public Caminho(int o, int d, int distancia, int qtd)
    {
        Destino = d;
        Origem = o;
        Distancia = distancia;
        QtdCaminhos = qtd;
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