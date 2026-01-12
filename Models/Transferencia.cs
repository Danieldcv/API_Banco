namespace ApiBanco.Models;

public class Transferencia {
    public int contaOrigem { get; set; }
    public int contaDestino { get; set; }
    public decimal valor { get; set; }
    public string senha { get; set; } = string.Empty; 
}