namespace ApiBanco.Models;

public class Conta {
    public int numeroConta { get; set; }
    public string nome { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string senha { get; set; } = string.Empty;
    public decimal saldo { get; set; }
    public List<Transacao> extrato { get; set; } = new List<Transacao>();
}