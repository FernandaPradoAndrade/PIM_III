using System;
using System.Text.RegularExpressions;

public class Usuario{
    public string Email {get; set;}
    public string CPF {get; set;}
    public string Senha {get; set;}
}

class Program{
    static void Main(){
        ValidarCPF();
    }

    private static void ValidarCPF(){
        Console.WriteLine("CPF para validar: ");
        string cpf = LimparCpf(Console.ReadLine());

        if (Validar(cpf)){
            Console.WriteLine($"CPF válido: {cpf}");
        }
        else{
            Console.WriteLine("CPF inválido");
        }
    }

    private static string LimparCpf(string cpf){
        return Regex.Replace(cpf ?? "", "[^0-9]", "");
    }

    private static bool Validar(string cpf){
        if (cpf.Length != 11) return false;

        string cpfBase = cpf.Substring(0,9);

        int resultado = 0;
        int contador = 10;

        foreach (char digito in cpfBase)
        {
            resultado += int.Parse(digito.ToString()) * contador--;
        }

        int digito1 = (resultado * 10) % 11;
        digito1 = digito1 <= 9 ? digito1 : 0;

        string cpf_atualizado = cpfBase + digito1;

        resultado = 0;
        contador = 11;

        foreach(char digito in cpf_atualizado)
        {
            resultado += int.Parse(digito.ToString()) * contador--;
        }

        int digito2 = (resultado * 10) % 11;
        digito2 = digito2 <= 9 ? digito2 : 0;

        string cpf_gerado = cpfBase + digito1 + digito2;

        return cpf == cpf_gerado;
    }

}