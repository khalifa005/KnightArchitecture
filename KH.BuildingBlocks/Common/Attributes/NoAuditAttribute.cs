using KH.BuildingBlocks.Apis.Entities;

namespace KH.BuildingBlocks.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NoAuditAttribute : Attribute
{
}

//[NoAudit] to stop ef to audit this entity
//public class Audit 
