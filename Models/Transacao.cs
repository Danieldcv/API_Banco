namespace ApiBanco.Models;

public class Transacao {
    public DateTime data { get; set; } = DateTime.Now;
    public string tipo { get; set; } = string.Empty;
    public decimal valor { get; set; }
}