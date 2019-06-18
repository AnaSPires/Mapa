using System;

//Ana Clara Sampaio Pires RA: 18201
//Ariane Paula Barros     RA: 18173
public class NoLista<Dado> where Dado : IComparable<Dado>
{ 
    Dado info;
    NoLista<Dado> prox;

    public NoLista(Dado info, NoLista<Dado> prox)
    {
        Info = info;
        Prox = prox;
    }

    public Dado Info
    {
        get { return info;  }
        set
        {
            if (value != null)
               info = value;
        }
    }

    public NoLista<Dado> Prox
    {
        get => prox;
        set => prox = value;
    }
}

