// Arquivo: Services/FileService.cs
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public static class FileService
{
    private const string FilePath = "pessoas.json";

    public static void ExportarParaJson(List<Pessoa> pessoas)
    {
        var json = JsonConvert.SerializeObject(pessoas, Formatting.Indented);
        File.WriteAllText(FilePath, json);
        Console.WriteLine($"Dados exportados para '{FilePath}' com sucesso!");
    }

    public static List<Pessoa> ImportarDeJson()
    {
        if (!File.Exists(FilePath))
        {
            Console.WriteLine($"Arquivo '{FilePath}' não encontrado.");
            return new List<Pessoa>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonConvert.DeserializeObject<List<Pessoa>>(json);
    }
}