using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace CadastroUser;

public class Usuario{
    public string Email {get; set;} = string.Empty;
    public string CPF {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
    public string NivelAcesso {get; set;} = string.Empty;
}

class Program{
    private static Usuario admin = new Usuario{
        CPF = "87888340008",
        Email = "admin@gmail.com",
        Senha = "1234",
        NivelAcesso = "Admin"
    };

    static string arquivo = "usuarios.json";
    static List<Usuario> usuarios = CarregarUsuarios();

    static void Main(){
        MenuInicial();
    }

    private static void MenuInicial(){
        Console.WriteLine("Escolha a opção desejada: ");
        Console.WriteLine("1- Cadastrar-se");
        Console.WriteLine("2- Login");
        string opcaoMenuInicial = Console.ReadLine();  

        if (opcaoMenuInicial == "1"){
            CadastrarEstudante();
        }  
        else if (opcaoMenuInicial == "2"){
            Login();            
        }
        else{
            Console.WriteLine("Opção inválida");
        }
    }

    private static void CadastrarEstudante()
    {
        Console.WriteLine("CPF: ");
        string cpf = LimparCpf(Console.ReadLine());

        if (!Validar(cpf))
        {
            Console.WriteLine("CPF inválido");
            return;
        }

        foreach (var user in usuarios)
        {
            if(user.CPF == cpf)
            {
                Console.WriteLine("CPF já cadastrado");
                return;
            }
        }

        Console.WriteLine("Email: ");
        string email = Console.ReadLine();

        Console.WriteLine("Senha: ");
        string senha = Console.ReadLine();

        string nivelAcesso = "Estudante";

        usuarios.Add(new Usuario
        {
            CPF = cpf,
            Email = email,
            Senha = senha,
            NivelAcesso = nivelAcesso
        });

        SalvarUsuarios();

        Console.WriteLine("Cadastro concluído com sucesso");
    }

    private static void Login()
    {
        Console.WriteLine("CPF: ");
        string cpf = LimparCpf(Console.ReadLine());

        Console.WriteLine("Senha: ");
        string senha = Console.ReadLine();

        foreach (var user in usuarios)
        {
            if (user.CPF == cpf && user.Senha == senha)
            {
                Console.WriteLine("Login concluído com sucesso");
                return;
            }
        }

        Console.WriteLine("CPF ou senha inválidos");

    }

    private static List<Usuario> CarregarUsuarios()
    {
        if (!File.Exists(arquivo))
            return new List<Usuario>();

        string json = File.ReadAllText(arquivo);
        return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
    }

    static Program()
    {
        if (!usuarios.Exists(a => a.CPF == admin.CPF))
        {
            usuarios.Add(admin);
            SalvarUsuarios();
        }
    }

    private static void SalvarUsuarios()
    {
        string json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions{
            WriteIndented = true});

        File.WriteAllText(arquivo, json);
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