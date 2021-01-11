using System.ComponentModel.DataAnnotations;

namespace PBManager.Dto.ViewModels
{
    public class ReportFormDataRequestViewModel
    {
        [Required]
        public string month { get; set; }

        [Required]
        public string year { get; set; }

        [Required]
        public string period { get; set; }

        [Required]
        public string datefrom { get; set; }

        [Required]
        public string dateto { get; set; }

    }
}
