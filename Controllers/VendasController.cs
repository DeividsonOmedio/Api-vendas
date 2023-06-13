using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Controllers.Context;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Controllers
{
   

    public class VendasController : ControllerBase
    {
        private readonly VendasContext _context;
        public VendasController(VendasContext context)
        {
            _context = context;

        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
           var tarefa = _context.Vendas;
            if (tarefa == null)
                return NotFound();
            
            return Ok(tarefa);
        }

        [HttpPost("Adicionar Nova Venda")]
        public IActionResult AdicionarVenda(int id_Do_Vendedor, List<string>produtos_Da_Venda, DateTime Data_da_Venda)
        {
            var verificarVendedor = _context.Vendedor.Find(id_Do_Vendedor);
            Vendas novaVenda = new Vendas();
           
            if (verificarVendedor != null)
            {
               
                if(produtos_Da_Venda.Count >= 0){
                    novaVenda.IdVendedor = id_Do_Vendedor;
                   novaVenda.Data = Data_da_Venda;
                   novaVenda.ProdutosVenda = conversao(produtos_Da_Venda);
                   if(novaVenda.ProdutosVenda == "")
                   {
                    return NotFound("Pelo menos 1 produto deve ser informado");
                   }
                    _context.Add(novaVenda);
                    _context.SaveChanges();
                    return Ok(novaVenda);
                }
                return NotFound("Pelo menos 1 item é necessario no campo itens");
            }
            return NotFound("Vendedor não cadastrado");
        }

        public string conversao(List<string> produtosVenda)
        {
            string retornoString = "";

           foreach (var produto in produtosVenda)
           {
            
                 retornoString += produto + ", ";
           }
           return retornoString;
        }

      

        [HttpPatch("Mudar situação de pagamento")]
        public IActionResult MudarSituacaoPagamento(int id, string status)
        {
            if (id == 0){
                    return NotFound("Campo Id é de preenchimento obrigatório");
                }
            if (string.IsNullOrEmpty(status)){
                    return NotFound("Campo Status é de preenchimento obrigatório");
                }
            status = status.ToUpper();
            var vendax = _context.Vendas.Find(id);
                if (vendax == null){
                    return NotFound("Código de venda não encontrada");
                }
                if (vendax.Status == "AGUARADANDO PAGAMENTO" || vendax.Status == "Aguardando Pagamento" ){
                    if(status == "PAGAMENTO APROVADO" || status == "CANCELADO"){
                        vendax.Status = status;
                        _context.Update(vendax);
                        _context.SaveChanges();
                        return Ok(vendax);
                    }
                    if(status != "CANCELADO" & status != "PAGAMENTO APROVADO")
                    {
                        return NotFound("Status AGUARADANDO PAGAMENTO só pode ser alterado para PAGAMENTO APROVADO ou CANCELADO");
                    }
                }
                if (vendax.Status == "PAGAMENTO APROVADO"){
                     if(status == "ENVIADO PARA A TRANSPORTADORA" || status == "CANCELADO"){
                        vendax.Status = status;
                        _context.Update(vendax);
                        _context.SaveChanges();
                        return Ok(vendax);
                     }
                    if(status != "ENVIADO PARA A TRANSPORTADORA" && status != "CANCELADO")
                    {
                        return NotFound("Status PAGAMENTO APROVADO só pode ser alterado para ENVIADO PARA A TRANSPORTADORA ou CANCELADO");
                    }
                }
                if (vendax.Status == "ENVIADO PARA A TRANSPORTADORA" ){
                     if(status == "ENTREGUE"){
                        vendax.Status = status;
                        _context.Update(vendax);
                        _context.SaveChanges();
                        return Ok(vendax);
                     }
                    if(vendax.Status != "Entregue")
                    {
                        return NotFound("Status ENVIADO PARA A TRANSPORTADORA só pode ser alterado para ENTREGUE");
                    }
                 
                }
                return NotFound("Revise os dados");
                
        }
   

    }
}