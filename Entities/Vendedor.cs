using System.ComponentModel.DataAnnotations;


namespace tech_test_payment_api.Controllers.Entities
{
    public class Vendedor
    {
        [Key]
        public int VendedorId { get; protected set; }

        [Required]
        public  string Nome { get; set; }
        
        [Required]
        public string CPF { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Telefone { get; set; }

        
      
    }
}