using System;

public class PilhaLista<Dado> : IStack<Dado> where Dado : IComparable<Dado>
{
  private NoLista<Dado> topo;
  private int tamanho;
  public PilhaLista()
  { // construtor
    topo = null;
    tamanho = 0;
  }
  public int Tamanho()
  {
    return tamanho;
  }
  public bool EstaVazia()
  {
    return (topo == null);
  }
  public void Empilhar(Dado o)
  {
    NoLista<Dado> novoNo = new NoLista<Dado>(o, topo);
    topo = novoNo; // topo passa a apontar o novo nó
    tamanho++; // atualiza número de elementos na pilha
  }
  public Dado OTopo() 
  {
    Dado o;
    if (EstaVazia())
       throw new PilhaVaziaException("Underflow da pilha");
    o = topo.Info;
    return o;
  }
  public Dado Desempilhar() 
  {
    if (EstaVazia())
       throw new PilhaVaziaException("Underflow da pilha");
    Dado o = topo.Info; // obtém o objeto do topo
    topo = topo.Prox; // avança topo para o nó seguinte
    tamanho--; // atualiza número de elementos na pilha
    return o; // devolve o objeto que estava no topo
  }

    //public PilhaLista<Dado> Copiar(PilhaLista<Dado> copiada)
    //{
    //    PilhaLista<Dado> copia = new PilhaLista<Dado>();
    //    PilhaLista<Dado> aux = copiada;

    //    while (!aux.EstaVazia())
    //        copia.Empilhar(aux.Desempilhar());
        
    //    return copia.Inverter();
    //}

        public bool Existe(Dado dado)
        {
        PilhaLista<Dado> pilhaAux = Clone();

            while (!EstaVazia())
            {
                if (OTopo().CompareTo(dado) == 0)
                {
                    this.topo = pilhaAux.topo;
                    this.tamanho = pilhaAux.tamanho;
                    return true;
                }
                Desempilhar();
            }
        this.topo = pilhaAux.topo;
        this.tamanho = pilhaAux.tamanho;

            return false;
    }

    public PilhaLista<Dado> Clone()
    {
        PilhaLista<Dado> pilhaAux = new PilhaLista<Dado>();
        pilhaAux.topo = topo;
        pilhaAux.tamanho = tamanho;

        return pilhaAux;
    }

    public PilhaLista<Dado> Inverter()
    {
        PilhaLista<Dado> aux = this;
        PilhaLista<Dado> outra = new PilhaLista<Dado>();

        while (!aux.EstaVazia())
            outra.Empilhar(aux.Desempilhar());

        return outra;
    }
}