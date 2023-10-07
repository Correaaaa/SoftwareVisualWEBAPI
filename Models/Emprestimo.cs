namespace Locadora.Models;
public class Emprestimo
{
    public int Id { get; set; } 
    public int ClienteId { get; set; } 
    public Cliente? Cliente { get; set; } 
    public int FilmeId { get; set; } 
    public Filme? Filme { get; set; } 
    public DateTime DataEmprestimo { get; set; } 
    public DateTime DataDevolucao { get; set; } 
}
