using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
  public class Arvore<Dado> where Dado : IComparable<Dado>
  {
    private NoArvore<Dado> raiz, atual, antecessor;

    int quantosNos = 0;
    public NoArvore<Dado> Raiz
    {
      get { return raiz; }
      set { raiz = value; }
    }
    public NoArvore<Dado> Atual
    {
      get { return atual; }
      set { atual = value; }
    }
    public NoArvore<Dado> Antecessor
    {
      get { return antecessor; }
      set { antecessor = value; }
    }

    public bool Existe(Dado procurado)  // pesquisa binária não recursiva
    {
      antecessor = null;
      atual = Raiz;
      while (atual != null)
      {
        if (atual.Info.CompareTo(procurado) == 0)
           return true;
        else
          {
          antecessor = atual;
          if (procurado.CompareTo(atual.Info) < 0)
            atual = atual.Esq; // Desloca à esquerda
          else
            atual = atual.Dir; // Desloca à direita
        }
      }
      return false; // Se atual == null, a chave não existe mas antecessor aponta o pai 
    }

        public Dado BuscaPorDado(Dado procurado)
        {
            antecessor = null;
            atual = Raiz;
            Dado dado = atual.Info;

            while (atual != null)
            {
                if (atual.Info.CompareTo(procurado) == 0)
                {
                    dado = atual.Info;
                    return dado;
                }
                else
                {
                    antecessor = atual;
                    if (procurado.CompareTo(atual.Info) < 0)
                        atual = atual.Esq; // Desloca à esquerda
                    else
                        atual = atual.Dir; // Desloca à direita
                }
            }
            return dado;
        }
        
    public void Incluir(Dado incluido)    // inclusão usando o método de pesquisa binária
    {
      if (Existe(incluido))
         throw new Exception("Informação repetida");
      else
      {
        var novoNo = new NoArvore<Dado>(incluido);
        if (incluido.CompareTo(antecessor.Info) < 0)
          antecessor.Esq = novoNo;
        else
          antecessor.Dir = novoNo;
        quantosNos++;
      }
    }

    private int QtosNos(NoArvore<Dado> noAtual)
    {
      if (noAtual == null)
        return 0;
      return 1 +                    // conta o nó atual
             QtosNos(noAtual.Esq) + // conta nós da subárvore esquerda
             QtosNos(noAtual.Dir);  // conta nós da subárvore direita
    }
    public int QuantosDados { get => this.QtosNos(this.Raiz); }
 }
}
