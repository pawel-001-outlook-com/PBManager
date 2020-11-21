namespace PBManager.Dto.Dtos
{
    public class ReportFormDataRequestDto
    {
        public string accountId { get; set; }
        public string categoryId { get; set; }
        public string subcategoryId { get; set; }
        public string projectId { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
}