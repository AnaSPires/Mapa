using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Caminho : IComparable<Caminho>
{
    int destino, origem;

    public Caminho(int o, int d)
    {
        Destino = d;
        Origem = o;
    }

    public int Destino { get => destino; set => destino = value; }
    public int Origem { get => origem; set => origem = value; }

    public int CompareTo(Caminho outroCaminho)
    {
        return  origem - outroCaminho.origem + destino - outroCaminho.destino;
    }

}