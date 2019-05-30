using System;
using System.IO;

class Cidade : IComparable<Cidade>  
{
    const int tamanhoMatricula = 5;
    const int tamanhoNome = 30;
    const int tamanhoSalario = 10;
    
    const int inicioMatricula = 0;
    const int inicioNome = inicioMatricula + tamanhoMatricula;
    const int inicioSalario = inicioNome + tamanhoNome;

    public int matricula;
    public string nome;
    public double salario;
    public Cidade()
    {
      matricula = 0;
      nome = "";
      salario = 0;
    }
    public Cidade(string linha)
    {
       matricula = int.Parse(linha.Substring(inicioMattricula, tamanhoMatricula));
       nome = linha.Substring(inicioNome, tamanhoNome);
       salario = double.Parse(linha.Substring(inicioSalario, tamanhoSalario));
    }
    public int CompareTo(Cidade outroFunc)
    {
       return matricula - outroFunc.matricula;
    }
}

