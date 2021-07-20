using System.ComponentModel.DataAnnotations;

namespace AutoOffice.Models.HomeViewModels {
  public class JobRemind {
    public string ID { get; set; }
    public string Email { get; set; }
    [StringLength(512)]
    public string Content { get; set; }
    public bool Finished { get; set; }
  }
}
