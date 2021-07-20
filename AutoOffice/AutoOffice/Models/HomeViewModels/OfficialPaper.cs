using System.ComponentModel.DataAnnotations;
using System;

namespace AutoOffice.Models.HomeViewModels {

  public enum ApprovementState {
    NotDecideYet,
    Approved,
    NotApproved
  }

  public class OfficialPaper {
    public string ID { get; set; }
    [StringLength(50)]
    public string Title { get; set; }
    [StringLength(512)]
    public string Content { get; set; }
    public DateTime Time { get; set; }
    public string FromEmail { get; set; }
    public string ToEmail { get; set; }
    public int Approvement { get; set; }

    public string displayApprovement() {
      switch(Approvement) {
        case (int)ApprovementState.NotDecideYet:
          return "Not Decide Yet";
        case (int)ApprovementState.Approved:
          return "Agreed";
        case (int)ApprovementState.NotApproved:
          return "Disagreed";
      }
      return "发生错误";
    }
  }
}
