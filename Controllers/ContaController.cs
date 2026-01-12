using Microsoft.AspNetCore.Mvc;
using ApiBanco.Models;

namespace ApiBanco.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase {
    private static List<Conta> contas = new List<Conta>();

    // Endpoint de cadastro
    [HttpPost("cadastro")]
    public IActionResult CriarConta([FromBody] Conta novaConta) {
        novaConta.numeroConta = new Random().Next(1, 100000000);
        novaConta.saldo = 1000;
        novaConta.extrato = new List<Transacao>();
        contas.Add(novaConta);
        return Ok(new { 
            mensagem = "Conta criada!", 
            titular = novaConta.nome, 
            numero = novaConta.numeroConta 
        });
    }

    // Endpoint de login
    [HttpPost("login")]
    public IActionResult Login([FromBody] Conta dados) {
        var conta = contas.FirstOrDefault(c => c.email == dados.email && c.senha == dados.senha);
        if (conta == null) {
            return Unauthorized(new { 
                mensagem = "Usuário ou senha inválidos." 
            });
        }
        return Ok(new { 
            mensagem = "Login realizado com sucesso!", 
            usuario = conta.nome,
        });
    }

    // Endpoint para obter o saldo da conta
    [HttpGet("saldo/{numeroConta}")]
    public IActionResult ObterSaldo(int numeroConta) {
        var conta = contas.FirstOrDefault(c => c.numeroConta == numeroConta);
        if (conta == null) {
            return NotFound(new { 
                mensagem = "Conta não encontrada." 
            });
        }
        return Ok(new { 
            saldo = conta.saldo 
        });
    }

    // Endpoint para obter o extrato da conta
    [HttpGet("extrato/{numeroConta}")]
    public IActionResult ObterExtrato(int numeroConta) {
        var conta = contas.FirstOrDefault(c => c.numeroConta == numeroConta);
        if (conta == null) {
            return NotFound(new { 
                mensagem = "Conta não encontrada." 
            });
        }
        return Ok(conta.extrato);
    }

    // Endpoint para transferir valores entre contas
    [HttpPost("transferir")]
    public IActionResult Transferir([FromBody] Transferencia pedido) {
        var contaOrigem = contas.FirstOrDefault(c => c.numeroConta == pedido.contaOrigem);
        var contaDestino = contas.FirstOrDefault(c => c.numeroConta == pedido.contaDestino);

        // Validações
        if (contaOrigem == null) return NotFound(new { mensagem = "Conta de origem não existe." });
        if (contaDestino == null) return NotFound(new { mensagem = "Conta de destino não existe." });
        if (contaOrigem.senha != pedido.senha) return BadRequest(new { mensagem = "Senha incorreta." });
        if (pedido.valor <= 0) return BadRequest(new { mensagem = "Valor inválido." });
        if (contaOrigem.saldo < pedido.valor) return BadRequest(new { mensagem = "Saldo insuficiente." });

        // Execução e registro da tranferência
        contaOrigem.saldo -= pedido.valor;
        contaDestino.saldo += pedido.valor;
        contaOrigem.extrato.Add(new Transacao {
            tipo = "Transferência Enviada",
            valor = -pedido.valor,
            data = DateTime.Now
        });
        contaDestino.extrato.Add(new Transacao {
            tipo = "Transferência Recebida",
            valor = pedido.valor,
            data = DateTime.Now
        });
        return Ok(new { mensagem = "Transferência realizada com sucesso!" });
    }
}