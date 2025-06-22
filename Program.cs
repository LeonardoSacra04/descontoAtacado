Console.Clear();

List<Varejo> produtos = new List<Varejo>
{
    new Varejo() {GTIN = "7891024110348", Descricao = "SABONETE OLEO DE ARGAN 90G PALMOLIVE", ValorVarejo = 2.88m, ValorAtacado = 2.51m, Unidades = 12},
    new Varejo() {GTIN = "7891048038017", Descricao = "CHÁ DE CAMOMILA DR.OETKER", ValorVarejo = 4.40m, ValorAtacado = 4.37m, Unidades = 3},
    new Varejo() {GTIN = "7896066334509", Descricao = "TORRADA TRADICIONAL WICKBOLD PACOTE 140G", ValorVarejo = 5.19m, ValorAtacado = null, Unidades = null},
    new Varejo() {GTIN = "7891700203142", Descricao = "BEBIDA À BASE DE SOJA MAÇÃ ADES CAIXA 200ML", ValorVarejo = 2.39m, ValorAtacado = 2.38m, Unidades = 6},
    new Varejo() {GTIN = "7894321711263", Descricao = "ACHOCOLATADO PÓ ORIGINAL TODDY POTE 400G", ValorVarejo = 9.79m, ValorAtacado = null, Unidades = null},
    new Varejo() {GTIN = "7896001250611", Descricao = "ADOÇANTE LÍQUIDO SUCRALOSE LINEA CAIXA 25ML", ValorVarejo = 9.89m, ValorAtacado = 9.10m, Unidades = 10},
    new Varejo() {GTIN = "7793306013029", Descricao = "CEREAL MATINAL CHOCOLATE KELLOGGS SUCRILHOS CAIXA 320G", ValorVarejo = 12.79m, ValorAtacado = 12.35m, Unidades = 3},
    new Varejo() {GTIN = "7896004400914", Descricao = "COCO RALADO SOCOCO 50G", ValorVarejo = 4.20m, ValorAtacado = 4.05m, Unidades = 6},
    new Varejo() {GTIN = "7898080640017", Descricao = "LEITE UHT INTEGRAL 1L COM TAMPA ITALAC", ValorVarejo = 6.99m, ValorAtacado = 6.89m, Unidades = 12},
    new Varejo() {GTIN = "7891025301516", Descricao = "DANONINHO PETIT SUISSE COM POLPA DE MORANGO 360G DANONE", ValorVarejo = 12.99m, ValorAtacado = null, Unidades = null},
    new Varejo() {GTIN = "7891030003115", Descricao = "CREME DE LEITE LEVE 200G MOCOCA", ValorVarejo = 2.88m, ValorAtacado = 3.09m, Unidades = 4}
};

List<Provador> pvd = new List<Provador>
{
    new Provador() {GTIN = "7891048038017", Quantidade = 1, Valor = 4.40m},
    new Provador() {GTIN = "7896004400914", Quantidade = 4, Valor = 16.80m},
    new Provador() {GTIN = "7891030003115", Quantidade = 1, Valor = 3.12m},
    new Provador() {GTIN = "7891024110348", Quantidade = 6, Valor = 17.28m},
    new Provador() {GTIN = "7898080640017", Quantidade = 24, Valor = 167.76m},
    new Provador() {GTIN = "7896004400914", Quantidade = 8, Valor = 33.60m},
    new Provador() {GTIN = "7891700203142", Quantidade = 8, Valor = 19.12m},
    new Provador() {GTIN = "7891048038017", Quantidade = 1, Valor = 4.40m},
    new Provador() {GTIN = "7793306013029", Quantidade = 3, Valor = 38.37m},
    new Provador() {GTIN = "7896066334509", Quantidade = 2, Valor = 10.38m}
};

decimal valorSubstotal = 0;
foreach (var item in pvd)
{
    valorSubstotal += item.Valor;
}

List<DescontoItem> Descontos = new List<DescontoItem>();

var Agrupar = new Dictionary<string, int>();

foreach (var venda in pvd)
{
    if (Agrupar.ContainsKey(venda.GTIN))
    {
        Agrupar[venda.GTIN] += venda.Quantidade;
    }
    else
    {
        Agrupar[venda.GTIN] = venda.Quantidade;
    }
}

foreach (var grupo in Agrupar)
{
    Varejo? produtoEncontrado = null;

    foreach (var prod in produtos)
    {
        if (prod.GTIN == grupo.Key)
        {
            produtoEncontrado = prod;
            break;
        }
    }
    if (produtoEncontrado != null && produtoEncontrado.ValorAtacado != null &&
    grupo.Value >= produtoEncontrado.Unidades)
    {
        decimal descontoUnitario = produtoEncontrado.ValorVarejo - produtoEncontrado.ValorAtacado.Value;
        decimal descontoTotal = descontoUnitario * grupo.Value;
        Descontos.Add(new DescontoItem
        {
            GTIN = produtoEncontrado.GTIN,
            Desconto = descontoTotal
        }); 
    }
}

Console.WriteLine("--- Desconto no Atacado ---");
foreach (var desconto in Descontos)
{
    string nomeProduto = "Produto desconhecido";
    foreach (var prod in produtos)
    {
        if (prod.GTIN == desconto.GTIN)
        {
            nomeProduto = prod.Descricao;
            break;
        }
    }
    Console.WriteLine($"{desconto.GTIN.PadRight(30, '.').Substring(0, 20)}{desconto.Desconto:C2}");
}

decimal totalDescontos = 0;
foreach (var d in Descontos)
{
    totalDescontos += d.Desconto;
}

decimal valorTotal = valorSubstotal - totalDescontos;

Console.WriteLine();
Console.WriteLine($"(+) Subtotal =    {valorSubstotal:C2}");
Console.WriteLine($"(-) Descontos =     {totalDescontos:C2}");
Console.WriteLine($"(=) Total =       {valorTotal:C2}");

class Varejo
{
    public required string GTIN { get; set; }
    public required string Descricao { get; set; }
    public decimal ValorVarejo { get; set; }
    public decimal? ValorAtacado { get; set; }
    public int? Unidades { get; set; }
}

class Provador
{
    public required string GTIN { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}

class DescontoItem
{
    public required string GTIN { get; set; }
    public decimal Desconto { get; set; }
}


// --- Desconto no Atacado ---
// 
// Descontos:
// 7896004400914        R$ 1,80
// 7898080640017        R$ 2,40
// 7891700203142        R$ 0,08
// 7793306013029        R$ 1,32
// 
// (+) Subtotal  =    R$ 315,23
// (-) Descontos =      R$ 5,60
// (=) Total     =    R$ 309,63