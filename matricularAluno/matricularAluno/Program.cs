using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace matricularAluno;

public class Materia
{
    public string nomeMateria { get; set; }

    public Materia(string nomeMateria)
    {
        this.nomeMateria = nomeMateria;

    }

}

public class Aluno
{
    public string CPF { get; set; }

    public List<Materia> materiasMatriculadas { get; set; } = new List<Materia>();

    public Aluno(string cpf)
    {
        this.CPF = cpf;
       

    } 


    public void Matricular(Materia materia)
    {
        materiasMatriculadas.Add(materia);
        Console.WriteLine($"Aluno com CPF {CPF} matriculado na matéria {materia.nomeMateria}");
        

    }

    public void Mostrarmaterias()
    {
        

            foreach (var materia in materiasMatriculadas)
            {

                Console.WriteLine("\n- " + materia.nomeMateria);
            }


    }




}






public class Program
{
    public static void Main(string[] args)
    {

        

                Console.Write("Digite o CPF do aluno: ");
                string cpf = Console.ReadLine();


                Aluno aluno = new Aluno(cpf);



                int opcao;


                do
                {
                    Console.WriteLine("\n--- MENU ---");
                    Console.WriteLine("1 - Matricular em matéria");
                    Console.WriteLine("2 - Ver matérias matriculadas");
                    Console.WriteLine("0 - Sair");
                    Console.Write("Escolha uma opção: ");

                    opcao = int.Parse(Console.ReadLine());

                    switch (opcao)
                    {
                        case 1:
                            Console.Write("Digite o nome da matéria: ");
                            string nomeMateria = Console.ReadLine();

                            Materia materia = new Materia(nomeMateria);
                            aluno.Matricular(materia);
                            break;

                        case 2:

                            {
                                Console.WriteLine("\nMatérias matriculadas:");
                                aluno.Mostrarmaterias();
                                break;
                            }


                        case 0:
                            Console.WriteLine("Encerrado programa");
                            break;

                    }




                } while (opcao != 0);



            }



        }
