using System;
using System.IO;

        /*  
           Autoras: 
           Ariane Paula Barros - 18173
           Ana Clara Sampaio Pires   - 18201

           A classe Cidade tem por objetivo armazenar o código, nome, posição X e 
           posição Y da cidade, alterando e retornando os valores dos atributos, 
           guardar as informações da cidade a partir de uma string e comparar o código
           desta com outra cidade.

       */

class Cidade : IComparable<Cidade>  
{
    const int tamanhoCod = 2;                           //Atributo constante que armazena o tamanho do código da cidade
    const int tamanhoNome = 15;                         //Atributo constante que armazena o tamanho do nome da cidade
    const int tamanhoX = 4;                             //Atributo constante que armazena o tamanho da posição X da cidade
    const int tamanhoY = 4;                             //Atributo constante que armazena o tamanho do posição Y da cidade

    const int inicioCod = 1;                            //Atributo constante que armazena a posição inicial do código da cidade
    const int inicioNome = inicioCod + tamanhoCod;      //Atributo constante que armazena a posição inicial do nome da cidade
    const int inicioX = inicioNome + tamanhoNome +1;    //Atributo constante que armazena a posição inicial da posição X da cidade
    const int inicioY = inicioX + tamanhoY +1;          //Atributo constante que armazena a posição inicial da posição Y da cidade

     int cod;                                           //Atributo que armazena o código da cidade
     string nome;                                       //Atributo que armazena o nome da cidade
     int x;                                             //Atributo que armazena a posição X da cidade
     int y;                                             //Atributo que armazena a posição Y da cidade
    public Cidade()                                     //Construtor sem parâmetros da classe Cidade
    {
      cod = 0;
      nome = "";
      x = 0;
      y = 0;
    }
    public Cidade(string linha)                         //Construtor que passa uma string, que representa a linha de um arquivo, como parâmetro
    {
       cod = int.Parse(linha.Substring(inicioCod, tamanhoCod));
       nome = linha.Substring(inicioNome, tamanhoNome);
       x = int.Parse(linha.Substring(inicioX, tamanhoX));
       y = int.Parse(linha.Substring(inicioY, tamanhoY));
    }

    public Cidade(int cod, string nome, int x, int y)   //Construtor comum da classe Cidade
    {
        this.cod = cod;
        this.nome = nome;
        this.x = x;
        this.y = y;
    }
    public Cidade(int cod)                              //Construtor da classe Cidade que passa o código por parâmetro
    {
        this.cod = cod;
    }

    public int Cod { get => cod;  }                     //Propriedade que altera e retorna o atributo "cod"
    public string Nome { get => nome; }                 //Propriedade que altera e retorna o atributo "nome"
    public int X { get => x; }                          //Propriedade que altera e retorna o atributo "X"
    public int Y { get => y; }                          //Propriedade que altera e retorna o atributo "Y"

    public int CompareTo(Cidade outraCid)               //Método que compara o código da cidade this com outra
    {
       return cod - outraCid.cod;
    }

   
}

