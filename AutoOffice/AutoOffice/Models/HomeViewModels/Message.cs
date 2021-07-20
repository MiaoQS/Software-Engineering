using System.ComponentModel.DataAnnotations;
using System;

namespace AutoOffice.Models.HomeViewModels
{
    public class Message
    {
        public string ID { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        [StringLength(256)]
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
