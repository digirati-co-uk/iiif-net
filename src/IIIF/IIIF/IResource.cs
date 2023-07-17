namespace IIIF;

/// <summary>
/// Interface for any resources that has an Id and Type
/// </summary>
public interface IResource
{
    string? Id { get; set; }
    string? Type { get; }
}