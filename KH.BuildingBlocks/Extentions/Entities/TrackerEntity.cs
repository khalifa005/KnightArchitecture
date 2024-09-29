using System.ComponentModel.DataAnnotations.Schema;

namespace KH.BuildingBlocks.Extentions.Entities
{
  public abstract partial class TrackerEntity : BaseEntity
  {
    //what about the relations for created users !! FK's

    [Column(Order = 101)]
    public DateTime CreatedDate { get; set; }
    [Column(Order = 102)]
    public long? CreatedById { get; set; }
    //public User? CreatedByUser { get; set; }
    [Column(Order = 103)]
    public DateTime? UpdatedDate { get; set; }
    //public User? UpdatedByUser { get; set; }

    [Column(Order = 104)]
    public long? UpdatedById { get; set; }
    [Column(Order = 105)]
    public bool IsDeleted { get; set; } = false;
    [Column(Order = 106)]
    public DateTime? DeletedDate { get; set; }
    [Column(Order = 107)]
    public long? DeletedById { get; set; }
    //public User? DeletedByUser { get; set; }

  }

}
