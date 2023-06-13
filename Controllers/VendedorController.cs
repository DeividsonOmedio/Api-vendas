using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Controllers.Context;
using tech_test_payment_api.Controllers.Entities;
using System.Globalization;
using System.Text.RegularExpressions;


namespace tech_test_payment_api.Controllers
{
     
    public class VendedorController : ControllerBase
    {
               
        private readonly VendasContext _context;
        public VendedorController(VendasContext context)
        {
            _context = context;
        }

        [HttpPost("Adicionar Vendedor")]
        public IActionResult AdicionarVendedor(string Nome, string CPF, string Telefone, string Email)
        {
            bool validarTelefone = ValidaTelefone(Telefone);
            bool validarCPF = IsCpf(CPF);
            bool validarEmail = IsValidEmail(Email);
            Vendedor vendedor = new Vendedor();
           
            if (string.IsNullOrEmpty(Nome))
            {
                return NotFound("Campo NOME deve ser preenchido");
            }
            else if (validarCPF == false)
            {
                return NotFound("Campo CPF deve ser preenchido e ser um CPF valido. ");
            }
            else if (validarTelefone == false)
            {
                return NotFound("Campo TELEFONE deve ser preenchido e no formato (XX)XXXXX-XXXX");
            }
            else if(validarEmail == false)
            {
                return NotFound("Digite um email válido");
            }
            else{
            vendedor.Nome = Nome;
            vendedor.CPF = CPF;
            vendedor.Telefone = Telefone;
            vendedor.Email = Email;
            _context.Add(vendedor);
            _context.SaveChanges();
            return Ok(vendedor);
            }

          
            
        } 
        [HttpGet("Obter Todos Vendedores")]
        public IActionResult ObterTodosVendedores()
        {
           var obter = _context.Vendedor;
            if (obter == null)
                return NotFound();
            
            return Ok(obter);
        }
        [HttpDelete("Deletar Vendedor")]
        public IActionResult DeletarVendedor(int id)
        {
            var vendedorDelete = _context.Vendedor.Find(id);
            if(vendedorDelete == null )
            {
                return NotFound("Id não encontrado no sistema");
            }
            
            _context.Remove(vendedorDelete);
            _context.SaveChanges();
            return Ok("Cadastro exluido com sucesso");
            
        }

        public bool ValidaTelefone(string telefone)
        {
            Regex Rgx = new Regex(@"^\(\d{2}\)\d{5}-\d{4}$"); //formato (XX)XXXXX-XXXX

            if (!Rgx.IsMatch(telefone))
                return false;
            else
                return true;
        }    


         public static bool IsCpf(string cpf)
	    {
		int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
		int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
		string tempCpf;
		string digito;
		int soma;
		int resto;
		cpf = cpf.Trim();
		cpf = cpf.Replace(".", "").Replace("-", "");
		if (cpf.Length != 11)
		   return false;
		tempCpf = cpf.Substring(0, 9);
		soma = 0;

		for(int i=0; i<9; i++)
		    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
		resto = soma % 11;
		if ( resto < 2 )
		    resto = 0;
		else
		   resto = 11 - resto;
		digito = resto.ToString();
		tempCpf = tempCpf + digito;
		soma = 0;
		for(int i=0; i<10; i++)
		    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
		resto = soma % 11;
		if (resto < 2)
		   resto = 0;
		else
		   resto = 11 - resto;
		digito = digito + resto.ToString();
		return cpf.EndsWith(digito);
	    }

         public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException )
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        
    }
}