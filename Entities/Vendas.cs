using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tech_test_payment_api.Entities
{


    public class Vendas
    {

        
      
        public int Id { get; protected set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Data")]
        public DateTime Data { get; set; }
        public string Status { get; set; } = "AGUARADANDO PAGAMENTO";

        [Column("Produtos")]
        [Required]
        public string ProdutosVenda { get; set;}
        [Required]
        public int IdVendedor {get; set;}
        

        
    }

    
   
    
}