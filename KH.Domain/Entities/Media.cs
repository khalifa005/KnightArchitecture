namespace KH.Domain.Entities
{
    public class Media : TrackerEntity
    {
        
        public string FileName { get; set; }
        public string OrignalName { get; set; }
        public string? ContentType { get; set; }
        public string Path { get; set; }
        public string Extention { get; set; }

        //related to ModelEnum
        public string Model { get; set; }
        //as fake FK of the related item
        public int ModelId { get; set; }
    }

}
