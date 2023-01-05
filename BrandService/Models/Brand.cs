using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BrandService.Models;

public class Brand
{
    public int Id { get; set; }
    
    public int? ParentId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Code { get; set; }
    
    [Required]
    public string Image { get; set; }
    
    [Required]
    public int Status { get; set; }
    
    [Required]
    public string Slug { get; set; }
    
    public string SortOrder { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public string ParentIdCode { get; set; }
    
    public string BrandIdCode { get; set; }
    
    public Brand? Parent { get; set; }
    
    public Brand[]? Children { get; set; }
}