using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        static string CONNECTION = "Server= localhost; Database= teste; Integrated Security=True";
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //CriarPessoa();
            BuscarPessoa();
      
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalSeconds;
            //1.3636021
            Console.WriteLine("Tempo gasto de leitura com sqlconection em  segundos " + elapsedMs);


            var watchDapper = System.Diagnostics.Stopwatch.StartNew();
            //CriarPessoa();
            BuscarPessoaDapper();

            watchDapper.Stop();
            var elapsedMsDapper = watchDapper.Elapsed.TotalSeconds;            
            Console.WriteLine("Tempo gasto de leitura com Dapper em segundos: " + elapsedMsDapper);
            Console.ReadKey();
        }

        private static IEnumerable<Pessoa> BuscarPessoaDapper()
        {
            using (SqlConnection conexao = new SqlConnection(CONNECTION))
            {
                return conexao.Query<Pessoa>(
                    "SELECT * FROM PESSOA");
            }
        }

        private static void BuscarPessoa()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            SqlConnection con = new SqlConnection(CONNECTION);
            con.Open();

            List<Pessoa> listPessoa = new List<Pessoa>();
           

            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $@"SELECT * FROM PESSOA";
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pessoa pessoa = new Pessoa();
                    pessoa.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    pessoa.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                    pessoa.Cpf = reader.GetString(reader.GetOrdinal("Cpf"));
                    pessoa.Rua = reader.GetString(reader.GetOrdinal("Rua"));
                    pessoa.Cep = reader.GetString(reader.GetOrdinal("Cep"));
                    pessoa.Numero = reader.GetString(reader.GetOrdinal("Numero"));
                    pessoa.Complemento = reader.GetString(reader.GetOrdinal("Complemento"));
                    pessoa.Sexo = reader.GetString(reader.GetOrdinal("Sexo"));
                    pessoa.Bairro = reader.GetString(reader.GetOrdinal("Bairro"));
                    pessoa.Sobrenome = reader.GetString(reader.GetOrdinal("Sobrenome"));
                    pessoa.Rg = reader.GetString(reader.GetOrdinal("Rg"));

                    listPessoa.Add(pessoa);

                }
                cmd.Dispose();

                con.Close();
            }
        }

        private static void CriarPessoa()
        {
            for (int i = 0; i < 10000; i++)
            {
                SqlConnection con = new SqlConnection(CONNECTION);
                con.Open();

                Pessoa pessoa = new Pessoa
                {
                    Id = i,
                    Nome = "nome " + i,
                    Numero = "numeoro " + i,
                    Rg = "rg1234" + i,
                    Rua = "rua " + i,
                    Bairro = "bairro " + i,
                    Cep = "38400" + i,
                    Complemento = "complemento " + i,
                    Sobrenome = "sobrenome " + 1,
                    Cpf = "1234" + i,
                    Sexo = "sexo " + i
                };

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"INSERT INTO PESSOA 
                        (Id, Nome, Numero, Rg, Bairro, Cep, Complemento, Sobrenome, Cpf, Sexo, Rua)
                        Values (
                        {pessoa.Id}, '{pessoa.Nome}', '{pessoa.Numero}', '{pessoa.Rg}', '{pessoa.Rua}',
                        '{pessoa.Bairro}', '{pessoa.Cep}', '{pessoa.Complemento}', '{pessoa.Sobrenome}',
                        '{pessoa.Cpf}', '{pessoa.Sexo}')";

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    con.Close();
                }
            }
        }
    }
}
