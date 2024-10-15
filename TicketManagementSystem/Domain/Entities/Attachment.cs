﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Domain.Entities;

public class Attachment
{
    public int Id { get; set; }
    public string? FileName { get; set; }
    public string? ServerFileName { get; set; }
    public int FileSize { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey(nameof(Ticket))] public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }

    [ForeignKey(nameof(Discussion))] public int DiscussionId { get; set; }
    public Discussion? Discussion { get; set; }
}