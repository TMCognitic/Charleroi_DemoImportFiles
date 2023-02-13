using DemoImportFiles.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace DemoImportFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DirectoryInfo directoryInfo = new DirectoryInfo(@"c:\Data");

            //using (SqlConnection connection = new SqlConnection())
            //{
            //    connection.ConnectionString = @"Data Source=DESKTOP-S4743TT\SQL2019DEV;Initial Catalog=Demo;User Id=ApiUser;Password=Test1234=";

            //    connection.Open();
            //    foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            //    {
            //        using (SqlCommand command = connection.CreateCommand())
            //        {
            //            command.CommandText = "AppUser.CSP_InsertFile";
            //            command.CommandType = CommandType.StoredProcedure;

            //            command.Parameters.AddWithValue("FileName", fileInfo.Name);
            //            command.Parameters.AddWithValue("FileContent", File.ReadAllBytes(fileInfo.FullName));

            //            Console.WriteLine($"File {fileInfo.Name} : {command.ExecuteScalar().ToString().ToUpper()}");
            //        }
            //    }
            //}

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7081/api/");

                using(HttpResponseMessage response = client.GetAsync("FileContent").Result)
                {
                    response.EnsureSuccessStatusCode();

                    string json = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(json);

                    IEnumerable<FileDescription>? files = JsonSerializer.Deserialize<FileDescription[]>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    if(files is not null)
                    {
                        foreach (FileDescription file in files)
                        {
                            Console.WriteLine($"{file.Uid.ToString().ToUpper()} : {file.FileName}");
                        }
                    }
                }
            }

            Guid guid = new Guid("6154d26c-a6ab-ed11-bfc1-9eb6d06d2c49");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7081/api/");

                using (HttpResponseMessage response = client.GetAsync($"FileContent/{guid}").Result)
                {
                    response.EnsureSuccessStatusCode();

                    string json = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(json);

                    FileContent? file = JsonSerializer.Deserialize<FileContent>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    if (file is not null)
                    {
                        File.WriteAllBytes(file.FileName, file.Content);
                    }
                }
            }
        }
    }
}