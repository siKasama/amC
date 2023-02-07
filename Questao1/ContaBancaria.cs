namespace Questao1
{
    public class ContaBancaria
    {
        private const double TAXA_SAQUE = 3.50;

        private int NumeroConta { get; set; }

        private string TitularConta { get; set; }

        private double SaldoConta { get; set; }

        public ContaBancaria(int numero, string titular)
        {
            NumeroConta = numero;
            TitularConta = titular;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            NumeroConta = numero;
            TitularConta = titular;

            Deposito(depositoInicial);
        }

        public void Deposito(double valor)
        {
            SaldoConta += valor;
        }

        public void Saque(double valor)
        {
            SaldoConta = SaldoConta - valor - TAXA_SAQUE;
        }

        public override string ToString()
        {
            return string.Format("Conta {0}, Titular: {1}, Saldo: $ {2} ", NumeroConta, TitularConta, SaldoConta.ToString("N2"));
        }
    }
}
