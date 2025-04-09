namespace APIGastos.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }
}