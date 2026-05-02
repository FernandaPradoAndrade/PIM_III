using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace CadastroUser;

public class Usuario{
    public string Nome {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string CPF {get; set;} = string.Empty;
    public string Senha {get; set;} = string.Empty;
    public string NivelAcesso {get; set;} = string.Empty;
    public bool Atividade {get; set;} = true;
}

class Program{
    private static Usuario admin = new Usuario{
        CPF = "87888340008",
        Nome = "Administrador",
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
        while (true)
        {
            Console.WriteLine("********MENU INICIAL*******");
            Console.WriteLine("Escolha a opção desejada: ");
            Console.WriteLine("1- Cadastrar-se");
            Console.WriteLine("2- Login");
            Console.WriteLine("3- Sair");
            string opcaoMenuInicial = Console.ReadLine() ?? "";  

            if (opcaoMenuInicial == "1"){
                CadastrarEstudante();
            }  
            else if (opcaoMenuInicial == "2"){
                Login();
            }
            else if (opcaoMenuInicial == "3"){
                return;
            }
            else{
                Console.WriteLine("Opção inválida");
            }
        }
    }

    private static void MenuAdmin()
    {
        while (true)
        {
            Console.WriteLine("********MENU ADMIN*******");
            Console.WriteLine("Escolha a opção desejada: ");
            Console.WriteLine("1- Cadastrar Tutor");
            Console.WriteLine("2- Editar usuário");
            Console.WriteLine("3- Ativar/Desativar usuário");
            Console.WriteLine("4- Sair");
            string opcaoMenuAdmin = Console.ReadLine() ?? "";  

            if (opcaoMenuAdmin == "1"){
                CadastrarTutor();
            }  
            else if (opcaoMenuAdmin == "2"){
                EditarUsuario();
            }
            else if (opcaoMenuAdmin == "3"){
                AtivarDesativarStatusUser();
            }
            else if (opcaoMenuAdmin == "4"){
                return;
            }
            else{
                Console.WriteLine("Opção inválida");
            }
        }
    }

    private static void CadastrarEstudante()
    {
        Console.WriteLine("CPF: ");
        string cpf = LimparCpf(Console.ReadLine() ?? "");

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

        Console.WriteLine("Nome: ");
        string nome = Console.ReadLine() ?? "";

        Console.WriteLine("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.WriteLine("Senha: ");
        string senha = Console.ReadLine() ?? "";

        string nivelAcesso = "Estudante";

        usuarios.Add(new Usuario
        {
            CPF = cpf,
            Nome = nome,
            Email = email,
            Senha = senha,
            NivelAcesso = nivelAcesso
        });

        SalvarUsuarios();

        Console.WriteLine("Cadastro concluído com sucesso");
    }

    private static void CadastrarTutor()
    {
        Console.WriteLine("CPF: ");
        string cpf = LimparCpf(Console.ReadLine() ?? "");

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

        Console.WriteLine("Nome: ");
        string nome = Console.ReadLine() ?? "";

        Console.WriteLine("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.WriteLine("Senha: ");
        string senha = Console.ReadLine() ?? "";

        usuarios.Add(new Usuario
        {
            CPF = cpf,
            Nome = nome,
            Email = email,
            Senha = senha,
            NivelAcesso = "Tutor"
        });

        SalvarUsuarios();

        Console.WriteLine("Tutor cadastrado com sucesso");
    }

    private static void Login()
    {
        Console.WriteLine("CPF: ");
        string cpf = LimparCpf(Console.ReadLine() ?? "");

        Console.WriteLine("Senha: ");
        string senha = Console.ReadLine() ?? "";

        foreach (var user in usuarios)
        {
            if (user.CPF == cpf && user.Senha == senha)
            {

                if (!user.Atividade)
                {
                    Console.WriteLine("Conta desativada");
                    return;
                }

                Console.WriteLine($"Bem-vindo, {user.Nome}! ({user.NivelAcesso})");

                if (user.NivelAcesso == "Admin")
                {
                    MenuAdmin();
                }
                
                return;
            }
        }

        Console.WriteLine("CPF ou senha inválidos");

    }

    private static void AtivarDesativarStatusUser()
    {
        Console.WriteLine("CPF do usuário: ");
        string cpf = LimparCpf(Console.ReadLine() ?? "");

        var user = usuarios.Find(u => u.CPF == cpf);

        if (user == null)
        {
            Console.WriteLine("Usuário não encontrado");
            return;
        }

        user.Atividade = !user.Atividade;

        SalvarUsuarios();

        Console.WriteLine(user.Atividade ? "Usuário ativado" : "Usuário desativado");
    }

    private static void EditarUsuario()
    {
        Console.WriteLine("CPF do usuário que deseja editar: ");
        string cpf = LimparCpf(Console.ReadLine() ?? "");

        var user = usuarios.Find(u => u.CPF == cpf);

        if (user == null)
        {
            Console.WriteLine("Usuário não encontrado");
            return;
        }

        if (user.NivelAcesso == "Admin")
        {
            Console.WriteLine("Não é permitido editar o administrador");
            return;
        }

        while (true)
        {
            Console.WriteLine($"Editando: {user.Nome} ({user.Email})");
            Console.WriteLine("1 - Alterar Email");
            Console.WriteLine("2 - Alterar Senha");
            Console.WriteLine("3 - Voltar");

            string opcao = Console.ReadLine() ?? "";

            if (opcao == "1")
            {
                Console.WriteLine("Novo email: ");
                string novoEmail = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(novoEmail))
                {
                    user.Email = novoEmail;
                    SalvarUsuarios();
                    Console.WriteLine("Email atualizado!");
                }
            }
            else if (opcao == "2")
            {
                Console.WriteLine("Nova senha: ");
                string novaSenha = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(novaSenha))
                {
                    user.Senha = novaSenha;
                    SalvarUsuarios();
                    Console.WriteLine("Senha atualizada!");
                }
            }
            else if (opcao == "3")
            {
                return;
            }
            else
            {
                Console.WriteLine("Opção inválida");
            }
        }
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