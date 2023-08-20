using System.Runtime.Serialization;

namespace ContainersApiTask.Models.Enumerations
{
    public static class StateExtension
    {
        //public static 
    }
    [DataContract]
    public enum StateEnum
    {
        [EnumMember(Value = "draft")]
        DRAFT,
        [EnumMember(Value = "moderation")]
        MODERATION,
        [EnumMember(Value = "published")]
        PUBLISHED

    }
}
