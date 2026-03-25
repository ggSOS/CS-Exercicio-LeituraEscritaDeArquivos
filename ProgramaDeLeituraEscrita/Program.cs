using System.Text;

namespace ProgramaDeLeituraEscrita
{
    internal static class Program
    {
        public const string directoryPath = "Arquivos";
        public static void Main(string[] args)
        {
            bool programaRodando = true;
            var comandos = new Dictionary<string, Action>()
            {
                {"1", LerArquivo},
                {"2", EscreverArquivo},
                {"3", () => programaRodando = false}
            };
            string respostaInput;
            string comandoInstrucoes = "\n> Insira um comando:\n  1 - [ler arquivo]\n  2 - [escrever arquivo]\n  3 - [encerrar programa]\n\t> ";
            string comandoInvalido = "\n> Comando Invalido!";


            while (programaRodando)
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                respostaInput = ValidarResposta(comandoInstrucoes, comandoInvalido);
                while (!comandos.ContainsKey(respostaInput))
                {
                    System.Console.WriteLine(comandoInvalido);
                    respostaInput = ValidarResposta(comandoInstrucoes, comandoInvalido);
                }
                comandos[respostaInput]();
            }
        }

        public static void EscreverArquivo()
        {
            List<string> extensoesPermitidas = new List<string> { ".sql", ".ddl" };
            string escreverPergunta = "\n> Qual o nome de arquivo que deseja inserir?\n\t> ";
            string escreverInvalido = "\n> Nome Invalido!";

            string escreverArquivoPath = ValidarResposta(escreverPergunta, escreverInvalido);

            while (!extensoesPermitidas.Contains(Path
                                                    .GetExtension(escreverArquivoPath)
                                                    .ToLower()))
            {
                System.Console.WriteLine("\n> Extensao Invalida!Apenas .sql e .dml.");
                escreverArquivoPath = ValidarResposta(escreverPergunta, escreverInvalido);
            }

            if (File.Exists($"{directoryPath}/{escreverArquivoPath}"))
            {
                escreverArquivoPath = SobrescreverArquivo(escreverArquivoPath);
            }

            string escreverArquivoTexto = ValidarResposta("\n> Insira o conteudo do arquivo que sera gravado:\n\t> ", "\n> Conteudo Invalido!");

            try
            {
                File.WriteAllText($"{directoryPath}/{escreverArquivoPath}", escreverArquivoTexto);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Sem permissão para escrever no arquivo.");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Diretório não encontrado.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Erro de leitura/escrita (arquivo em uso ou disco cheio).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n> Erro inesperado: {ex.Message}");
            }
        }

        public static string ValidarResposta(string pergunta, string mensagemErro)
        {
            System.Console.Write(pergunta);
            string? validarNomePath = Console.ReadLine();
            while (validarNomePath == null || validarNomePath.Trim().Length == 0)
            {
                System.Console.Write($"{mensagemErro}{pergunta}");
                validarNomePath = Console.ReadLine();
            }
            return validarNomePath;
        }

        public static string SobrescreverArquivo(string sobrescreverArquivoPath)
        {
            Dictionary<string, Boolean> sobrescritaRespostas = new Dictionary<string, Boolean>{
                {"1", true},
                {"2", false}
            };
            string? respostaInput = " ";

            while (File.Exists($"{directoryPath}/{sobrescreverArquivoPath}"))
            {
                while (!sobrescritaRespostas.ContainsKey(respostaInput))
                {
                    respostaInput = ValidarResposta("\n> Nome de arquivo ja existente!\n> Deseja:\n  1 - [sobrescrever arquivo existente]\n  2 - [mudar nome do arquivo]\n\t> ", "\n> Comando Invalido!");
                }

                if (sobrescritaRespostas[respostaInput])
                {
                    return sobrescreverArquivoPath;
                }
                sobrescreverArquivoPath = ValidarResposta("\n> Qual o novo nome de arquivo que deseja inserir?\n\t> ", "\n> Nome Invalido!");
            }
            return sobrescreverArquivoPath;
        }

        public static void LerArquivo()
        {
            if (!Directory.EnumerateFileSystemEntries(directoryPath).Any())
            {
                System.Console.WriteLine("\n> Nao ha nenhum arquivo para ser lido!");
                return;
            }

            string[] arquivos = Directory.GetFiles(directoryPath);
            StringBuilder arquivosSB = new StringBuilder();
            foreach (string arquivo in arquivos)
            {
                arquivosSB.Append($"\n  - {Path.GetFileName(arquivo)}");
            }
            string lerPergunta = $"\n> Lista de arquivos:{arquivosSB}\n> Qual o nome do arquivo que deseja ler?\n\t> ";
            string lerInvalido = "\n> Nome Invalido!";

            string lerArquivoPath = ValidarResposta(lerPergunta, lerInvalido);

            while (!File.Exists($"{directoryPath}/{lerArquivoPath}"))
            {
                System.Console.WriteLine("\n> Arquivo nao existe!");
                lerArquivoPath = ValidarResposta(lerPergunta, lerInvalido);
            }

            try
            {
                string[] textoEmLinhas = File.ReadAllLines($"{directoryPath}/{lerArquivoPath}");
                foreach (string linha in textoEmLinhas)
                {
                    System.Console.WriteLine(linha);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Arquivo não encontrado.");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Diretório não encontrado.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Sem permissão para acessar o arquivo.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"\n> {ex.Message} - Erro de leitura (arquivo em uso ou disco).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n> Erro inesperado: {ex.Message}");
            }
        }
    }
}