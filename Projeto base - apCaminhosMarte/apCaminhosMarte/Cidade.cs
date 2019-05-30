using System;
using System.IO;

class Cidade : IComparable<Cidade>  
{
    const int tamanhoCod = 2;
    const int tamanhoNome = 15;
    const int tamanhoX = 4;
    const int tamanhoY = 4;
    
    const int inicioCod = 1;
    const int inicioNome = inicioCod + tamanhoCod;
    const int inicioX = inicioNome + tamanhoNome +1;
    const int inicioY = inicioX + tamanhoY +1;

    public int cod;
    public string nome;
    public int x;
    public int y;
    public Cidade()
    {
      cod = 0;
      nome = "";
      x = 0;
      y = 0;
    }
    public Cidade(string linha)
    {
       cod = int.Parse(linha.Substring(inicioCod, tamanhoCod));
       nome = linha.Substring(inicioNome, tamanhoNome);
       x = int.Parse(linha.Substring(inicioX, tamanhoX));
       y = int.Parse(linha.Substring(inicioY, tamanhoY));
    }
    public int CompareTo(Cidade outraCid)
    {
       return cod - outraCid.cod;
    }
}

