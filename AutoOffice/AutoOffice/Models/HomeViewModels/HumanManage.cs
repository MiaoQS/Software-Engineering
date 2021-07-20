using System.ComponentModel.DataAnnotations;

namespace AutoOffice.Models.HomeViewModels
{
    public class HumanManage
    {
        public string ID { get; set; }
        public string Email { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string Job { get; set; }
    }
}
