﻿namespace Domain.DTOs.Responses;

public class TicketResponse
{
    #region Public properties declaration

    public string TicketIdView => $"T{TicketId.ToString().PadLeft(5, '0')}";
    public string? AssignedToId { get; set; }
    public List<AttachmentResponse>? Attachments { get; set; }
    public string? Category { get; set; }
    public int CategoryId { get; set; }
    public string? ClosedBy { get; set; }
    public DateTime? ClosedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Description { get; set; }
    public DateTime ExpectedDate { get; set; }
    public string? Priority { get; set; }
    public int PriorityId { get; set; }
    public string? Product { get; set; }
    public int ProductId { get; set; }
    public string? RaisedBy { get; set; }
    public string? RaisedByName { get; set; }
    public string? Status { get; set; }
    public string? Summary { get; set; }
    public int TicketId { get; set; }
    public string? RaisedByAvatar { get; set; }

    #endregion
}