namespace Locadora.Models;
public class Filme
{
    public int Id { get; set; } // Propriedade para identificar exclusivamente um filme
    public string? Titulo { get; set; } // Título do filme
    public string? Diretor { get; set; } // Diretor do filme
    public int AnoLancamento { get; set; } // Ano de lançamento do filme
    public int ClassificacaoEtaria { get; set; } 
    
}
