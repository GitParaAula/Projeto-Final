using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly PagamentoRepositorio _repositorio;

        public PagamentoController(PagamentoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View(); // Abre a view para cadastro
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Cadastrar(Pagamento pagamento)
        {
            // Pegue os valores "crus" enviados pelo form (string) para validar conforme solicitado.
            var titularRaw = Request.Form["Titular"].ToString() ?? string.Empty;
            var valorRaw = Request.Form["Valor"].ToString() ?? string.Empty; // manter vírgula
            var metodoRaw = Request.Form["MetodoPagamento"].ToString() ?? string.Empty;
            var codigoUsuarioRaw = Request.Form["Codigo_Usuario"].ToString() ?? string.Empty;

            // 1) Validações básicas de composição dos campos
            // Titular: apenas letras e espaços
            var titularRegex = new Regex(@"^[\p{L} ]+$"); // \p{L} = letra unicode
            // Valor: apenas dígitos e vírgula (pode ter várias vírgulas aqui, checaremos estrutura depois)
            var valorRegex = new Regex(@"^[0-9,]+$");
            // Metodo: apenas letras e espaços
            var metodoRegex = new Regex(@"^[\p{L} ]+$");
            // Codigo usuario: apenas dígitos
            var codigoRegex = new Regex(@"^[0-9]+$");

            if (!titularRegex.IsMatch(titularRaw) ||
                !valorRegex.IsMatch(valorRaw) ||
                !metodoRegex.IsMatch(metodoRaw) ||
                !codigoRegex.IsMatch(codigoUsuarioRaw))
            {
                TempData["Erro"] = "Preencha os campos corretamente";
                return View(pagamento);
            }

            // 2) Titular até 50 caracteres
            if (titularRaw.Length > 50)
            {
                TempData["Erro"] = "O titular só pode ter até 50 digitos.";
                return View(pagamento);
            }

            // 3) Valor até 8 caracteres no total (contando a vírgula)
            if (valorRaw.Length > 8)
            {
                TempData["Erro"] = "O valor só pode ter até 8 digitos.";
                return View(pagamento);
            }

            // 4) Metodo de pagamento até 20 caracteres
            if (metodoRaw.Length > 20)
            {
                TempData["Erro"] = "O metodo de pagamento só pode ter até 20 digitos.";
                return View(pagamento);
            }

            // 5) Codigo usuario até 10 digitos
            if (codigoUsuarioRaw.Length > 10)
            {
                TempData["Erro"] = "O codigo de usúario só pode ter até 10 digitos";
                return View(pagamento);
            }

            // 6) Verificar forma de pagamento válida: cartão crédito, cartão débito ou pix
            // Normalizar: remover acentos e espaços, comparar em lowercase
            string RemoveDiacritics(string text)
            {
                var normalized = text.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (var ch in normalized)
                {
                    var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                    if (uc != UnicodeCategory.NonSpacingMark)
                        sb.Append(ch);
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }

            var metodoNorm = RemoveDiacritics(metodoRaw).ToLower().Replace(" ", "").Replace("-", "");
            bool metodoValido = false;
            if (metodoNorm == "pix")
            {
                metodoValido = true;
            }
            else if (metodoNorm.Contains("cartao") || metodoNorm.Contains("cartao"))
            {
                // aceitar se contiver cartao + credito ou cartao + debito
                if (metodoNorm.Contains("credito") || metodoNorm.Contains("debito"))
                    metodoValido = true;
            }
            // Exemplo aceitável: "cartãodecrédito", "cartao de credito", "Cartão Crédito", "pix"
            if (!metodoValido)
            {
                TempData["Erro"] = "Forma de pagamento inválida";
                return View(pagamento);
            }

            // 7) Verificar se o codigo de usuario existe no banco
            if (!int.TryParse(codigoUsuarioRaw, out int codigoUsuarioInt))
            {
                TempData["Erro"] = "Preencha os campos corretamente";
                return View(pagamento);
            }

            var usuarioExiste = _repositorio.UsuarioExiste(codigoUsuarioInt);
            if (!usuarioExiste)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return View(pagamento);
            }

            // 8) Estrutura do valor: 1 a 5 dígitos + ',' + 2 dígitos (obrigatório a vírgula e exatamente 2 decimais)
            // Regex: ^\d{1,5},\d{2}$
            var valorEstruturaRegex = new Regex(@"^\d{1,5},\d{2}$");
            if (!valorEstruturaRegex.IsMatch(valorRaw))
            {
                TempData["Erro"] = "Estrutura inválida inserida em valor, o campo deve conter obrigatoriamente uma vírgula e dois dígitos após a vírgula";
                return View(pagamento);
            }

            // Tudo ok: converter o valor para decimal usando vírgula como separador (pt-BR)
            if (!Decimal.TryParse(valorRaw, NumberStyles.Number, new CultureInfo("pt-BR"), out decimal valorDecimal))
            {
                TempData["Erro"] = "Erro ao converter o valor. Use formato 12345,67";
                return View(pagamento);
            }

            // Preencher o objeto pagamento com os valores processados (em caso model binding não trouxe)
            pagamento.Titular = titularRaw;
            pagamento.Valor = valorDecimal;
            pagamento.MetodoPagamento = metodoRaw;
            pagamento.Codigo_Usuario = codigoUsuarioInt;

            // Persistir no banco
            try
            {
                _repositorio.CadastrarPagamento(pagamento);
            }
            catch (Exception ex)
            {
                // opcional: registrar log
                TempData["Erro"] = "Erro ao registrar pagamento: " + ex.Message;
                return View(pagamento);
            }

            // Redireciona para a view de confirmação
            return View("~/Views/ConfirmarPagamento/ConfirmarPagamento.cshtml");
        }

        [HttpGet]
        public IActionResult ConsultaHistorico()
        {
            return View("~/Views/Pagamento/ConsultaHistorico.cshtml");
        }

        [HttpGet]
        public IActionResult HistoricoCliente(int codigoCliente)
        {
            var pagamentos = _repositorio.HistoricoUltimos10(codigoCliente);
            return View("~/Views/Pagamento/HistoricoCliente.cshtml", pagamentos);
        }
    }
}