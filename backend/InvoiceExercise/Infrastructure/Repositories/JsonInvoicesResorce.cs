using Application.Dtos.Json;
using Application.Interfaces;
using System.Text.Json;

namespace InvoiceExercise.Infrastructure.Repositories
{
    public class JsonInvoicesResorce : IInvoicesJson
    {
        public List<InvoiceJsonDto> ReadInvoices(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Archivo no encontrado: {filePath}");

            var jsonString = File.ReadAllText(filePath);

            // Case-insensitive para asegurar match con el JSON
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString |
                            System.Text.Json.Serialization.JsonNumberHandling.WriteAsString
            };

            var root = JsonSerializer.Deserialize<RootJsonDto>(jsonString, options);
            return root?.invoices ?? new List<InvoiceJsonDto>();
        }
    }
    public class IntToStringConverter : System.Text.Json.Serialization.JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64().ToString();
            }
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }


}
