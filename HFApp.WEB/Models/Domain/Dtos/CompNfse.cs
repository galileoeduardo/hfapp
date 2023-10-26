using System.Xml.Serialization;

namespace HFApp.WEB.Models.Domain.Dtos
{
    
    public class CompNfseDto
    {
        [XmlElement("CompNfse")]
        public CompNfse CompNfse { get; set; }
    }
    [Serializable]
    public class CompNfse
    {
        [XmlElement("Nfse")]
        Nfse Nfse { get; set; }
    }

    public class Nfse
    {
        [XmlElement("InfNfse")]
        InfNfse InfNfse { get; set; }
    }

    public class InfNfse
    {
        [XmlElement("Numero")]
        public int Numero { get; set; }

        
    }

    public class IdentificacaoRps
    {
        [XmlElement("Numero")]
        public int Numero { get; set; }

        [XmlElement("Serie")]
        public int Serie { get; set; }

        [XmlElement("Tipo")]
        public int Tipo { get; set; }
    }

    public class Servico
    {
        [XmlElement("Valores")]
        Valores Valores { get; set; }
    }

    public class Valores
    {
        [XmlElement("ValorServicos")]
        int ValorServicos { get; set; }

        [XmlElement("IssRetido")]
        int IssRetido { get; set; }

        [XmlElement("ValorIss")]
        double ValorIss { get; set; }

        [XmlElement("BaseCalculo")]
        double BaseCalculo { get; set;}

        [XmlElement("Aliquota")]
        double Aliquota { get; set; }

        [XmlElement("ValorLiquidoNfse")]
        double ValorLiquidoNfse { get; set; }
    }

    public class PrestadorServico
    {
        [XmlElement("IdentificacaoPrestador")]
        IdentificacaoPrestador IdentificacaoPrestador { get; set; }

    }

    public class IdentificacaoPrestador
    {
        [XmlElement("Cnpj")]
        public string Cnpj { get; set; }

        [XmlElement("InscricaoMunicipal")]
        public string InscricaoMunicipal { get; set; }

        [XmlElement("RazaoSocial")]
        public string RazaoSocial { get; set; }

        [XmlElement("NomeFantasia")]
        public string NomeFantasia { get; set; }
    }

    public class TomadorServico
    {
        [XmlElement("IdentificacaoTomador")]
        IdentificacaoTomador IdentificacaoTomador { get; set; }
        [XmlElement("IdentificacaoTomador")]
        public string RazaoSocial { get; set; }

    }

    public class IdentificacaoTomador
    {
        [XmlElement("CpfCnpj")]
        public CpfCnpj CpfCnpj { get; set; }
    }

    public class CpfCnpj
    {
        [XmlElement("Cnpj")]
        public string Cnpj { get; set; }

        [XmlElement("Cpf")]
        public string Cpf { get; set; }
    }
}
